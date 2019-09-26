using Assets.Scripts.Settings;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Components
{
    public struct NoiseSettings : IComponentData
    {
        public float amplitude;
        public float frequency;
        public int octaves;
        public float amplitudeScale;
        public float frequencyScale;

        public NoiseSettings(NoiseEditorSettings noiseSettings)
        {
            amplitude = noiseSettings.amplitude;
            frequency = noiseSettings.frequency;
            octaves = noiseSettings.ocataves;
            amplitudeScale = noiseSettings.amplitudeScale;
            frequencyScale = noiseSettings.frequencyScale;
        }

        public float Evaluate(float2 position)
        {
            float height = 0;
            float curAmplitude = amplitude;
            float curFrequency = frequency;
            for (int i = 0; i < octaves; i++)
            {
                height += noise.snoise(position * curFrequency) * curAmplitude;
                curAmplitude *= amplitudeScale;
                curFrequency *= frequencyScale;
            }
            return height;
        }
    }
}
