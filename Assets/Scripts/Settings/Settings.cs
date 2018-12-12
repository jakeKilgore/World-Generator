using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Settings {
    [CreateAssetMenu()]
    public class Settings : ScriptableObject {
        [HideInInspector]
        public bool foldout = true;
    }
}