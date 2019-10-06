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
            typeof(HexCoordinates),
            typeof(IsTile),
            typeof(LocalToWorld),
            typeof(Neighbors),
            typeof(RenderMesh),
            typeof(Translation),
            typeof(TrianglePoint),
            typeof(UV),
            typeof(Vertex)
        );

        /// <summary>   Generates a tile given a given noise filter. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="coordinates">      A filter specifying the noise. </param>
        /// <param name="groundMaterial">   The ground material. </param>
        public static bool Generate(HexCoordinates coordinates, Material groundMaterial, NativeHashMap<int3, Entity> tiles)
        {
            if (tiles.ContainsKey(coordinates))
            {
                return false;
            }
            Entity tile = entityManager.CreateEntity(archetype);
            entityManager.SetComponentData(tile, coordinates);
            entityManager.SetSharedComponentData(tile, new RenderMesh {
                mesh = new Mesh(),
                material = groundMaterial,
                receiveShadows = true,
                castShadows = UnityEngine.Rendering.ShadowCastingMode.On
            });
            entityManager.SetName(tile, "Tile: " + coordinates.ToString());
            AddNeighbors(tile, coordinates, tiles);
            tiles.TryAdd(coordinates, tile);
            return true;
        }

        public static void AddNeighbors(Entity tile, int3 coordinates, NativeHashMap<int3, Entity> tiles)
        {
            Entity east = FindNeighbor(Direction.East, tile, coordinates, tiles);
            Entity north = FindNeighbor(Direction.North, tile, coordinates, tiles);
            Entity northWest = FindNeighbor(Direction.NorthWest, tile, coordinates, tiles);
            Entity south = FindNeighbor(Direction.South, tile, coordinates, tiles);
            Entity southEast = FindNeighbor(Direction.SouthEast, tile, coordinates, tiles);
            Entity west = FindNeighbor(Direction.West, tile, coordinates, tiles);
            entityManager.SetComponentData(tile, new Neighbors(east, north, northWest, south, southEast, west));
        }

        public static Entity FindNeighbor(Direction direction, Entity tile, HexCoordinates coordinates, NativeHashMap<int3, Entity> tiles)
        {
            Entity neighbor;
            int3 vector = Directions.direction[(int)direction];
            if (tiles.TryGetValue(coordinates + vector, out neighbor))
            {
                AddTileToNeighbor(tile, neighbor, direction);
                return neighbor;
            }
            else
            {
                return Entity.Null;
            }
        }

        public static void AddTileToNeighbor(Entity tile, Entity neighbor, Direction direction)
        {
            Neighbors neighbors = entityManager.GetComponentData<Neighbors>(neighbor);
            if (direction is Direction.East)
            {
                neighbors.west = tile;
            }
            else if (direction is Direction.North)
            {
                neighbors.south = tile;
            }
            else if (direction is Direction.NorthWest)
            {
                neighbors.southEast = tile;
            }
            else if (direction is Direction.South)
            {
                neighbors.north = tile;
            }
            else if (direction is Direction.SouthEast)
            {
                neighbors.northWest = tile;
            }
            else if (direction is Direction.West)
            {
                neighbors.east = tile;
            }
            entityManager.SetComponentData(neighbor, neighbors);
        }
    }
}
