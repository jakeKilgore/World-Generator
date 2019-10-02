// file:	Assets\Scripts\Systems\Render\Jobs\GenerateMeshNormals.cs
//
// summary:	Implements the generate mesh normals class
using Assets.Scripts.Components;
using Assets.Scripts.Components.BufferElements;
using Assets.Scripts.Components.Flags;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Scripts.Systems.Render.Jobs
{
    /// <summary>   A job for generating normals for tile entity meshes. </summary>
    ///
    /// <remarks>   The Vitulus, 10/1/2019. </remarks>
    [BurstCompile]
    [RequireComponentTag(typeof(IsTile), typeof(Vertex), typeof(Normal), typeof(TrianglePoint))]
    [ExcludeComponent(typeof(HasMesh))]
    public struct GenerateMeshNormals : IJobForEachWithEntity<HexCoordinates>
    {
        /// <summary>   The vertex buffers. </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Vertex> vertexBuffers;

        /// <summary>   The normal buffers. </summary>
        [NativeDisableParallelForRestriction]
        [WriteOnly] BufferFromEntity<Normal> normalBuffers;

        public void Execute(Entity entity, int index, ref HexCoordinates c0)
        {
            DynamicBuffer<Vertex> vertices = vertexBuffers[entity];
            DynamicBuffer<Normal> normals = normalBuffers[entity];
            for (int i = 0; i < vertices.Length; i++)
            {

            }
        }
    }
}
