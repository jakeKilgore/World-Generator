////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\Systems\RenderTiles.cs
//
// summary:	Implements the render tiles class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Assets.Scripts.Components;
using Assets.Scripts.Components.Flags;
using Assets.Scripts.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace Assets.Scripts.Systems {
    /// <summary>   A system for rendering tile entities. </summary>
    ///
    /// <remarks>   The Vitulus, 8/15/2019. </remarks>
    public class RenderTiles : ComponentSystem {

        /// <summary>   A filter specifying the noise. </summary>
        private NoiseFilter noiseFilter;

        /// <summary>   Set up the system when it is first created. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        protected override void OnCreate() {
            base.OnCreate();
            noiseFilter = new NoiseFilter();
        }

        /// <summary>   Code to run every frame. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        protected override void OnUpdate() {
            Entities.WithAll<IsTile>().ForEach((Entity tile) => {
                GenerateMesh(tile);
            });
        }

        /// <summary>   Generates a mesh for a given entity. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        ///
        /// <param name="tile"> The tile to generate a mesh for. </param>
        private void GenerateMesh(Entity tile) {
            float2 position = EntityManager.GetComponentData<HexCoordinates>(tile).Position();
            int numRings = 10;
            NativeArray<Vector3> vertices = new NativeArray<Vector3>(GenerateTileVertices.AllocationSpaceForVertexArray(numRings), Allocator.Persistent);
            NativeArray<int> triangles = new NativeArray<int>(GenerateTileTriangles.AllocationSpaceForDrawTrianglesArray(numRings), Allocator.Persistent);

            GenerateTileVertices verticesJob = new GenerateTileVertices(vertices, numRings, position, noiseFilter);
            GenerateTileTriangles trianglesJob = new GenerateTileTriangles(triangles, numRings);
            verticesJob.Schedule().Complete();
            trianglesJob.Schedule().Complete();

            Mesh tileMesh = new Mesh {
                vertices = verticesJob.verticesArray.ToArray(),
                triangles = trianglesJob.drawTrianglesArray.ToArray()
            };

            EntityManager.SetSharedComponentData(tile, new RenderMesh {
                mesh = tileMesh,
                material = new Material(Shader.Find("Standard"))
            });

            vertices.Dispose();
            triangles.Dispose();
        }
    }
}
