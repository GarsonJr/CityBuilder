using System;
using System.Collections;
using System.Collections.Generic;
using CityBuilder.Model;
using UnityEngine;

namespace CityBuilder.View
{
	public class CityView : MonoBehaviour
	{
		[SerializeField] private Builder _builder;
		[SerializeField] private Transform _ground;
		[SerializeField] private Transform _cellsParent;
		[SerializeField] private CellView _cellPrefab;
		[SerializeField] private int _cellsInstantiatedPerFrame = 200;

		public event Action<BuildingView> Built;

		public IReadOnlyList<BuildingView> Buildings => _buildings;

		public void Init(GameView gameView, Game gameModel, CameraController cameraController, InputHandler inputHandler)
		{
			_game = gameModel;
			_builder.Init(gameView, gameModel, cameraController, inputHandler);
			_buildings = new List<BuildingView>();
			gameView.GameModeChanged += OnGameModeChanged;
		}

		public IProgress Create()
		{
			var instantiationProgress = new InstantiationProgress(_game.Config.CityLength * _game.Config.CityWidth);
			StartCoroutine(InstantiateCity(instantiationProgress));
			return instantiationProgress;
		}

		private void OnGameModeChanged(GameMode mode)
		{
			_currentMode = mode;

			var buildingMode = mode == GameMode.Building;
			foreach (var cell in _cells)
			{
				cell.SetColorMode(buildingMode);
			}

			foreach (var building in _buildings)
			{
				building.SetColorMode(buildingMode ? BuildingView.ColorMode.TransparentValid : BuildingView.ColorMode.Base);
			}

			if (buildingMode)
				_builder.Built += OnBuilt;
			else
				_builder.Built -= OnBuilt;
		}

		private void OnBuilt(BuildingView building, int x, int y)
		{
			for (int i = x; i < x + building.Config.Width; i++)
			{
				for (int j = y; j < y + building.Config.Length; j++)
				{
					_cells[i, j].SetColorMode(true);
				}
			}

			_buildings.Add(building);
			Built?.Invoke(building);
		}

		private IEnumerator InstantiateCity(InstantiationProgress progress)
		{
			var cityWidth = _game.Config.CellSize * _game.Config.CityWidth;
			var cityLength = _game.Config.CellSize * _game.Config.CityLength;
			_ground.localScale = new Vector3(cityWidth, 1, cityLength);

			var counter = 0;
			_cells = new CellView[_game.Config.CityWidth, _game.Config.CityLength];
			for (int i = 0; i < _game.Config.CityWidth; i++)
			{
				for (int j = 0; j < _game.Config.CityLength; j++)
				{
					if (counter >= _cellsInstantiatedPerFrame)
					{
						progress.AddLoadedCells(counter);
						counter = 0;
						yield return null;
					}

					var position = new Vector3(
						i * _game.Config.CellSize - cityWidth / 2 + _game.Config.CellSize / 2,
						0.001f, j * _game.Config.CellSize - cityLength / 2 + _game.Config.CellSize / 2);
					var instance = Instantiate(_cellPrefab, position, Quaternion.identity, _cellsParent);
					instance.Init(_game.City.Cells[i, j]);
					_cells[i, j] = instance;
					++counter;
				}
			}

			progress.AddLoadedCells(counter);
		}

		private class InstantiationProgress : IProgress
		{
			public InstantiationProgress(int totalCells)
			{
				_totalCells = totalCells;
			}

			public void AddLoadedCells(int count) => _currentlyLoadedCells += count;

			public float GetProgress() => _currentlyLoadedCells / (float) _totalCells;

			private readonly int _totalCells;

			private int _currentlyLoadedCells;
		}

		private Game _game;
		private GameMode _currentMode;
		private CellView[,] _cells;
		private List<BuildingView> _buildings;
	}
}