using UnityEngine;

namespace Settings {
    public class MapSettings : Settings {
        [Range(0, 4)]
        public int maxLayer;
        [Range(0, 4)]
        public int minLayer;
    }
}