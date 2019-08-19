////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Jobs\TileTriangles.cs
//
// summary:	Implements the tile triangles class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Assertions;
using System;

namespace Assets.Scripts.Jobs {
    /// <summary>   A job for generating the triangles for a hexagonal tile mesh. </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    [BurstCompile]
    public struct TileTriangles : IJob {

        /// <summary>   A native array to fill with the instructions for drawing the triangles. </summary>
        [WriteOnly] public NativeArray<int> drawTrianglesArray;
        /// <summary>   Number of rings of triangles around the center of the hex. </summary>
        public int totalRings;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="drawTrianglesArray">   A native array to fill with the instructions for drawing
        ///                                     the triangles. </param>
        /// <param name="totalRings">           Number of rings of triangles around the center of the
        ///                                     hex. </param>
        public TileTriangles(NativeArray<int> drawTrianglesArray, int totalRings) {
            this.drawTrianglesArray = drawTrianglesArray;
            this.totalRings = totalRings;
        }

        /// <summary>   Execute the job, filling the draw triangles array. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        public void Execute() {
            if (!drawTrianglesArray.IsCreated) {
                throw new ArgumentException("A native array for drawing triangles must be provided.");
            }
            if (totalRings <= 0) {
                throw new ArgumentException("The number of layers must be provided and must be greater than 0.");
            }
            if (drawTrianglesArray.Length != AllocationSpaceForDrawTrianglesArray(totalRings)) {
                throw new ArgumentException(
                    "The number of layers and the size of the triangle drawing array do not match. " +
                    "Use the AllocationSpaceForDrawTrianglesArray(numLayers) function."
                );
            }

            int drawTrianglesIndex = 0;
            for (int currentLayer = 1; currentLayer <= totalRings; currentLayer++) {
                drawTrianglesIndex += DrawRing(currentLayer, drawTrianglesIndex);
            }
        }

        /// <summary>   Draw the ring of triangles. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="currentRing">          The current ring. </param>
        /// <param name="drawTrianglesIndex">   Zero-based index of the draw triangles array. </param>
        ///
        /// <returns>   The space used in the draw triangles array. </returns>
        private int DrawRing(int currentRing, int drawTrianglesIndex) {
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

                    trianglesDrawn += DrawTriangle(currentNode, vertex1, vertex2, drawTrianglesIndex + trianglesDrawn);

                    currentVertex++;
                }
                if (currentRing != 1) {
                    int vertex1 = currentVertex;
                    int vertex2 = currentNode + 1;
                    if (vertex2 > endNode) {
                        vertex2 = startNode;
                    }

                    trianglesDrawn += DrawTriangle(currentNode, vertex1, vertex2, drawTrianglesIndex + trianglesDrawn);

                    if (currentNode == endNode) {
                        trianglesDrawn += DrawTriangle(startNode, endVertex, startVertex, drawTrianglesIndex + trianglesDrawn);
                    }
                }
            }

            return trianglesDrawn;
        }

        /// <summary>   The number of triangles a given node should draw. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="currentNode">  The current node. </param>
        /// <param name="startNode">    The start node. </param>
        /// <param name="endNode">      The end node. </param>
        ///
        /// <returns>   The number of triangles to draw. </returns>
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

        /// <summary>   Query if 'currentNode' is one of the six corners of the current ring. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="currentNode">  The current node. </param>
        /// <param name="startNode">    The start node. </param>
        /// <param name="endNode">      The end node. </param>
        ///
        /// <returns>   True if corner node, false if not. </returns>
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

        /// <summary>   Draw the current triangle. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="currentNode">  The current node. </param>
        /// <param name="vertex1">      The first vertex. </param>
        /// <param name="vertex2">      The second vertex. </param>
        /// <param name="index">        Zero-based index of the. </param>
        ///
        /// <returns>   The number of indices in the draw triangles array used. </returns>
        private int DrawTriangle(int currentNode, int vertex1, int vertex2, int index) {
            drawTrianglesArray[index] = currentNode;
            drawTrianglesArray[index + 1] = vertex1;
            drawTrianglesArray[index + 2] = vertex2;
            return 3;
        }

        /// <summary>   Allocation space for draw triangles array. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="totalRings">   Number of rings in the hexagon. </param>
        ///
        /// <returns>   The space required for the array. </returns>
        public static int AllocationSpaceForDrawTrianglesArray(int totalRings) {
            return 3 * HexMath.CheckTrianglesInHex(totalRings);
        }
    }
}
