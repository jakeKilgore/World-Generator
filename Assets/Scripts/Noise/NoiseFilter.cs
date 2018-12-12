using Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Noise {
    public class NoiseFilter {
        OpenSimplexNoise noise;
        TerrainSettings settings;

        public NoiseFilter(TerrainSettings settings) {
            this.settings = settings;
            noise = new OpenSimplexNoise();
        }

        public float Evaluate(float x, float y) {
            float noiseValue = 0;
            float frequency = settings.baseRoughness;
            float amplitude = settings.baseStrength;
            for(int i = 0; i < settings.noiseLayers; i++) {
                noiseValue += (float)noise.Evaluate(x * frequency, y * frequency) * amplitude;
                frequency *= settings.roughnessScale;
                amplitude *= settings.strengthScale;
            }
            return noiseValue;
        }

        public void ApplyNoise(Vector3[] vertices, Vector3 origin) {
            for (int i = 0; i < vertices.Length; i++) {
                vertices[i].y += Evaluate(origin.x + vertices[i].x, origin.z + vertices[i].z);
            }
        }
    }
}
