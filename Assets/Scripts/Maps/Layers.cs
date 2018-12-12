using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Maps {
    public static class Layers {
        public static readonly int min;
        public static readonly int max;

        static Layers() {
            min = (int)Enum.GetValues(typeof(LayerNames)).Cast<LayerNames>().Min();
            max = (int)Enum.GetValues(typeof(LayerNames)).Cast<LayerNames>().Max();
        }

        public static string GetName(int value) {
            return ((LayerNames)value).ToString();
        }

        private enum LayerNames {
            District,
            Municipality,
            Holding,
            Province,
            Region,
            Area,
            Subcontinent,
            Continent,
            Hemisphere
        }
    }
}