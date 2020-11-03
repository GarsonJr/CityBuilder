using System;
using System.Collections.Generic;
using CityBuilder.Config;

namespace CityBuilder.Model
{
	public class City
	{
		public readonly Cell[,] Cells;
		public IReadOnlyList<Building> Buildings => _buildings;
		public int TotalPower { get; private set; }

		public event Action<Building> Built;

		public City(GameConfig config)
		{
			_buildings = new List<Building>();
			Cells = new Cell[config.CityWidth, config.CityLength];

			for (int i = 0; i < config.CityWidth; i++)
			{
				for (int j = 0; j < config.CityLength; j++)
				{
					Cells[i, j] = new Cell(i, j);
				}
			}
		}

		public void Build(BuildingConfig buildingConfig, int x, int y)
		{
			var building = new Building(buildingConfig);
			var maxX = x + buildingConfig.Width;
			var maxY = y + buildingConfig.Length;
			for (int i = x; i < maxX; i++)
			{
				for (int j = y; j < maxY; j++)
				{
					Cells[i, j].Build(building);
				}
			}

			_buildings.Add(building);

			TotalPower += building.Power;
			
			Built?.Invoke(building);
		}

		private List<Building> _buildings;
	}
}