// file:	Assets\Scripts\Systems\Render\BufferRender.cs
//
// summary:	Implements the buffer render class
using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Assets.Scripts.Components.BufferElements;
using Assets.Scripts.Systems.Render.Jobs;
using Unity.Collections;
using Assets.Scripts.Components.Flags;
using Assets.Scripts.Components;

namespace Assets.Scripts.Systems.Render
{
    /// <summary>   A system for generating the mesh data for tile entities. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(UpdateTileMeshes))]
    public class GenerateTileMeshData : JobComponentSystem
    {
        /// <summary>   The command buffer system. </summary>
        EndSimulationEntityCommandBufferSystem commandBufferSystem;

        /// <summary>   Executes the create action. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        protected override void OnCreate()
        {
            base.OnCreateManager();
            commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        /// <summary>   Executes the update action. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="inputDeps">    The input deps. </param>
        ///
        /// <returns>   A JobHandle. </returns>
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            NoiseSettings noise = GetSingleton<NoiseSettings>();
            MapSettings mapData = GetSingleton<MapSettings>();

            BufferFromEntity<Vertex> vertexBuffers = GetBufferFromEntity<Vertex>();
            BufferFromEntity<UV> uvBuffers = GetBufferFromEntity<UV>();
            BufferFromEntity<Normal> normalBuffers = GetBufferFromEntity<Normal>();
            GenerateMeshVerticesNormalsAndUVs vertices = new GenerateMeshVerticesNormalsAndUVs(vertexBuffers, uvBuffers, normalBuffers, noise, mapData);
            JobHandle vertexJob = vertices.Schedule(this, inputDeps);

            BufferFromEntity<TrianglePoint> triangleBuffers = GetBufferFromEntity<TrianglePoint>();
            GenerateMeshTriangles triangles = new GenerateMeshTriangles(triangleBuffers, mapData);
            JobHandle triangleJob = triangles.Schedule(this, inputDeps);

            JobHandle jobs = JobHandle.CombineDependencies(vertexJob, triangleJob);
            commandBufferSystem.AddJobHandleForProducer(jobs);
            return jobs;
        }
    }
}
