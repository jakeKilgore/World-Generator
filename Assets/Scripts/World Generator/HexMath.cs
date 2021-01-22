using Unity.Mathematics;

namespace Assets.Scripts.WorldGenerator {

    /// <summary>   Mathematics for the hexagonal tile implementation. </summary>
    public static class HexMath
    {
        private static readonly float widthMultiple = math.sqrt(3f);
        private static readonly float heightMultiple = 2f;

        /// <summary>   The width multiple of a hexagon. </summary>
        /// <remarks>   Multiply the radius of a pointy-top hexagon by this number to get the width from side to side.</remarks>
        public static float WidthMultiple => widthMultiple;
        /// <summary>   The height multiple of a hexagon. </summary>
        /// <remarks>   Multiply the radius of a pointy-top hexagon by this number to get the height from point to point.</remarks>
        public static float HeightMultiple => heightMultiple;

        /// <summary>   Returns the location of the hex in the worldspace. </summary>
        /// <param name="coordinates">  The position of the hex in tile coordinates. </param>
        /// <param name="radius">       The distance from the center of the hexagon to the points. </param>
        /// <returns>   The position in the worldspace coordinates. </returns>
        public static float2 Position(int2 coordinates, float radius = 1f)
        {
            var height = HeightMultiple * radius;
            var width = WidthMultiple * radius;
            var posX = WidthMultiple * (coordinates.x + (coordinates.y / 2f));
            var posY = HeightMultiple * .75f * coordinates.y;

            return new float2(posX, posY);
        }

        /// <summary>   Check the number of vertices in the hexagon mesh. </summary>
        /// <param name="resolution">   The resolution level of the hexagon mesh. </param>
        /// <returns>   The number of vertices in the hexagon. </returns>
        public static int NumVerticesInHex(int resolution)
        {
            return 3 * resolution * (resolution + 1) + 1;
        }

        /// <summary>   Check the number of vertices added by the current resolution level. </summary>
        /// <param name="resolution">  The current resolution level. </param>
        /// <returns>   The number of vertices added by the current resolution level. </returns>
        public static int NumVerticesOfCurrentResolution(int resolution)
        {
            return 6 * resolution;
        }

        /// <summary>   Check the number of triangles in the hexagon mesh. </summary>
        /// <param name="resolution">  The resolution level of the hexagon. </param>
        /// <returns>   The number of triangles in the hexagon. </returns>
        public static int NumTrianglesInHex(int resolution)
        {
            return resolution * (resolution + 1) * (resolution + 2);
        }

        /// <summary>   Check the number of triangles added by the current resolution level. </summary>
        /// <param name="resolution">  The current resolution level. </param>
        /// <returns>   The number of triangles added by the current resolution level. </returns>
        public static int NumTrianglesOfCurrentResolution(int resolution)
        {
            return 3 * resolution * (resolution + 1);
        }

        /// <summary>   Find the minimum resolution level that includes the given vertex. </summary>
        /// <param name="vertexIndex">  The index of the vertex in the hexagon mesh. </param>
        /// <returns>   The minimum resolution level that includes the given vertex. </returns>
        public static int MinResolutionOfVertex(int vertexIndex)
        {
            if (vertexIndex == 0) { return 0; }
            var a = 3;
            var b = 3;
            var c = 1 - vertexIndex;
            int resolution = (int)(-b + math.sqrt(b * b - (4 * a * c))) / (2 * a);
            return resolution + 1;
        }
    }
}
