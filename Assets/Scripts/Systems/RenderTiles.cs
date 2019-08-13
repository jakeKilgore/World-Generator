using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

public class RenderTiles : ComponentSystem {
    private NoiseFilter noiseFilter;

    protected override void OnCreate() {
        base.OnCreate();
        noiseFilter = new NoiseFilter();
    }

    protected override void OnUpdate() {
        Entities.WithAll<IsTile>().ForEach((Entity tile) => {
            GenerateMesh(tile);
        });
    }

    private void GenerateMesh(Entity tile) {
        float2 position = EntityManager.GetComponentData<HexCoordinates>(tile).Position();
        int numRings = 10;
        NativeArray<Vector3> vertices = new NativeArray<Vector3>(TileVertices.AllocationSpaceForVertexArray(numRings), Allocator.Persistent);
        NativeArray<int> triangles = new NativeArray<int>(TileTriangles.AllocationSpaceForDrawTrianglesArray(numRings), Allocator.Persistent);

        TileVertices verticesJob = new TileVertices(vertices, numRings, position, noiseFilter);
        TileTriangles trianglesJob = new TileTriangles(triangles, numRings);
        verticesJob.Schedule().Complete();
        trianglesJob.Schedule().Complete();

        Mesh tileMesh = new Mesh {
            vertices = verticesJob.vertices.ToArray(),
            triangles = trianglesJob.drawTriangles.ToArray()
        };

        EntityManager.SetSharedComponentData(tile, new RenderMesh {
            mesh = tileMesh,
            material = new Material(Shader.Find("Standard"))
        });

        vertices.Dispose();
        triangles.Dispose();
    }
}
