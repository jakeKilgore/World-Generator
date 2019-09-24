using Assets.Scripts.Settings;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Components
{
    public struct NoiseSettings : IComponentData
    {
        public float amplitude;
        public float frequency;

        public NoiseSettings(NoiseEditorSettings noiseSettings)
        {
            amplitude = noiseSettings.amplitude;
            frequency = noiseSettings.frequency;
        }

        public float Evaluate(float2 position)
        {
            return noise.snoise(position * frequency) * amplitude;
        }
    }
}
