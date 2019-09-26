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
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Vertex))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateVerticesBuffer : IJobForEachWithEntity<HexCoordinates>
    {
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Vertex> vertexBuffers;
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<UV> uvBuffers;
        readonly NoiseSettings noiseSettings;
        readonly MapSettings mapSettings;

        public GenerateVerticesBuffer(BufferFromEntity<Vertex> vertexBuffers, BufferFromEntity<UV> uvBuffers, NoiseSettings noiseSettings, MapSettings mapSettings) {
            this.vertexBuffers = vertexBuffers;
            this.uvBuffers = uvBuffers;
            this.noiseSettings = noiseSettings;
            this.mapSettings = mapSettings;
        }

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

        private float FindArc(int verticesInRing) {
            return -2 * math.PI / verticesInRing;
        }

        private float FindDistance(int currentRing, float angle) {
            float hypotenuse = (float)currentRing / mapSettings.levelOfDetail;
            float interiorAngle = math.radians(30);
            angle = (2 * math.PI + angle) % math.radians(60);
            if (angle > interiorAngle) {
                angle = math.radians(60) - angle;
            }
            return hypotenuse * math.cos(interiorAngle) / math.cos(angle);
        }

        private float2 FindPoint(float angle, float distance) {
            float pointX = distance * math.cos(angle);
            float pointY = distance * math.sin(angle);
            return new float2(pointX, pointY);
        }

        private Vector3 DrawVertex(float2 point) {
            point *= mapSettings.scale;
            return new Vector3(point.x, Noise.Evaluate(point, noiseSettings), point.y);
        }

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