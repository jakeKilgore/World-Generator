// file:	Assets\Scripts\Settings\MapEditorSettings.cs
//
// summary:	Implements the map editor settings class
using System;
using UnityEngine;

namespace Assets.Scripts.Settings
{
    /// <summary>   The settings of the map for changing in the editor. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [CreateAssetMenu()]
    public class MapEditorSettings : Setting
    {
        /// <summary>   The level of detail of all tiles. </summary>
        [Range(1, 100)]
        public int levelOfDetail;
        /// <summary>   The scale of the tiles. </summary>
        [Range(1, 2)]
        public float scale;
    }
}
