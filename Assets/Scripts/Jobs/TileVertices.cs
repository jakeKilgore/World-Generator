////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Jobs\TileVertices.cs
//
// summary:	Implements the tile vertices class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Entities;
using UnityEngine;

/// <summary>   A job for generating the vertices for a hexagonal tile mesh. </summary>
///
/// <remarks>   The Vitulus, 8/15/2019. </remarks>
[BurstCompile]
public struct GenerateTileVertices : IJob {

    /// <summary>   An array to fill with vertices. </summary>
    [WriteOnly] public NativeArray<Vector3> verticesArray;
    /// <summary>   The total rings. </summary>
    public int totalRings;
    /// <summary>   The position of the hex in the world space. </summary>
    public float2 position;
    /// <summary>   A filter specifying the noise. </summary>
    public NoiseFilter noiseFilter;

    /// <summary>   Constructor. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="verticesArray">    An array to fill with vertices. </param>
    /// <param name="totalRings">       The total rings. </param>
    /// <param name="position">         The position of the hex in the world space. </param>
    /// <param name="noiseFilter">      A filter specifying the noise. </param>
    public GenerateTileVertices(NativeArray<Vector3> verticesArray, int totalRings, float2 position, NoiseFilter noiseFilter) {
        this.verticesArray = verticesArray;
        this.totalRings = totalRings;
        this.position = position;
        this.noiseFilter = noiseFilter;
    }

    /// <summary>   Execute the job, filling the vertices array. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
    ///                                         illegal values. </exception>
    public void Execute() {
        if (!verticesArray.IsCreated) {
            throw new ArgumentException("A native array for storing vertices must be provided.");
        }
        if (totalRings <= 0) {
            throw new ArgumentException("The number of layers must be provided and must be greater than 0.");
        }
        if (verticesArray.Length != AllocationSpaceForVertexArray(totalRings)) {
            throw new ArgumentException(
                "The number of layers and the size of the vertex array do not match. " +
                "Use the AllocationSpaceForVertexArray(numLayers) function."
            );
        }

        verticesArray[0] = DrawVertex(position);
        int vertexIndex = 1;
        for (int currentRing = 1; currentRing <= totalRings; currentRing++) {
            vertexIndex = DrawRing(currentRing, vertexIndex);
        }
    }

    /// <summary>   Draw the ring of vertices. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="currentRing">  The current ring. </param>
    /// <param name="vertexIndex">  Zero-based index of the vertex array. </param>
    ///
    /// <returns>   The space used in the vertex array. </returns>
    private int DrawRing(int currentRing, int vertexIndex) {
        int verticesInRing = HexMath.CheckVerticesInLayer(currentRing);
        float arcBetweenPoints = FindArc(verticesInRing);
        float angle = math.PI / 2;
        for (int i = vertexIndex; i < vertexIndex + verticesInRing; i++) {
            float distanceMultiple = FindDistance(currentRing, angle);
            float2 point = FindPoint(angle, distanceMultiple);
            verticesArray[i] = DrawVertex(point);
            angle += arcBetweenPoints;
        }
        return vertexIndex + verticesInRing;
    }

    /// <summary>   Searches for the arc between vertices in a given ring. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="verticesInRing">   The number of vertices in the current ring. </param>
    ///
    /// <returns>   The arc between vertices. </returns>
    private float FindArc(int verticesInRing) {
        return -2 * math.PI / verticesInRing;
    }

    /// <summary>   Searches for the distance from the center to the current vertex. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="currentRing">  The current ring. </param>
    /// <param name="angle">        The angle of the vertex. </param>
    ///
    /// <returns>   The distance from the center to the current vertex. </returns>
    private float FindDistance(int currentRing, float angle) {
        float hypotenuse = (float)currentRing / totalRings;
        float interiorAngle = math.radians(30);
        angle = (2 * math.PI + angle) % math.radians(60);
        if (angle > interiorAngle) {
            angle = math.radians(60) - angle;
        }
        return hypotenuse * math.cos(interiorAngle) / math.cos(angle);
    }

    /// <summary>   Searches for a point, given an angle and a distance. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="angle">    The angle of the point. </param>
    /// <param name="distance"> The distance to the point. </param>
    ///
    /// <returns>   The point. </returns>
    private float2 FindPoint(float angle, float distance) {
        float pointX = position.x + distance * math.cos(angle);
        float pointY = position.y + distance * math.sin(angle);
        return new float2(pointX, pointY);
    }

    /// <summary>   Draw a vertex. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="point">    The point. </param>
    ///
    /// <returns>   The vertex as a Vector3. </returns>
    private Vector3 DrawVertex(float2 point) {
        return new Vector3(point.x, noiseFilter.Evaluate(point), point.y);
    }

    /// <summary>   Allocation space for vertex array. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="numRings"> Number of rings in the hexagon. </param>
    ///
    /// <returns>   The space required for the vertex array. </returns>
    public static int AllocationSpaceForVertexArray(int numRings) {
        return HexMath.CheckVerticesInHex(numRings);
    }
}
