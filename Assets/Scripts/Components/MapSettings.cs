using Unity.Entities;

namespace Assets.Scripts.Components
{
    public struct MapSettings : IComponentData
    {
        public int levelOfDetail;

        public MapSettings(int numRings)
        {
            levelOfDetail = numRings;
        }
    }
}