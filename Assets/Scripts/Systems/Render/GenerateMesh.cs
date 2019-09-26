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
    public class GenerateMesh : ComponentSystem
    {
        EntityQueryBuilder.F_ESBBB<RenderMesh, Vertex, Triangle, UV> assignMesh;

        protected override void OnCreate() {
            base.OnCreate();
            assignMesh = AssignMesh;
        }

        protected override void OnUpdate() {
            Entities.WithNone<HasMesh>().ForEach(assignMesh);
        }

        private void AssignMesh(Entity entity, RenderMesh meshComponent, DynamicBuffer<Vertex> vertices, DynamicBuffer<Triangle> triangles, DynamicBuffer<UV> uvs) {
            meshComponent.mesh.Clear();
            if (vertices.Length == 0) {
                return;
            }

            List<Vector3> vertexList = new List<Vector3>();
            ListExtensions.AddRange(vertexList, vertices.Reinterpret<Vector3>());
            List<int> triangleList = new List<int>();
            ListExtensions.AddRange(triangleList, triangles.Reinterpret<int>());
            List<Vector2> uvList = new List<Vector2>();
            ListExtensions.AddRange(uvList, uvs.Reinterpret<Vector2>());
            meshComponent.mesh.SetVertices(vertexList);
            meshComponent.mesh.SetTriangles(triangleList, 0);
            meshComponent.mesh.SetUVs(0, uvList);
            meshComponent.mesh.RecalculateBounds();
            meshComponent.mesh.RecalculateNormals();
            meshComponent.mesh.RecalculateTangents();
            meshComponent.receiveShadows = true;
            meshComponent.castShadows = UnityEngine.Rendering.ShadowCastingMode.On;
            
            PostUpdateCommands.SetSharedComponent(entity, meshComponent);
            PostUpdateCommands.AddComponent(entity, typeof(HasMesh));
        }
    }
}
