// file:	Assets\Scripts\Systems\Render\GenerateMesh.cs
//
// summary:	Implements the generate mesh class
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
    /// <summary>   A system for assigning mesh data to tile entities. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    [UpdateAfter(typeof(GenerateTileMeshData))]
    public class UpdateTileMeshes : ComponentSystem
    {
        /// <summary>   The delegate for assigning mesh data to the entity. </summary>
        EntityQueryBuilder.F_ESBBBB<RenderMesh, Vertex, TrianglePoint, UV, Normal> assignMesh;

        /// <summary>   Executes the create action. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        protected override void OnCreate() {
            base.OnCreate();
            assignMesh = AssignMesh;
        }

        /// <summary>   Executes the update action. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        protected override void OnUpdate() {
            Entities.WithNone<HasMesh>().ForEach(assignMesh);
        }

        /// <summary>   Assign the mesh data to the entity. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="entity">           The entity. </param>
        /// <param name="meshComponent">    The mesh component. </param>
        /// <param name="vertices">         The vertices. </param>
        /// <param name="triangles">        The triangles. </param>
        /// <param name="uvs">              The uvs. </param>
        private void AssignMesh(Entity entity, RenderMesh meshComponent, DynamicBuffer<Vertex> vertices, DynamicBuffer<TrianglePoint> triangles, DynamicBuffer<UV> uvs, DynamicBuffer<Normal> normals) {
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
            List<Vector3> normalList = new List<Vector3>();
            ListExtensions.AddRange(normalList, normals.Reinterpret<Vector3>());
            meshComponent.mesh.SetVertices(vertexList);
            meshComponent.mesh.SetTriangles(triangleList, 0);
            meshComponent.mesh.SetUVs(0, uvList);
            meshComponent.mesh.SetNormals(normalList);
            //meshComponent.mesh.RecalculateBounds();
            //meshComponent.mesh.RecalculateNormals();
            //meshComponent.mesh.RecalculateTangents();
            
            PostUpdateCommands.SetSharedComponent(entity, meshComponent);
            PostUpdateCommands.AddComponent(entity, typeof(HasMesh));
        }
    }
}
