using Assets.Scripts;
using Assets.Scripts.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Editor
{
    [CustomEditor(typeof(WorldGenerator))]
    public class WorldGenerationEditor : UnityEditor.Editor
    {
        WorldGenerator worldGenerator;
        UnityEditor.Editor mapEditor;
        UnityEditor.Editor noiseEditor;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            DrawSettingsEditor(worldGenerator.MapSettings, worldGenerator.Regenerate, ref worldGenerator.MapSettings.foldout, ref mapEditor);
            DrawSettingsEditor(worldGenerator.NoiseSettings, worldGenerator.Regenerate, ref worldGenerator.NoiseSettings.foldout, ref noiseEditor);
        }

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

                if (check.changed && regenerate != null)
                {
                    regenerate();
                }
            }
        }

        private void OnEnable()
        {
            worldGenerator = (WorldGenerator)target;
        }
    }
}
