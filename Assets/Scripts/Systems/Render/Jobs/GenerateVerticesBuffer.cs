// file:	Assets\Scripts\Systems\Render\Jobs\GenerateVerticesBuffer.cs
//
// summary:	Implements the generate vertices buffer class
using System.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Assets.Scripts.Components;
using Assets.Scripts.Components.BufferElements;
using UnityEngine;

namespace Assets.Scripts.Systems.Render.Jobs
{
    /// <summary>   Buffer for generate vertices. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Vertex))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateVerticesBuffer : IJobForEachWithEntity<HexCoordinates>
    {
        /// <summary>   The vertex buffers. </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Vertex> vertexBuffers;
        /// <summary>   The uv buffers. </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<UV> uvBuffers;
        /// <summary>   The noise settings. </summary>
        readonly NoiseSettings noiseSettings;
        /// <summary>   The map settings. </summary>
        readonly MapSettings mapSettings;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="vertexBuffers">    The vertex buffers. </param>
        /// <param name="uvBuffers">        The uv buffers. </param>
        /// <param name="noiseSettings">    The noise settings. </param>
        /// <param name="mapSettings">      The map settings. </param>
        public GenerateVerticesBuffer(BufferFromEntity<Vertex> vertexBuffers, BufferFromEntity<UV> uvBuffers, NoiseSettings noiseSettings, MapSettings mapSettings) {
            this.vertexBuffers = vertexBuffers;
            this.uvBuffers = uvBuffers;
            this.noiseSettings = noiseSettings;
            this.mapSettings = mapSettings;
        }

        /// <summary>   Executes. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="entity">       The entity. </param>
        /// <param name="index">        Zero-based index of the. </param>
        /// <param name="coordinates">  [in,out] The coordinates. </param>
        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates coordinates) {
            DynamicBuffer<Vertex> vertices = vertexBuffers[entity];
            vertices.Clear();
            DynamicBuffer<UV> uvs = uvBuffers[entity];
            uvs.Clear();
            float2 position = HexMath.Position(coordinates);
            vertices.Add(DrawVertex(position));
            uvs.Add(new Vector2(.5f, .5f));
            for (int currentRing = 1; currentRing <= mapSettings.levelOfDetail; currentRing++) {
                DrawRing(vertices, uvs, currentRing, position);
            }
        }

        /// <summary>   Draw ring. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="vertices">     The vertices. </param>
        /// <param name="uvs">          The uvs. </param>
        /// <param name="currentRing">  The current ring. </param>
        /// <param name="position">     The position. </param>
        private void DrawRing(DynamicBuffer<Vertex> vertices, DynamicBuffer<UV> uvs, int currentRing, float2 position) {
            int verticesInRing = HexMath.CheckVerticesInLayer(currentRing);
            float arcBetweenPoints = FindArc(verticesInRing);
            float angle = math.PI / 2;
            for (int i = 0; i < verticesInRing; i++) {
                float distanceMultiple = FindDistance(currentRing, angle);
                float2 point = FindPoint(angle, distanceMultiple);
                vertices.Add(DrawVertex(position + point));
                uvs.Add(ProjectPoint(point));
                angle += arcBetweenPoints;
            }
        }

        /// <summary>   Searches for the first arc. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="verticesInRing">   The vertices in ring. </param>
        ///
        /// <returns>   The found arc. </returns>
        private float FindArc(int verticesInRing) {
            return -2 * math.PI / verticesInRing;
        }

        /// <summary>   Searches for the first distance. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="currentRing">  The current ring. </param>
        /// <param name="angle">        The angle. </param>
        ///
        /// <returns>   The found distance. </returns>
        private float FindDistance(int currentRing, float angle) {
            float hypotenuse = (float)currentRing / mapSettings.levelOfDetail;
            float interiorAngle = math.radians(30);
            angle = (2 * math.PI + angle) % math.radians(60);
            if (angle > interiorAngle) {
                angle = math.radians(60) - angle;
            }
            return hypotenuse * math.cos(interiorAngle) / math.cos(angle);
        }

        /// <summary>   Searches for the first point. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="angle">    The angle. </param>
        /// <param name="distance"> The distance. </param>
        ///
        /// <returns>   The found point. </returns>
        private float2 FindPoint(float angle, float distance) {
            float pointX = distance * math.cos(angle);
            float pointY = distance * math.sin(angle);
            return new float2(pointX, pointY);
        }

        /// <summary>   Draw vertex. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   A Vector3. </returns>
        private Vector3 DrawVertex(float2 point) {
            point *= mapSettings.scale;
            return new Vector3(point.x, Noise.Evaluate(point, noiseSettings), point.y);
        }

        /// <summary>   Project point. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   A Vector2. </returns>
        private Vector2 ProjectPoint(float2 point)
        {
            float maxX = HexMath.WidthMultiple;
            float minX = -HexMath.WidthMultiple;
            float maxY = HexMath.HeightMultiple;
            float minY = -HexMath.HeightMultiple;
            float x = (point.x - minX) / (maxX - minX);
            float y = (point.y - minY) / (maxY - minY);
            return new Vector2(x, y);
        }
    }
}