// file:	Assets\Scripts\Components\NoiseSettings.cs
//
// summary:	Implements the noise settings class
using Assets.Scripts.Settings;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Components
{
    /// <summary>   A noise settings. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    public struct NoiseSettings : IComponentData
    {
        /// <summary>   The amplitude. </summary>
        public float amplitude;
        /// <summary>   The frequency. </summary>
        public float frequency;
        /// <summary>   The octaves. </summary>
        public int octaves;
        /// <summary>   The amplitude scale. </summary>
        public float amplitudeScale;
        /// <summary>   The frequency scale. </summary>
        public float frequencyScale;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="noiseSettings">    The noise settings. </param>
        public NoiseSettings(NoiseEditorSettings noiseSettings)
        {
            amplitude = noiseSettings.amplitude;
            frequency = noiseSettings.frequency;
            octaves = noiseSettings.ocataves;
            amplitudeScale = noiseSettings.amplitudeScale;
            frequencyScale = noiseSettings.frequencyScale;
        }
    }
}
