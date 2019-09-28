// file:	Assets\Scripts\Settings\NoiseEditorSettings.cs
//
// summary:	Implements the noise editor settings class
using UnityEngine;

namespace Assets.Scripts.Settings
{
    /// <summary>   A noise editor settings. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [CreateAssetMenu()]
    public class NoiseEditorSettings : Setting
    {
        /// <summary>   The amplitude. </summary>
        [Range(0, 2)]
        public float amplitude;
        /// <summary>   The frequency. </summary>
        [Range(0, 2)]
        public float frequency;
        /// <summary>   The ocataves. </summary>
        [Range(0, 8)]
        public int ocataves;
        /// <summary>   The amplitude scale. </summary>
        [Range(0.0001f, .75f)]
        public float amplitudeScale = .5f;
        /// <summary>   The frequency scale. </summary>
        [Range(1.0001f, 2)]
        public float frequencyScale = 2f;
    }
}
