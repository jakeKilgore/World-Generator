using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Assets.Scripts.Components.BufferElements;
using Unity.Collections;
using Unity.Rendering;

namespace Assets.Scripts.Systems.Render
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateBefore(typeof(BufferRender))]
    public class GenerateMesh : ComponentSystem
    {
        EntityQueryBuilder.F_ESBB<RenderMesh, Vertex, Triangle> assignMesh;
        Material material;

        protected override void OnCreate() {
            base.OnCreate();
            assignMesh = AssignMesh;
            material = new Material(Shader.Find("Standard"));
        }

        protected override void OnUpdate() {
            Entities.ForEach(assignMesh);
        }

        private void AssignMesh(Entity entity, RenderMesh meshComponent, DynamicBuffer<Vertex> vertices, DynamicBuffer<Triangle> triangles) {
            meshComponent.mesh.Clear();
            meshComponent.mesh.vertices = Utilities.DynamicBufferToArray(vertices.Reinterpret<Vector3>());
            meshComponent.mesh.triangles = Utilities.DynamicBufferToArray(triangles.Reinterpret<int>());
            meshComponent.material = material;
            
            PostUpdateCommands.SetSharedComponent(entity, meshComponent);
            //buffer.SetSharedComponent(entity, componentData);
        }
    }
}