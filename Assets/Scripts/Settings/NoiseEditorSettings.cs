// file:	Assets\Scripts\Settings\NoiseEditorSettings.cs
//
// summary:	Implements the noise editor settings class
using UnityEngine;

namespace Assets.Scripts.Settings
{
    /// <summary>   The settings for the noise in the map for changing in the editor. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [CreateAssetMenu()]
    public class NoiseEditorSettings : Setting
    {
        /// <summary>   The amplitude of the noise. </summary>
        [Range(0, 2)]
        public float amplitude;
        /// <summary>   The frequency of the noise. </summary>
        [Range(0, 2)]
        public float frequency;
        /// <summary>   The octaves. This is how many layers of noise will be applied to the map. </summary>
        [Range(0, 8)]
        public int ocataves;

        /// <summary>
        /// The amplitude scale. This is how much the amplitude will change for each layer of noise.
        /// </summary>
        [Range(0.0001f, .75f)]
        public float amplitudeScale = .5f;

        /// <summary>
        /// The frequency scale. This is how much the amplitude will change for each layer of noise.
        /// </summary>
        [Range(1.0001f, 2)]
        public float frequencyScale = 2f;
    }
}
