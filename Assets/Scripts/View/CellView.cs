using CityBuilder.Model;
using UnityEngine;

namespace CityBuilder.View
{
	public class CellView : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _spriteRenderer;
		[SerializeField] private Color _idleColor;
		[SerializeField] private Color _freeColor;
		[SerializeField] private Color _busyColor;
		
		public void Init(Cell model)
		{
			_model = model;
		}

		public void SetColorMode(bool buildingMode)
		{
			var color = buildingMode ? (_model.Building == null ? _freeColor : _busyColor) : _idleColor;
			_spriteRenderer.color = color;
		}

		private Cell _model;
	}
}