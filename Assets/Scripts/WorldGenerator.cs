////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	Assets\Scripts\WorldGenerator.cs
//
// summary:	Implements the world generator class
////////////////////////////////////////////////////////////////////////////////////////////////////

using Assets.Scripts.Entities;
using Assets.Scripts.Settings;
using BovineLabs.NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Assets.Scripts.Jobs;
using Unity.Jobs;
using Assets.Scripts.Components.Flags;
using Assets.Scripts.Components;

namespace Assets.Scripts
{

    /// <summary>   A world generator. </summary>
    ///
    /// <remarks>   The Vitulus, 8/13/2019. </remarks>
    public class WorldGenerator : MonoBehaviour {
        [SerializeField]
        private MapSettings mapSettings;

        public MapSettings MapSettings { get => mapSettings; }

        /// <summary>   Starts this object. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        void Start() {
            NoiseFilter noiseFilter = new NoiseFilter();
            TileEntityFactory.Generate(new HexCoordinates(-1, 0), noiseFilter, mapSettings);
            TileEntityFactory.Generate(new HexCoordinates(0, 0), noiseFilter, mapSettings);
            TileEntityFactory.Generate(new HexCoordinates(1, 0), noiseFilter, mapSettings);
            TileEntityFactory.Generate(new HexCoordinates(0, -1), noiseFilter, mapSettings);
            TileEntityFactory.Generate(new HexCoordinates(0, 1), noiseFilter, mapSettings);
            TileEntityFactory.Generate(new HexCoordinates(1, -1), noiseFilter, mapSettings);
            TileEntityFactory.Generate(new HexCoordinates(-1, 1), noiseFilter, mapSettings);
        }

        public void Regenerate()
        {
            BeginInitializationEntityCommandBufferSystem bufferSystem = World.Active.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            EntityCommandBuffer.Concurrent commandBuffer = bufferSystem.CreateCommandBuffer().ToConcurrent();

            ClearMeshes clearMeshes = new ClearMeshes(mapSettings.numRings, commandBuffer);
            JobHandle jobHandle = clearMeshes.Schedule(World.Active.EntityManager.CreateEntityQuery(typeof(IsTile), typeof(HasMesh), typeof(NumRings)));

            bufferSystem.AddJobHandleForProducer(jobHandle);
        }
    }
}
