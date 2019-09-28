////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\HexMath.cs
//
// summary:	Implements the hexadecimal mathematics class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Assets.Scripts.Components;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace Assets.Scripts {

    /// <summary>   Mathematics for the hexagonal tile implementation. </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    public static class HexMath {

        /// <summary>   The width multiple. </summary>
        public static readonly float WidthMultiple = math.sqrt(3);
        /// <summary>   The height multiple. </summary>
        public static readonly float HeightMultiple = 2F;

        /// <summary>   Returns the location of the hex in the worldspace. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="coordinate">   The coordinate. </param>
        ///
        /// <returns>   A float2 position in the worldspace. </returns>
        public static float2 Position(HexCoordinates coordinate)
        {
            float height = HeightMultiple;
            float width = WidthMultiple;
            float posX = WidthMultiple * (coordinate.Column + (coordinate.Row / 2f));
            float posY = HeightMultiple * 3 / 4 * coordinate.Row;

            return new float2(posX, posY);
        }

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
}
