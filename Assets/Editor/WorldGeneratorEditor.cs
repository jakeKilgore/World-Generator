// file:	Assets\Editor\WorldGeneratorEditor.cs
//
// summary:	Implements the world generator editor class
using Assets.Scripts;
using Assets.Scripts.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    /// <summary>   Editor for world generation. </summary>
    ///
    /// <remarks>   The Vitulus, 9/28/2019. </remarks>
    [CustomEditor(typeof(WorldGenerator))]
    public class WorldGenerationEditor : UnityEditor.Editor
    {
        /// <summary>   The world generator. </summary>
        WorldGenerator worldGenerator;
        /// <summary>   The map editor. </summary>
        UnityEditor.Editor mapEditor;
        /// <summary>   The noise editor. </summary>
        UnityEditor.Editor noiseEditor;

        /// <summary>   Create a custom inspector for this gameobject. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawSettingsEditor(worldGenerator.MapSettings, worldGenerator.Regenerate, ref worldGenerator.MapSettings.foldout, ref mapEditor);
            DrawSettingsEditor(worldGenerator.NoiseSettings, worldGenerator.Regenerate, ref worldGenerator.NoiseSettings.foldout, ref noiseEditor);
        }

        /// <summary>   Draw settings editor. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        ///
        /// <param name="settings">     Options for controlling the operation. </param>
        /// <param name="regenerate">   An action that will regenerate the gameobject after a setting is changed. </param>
        /// <param name="foldout">      [in,out] Whether to display the editor window. </param>
        /// <param name="editor">       [in,out] The editor. </param>
        void DrawSettingsEditor(ScriptableObject settings, Action regenerate, ref bool foldout, ref UnityEditor.Editor editor)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
            if (!foldout)
            {
                return;
            }
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                CreateCachedEditor(settings, null, ref editor);
                editor.OnInspectorGUI();

                if (check.changed && regenerate != null && EditorApplication.isPlaying)
                {
                    regenerate();
                }
            }
        }

        /// <summary>   Executes when enabled. </summary>
        ///
        /// <remarks>   The Vitulus, 9/28/2019. </remarks>
        private void OnEnable()
        {
            worldGenerator = (WorldGenerator)target;
        }
    }
}
