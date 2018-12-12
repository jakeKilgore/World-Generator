using System;
using Maps;
using UnityEngine;
using Settings;

namespace WorldGeneration {
    public class WorldGenerator : MonoBehaviour {
        public MapSettings mapSettings;
        public TerrainSettings terrainSettings;
        public World world;
        public TerrainGenerator terrainGenerator;

        void Start() {
            world = new World(mapSettings);
            world.Generate();

            terrainGenerator = new TerrainGenerator(terrainSettings);
            terrainGenerator.Generate(world.Tile);
        }
    }
}