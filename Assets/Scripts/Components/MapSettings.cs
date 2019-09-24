using Assets.Scripts.Settings;
using Unity.Entities;

namespace Assets.Scripts.Components
{
    public struct MapSettings : IComponentData
    {
        public int levelOfDetail;

        public MapSettings(MapEditorSettings mapSettings)
        {
            levelOfDetail = mapSettings.levelOfDetail;
        }
    }
}