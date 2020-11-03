using CityBuilder.Config;

namespace CityBuilder.Model
{
	public class Building
	{
		public readonly int Power;

		public Building(BuildingConfig config)
		{
			Power = config.Power;
		}
	}
}