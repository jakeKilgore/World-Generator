// file:	Assets\Scripts\Systems\Render\Jobs\GenerateTrianglesBuffer.cs
//
// summary:	Implements the generate triangles buffer class
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Assets.Scripts.Components;
using Assets.Scripts.Components.BufferElements;

namespace Assets.Scripts.Systems.Render.Jobs
{
    /// <summary>   A job for generating triangle buffers for an entity's mesh. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(TrianglePoint))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateMeshTriangles : IJobForEachWithEntity<HexCoordinates>
    {
        /// <summary>   A collection of TrianglePoint dynamic buffers. </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<TrianglePoint> triangleBuffers;
        /// <summary>   Information describing the map. </summary>
        readonly MapSettings mapSettings;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="triangleBuffers">  The triangle buffers. </param>
        /// <param name="mapSettings">      The map settings. </param>
        public GenerateMeshTriangles(BufferFromEntity<TrianglePoint> triangleBuffers, MapSettings mapSettings)
        {
            this.triangleBuffers = triangleBuffers;
            this.mapSettings = mapSettings;
        }

        /// <summary>   Executes the job. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="entity">       The entity. </param>
        /// <param name="index">        Zero-based index of the entity. </param>
        /// <param name="coordinates">  [in,out] The coordinates of the entity. </param>
        public void Execute(Entity entity, int index, [ReadOnly] ref HexCoordinates coordinates)
        {
            DynamicBuffer<TrianglePoint> triangles = triangleBuffers[entity];
            triangles.Clear();
            for (int currentLayer = 1; currentLayer <= mapSettings.levelOfDetail; currentLayer++)
            {
                DrawRing(triangles, currentLayer);
            }
        }

        /// <summary>   Draw a ring of triangles. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="triangles">    The triangles. </param>
        /// <param name="currentRing">  The current ring. </param>
        private void DrawRing(DynamicBuffer<TrianglePoint> triangles, int currentRing) {
            int startNode = HexMath.CheckVerticesInHex(currentRing - 2) - 1;
            int endNode = HexMath.CheckVerticesInHex(currentRing - 1) - 1;
            if (currentRing != 1)
            {
                startNode++;
            }

            int startVertex = endNode + 1;
            int endVertex = HexMath.CheckVerticesInHex(currentRing) - 1;

            int currentVertex = startVertex;
            for (int currentNode = startNode; currentNode <= endNode; currentNode++)
            {
                int trianglesPerNode = TrianglesPerNode(currentNode, startNode, endNode);

                for (int nodeTriangles = 0; nodeTriangles < trianglesPerNode; nodeTriangles++)
                {
                    if (currentRing != 1 && currentNode == startNode && nodeTriangles == 0)
                    {
                        continue;
                    }

                    int vertex1 = currentVertex;
                    int vertex2 = currentVertex + 1;

                    if (vertex2 > endVertex)
                    {
                        vertex2 = currentRing == 1 ? startVertex : startNode;
                    }

                    DrawTriangle(triangles, currentNode, vertex1, vertex2);

                    currentVertex++;
                }

                if (currentRing != 1)
                {
                    int vertex1 = currentVertex;
                    int vertex2 = currentNode + 1;

                    if (vertex2 > endNode)
                    {
                        vertex2 = startNode;
                    }

                    DrawTriangle(triangles, currentNode, vertex1, vertex2);

                    if (currentNode == endNode)
                    {
                        DrawTriangle(triangles, startNode, endVertex, startVertex);
                    }
                }
            }
        }

        /// <summary>   Triangles per node. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="currentNode">  The current node. </param>
        /// <param name="startNode">    The start node. </param>
        /// <param name="endNode">      The end node. </param>
        ///
        /// <returns>   The number of triangles connected to the current vertex. </returns>
        private int TrianglesPerNode(int currentNode, int startNode, int endNode)
        {
            if (currentNode == 0) {
                return 6;
            }

            if (IsCornerNode(currentNode, startNode, endNode))
            {
                return 2;
            }
            else
            {
                return 1;
            }
        }

        /// <summary>   Query if 'currentNode' is corner node. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="currentNode">  The current node. </param>
        /// <param name="startNode">    The start node. </param>
        /// <param name="endNode">      The end node. </param>
        ///
        /// <returns>   True if corner node, false if not. </returns>
        private bool IsCornerNode(int currentNode, int startNode, int endNode)
        {
            int range = endNode - (startNode - 1);
            if (range == 0)
            {
                return false;
            }

            int cornerDistance = range / 6;
            if ((currentNode - startNode) % cornerDistance == 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>   Draw triangle. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="triangles">    The triangles. </param>
        /// <param name="currentNode">  The current node. </param>
        /// <param name="vertex1">      The first vertex. </param>
        /// <param name="vertex2">      The second vertex. </param>
        private void DrawTriangle(DynamicBuffer<TrianglePoint> triangles, int currentNode, int vertex1, int vertex2)
        {
            triangles.Add(currentNode);
            triangles.Add(vertex1);
            triangles.Add(vertex2);
        }
    }
}
