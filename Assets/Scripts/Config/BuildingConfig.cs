using System;

namespace CityBuilder.Config
{
	[Serializable]
	public class BuildingConfig
	{
		public string Name;
		public int Width;
		public int Length;
		public float Height;
		public int Power;
	}
}