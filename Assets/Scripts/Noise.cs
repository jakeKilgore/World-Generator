// file:	Assets\Scripts\Noise.cs
//
// summary:	Implements the noise class
using Assets.Scripts.Components;
using Unity.Mathematics;

/// <summary>   A noise. </summary>
///
/// <remarks>   The Vitulus, 9/28/2019. </remarks>
public class Noise
{
    /// <summary>   Evaluates the height at a point on a 2D plane in the world. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    ///
    /// <param name="position"> The position. </param>
    /// <param name="settings"> Options for controlling the operation. </param>
    ///
    /// <returns>   The height at the position. </returns>
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
