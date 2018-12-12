using Maps;
using Noise;
using Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator {
    TerrainSettings terrainSettings;
    NoiseFilter noiseFilter;
    GameObject world;

    public TerrainGenerator(TerrainSettings terrainSettings) {
        this.terrainSettings = terrainSettings;
        noiseFilter = new NoiseFilter(terrainSettings);
        world = new GameObject("World");
    }

    public void Generate(Tile tile) {
        if(tile.Layer < terrainSettings.renderLayer) {
            throw new ArgumentException("Tile cannot be rendered with current render layer.");
        }
        if(tile.Layer > terrainSettings.renderLayer) {
            foreach(Tile child in tile.Values) {
                Generate(child);
            }
            return;
        }

        //TODO: Split meshes into different class
        //      Group objects by layer
        //      Hexes do not line up at higher layers. Angle and sizing is slightly off.
        GameObject gameObject = new GameObject();
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
        Vector3 position = tile.Position();

        Vector3[] vertices = new Vector3[7];
        int[] triangles = new int[18];
        
        float distanceMultiple = Mathf.Pow(Mathf.Sqrt(7), tile.Layer);
        float degree_offset = (tile.Layer * 19.11f) - 30;
        float noise = noiseFilter.Evaluate(position.x, position.z);
        vertices[0] = new Vector3(position.x, noise, position.z);
        for(int i = 0; i < 6; i++) {
            float multiple = Mathf.Pow(2.65f, tile.Layer);
            float degrees = (60 * i) + degree_offset;
            float radians = Mathf.PI / 180 * degrees;
            float posX = position.x + multiple * Mathf.Cos(radians);
            float posZ = position.z + multiple * Mathf.Sin(radians);
            float posY = noiseFilter.Evaluate(posX, posZ);
            vertices[i + 1] = new Vector3(posX, posY, posZ);
        }

        int index = 0;
        for(int j = 1; j < 7; j++) {
            triangles[index++] = 0;
            if(j != 6) {
                triangles[index++] = j + 1;
            }
            else {
                triangles[index++] = 1;
            }
            triangles[index++] = j;
        }

        gameObject.name = tile.ToString();
        gameObject.transform.parent = world.transform;
        meshRenderer.sharedMaterial = new Material(Shader.Find("Standard"));

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        meshFilter.sharedMesh = mesh;
    }

    public void Regenerate() {
        UnityEngine.Object.Destroy(world);
        world = new GameObject("World");
    }
}
