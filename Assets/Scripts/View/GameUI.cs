using System.Collections.Generic;
using CityBuilder.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CityBuilder.View
{
	public class GameUI : MonoBehaviour
	{
		[SerializeField] private Canvas _canvas;
		[SerializeField] private Text _powerText;
		[SerializeField] private PowerView[] _powerViews;

		public void Init(City city, CityView cityView, CameraController cameraController)
		{
			_cityModel = city;
			_cityView = cityView;
			_cityView.Built += OnBuilt;
			_buildings = new List<BuildingPowerPair>();
			_camera = cameraController.Camera;

			_freePowerViews = new List<PowerView>(_powerViews);
		}

		private void Update()
		{
			for (var i = 0; i < _buildings.Count; i++)
			{
				var pair = _buildings[i];
				var worldPos = pair.Building.Position + Vector3.up * (pair.Building.Config.Height + 0.5f);
				var screenPoint = _camera.WorldToScreenPoint(worldPos);
				if (screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 ||
				    screenPoint.y > Screen.height)
				{
					if (pair.HasPower)
					{
						_freePowerViews.Add(pair.Power);
						pair.Power.Hide();
						pair.ClearPower();
					}

					continue;
				}

				if (!pair.HasPower)
				{
					var index = _freePowerViews.Count - 1;
					var powerView = _freePowerViews[index];
					_freePowerViews.RemoveAt(index);
					powerView.Show(pair.Building.Config.Power);
					powerView.SetPosition(screenPoint / _canvas.scaleFactor);
					pair.SetPower(powerView);
				}
				else
					pair.Power.SetPosition(screenPoint / _canvas.scaleFactor);
			}
		}

		private void OnBuilt(BuildingView building)
		{
			_powerText.text = _cityModel.TotalPower.ToString();

			_buildings.Add(new BuildingPowerPair(building));
		}

		private class BuildingPowerPair
		{
			public readonly BuildingView Building;
			public PowerView Power { get; private set; }
			public bool HasPower { get; private set; }

			public BuildingPowerPair(BuildingView building)
			{
				Building = building;
			}

			public void SetPower(PowerView powerView)
			{
				Power = powerView;
				HasPower = true;
			}

			public void ClearPower()
			{
				Power = null;
				HasPower = false;
			}
		}

		private City _cityModel;
		private CityView _cityView;
		private List<BuildingPowerPair> _buildings;
		private Camera _camera;

		private List<PowerView> _freePowerViews;
	}
}