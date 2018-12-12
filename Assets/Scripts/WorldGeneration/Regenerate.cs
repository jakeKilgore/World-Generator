using Maps;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WorldGeneration;

public static class Regenerate {

    public static void Map(World world) {
        if (world == null) {
            return;
        }
        while (world.Tile.Layer > world.mapSettings.maxLayer) {
            RemoveLayer(world);
        }
        while (world.Tile.Layer < world.mapSettings.maxLayer) {
            AddLayer(world);
        }
    }

    public static void Terrain(World world) {
        if (world == null) {
            return;
        }
    }

    private static void AddLayer(World world) {
        if (world.Tile == null) {
            throw new ArgumentException("World tile is null");
        }
        Tile tile = world.Tile;
        int layer = tile.Layer;
        Tile parent = new Tile(Directions.O, null, world, layer + 1);
        parent.GenerateMap(tile);
        world.Tile = parent;
    }

    private static void RemoveLayer(World world) {
        if (world.Tile == null) {
            throw new InvalidOperationException("World tile is null.");
        }
        Tile tile = world.Tile;
        int layer = tile.Layer;
        if (layer == 0) {
            throw new InvalidOperationException("No layer to remove.");
        }
        foreach (Coordinates direction in new Directions()) {
            if (!tile.ContainsKey(direction)) {
                continue;
            }
            ((Tile)tile[direction]).Delete();
        }
        world.Tile = (Tile)tile[Directions.O];
        world.Layers.Remove(layer);
    }
}
