using System.Collections;
using Unity.Entities;
using Assets.Scripts.Components.Flags;
using Unity.Collections;
using Assets.Scripts.Components;

namespace Assets.Scripts.Jobs
{
    public struct ClearMeshes : IJobForEachWithEntity<HasMesh, NumRings>
    {
        EntityCommandBuffer.Concurrent commandBuffer;
        int numRings;

        public ClearMeshes(int numRings, EntityCommandBuffer.Concurrent commandBuffer)
        {
            this.numRings = numRings;
            this.commandBuffer = commandBuffer;
        }

        public void Execute(Entity entity, int index, [ReadOnly] ref HasMesh c0, ref NumRings numRings)
        {
            numRings.value = this.numRings;
            commandBuffer.RemoveComponent<HasMesh>(index, entity);
        }
    }
}