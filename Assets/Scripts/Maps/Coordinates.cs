namespace Maps {
    /// <summary>
    /// Cubic coordinate system for representing a map of hexes in 2d space.
    /// 
    /// Implementation inspired by https://www.redblobgames.com/grids/hexagons/
    /// </summary>
    public class Coordinates {
        private readonly int x;
        private readonly int y;
        private readonly int z;

        public int X {
            get {
                return x;
            }
        }

        public int Y {
            get {
                return y;
            }
        }

        public int Z {
            get {
                return z;
            }
        }

        public Coordinates(int x, int y) {
            this.x = x;
            this.y = y;
            this.z = -(x + y);
        }

        public Coordinates(Coordinates coordinates) {
            this.x = coordinates.x;
            this.y = coordinates.y;
            this.z = coordinates.z;
        }

        /// <summary>
        /// Create a new set of coordinates from two sets of coordinates added together.
        /// </summary>
        /// <param name="other">The set of coordinates to add to the current set.</param>
        /// <returns>A new set of coordinates which is equal to this set + the other set.</returns>
        public Coordinates Add(Coordinates other) {
            return new Coordinates(this.X + other.X, this.Y + other.Y);
        }

        public override string ToString() {
            return "(" + X + ", " + Y + ")";
        }

        public override bool Equals(object obj) {
            var coordinates = obj as Coordinates;
            return coordinates != null &&
                   X == coordinates.X &&
                   Y == coordinates.Y &&
                   Z == coordinates.Z;
        }

        public override int GetHashCode() {
            var hashCode = -237065584;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }
    }
}