using CityBuilder.Config;

namespace CityBuilder.Model
{
    public class Game
    {
        public readonly GameConfig Config;
        public readonly City City;

        public Game(GameConfig config)
        {
            Config = config;

            City = new City(Config);
        }
    }
}