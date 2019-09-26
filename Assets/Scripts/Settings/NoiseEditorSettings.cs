using UnityEngine;

namespace Assets.Scripts.Settings
{
    [CreateAssetMenu()]
    public class NoiseEditorSettings : Setting
    {
        [Range(0, 2)]
        public float amplitude;
        [Range(0, 2)]
        public float frequency;
        [Range(0, 8)]
        public int ocataves;
        [Range(0.0001f, .75f)]
        public float amplitudeScale = .5f;
        [Range(1.0001f, 2)]
        public float frequencyScale = 2f;
    }
}
