// file:	Assets\Scripts\Systems\Render\Jobs\GenerateMeshNormals.cs
//
// summary:	Implements the generate mesh normals class
using System;
using Assets.Scripts.Components;
using Assets.Scripts.Components.BufferElements;
using Assets.Scripts.Components.Flags;
using BovineLabs.Entities.Extensions;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Scripts.Systems.Render.Jobs
{
    /// <summary>   A generate mesh normals. </summary>
    ///
    /// <remarks>   The Vitulus, 10/7/2019. </remarks>
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Normal), typeof(Vertex), typeof(TrianglePoint))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateMeshNormals : IJobForEachWithEntity<HexCoordinates>
    {
        /// <summary>   The vertex buffers. </summary>
        [ReadOnly] BufferFromEntity<Vertex> vertexBuffers;
        /// <summary>   The triangle buffers. </summary>
        [ReadOnly] BufferFromEntity<TrianglePoint> triangleBuffers;
        /// <summary>   The normal buffers. </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Normal> normalBuffers;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 10/7/2019. </remarks>
        ///
        /// <param name="vertexBuffers">    The vertex buffers. </param>
        /// <param name="triangleBuffers">  The triangle buffers. </param>
        /// <param name="normalBuffers">    The normal buffers. </param>
        ///
        /// ### <param name="mapSettings">  The map settings. </param>
        public GenerateMeshNormals(BufferFromEntity<Vertex> vertexBuffers, BufferFromEntity<TrianglePoint> triangleBuffers, BufferFromEntity<Normal> normalBuffers)
        {
            this.vertexBuffers = vertexBuffers;
            this.triangleBuffers = triangleBuffers;
            this.normalBuffers = normalBuffers;
        }

        /// <summary>   Executes. </summary>
        ///
        /// <remarks>   The Vitulus, 10/7/2019. </remarks>
        ///
        /// <param name="entity">       The entity. </param>
        /// <param name="index">        Zero-based index of the. </param>
        /// <param name="coordinates">  [in,out] The coordinates. </param>
        public void Execute(Entity entity, int index, ref HexCoordinates coordinates)
        {
            DynamicBuffer<Vertex> vertices = vertexBuffers[entity];
            DynamicBuffer<TrianglePoint> triangles = triangleBuffers[entity];
            DynamicBuffer<Normal> normals = normalBuffers[entity];
            DynamicBufferExtensions.ResizeInitialized(normals, vertices.Length);

            int numTriangles = triangles.Length / 3;
            for (int i = 0; i < numTriangles; i++)
            {
                int triangleIndex = i * 3;
                int vertexIndexA = triangles[triangleIndex];
                int vertexIndexB = triangles[triangleIndex + 1];
                int vertexIndexC = triangles[triangleIndex + 2];

                float3 triangleNormal = TriangleNormal(vertices[vertexIndexA], vertices[vertexIndexB], vertices[vertexIndexC]);

                normals[vertexIndexA] += triangleNormal;
                normals[vertexIndexB] += triangleNormal;
                normals[vertexIndexC] += triangleNormal;
            }

            for (int i = 0; i < normals.Length; i++)
            {
                math.normalize(normals[i]);
            }
        }

        /// <summary>   Triangle normal. </summary>
        ///
        /// <remarks>   The Vitulus, 10/7/2019. </remarks>
        ///
        /// <param name="vertexA">  The vertex a. </param>
        /// <param name="vertexB">  The vertex b. </param>
        /// <param name="vertexC">  The vertex c. </param>
        ///
        /// <returns>   A float3. </returns>
        private float3 TriangleNormal(float3 vertexA, float3 vertexB, float3 vertexC)
        {
            float3 sideAB = vertexB - vertexA;
            float3 sideAC = vertexC - vertexA;
            return math.normalize(math.cross(sideAB, sideAC));
        }
    }
}