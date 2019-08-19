////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\NoiseFilter.cs
//
// summary:	Implements the noise filter class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

namespace Assets.Scripts {

    /// <summary>   A noise filter. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    public struct NoiseFilter {

        /// <summary>   Evaluates the given coordinates. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="coords">   The coordinates. </param>
        ///
        /// <returns>   The height value at the given coordinates. </returns>
        public float Evaluate(float2 coords) {
            return noise.snoise(coords / 2);
        }
    }
}
