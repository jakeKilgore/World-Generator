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
    /// <summary>   A buffer render. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class BufferRender : JobComponentSystem
    {
        /// <summary>   The buffer system. </summary>
        EndSimulationEntityCommandBufferSystem bufferSystem;

        /// <summary>   Executes the create action. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        protected override void OnCreate() {
            base.OnCreateManager();
            bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        /// <summary>   Executes the update action. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="inputDeps">    The input deps. </param>
        ///
        /// <returns>   A JobHandle. </returns>
        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            NoiseSettings noise = GetSingleton<NoiseSettings>();
            MapSettings mapData = GetSingleton<MapSettings>();

            BufferFromEntity<Vertex> vertexBuffers = GetBufferFromEntity<Vertex>();
            BufferFromEntity<UV> uvBuffers = GetBufferFromEntity<UV>();
            GenerateVerticesBuffer vertices = new GenerateVerticesBuffer(vertexBuffers, uvBuffers, noise, mapData);
            JobHandle vertexJob = vertices.Schedule(this, inputDeps);

            BufferFromEntity<TrianglePoint> triangleBuffers = GetBufferFromEntity<TrianglePoint>();
            GenerateTrianglesBuffer triangles = new GenerateTrianglesBuffer(triangleBuffers, mapData);
            JobHandle triangleJob = triangles.Schedule(this, inputDeps);

            JobHandle jobs = JobHandle.CombineDependencies(vertexJob, triangleJob);
            bufferSystem.AddJobHandleForProducer(jobs);
            return jobs;
        }
    }
}
