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
using System;

namespace Assets.Scripts.Systems.Render.Jobs
{
    /// <summary>   A job for generating the vertices and UVs for a mesh. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Vertex), typeof(UV), typeof(Normal))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateMeshVerticesAndUVs : IJobForEachWithEntity<HexCoordinates>
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
        /// <param name="normalBuffers">    The normal buffers. </param>
        /// <param name="noiseSettings">    The noise settings. </param>
        /// <param name="mapSettings">      The map settings. </param>
        public GenerateMeshVerticesAndUVs(BufferFromEntity<Vertex> vertexBuffers, BufferFromEntity<UV> uvBuffers, NoiseSettings noiseSettings, MapSettings mapSettings)
        {
            this.vertexBuffers = vertexBuffers;
            this.uvBuffers = uvBuffers;
            this.noiseSettings = noiseSettings;
            this.mapSettings = mapSettings;
        }

        /// <summary>   Executes the job. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="entity">       The entity. </param>
        /// <param name="index">        Zero-based index of the entity. </param>
        /// <param name="coordinates">  [in,out] The coordinates of the entity. </param>
        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates coordinates)
        {
            DynamicBuffer<Vertex> vertices = vertexBuffers[entity];
            vertices.Clear();

            DynamicBuffer<UV> uvs = uvBuffers[entity];
            uvs.Clear();

            float2 position = HexMath.Position(coordinates);
            Vector3 vertex = DrawVertex(position);
            vertices.Add(vertex);
            uvs.Add(new Vector2(.5f, .5f));

            for (int currentRing = 1; currentRing <= mapSettings.levelOfDetail; currentRing++)
            {
                DrawRing(vertices, uvs, currentRing, position);
            }
        }

        /// <summary>   Draw ring. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="vertices">     The vertices. </param>
        /// <param name="uvs">          The uvs. </param>
        /// <param name="normals">      The normals. </param>
        /// <param name="currentRing">  The current ring. </param>
        /// <param name="position">     The position. </param>
        private void DrawRing(DynamicBuffer<Vertex> vertices, DynamicBuffer<UV> uvs, int currentRing, float2 position)
        {
            int verticesInRing = HexMath.CheckVerticesInLayer(currentRing);
            float arcBetweenPoints = FindArc(verticesInRing);
            float angle = math.PI / 2;

            for (int i = 0; i < verticesInRing; i++)
            {
                float distanceMultiple = FindDistance(currentRing, angle);
                float2 point = FindPoint(angle, distanceMultiple);

                Vector3 vertex = DrawVertex(position + point);
                vertices.Add(vertex);
                uvs.Add(ProjectPoint(point));
                angle += arcBetweenPoints;
            }
        }

        /// <summary>   Searches for the arc between vertices. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="verticesInRing">   The number of vertices in the current ring. </param>
        ///
        /// <returns>   The arc between vertices. </returns>
        private float FindArc(int verticesInRing)
        {
            return -2 * math.PI / verticesInRing;
        }

        /// <summary>   Searches for the distance between the center and the vertex. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="currentRing">  The current ring. </param>
        /// <param name="angle">        The angle. </param>
        ///
        /// <returns>   The distance to the vertex. </returns>
        private float FindDistance(int currentRing, float angle)
        {
            float hypotenuse = (float)currentRing / mapSettings.levelOfDetail;
            float interiorAngle = math.radians(30);
            angle = (2 * math.PI + angle) % math.radians(60);

            if (angle > interiorAngle)
            {
                angle = math.radians(60) - angle;
            }
            return hypotenuse * math.cos(interiorAngle) / math.cos(angle);
        }

        /// <summary>   Finds the vertex given an angle and a distance. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="angle">    The angle. </param>
        /// <param name="distance"> The distance. </param>
        ///
        /// <returns>   The vertex. </returns>
        private float2 FindPoint(float angle, float distance)
        {
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
        /// <returns>   A Vector3 representing the vertex. </returns>
        private Vector3 DrawVertex(float2 point)
        {
            return new Vector3(point.x, Noise.Evaluate(point, noiseSettings), point.y) * mapSettings.scale;
        }

        /// <summary>   Find the UV of a point. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="point">    The point. </param>
        ///
        /// <returns>   A Vector2 representing the UV. </returns>
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
