using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Assets.Scripts.Components;
using Assets.Scripts.Components.BufferElements;

namespace Assets.Scripts.Systems.Render.Jobs
{
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Triangle))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateTrianglesBuffer : IJobForEachWithEntity<HexCoordinates>
    {
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Triangle> triangleBuffers;
        readonly MapSettings mapData;

        public GenerateTrianglesBuffer(BufferFromEntity<Triangle> triangleBuffers, MapSettings mapData) {
            this.triangleBuffers = triangleBuffers;
            this.mapData = mapData;
        }

        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates coordinates) {
            DynamicBuffer<Triangle> triangles = triangleBuffers[entity];
            triangles.Clear();
            for (int currentLayer = 1; currentLayer <= mapData.levelOfDetail; currentLayer++) {
                DrawRing(triangles, currentLayer);
            }
        }

        private void DrawRing(DynamicBuffer<Triangle> triangles, int currentRing) {
            int startNode = HexMath.CheckVerticesInHex(currentRing - 2) - 1;
            int endNode = HexMath.CheckVerticesInHex(currentRing - 1) - 1;
            if (currentRing != 1) {
                startNode++;
            }

            int startVertex = endNode + 1;
            int endVertex = HexMath.CheckVerticesInHex(currentRing) - 1;

            int currentVertex = startVertex;
            for (int currentNode = startNode; currentNode <= endNode; currentNode++) {
                int trianglesPerNode = TrianglesPerNode(currentNode, startNode, endNode);
                for (int nodeTriangles = 0; nodeTriangles < trianglesPerNode; nodeTriangles++) {
                    if (currentRing != 1 && currentNode == startNode && nodeTriangles == 0) {
                        continue;
                    }
                    int vertex1 = currentVertex;
                    int vertex2 = currentVertex + 1;

                    if (vertex2 > endVertex) {
                        vertex2 = currentRing == 1 ? startVertex : startNode;
                    }

                    DrawTriangle(triangles, currentNode, vertex1, vertex2);

                    currentVertex++;
                }
                if (currentRing != 1) {
                    int vertex1 = currentVertex;
                    int vertex2 = currentNode + 1;
                    if (vertex2 > endNode) {
                        vertex2 = startNode;
                    }

                    DrawTriangle(triangles, currentNode, vertex1, vertex2);

                    if (currentNode == endNode) {
                        DrawTriangle(triangles, startNode, endVertex, startVertex);
                    }
                }
            }
        }

        private int TrianglesPerNode(int currentNode, int startNode, int endNode) {
            if (currentNode == 0) {
                return 6;
            }
            if (IsCornerNode(currentNode, startNode, endNode)) {
                return 2;
            } else {
                return 1;
            }
        }

        private bool IsCornerNode(int currentNode, int startNode, int endNode) {
            int range = endNode - (startNode - 1);
            if (range == 0) {
                return false;
            }

            int cornerDistance = range / 6;
            if ((currentNode - startNode) % cornerDistance == 0) {
                return true;
            }
            return false;
        }

        private void DrawTriangle(DynamicBuffer<Triangle> triangles, int currentNode, int vertex1, int vertex2) {
            triangles.Add(currentNode);
            triangles.Add(vertex1);
            triangles.Add(vertex2);
        }
    }
}
