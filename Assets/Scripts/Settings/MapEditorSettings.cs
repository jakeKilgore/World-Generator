using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [CreateAssetMenu()]
    public class MapEditorSettings : Setting
    {
        [Range(1, 100)]
        public int levelOfDetail;
        [Range(1, 2)]
        public float scale;
    }
}