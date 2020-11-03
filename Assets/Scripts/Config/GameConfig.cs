using System;

namespace CityBuilder.Config
{
    [Serializable]
    public class GameConfig
    {
        public float CellSize;
        public int CityWidth;
        public int CityLength;
        public BuildingConfig[] Buildings;
    }
}