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
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class BufferRender : JobComponentSystem
    {
        EndSimulationEntityCommandBufferSystem bufferSystem;

        protected override void OnCreate() {
            base.OnCreateManager();
            bufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps) {
            NoiseSettings noise = GetSingleton<NoiseSettings>();
            MapSettings mapData = GetSingleton<MapSettings>();

            BufferFromEntity<Vertex> vertexBuffers = GetBufferFromEntity<Vertex>();
            GenerateVerticesBuffer vertices = new GenerateVerticesBuffer(vertexBuffers, noise, mapData);
            JobHandle vertexJob = vertices.Schedule(this, inputDeps);

            BufferFromEntity<Triangle> triangleBuffers = GetBufferFromEntity<Triangle>();
            GenerateTrianglesBuffer triangles = new GenerateTrianglesBuffer(triangleBuffers, mapData);
            JobHandle triangleJob = triangles.Schedule(this, inputDeps);

            JobHandle jobs = JobHandle.CombineDependencies(vertexJob, triangleJob);
            bufferSystem.AddJobHandleForProducer(jobs);
            return jobs;
        }
    }
}
