namespace CityBuilder.Model
{
	public class Cell
	{
		public readonly int X;
		public readonly int Y;
		public Building Building { get; private set; }

		public Cell(int x, int y)
		{
			X = x;
			Y = y;
		}

		public void Build(Building building) => Building = building;
	}
}