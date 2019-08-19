////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\WorldGenerator.cs
//
// summary:	Implements the world generator class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Assets.Scripts.Entities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts {

    /// <summary>   A world generator. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    public class WorldGenerator : MonoBehaviour {

        /// <summary>   Starts this object. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        void Start() {
            NoiseFilter noiseFilter = new NoiseFilter();
            TileEntityFactory.Generate(noiseFilter);
        }
    }
}
