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
        readonly NoiseSettings noise;
        readonly MapSettings mapData;

        public GenerateVerticesBuffer(BufferFromEntity<Vertex> vertexBuffers, BufferFromEntity<UV> uvBuffers, NoiseSettings noise, MapSettings mapData) {
            this.vertexBuffers = vertexBuffers;
            this.uvBuffers = uvBuffers;
            this.noise = noise;
            this.mapData = mapData;
        }

        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates coordinates) {
            DynamicBuffer<Vertex> vertices = vertexBuffers[entity];
            vertices.Clear();
            DynamicBuffer<UV> uvs = uvBuffers[entity];
            uvs.Clear();
            float2 position = coordinates.Position();
            vertices.Add(DrawVertex(position));
            uvs.Add((Vector2) position);
            for (int currentRing = 1; currentRing <= mapData.levelOfDetail; currentRing++) {
                DrawRing(vertices, uvs, currentRing, position, mapData.levelOfDetail);
            }
        }

        private void DrawRing(DynamicBuffer<Vertex> vertices, DynamicBuffer<UV> uvs, int currentRing, float2 position, int numRings) {
            int verticesInRing = HexMath.CheckVerticesInLayer(currentRing);
            float arcBetweenPoints = FindArc(verticesInRing);
            float angle = math.PI / 2;
            for (int i = 0; i < verticesInRing; i++) {
                float distanceMultiple = FindDistance(currentRing, angle, numRings);
                float2 point = FindPoint(angle, distanceMultiple, position);
                vertices.Add(DrawVertex(point));
                uvs.Add((Vector2)point);
                angle += arcBetweenPoints;
            }
        }

        private float FindArc(int verticesInRing) {
            return -2 * math.PI / verticesInRing;
        }

        private float FindDistance(int currentRing, float angle, int numRings) {
            float hypotenuse = (float)currentRing / numRings;
            float interiorAngle = math.radians(30);
            angle = (2 * math.PI + angle) % math.radians(60);
            if (angle > interiorAngle) {
                angle = math.radians(60) - angle;
            }
            return hypotenuse * math.cos(interiorAngle) / math.cos(angle);
        }

        private float2 FindPoint(float angle, float distance, float2 position) {
            float pointX = position.x + distance * math.cos(angle);
            float pointY = position.y + distance * math.sin(angle);
            return new float2(pointX, pointY);
        }

        private Vector3 DrawVertex(float2 point) {
            return new Vector3(point.x, noise.Evaluate(point), point.y);
        }
    }
}