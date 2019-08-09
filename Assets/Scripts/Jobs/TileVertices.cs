using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;

[BurstCompile]
public struct TileVertices : IJobForEachWithEntity<Coordinates> {
    [WriteOnly] public NativeArray<Vector3> vertices;
    public int numLayers;

    public void Execute(Entity entity, int index, ref Coordinates coordinates) {
        if (!vertices.IsCreated) {
            throw new ArgumentException("A native array for storing vertices must be provided.");
        }
        if (numLayers <= 0) {
            throw new ArgumentException("The number of layers must be provided and must be greater than 0.");
        }
        if (vertices.Length != AllocationSpaceForVertexArray(numLayers)) {
            throw new ArgumentException(
                "The number of layers and the size of the vertex array do not match. " +
                "Use the AllocationSpaceForVertexArray(numLayers) function."
            );
        }

        int vertexIndex = 0;
        vertexIndex += DrawVertex(0, 0, 0);
        for (int currentLayer = 1; currentLayer <= numLayers; currentLayer++) {
            vertexIndex += DrawLayer(currentLayer, vertexIndex);
        }
    }

    private int DrawLayer(int layer, int vertexIndex) {
        return 0;
    }

    private int DrawVertex(int x, int y, int z) {
        return 1;
    }

    public static int AllocationSpaceForVertexArray(int numLayers) {
        return HexMath.CheckVerticesInHex(numLayers);
    }
}
