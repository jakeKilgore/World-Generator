// file:	Assets\Scripts\Jobs\ClearMeshes.cs
//
// summary:	Implements the clear meshes class
using System.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Unity.Collections;
using Assets.Scripts.Components;

namespace Assets.Scripts.Jobs
{
    /// <summary>   A job that clears all meshes from entities that have meshes. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    public struct ClearMeshes : IJobForEachWithEntity<HasMesh>
    {
        /// <summary>   Buffer for command data. </summary>
        EntityCommandBuffer.Concurrent commandBuffer;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="commandBuffer">    Buffer for command data. </param>
        public ClearMeshes(EntityCommandBuffer.Concurrent commandBuffer)
        {
            this.commandBuffer = commandBuffer;
        }

        /// <summary>   Executes the job. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="entity">   The entity. </param>
        /// <param name="index">    Zero-based index of the entity. </param>
        /// <param name="meshFlag"> [in,out] A flag marking that the entity has a mesh. </param>
        public void Execute(Entity entity, int index, [ReadOnly] ref HasMesh meshFlag)
        {
            commandBuffer.RemoveComponent<HasMesh>(index, entity);
        }
    }
}
