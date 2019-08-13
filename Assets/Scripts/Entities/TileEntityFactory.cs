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
        typeof(HexCoordinates),
        typeof(IsTile)
    );

    public static void Generate(NoiseFilter noiseFilter) {
        Entity tile = entityManager.CreateEntity(archetype);
    }
}
