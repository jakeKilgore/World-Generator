using Unity.Entities;

public struct MapData : IComponentData
{
    public int levelOfDetail;

    public MapData(int numRings)
    {
        levelOfDetail = numRings;
    }
}
