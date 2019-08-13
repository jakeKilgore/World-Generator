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
public struct TileVertices : IJob {
    [WriteOnly] public NativeArray<Vector3> vertices;
    public int totalRings;
    public float2 position;
    public NoiseFilter noiseFilter;

    public TileVertices(NativeArray<Vector3> vertices, int totalRings, float2 position, NoiseFilter noiseFilter) {
        this.vertices = vertices;
        this.totalRings = totalRings;
        this.position = position;
        this.noiseFilter = noiseFilter;
    }

    public void Execute() {
        if (!vertices.IsCreated) {
            throw new ArgumentException("A native array for storing vertices must be provided.");
        }
        if (totalRings <= 0) {
            throw new ArgumentException("The number of layers must be provided and must be greater than 0.");
        }
        if (vertices.Length != AllocationSpaceForVertexArray(totalRings)) {
            throw new ArgumentException(
                "The number of layers and the size of the vertex array do not match. " +
                "Use the AllocationSpaceForVertexArray(numLayers) function."
            );
        }

        vertices[0] = DrawVertex(position);
        int vertexIndex = 1;
        for (int currentRing = 1; currentRing <= totalRings; currentRing++) {
            vertexIndex = DrawLayer(currentRing, vertexIndex);
        }
    }

    private int DrawLayer(int currentRing, int vertexIndex) {
        int verticesInRing = HexMath.CheckVerticesInLayer(currentRing);
        float arcBetweenPoints = FindArc(verticesInRing);
        float angle = math.PI / 2;
        for (int i = vertexIndex; i < vertexIndex + verticesInRing; i++) {
            float distanceMultiple = FindDistance(currentRing, angle);
            float2 point = FindPoint(angle, distanceMultiple);
            vertices[i] = DrawVertex(point);
            angle += arcBetweenPoints;
        }
        return vertexIndex + verticesInRing;
    }

    private float FindArc(int verticesInRing) {
        return -2 * math.PI / verticesInRing;
    }

    private float FindDistance(int currentRing, float angle) {
        float hypotenuse = (float)currentRing / totalRings;
        float interiorAngle = math.radians(30);
        angle = (2 * math.PI + angle) % math.radians(60);
        if (angle > interiorAngle) {
            angle = math.radians(60) - angle;
        }
        return hypotenuse * math.cos(interiorAngle) / math.cos(angle);
    }

    private float2 FindPoint(float angle, float distance) {
        float pointX = position.x + distance * math.cos(angle);
        float pointY = position.y + distance * math.sin(angle);
        return new float2(pointX, pointY);
    }

    private Vector3 DrawVertex(float2 point) {
        return new Vector3(point.x, noiseFilter.Evaluate(point), point.y);
    }

    public static int AllocationSpaceForVertexArray(int numRings) {
        return HexMath.CheckVerticesInHex(numRings);
    }
}
