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
    public class WorldGenerator : MonoBehaviour
    {
        [SerializeField]
        private MapEditorSettings mapSettings;

        private EntityManager entityManager;
        private EntityQuery noiseQuery;
        private EntityQuery mapQuery;

        public MapEditorSettings MapSettings { get => mapSettings; }

        /// <summary>   Starts this object. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        void Start()
        {
            entityManager = World.Active.EntityManager;
            noiseQuery = entityManager.CreateEntityQuery(typeof(NoiseSettings));
            mapQuery = entityManager.CreateEntityQuery(typeof(MapSettings));

            entityManager.CreateEntity(typeof(NoiseSettings));
            noiseQuery.SetSingleton(new NoiseSettings(0));
            entityManager.CreateEntity(typeof(MapSettings));
            mapQuery.SetSingleton(new MapSettings(mapSettings.levelOfDetail));

            Tile.Generate(new HexCoordinates(-1, 0));
            Tile.Generate(new HexCoordinates(0, 0));
            Tile.Generate(new HexCoordinates(1, 0));
            Tile.Generate(new HexCoordinates(0, -1));
            Tile.Generate(new HexCoordinates(0, 1));
            Tile.Generate(new HexCoordinates(1, -1));
            Tile.Generate(new HexCoordinates(-1, 1));
        }

        public void Regenerate()
        {
            mapQuery.SetSingleton(new MapSettings(mapSettings.levelOfDetail));
            BeginInitializationEntityCommandBufferSystem bufferSystem = World.Active.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            EntityCommandBuffer.Concurrent commandBuffer = bufferSystem.CreateCommandBuffer().ToConcurrent();

            ClearMeshes clearMeshes = new ClearMeshes(commandBuffer);
            JobHandle jobHandle = clearMeshes.Schedule(World.Active.EntityManager.CreateEntityQuery(typeof(IsTile), typeof(HasMesh)));

            bufferSystem.AddJobHandleForProducer(jobHandle);
        }
    }
}
