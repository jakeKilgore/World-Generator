using Maps;
using System;
using System.Linq;
using UnityEngine;

namespace Settings {
    public class TerrainSettings : Settings {
        [Range(0, 2)]
        public float baseStrength = 1f;
        [Range(0, 2)]
        public float baseRoughness = 1f;
        [Range(0, 8)]
        public int noiseLayers = 1;
        [Range(0.0001f, .75f)]
        public float strengthScale = .5f;
        [Range(1.0001f, 2)]
        public float roughnessScale = 2f;

        [Range(0, 4)]
        public int renderLayer = 0;
        [Range(2, 10)]
        public int resolution = 0;
    }
}