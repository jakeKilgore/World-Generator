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
    public struct GenerateTrianglesBuffer : IJobForEachWithEntity<HexCoordinates>
    {
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Triangle> entityBuffers;
        readonly int numRings;

        public GenerateTrianglesBuffer(BufferFromEntity<Triangle> entityBuffers, int numRings) {
            this.entityBuffers = entityBuffers;
            this.numRings = numRings;
        }

        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates c0) {
            DynamicBuffer<Triangle> triangles = entityBuffers[entity];
            triangles.Clear();
            int triangleIndex = 0;
            for (int currentLayer = 1; currentLayer <= numRings; currentLayer++) {
                triangleIndex += DrawRing(triangles, currentLayer, triangleIndex);
            }
        }

        private int DrawRing(DynamicBuffer<Triangle> triangles, int currentRing, int triangleIndex) {
            int startNode = HexMath.CheckVerticesInHex(currentRing - 2) - 1;
            int endNode = HexMath.CheckVerticesInHex(currentRing - 1) - 1;
            if (currentRing != 1) {
                startNode++;
            }

            int startVertex = endNode + 1;
            int endVertex = HexMath.CheckVerticesInHex(currentRing) - 1;

            int trianglesDrawn = 0;
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

                    trianglesDrawn += DrawTriangle(triangles, currentNode, vertex1, vertex2);

                    currentVertex++;
                }
                if (currentRing != 1) {
                    int vertex1 = currentVertex;
                    int vertex2 = currentNode + 1;
                    if (vertex2 > endNode) {
                        vertex2 = startNode;
                    }

                    trianglesDrawn += DrawTriangle(triangles, currentNode, vertex1, vertex2);

                    if (currentNode == endNode) {
                        trianglesDrawn += DrawTriangle(triangles, startNode, endVertex, startVertex);
                    }
                }
            }

            return trianglesDrawn;
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

        private int DrawTriangle(DynamicBuffer<Triangle> triangles, int currentNode, int vertex1, int vertex2) {
            triangles.Add(currentNode);
            triangles.Add(vertex1);
            triangles.Add(vertex2);
            return 3;
        }

        private int AllocationSpaceForDrawTrianglesArray(int numRings) {
            return 3 * HexMath.CheckTrianglesInHex(numRings);
        }
    }
}
