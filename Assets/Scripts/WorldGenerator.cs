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
        /// <summary>   The map settings changeable in the editor menu. </summary>
        public MapEditorSettings mapSettings;
        /// <summary>   The noise settings changeable in the editor menu. </summary>
        public NoiseEditorSettings noiseSettings;
        /// <summary>   The ground material. </summary>
        public Material groundMaterial;

        /// <summary>   The active world's entity manager. </summary>
        private EntityManager entityManager;
        /// <summary>   An entity query for modifying the NoiseSettings singleton. </summary>
        private EntityQuery noiseQuery;
        /// <summary>   An entity query for modifying the MapSettings singleton. </summary>
        private EntityQuery mapQuery;

        /// <summary>   Gets the current map settings. </summary>
        ///
        /// <value> The map settings. </value>
        public MapEditorSettings MapSettings { get => mapSettings; }

        /// <summary>   Gets the current noise settings. </summary>
        ///
        /// <value> The noise settings. </value>
        public NoiseEditorSettings NoiseSettings { get => noiseSettings; }

        /// <summary>   Starts this object. </summary>
        ///
        /// <remarks>   The Vitulus, 8/13/2019. </remarks>
        void Start()
        {
            entityManager = World.Active.EntityManager;
            noiseQuery = entityManager.CreateEntityQuery(typeof(NoiseSettings));
            mapQuery = entityManager.CreateEntityQuery(typeof(MapSettings));

            entityManager.CreateEntity(typeof(NoiseSettings));
            noiseQuery.SetSingleton(new NoiseSettings(noiseSettings));
            entityManager.CreateEntity(typeof(MapSettings));
            mapQuery.SetSingleton(new MapSettings(mapSettings));

            Tile.Generate(new HexCoordinates(-1, 0), groundMaterial);
            Tile.Generate(new HexCoordinates(0, 0), groundMaterial);
            Tile.Generate(new HexCoordinates(1, 0), groundMaterial);
            Tile.Generate(new HexCoordinates(0, -1), groundMaterial);
            Tile.Generate(new HexCoordinates(0, 1), groundMaterial);
            Tile.Generate(new HexCoordinates(1, -1), groundMaterial);
            Tile.Generate(new HexCoordinates(-1, 1), groundMaterial);
        }

        /// <summary>   Regenerates the world map. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        public void Regenerate()
        {
            mapQuery.SetSingleton(new MapSettings(mapSettings));
            noiseQuery.SetSingleton(new NoiseSettings(noiseSettings));
            BeginInitializationEntityCommandBufferSystem bufferSystem = World.Active.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            EntityCommandBuffer.Concurrent commandBuffer = bufferSystem.CreateCommandBuffer().ToConcurrent();

            ClearMeshes clearMeshes = new ClearMeshes(commandBuffer);
            JobHandle jobHandle = clearMeshes.Schedule(World.Active.EntityManager.CreateEntityQuery(typeof(IsTile), typeof(HasMesh)));

            bufferSystem.AddJobHandleForProducer(jobHandle);
        }
    }
}
