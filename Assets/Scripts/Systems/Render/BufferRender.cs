using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Assets.Scripts.Components.BufferElements;
using Assets.Scripts.Systems.Render.Jobs;
using Unity.Collections;
using Assets.Scripts.Components.Flags;

namespace Assets.Scripts.Systems.Render
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class BufferRender : JobComponentSystem
    {
        EndSimulationEntityCommandBufferSystem bufferSystem;

        protected override void OnCreate() {
            base.OnCreateManager();
            bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            NoiseData noise = GetSingleton<NoiseData>();

            BufferFromEntity<Vertex> vertexBuffers = GetBufferFromEntity<Vertex>();
            GenerateVerticesBuffer vertices = new GenerateVerticesBuffer(vertexBuffers, noise);
            JobHandle vertexJob = vertices.Schedule(this, inputDeps);

            BufferFromEntity<Triangle> triangleBuffers = GetBufferFromEntity<Triangle>();
            GenerateTrianglesBuffer triangles = new GenerateTrianglesBuffer(triangleBuffers);
            JobHandle triangleJob = triangles.Schedule(this, inputDeps);

            JobHandle jobs = JobHandle.CombineDependencies(vertexJob, triangleJob);
            bufferSystem.AddJobHandleForProducer(jobs);
            return jobs;
        }
    }
}
