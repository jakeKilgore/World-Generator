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
    }
}