using Assets.Scripts.Components;
using Unity.Mathematics;

public class Noise
{
    public static float Evaluate(float2 position, NoiseSettings settings)
    {
        float height = 0;
        float amplitude = settings.amplitude;
        float frequency = settings.frequency;
        for (int i = 0; i < settings.octaves; i++)
        {
            height += noise.snoise(position * frequency) * amplitude;
            amplitude *= settings.amplitudeScale;
            frequency *= settings.frequencyScale;
        }
        return height;
    }
}
