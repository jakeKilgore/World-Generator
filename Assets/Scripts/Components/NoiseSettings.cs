﻿// file:	Assets\Scripts\Components\NoiseSettings.cs
//
// summary:	Implements the noise settings class
using Assets.Scripts.Settings;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Components
{
    /// <summary>   The noise settings for the world map. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    public struct NoiseSettings : IComponentData
    {
        /// <summary>   The amplitude of the noise. </summary>
        public float amplitude;
        /// <summary>   The frequency of the noise. </summary>
        public float frequency;
        /// <summary>   The octaves. This is how many layers of noise will be applied to the map. </summary>
        public int octaves;

        /// <summary>
        /// The amplitude scale. This is how much the amplitude will change for each layer of noise.
        /// </summary>
        public float amplitudeScale;

        /// <summary>
        /// The frequency scale. This is how much the amplitude will change for each layer of noise.
        /// </summary>
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
