﻿// file:	Assets\Scripts\Components\MapSettings.cs
//
// summary:	Implements the map settings class
using Assets.Scripts.Settings;
using Unity.Entities;

namespace Assets.Scripts.Components
{
    /// <summary>   A map settings. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    public struct MapSettings : IComponentData
    {
        /// <summary>   The level of detail. </summary>
        public int levelOfDetail;
        /// <summary>   The scale. </summary>
        public float scale;

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="mapSettings">  The map settings. </param>
        public MapSettings(MapEditorSettings mapSettings)
        {
            levelOfDetail = mapSettings.levelOfDetail;
            scale = mapSettings.scale;
        }
    }
}