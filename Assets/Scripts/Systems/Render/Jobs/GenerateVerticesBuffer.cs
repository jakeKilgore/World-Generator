using UnityEngine;
using System.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Unity.Burst;
using Unity.Collections;
using Unity.Mathematics;
using Assets.Scripts.Components;
using Assets.Scripts.Components.BufferElements;

namespace Assets.Scripts.Systems.Render.Jobs
{
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Vertex))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateVerticesBuffer : IJobForEachWithEntity<HexCoordinates>
    {
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Vertex> entityBuffers;
        readonly int numRings;
        readonly NoiseFilter noiseFilter;

        public GenerateVerticesBuffer(BufferFromEntity<Vertex> entityBuffers, int numRings, NoiseFilter noiseFilter) {
            this.entityBuffers = entityBuffers;
            this.numRings = numRings;
            this.noiseFilter = noiseFilter;
        }

        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates coordinates) {
            DynamicBuffer<Vertex> vertices = entityBuffers[entity];
            vertices.Clear();
            float2 position = coordinates.Position();
            vertices.Add(DrawVertex(position));
            int vertexIndex = 1;
            for (int currentRing = 1; currentRing <= numRings; currentRing++) {
                vertexIndex = DrawRing(vertices, currentRing, vertexIndex, position);
            }
        }

        private int DrawRing(DynamicBuffer<Vertex> vertices, int currentRing, int vertexIndex, float2 position) {
            int verticesInRing = HexMath.CheckVerticesInLayer(currentRing);
            float arcBetweenPoints = FindArc(verticesInRing);
            float angle = math.PI / 2;
            for (int i = vertexIndex; i < vertexIndex + verticesInRing; i++) {
                float distanceMultiple = FindDistance(currentRing, angle);
                float2 point = FindPoint(angle, distanceMultiple, position);
                vertices.Add(DrawVertex(point));
                angle += arcBetweenPoints;
            }
            return vertexIndex + verticesInRing;
        }

        private float FindArc(int verticesInRing) {
            return -2 * math.PI / verticesInRing;
        }

        private float FindDistance(int currentRing, float angle) {
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
            return new Vector3(point.x, noiseFilter.Evaluate(point), point.y);
        }
        private int AllocationSpaceForVertexArray(int numRings) {
            return HexMath.CheckVerticesInHex(numRings);
        }
    }
}