////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\HexMath.cs
//
// summary:	Implements the hexadecimal mathematics class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>   Mathematics for the hexagonal tile implementation. </summary>
///
/// <remarks>   The Vitulus, 8/15/2019. </remarks>
public static class HexMath {

    /// <summary>   Check the number of vertices in the hexagon. </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    ///
    /// <param name="totalRings">   The number of rings in the hexagon. </param>
    ///
    /// <returns>   The number of vertices in the hexagon. </returns>
    public static int CheckVerticesInHex(int totalRings) {
        return 3 * totalRings * (totalRings + 1) + 1;
    }

    /// <summary>   Check the number of vertices in the given ring. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="currentRing">  The current ring. </param>
    ///
    /// <returns>   The number of vertices in the given ring. </returns>
    public static int CheckVerticesInLayer(int currentRing) {
        return 6 * currentRing;
    }

    /// <summary>   Check the number of triangles in the hexagon. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="totalLayers">  The number of rings in the hexagon. </param>
    ///
    /// <returns>   The number of triangles in the hexagon. </returns>
    public static int CheckTrianglesInHex(int totalLayers) {
        return totalLayers * (totalLayers + 1) * (totalLayers + 2);
    }

    /// <summary>   Check the number of triangles in the given ring. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    ///
    /// <param name="currentring">  The current ring. </param>
    ///
    /// <returns>   The number of triangles in the given ring. </returns>
    public static int CheckTrianglesInLayer(int currentring) {
        return 3 * currentring * (currentring + 1);
    }
}
