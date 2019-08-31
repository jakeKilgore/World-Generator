using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    [CreateAssetMenu()]
    public class MapSettings : Setting
    {
        [Range(1, 100)]
        public int numRings;
    }
}