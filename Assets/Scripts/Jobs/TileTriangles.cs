using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Burst;
using UnityEngine;
using Unity.Collections;
using UnityEngine.Assertions;
using System;

/* The formula for finding the number of vertices in a hexagon given a number of layers is:
 * v = 3n(n + 1) + 1, where n = the number of layers.
 * 
 * The formula for finding the number of vertices in a given layer is:
 * v = 6 * n, where n = the current layer.
 * 
 * The formula for finding the number of triangles in a hexagon given a number of layers is:
 * t = n(n + 1)(n + 2), where n = the number of layers.
 * 
 * The formula for finding the number of triangles in the current layer is:
 * t = 3n(n + 1), where n = the number of layers.
 */

[BurstCompile]
public struct TileTriangles : IJob {
    [WriteOnly] public NativeArray<int> drawTriangles;
    public int numLayers;

    public TileTriangles(NativeArray<int> drawTriangles, int numLayers) {
        this.drawTriangles = drawTriangles;
        this.numLayers = numLayers;
    }

    public void Execute() {
        if (!drawTriangles.IsCreated) {
            throw new ArgumentException("A native array for drawing triangles must be provided.");
        }
        if (numLayers <= 0) {
            throw new ArgumentException("The number of layers must be provided and must be greater than 0.");
        }
        if (drawTriangles.Length != AllocationSpaceForDrawTrianglesArray(numLayers)) {
            throw new ArgumentException(
                "The number of layers and the size of the triangle drawing array do not match. " +
                "Use the AllocationSpaceForDrawTrianglesArray(numLayers) function."
            );
        }

        int drawTrianglesIndex = 0;
        for (int currentLayer = 1; currentLayer <= numLayers; currentLayer++) {
            drawTrianglesIndex += DrawLayer(currentLayer, drawTrianglesIndex);
        }
    }

    private int DrawLayer(int layer, int drawTrianglesIndex) {
        int startNode = HexMath.CheckVerticesInHex(layer - 2) - 1;
        int endNode = HexMath.CheckVerticesInHex(layer - 1) - 1;
        if (layer != 1) {
            startNode++;
        }

        int startVertex = endNode + 1;
        int endVertex = HexMath.CheckVerticesInHex(layer) - 1;
        
        int trianglesDrawn = 0;
        int currentVertex = startVertex;
        for (int currentNode = startNode; currentNode <= endNode; currentNode++) {
            int trianglesPerNode = TrianglesPerNode(currentNode, startNode, endNode);
            for (int nodeTriangles = 0; nodeTriangles < trianglesPerNode; nodeTriangles++) {
                if (layer != 1 && currentNode == startNode && nodeTriangles == 0) {
                    continue;
                }
                int vertex1 = currentVertex;
                int vertex2 = currentVertex + 1;

                if (vertex2 > endVertex) {
                    vertex2 = layer == 1 ? startVertex : startNode;
                }

                trianglesDrawn += DrawTriangle(currentNode, vertex1, vertex2, drawTrianglesIndex + trianglesDrawn);

                currentVertex++;
            }
            if (layer != 1) {
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

    private int TrianglesPerNode(int node, int startNode, int endNode) {
        if (node == 0) {
            return 6;
        }
        if (IsCornerNode(node, startNode, endNode)) {
            return 2;
        }
        else {
            return 1;
        }
    }

    private bool IsCornerNode(int node, int startNode, int endNode) {
        int range = endNode - (startNode - 1);
        if (range == 0) {
            return false;
        }

        int cornerDistance = range / 6;
        if ((node - startNode) % cornerDistance == 0) {
            return true;
        }
        return false;
    }

    private int DrawTriangle(int node, int vertex1, int vertex2, int index) {
        drawTriangles[index] = node;
        drawTriangles[index + 1] = vertex1;
        drawTriangles[index + 2] = vertex2;
        return 1;
    }

    public static int AllocationSpaceForDrawTrianglesArray(int numLayers) {
        return 3 * HexMath.CheckTrianglesInHex(numLayers);
    }
}
