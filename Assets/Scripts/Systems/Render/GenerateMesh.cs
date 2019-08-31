using UnityEngine;
using System.Collections;
using Unity.Entities;
using Unity.Jobs;
using Assets.Scripts.Components.BufferElements;
using Unity.Rendering;
using BovineLabs.Entities.Extensions;
using System.Collections.Generic;
using Assets.Scripts.Components.Flags;

namespace Assets.Scripts.Systems.Render
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(BufferRender))]
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
            Entities.WithNone<HasMesh>().ForEach(assignMesh);
        }

        private void AssignMesh(Entity entity, RenderMesh meshComponent, DynamicBuffer<Vertex> vertices, DynamicBuffer<Triangle> triangles) {
            meshComponent.mesh.Clear();
            if (vertices.Length == 0) {
                return;
            }

            List<Vector3> vertexList = new List<Vector3>();
            ListExtensions.AddRange(vertexList, vertices.Reinterpret<Vector3>());
            List<int> triangleList = new List<int>();
            ListExtensions.AddRange(triangleList, triangles.Reinterpret<int>());
            meshComponent.mesh.SetVertices(vertexList);
            meshComponent.mesh.SetTriangles(triangleList, 0);
            meshComponent.material = material;
            
            PostUpdateCommands.SetSharedComponent(entity, meshComponent);
            PostUpdateCommands.AddComponent(entity, typeof(HasMesh));
        }
    }
}
