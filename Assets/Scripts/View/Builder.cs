using System;
using CityBuilder.Config;
using CityBuilder.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CityBuilder.View
{
	public class Builder : MonoBehaviour
	{
		[SerializeField] private BuildingView _buildingPrefab;
		[SerializeField] private Transform _buildingsParent;

		public event Action<BuildingView, int, int> Built;

		public void Init(GameView gameView, Game gameModel, CameraController cameraController, InputHandler inputHandler)
		{
			_gameModel = gameModel;
			_mainCamera = cameraController.Camera;
			gameView.GameModeChanged += OnGameModeChanged;
			inputHandler.Click += OnClick;
		}

		private void Update()
		{
			if (_currentMode != GameMode.Building)
				return;
			
			var ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			if (!Physics.Raycast(ray, out var hitInfo, 100f))
			{
				_isPositionValid = false;
				_buildingTemplate.SetPosition(Vector3.up * 100);
				return;
			}

			var config = _gameModel.Config;
			var cityWidth = config.CellSize * config.CityWidth;
			var cityLength = config.CellSize * config.CityLength;
			var buildingWidth = config.CellSize * _buildingTemplate.Config.Width;
			var buildingLength = config.CellSize * _buildingTemplate.Config.Length;
			var x = (int) ((hitInfo.point.x + cityWidth / 2 - buildingWidth / 2) / config.CellSize);
			var y = (int) ((hitInfo.point.z + cityLength / 2 - buildingLength / 2) / config.CellSize);
			var maxX = x + _buildingTemplate.Config.Width - 1;
			var maxY = y + _buildingTemplate.Config.Length - 1;

			if (x < 0 || y < 0 || maxX >= _gameModel.City.Cells.GetLength(0) ||
			    maxY >= _gameModel.City.Cells.GetLength(1))
			{
				_isPositionValid = false;
				_buildingTemplate.SetColorMode(BuildingView.ColorMode.TransparentInvalid);
				return;
			}

			_isPositionValid = true;
			var startX = x - 1 < 0 ? x : x - 1;
			var startY = y - 1 < 0 ? y : y - 1;
			var endX = maxX + 1 >= _gameModel.City.Cells.GetLength(0) ? maxX : maxX + 1;
			var endY = maxY + 1 >= _gameModel.City.Cells.GetLength(1) ? maxY : maxY + 1;
			for (int i = startX; i < endX + 1; i++)
			{
				for (int j = startY; j < endY + 1; j++)
				{
					_isPositionValid &= _gameModel.City.Cells[i, j].Building == null;
					if (!_isPositionValid)
						break;
				}
			}

			var position = new Vector3(x * config.CellSize - cityWidth / 2 + buildingWidth / 2,
				0, y * config.CellSize - cityLength / 2 + buildingLength / 2);
			_currentX = x;
			_currentY = y;
			_buildingTemplate.SetPosition(position);
			_buildingTemplate.SetColorMode(_isPositionValid ? BuildingView.ColorMode.TransparentValid
															: BuildingView.ColorMode.TransparentInvalid);
		}

		private void OnClick(Vector2 _)
		{
			if (!_isPositionValid)
				return;

			_gameModel.City.Build(_buildingTemplate.Config, _currentX, _currentY);
			Built?.Invoke(_buildingTemplate, _currentX, _currentY);
			CreateRandomTemplate();
		}

		private void OnGameModeChanged(GameMode mode)
		{
			_currentMode = mode;
			if (_currentMode == GameMode.Building)
			{
				CreateRandomTemplate();
			}
			else
			{
				Destroy(_buildingTemplate.gameObject);
				_isPositionValid = false;
			}
		}

		private void CreateRandomTemplate()
		{
			var config = PickBuildingConfig();
			_buildingTemplate =
				Instantiate(_buildingPrefab, Vector3.up * 100, Quaternion.identity, _buildingsParent);
			_buildingTemplate.Init(config, _gameModel.Config.CellSize);
			_buildingTemplate.SetColorMode(BuildingView.ColorMode.TransparentValid);
		}

		private BuildingConfig PickBuildingConfig()
		{
			var rnd = Random.Range(0, _gameModel.Config.Buildings.Length);
			return _gameModel.Config.Buildings[rnd];
		}

		private Game _gameModel;
		private Camera _mainCamera;
		private GameMode _currentMode;
		private BuildingView _buildingTemplate;
		private bool _isPositionValid;
		private int _currentX;
		private int _currentY;
	}
}