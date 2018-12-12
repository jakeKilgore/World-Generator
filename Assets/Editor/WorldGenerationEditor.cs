using Maps;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WorldGeneration;

[CustomEditor(typeof(WorldGenerator))]
public class WorldGenerationEditor : Editor {
    WorldGenerator worldGenerator;
    Editor mapEditor;
    Editor terrainEditor;

    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        NormalizeSettings();

        DrawSettingsEditor(worldGenerator.mapSettings, Regenerate.Map, ref worldGenerator.mapSettings.foldout, ref mapEditor);
        DrawSettingsEditor(worldGenerator.terrainSettings, Regenerate.Terrain, ref worldGenerator.terrainSettings.foldout, ref terrainEditor);
    }

    private void NormalizeSettings() {
        if(worldGenerator.mapSettings.maxLayer < worldGenerator.mapSettings.minLayer) {
            worldGenerator.mapSettings.minLayer = worldGenerator.mapSettings.maxLayer;
        }
        if(worldGenerator.terrainSettings.renderLayer > worldGenerator.mapSettings.maxLayer) {
            worldGenerator.terrainSettings.renderLayer = worldGenerator.mapSettings.maxLayer;
        }
    }

    void DrawSettingsEditor(ScriptableObject settings, Action<World> regenerate, ref bool foldout, ref Editor editor) {
        foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);
        if(!foldout) {
            return;
        }
        using (var check = new EditorGUI.ChangeCheckScope()) {
            CreateCachedEditor(settings, null, ref editor);
            editor.OnInspectorGUI();

            if (check.changed && regenerate != null) {
                regenerate(worldGenerator.world);
            }
        }
    }

    private void OnEnable() {
        worldGenerator = (WorldGenerator)target;
    }
}
