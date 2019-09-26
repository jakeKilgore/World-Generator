using Assets.Scripts.Settings;
using Unity.Entities;

namespace Assets.Scripts.Components
{
    public struct MapSettings : IComponentData
    {
        public int levelOfDetail;
        public float scale;

        public MapSettings(MapEditorSettings mapSettings)
        {
            levelOfDetail = mapSettings.levelOfDetail;
            scale = mapSettings.scale;
        }
    }
}