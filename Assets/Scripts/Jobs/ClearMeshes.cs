using System.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Unity.Collections;
using Assets.Scripts.Components;

namespace Assets.Scripts.Jobs
{
    public struct ClearMeshes : IJobForEachWithEntity<HasMesh>
    {
        EntityCommandBuffer.Concurrent commandBuffer;

        public ClearMeshes(EntityCommandBuffer.Concurrent commandBuffer)
        {
            this.commandBuffer = commandBuffer;
        }

        public void Execute(Entity entity, int index, [ReadOnly] ref HasMesh c0)
        {
            commandBuffer.RemoveComponent<HasMesh>(index, entity);
        }
    }
}
