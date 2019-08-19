////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Entities\TileEntityFactory.cs
//
// summary:	Implements the tile entity factory class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using System;
using Assets.Scripts.Components;
using Assets.Scripts.Components.Flags;

namespace Assets.Scripts.Entities {

    /// <summary>   A factory class for generating tile entities. </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    public static class TileEntityFactory {

        /// <summary>   The world's entity manager. </summary>
        private static EntityManager entityManager = World.Active.EntityManager;

        /// <summary>   The tile archetype. </summary>
        private static EntityArchetype archetype = entityManager.CreateArchetype(
        typeof(Translation),
        typeof(RenderMesh),
        typeof(LocalToWorld),
        typeof(HexCoordinates),
        typeof(IsTile)
    );

        /// <summary>   Generates a tile given a given noise filter. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="noiseFilter">  A filter specifying the noise. </param>
        public static void Generate(NoiseFilter noiseFilter) {
            Entity tile = entityManager.CreateEntity(archetype);
        }
    }
}
