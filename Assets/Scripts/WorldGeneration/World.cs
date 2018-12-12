using Maps;
using Noise;
using Settings;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace WorldGeneration {
    /// <summary>
    /// The object used for world generation.
    /// </summary>
    public class World {
        public readonly MapSettings mapSettings;
        private readonly Dictionary<int, Map> layers;
        private Tile tile;

        public Tile Tile {
            get {
                return tile;
            }
            set {
                if(value == null) {
                    throw new ArgumentNullException("Tile cannot be null.");
                }
                tile = value;
            }
        }

        public Dictionary<int, Map> Layers {
            get {
                return layers;
            }
        }

        public World(MapSettings mapSettings) {
            this.mapSettings = mapSettings;
            layers = new Dictionary<int, Map>();
        }

        public void Generate() {
            tile = new Tile(Directions.O, null, this, mapSettings.maxLayer);
            tile.GenerateMap();
        }
    }
}