﻿////////////////////////////////////////////////////////////////////////////////////////////////////
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
using Assets.Scripts.Components.BufferElements;
using Assets.Scripts.Systems.Render;
using Assets.Scripts.Settings;

namespace Assets.Scripts.Entities
{

    /// <summary>   A factory class for generating tile entities. </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    public static class Tile
    {
        /// <summary>   The world's entity manager. </summary>
        private static readonly EntityManager entityManager = World.Active.EntityManager;

        /// <summary>   The tile archetype. </summary>
        private static EntityArchetype archetype = entityManager.CreateArchetype(
            typeof(Translation),
            typeof(RenderMesh),
            typeof(LocalToWorld),
            typeof(HexCoordinates),
            typeof(IsTile),
            typeof(Triangle),
            typeof(Vertex),
            typeof(NumRings)
        );

        /// <summary>   Generates a tile given a given noise filter. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="noiseFilter">  A filter specifying the noise. </param>
        public static void Generate(HexCoordinates coordinates, MapSettings mapSettings)
        {
            Entity tile = entityManager.CreateEntity(archetype);
            entityManager.SetComponentData(tile, coordinates);
            entityManager.SetSharedComponentData(tile, new RenderMesh {
                mesh = new Mesh()
            });
            entityManager.SetComponentData(tile, new NumRings(mapSettings.numRings));
        }
    }
}