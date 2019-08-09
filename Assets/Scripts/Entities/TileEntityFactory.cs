using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;

/// <summary>
/// 
/// </summary>
public static class TileEntityFactory {
    private static EntityManager entityManager = World.Active.EntityManager;
    private static EntityArchetype archetype = entityManager.CreateArchetype(
        typeof(Translation),
        typeof(RenderMesh),
        typeof(LocalToWorld),
        typeof(Coordinates),
        typeof(IsTile)
    );

    public static void Generate(NoiseFilter noiseFilter) {
        Entity tile = entityManager.CreateEntity(archetype);
        int layers = 5;
        NativeArray<int> drawTriangles = new NativeArray<int>(TileTriangles.AllocationSpaceForDrawTrianglesArray(layers), Allocator.TempJob);
        JobHandle job = new TileTriangles {
            drawTriangles = drawTriangles,
            numLayers = layers
        }.Schedule();
        job.Complete();
        drawTriangles.Dispose();
    }
}
