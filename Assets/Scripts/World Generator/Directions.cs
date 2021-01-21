using Unity.Mathematics;

namespace Assets.Scripts
{
    public enum Direction
    {
        East,
        NorthEast,
        NorthWest,
        SouthWest,
        SouthEast,
        West
    }

    public static class Directions
    {
        public static readonly int3[] direction = { new int3(1, 0, -1), new int3(0, 1, -1), new int3(-1, 1, 0), new int3(0, -1, 1), new int3(1, -1, 0), new int3(-1, 0, 1) };
    }
}
