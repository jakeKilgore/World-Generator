using System.Collections;
using System.Collections.Generic;

namespace Maps {
    public class Directions : IEnumerable<Coordinates> {
        public static readonly Coordinates O = new Coordinates(0, 0);
        public static readonly Coordinates NE = new Coordinates(0, 1);
        public static readonly Coordinates E = new Coordinates(1, 0);
        public static readonly Coordinates SE = new Coordinates(1, -1);
        public static readonly Coordinates SW = new Coordinates(0, -1);
        public static readonly Coordinates W = new Coordinates(-1, 0);
        public static readonly Coordinates NW = new Coordinates(-1, 1);

        public IEnumerator<Coordinates> GetEnumerator() {
            yield return NE;
            yield return E;
            yield return SE;
            yield return SW;
            yield return W;
            yield return NW;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}