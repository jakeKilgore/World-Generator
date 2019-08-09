using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Collections;
using UnityEngine;
using Unity.Burst;

public class RenderTiles : ComponentSystem {
    private NoiseFilter noiseFilter;

    protected override void OnCreate() {
        base.OnCreate();
        noiseFilter = new NoiseFilter();
    }

    protected override void OnUpdate() {
        EntityQuery query = Entities.WithAll<IsTile>().ToEntityQuery();
        NativeArray<Entity> tiles = query.ToEntityArray(Allocator.TempJob);
        tiles.Dispose();
    }

    private void GenerateMeshTest(Entity tile) {
        Vector3[] vertices = new Vector3[7];
        int[] triangles = new int[18];

        vertices[0] = new Vector3(0, 0, 0);
        for (int i = 0; i < 6; i++) {
            float degrees = (60 * i);
            float radians = (math.PI / 180) * degrees;
            float posX = math.cos(radians);
            float posZ = math.sin(radians);
            vertices[i + 1] = new Vector3(posX, noiseFilter.Evalutate(new float2(posX, posZ)), posZ);
        }

        int triIndex = 0;
        for (int j = 1; j < 7; j++) {
            triangles[triIndex++] = 0;
            if (j != 6) {
                triangles[triIndex++] = j + 1;
            } else {
                triangles[triIndex++] = 1;
            }
            triangles[triIndex++] = j;
        }

        Mesh tileMesh = new Mesh {
            vertices = vertices,
            triangles = triangles
        };

        EntityManager.SetSharedComponentData(tile, new RenderMesh {
            mesh = tileMesh,
            material = new Material(Shader.Find("Standard"))
        });
    }
}
