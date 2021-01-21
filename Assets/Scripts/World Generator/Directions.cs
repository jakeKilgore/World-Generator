using Unity.Mathematics;

namespace Assets.Scripts.WorldGenerator
{
    /// <summary>   Collection of the valid directions to move out of a hexagonal tile. </summary>
    public static class Direction
    {
        private static readonly int3 east = new int3(1, 0, -1);
        private static readonly int3 northEast = new int3(0, 1, -1);
        private static readonly int3 northWest = new int3(-1, 1, 0);
        private static readonly int3 west = new int3(0, -1, 1);
        private static readonly int3 southWest = new int3(1, -1, 0);
        private static readonly int3 southEast = new int3(-1, 0, 1);

        /// <summary> Vector pointing east from the hex. </summary>
        public static ref readonly int3 East => ref east;
        /// <summary> Vector pointing northeast from the hex. </summary>
        public static ref readonly int3 NorthEast => ref northEast;
        /// <summary> Vector pointing northwest from the hex. </summary>
        public static ref readonly int3 NorthWest => ref northWest;
        /// <summary> Vector pointing west from the hex. </summary>
        public static ref readonly int3 West => ref west;
        /// <summary> Vector pointing southwest from the hex. </summary>
        public static ref readonly int3 SouthWest => ref southWest;
        /// <summary> Vector pointing southeast from the hex. </summary>
        public static ref readonly int3 SouthEast => ref southEast;
    }
}
