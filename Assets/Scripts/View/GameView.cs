using System;
using CityBuilder.Model;
using UnityEngine;

namespace CityBuilder.View
{
	public class GameView : MonoBehaviour
	{
		[SerializeField] private CameraController _camera;
		[SerializeField] private CityView _city;
		[SerializeField] private InputHandler _inputHandler;
		[SerializeField] private GameUI _gameUI;

		public event Action<GameMode> GameModeChanged;
		public GameMode CurrentMode { get; private set; }

		public IProgress Initialize(Game game)
		{
			_game = game;
			_camera.Init(_inputHandler, _game.Config);
			_city.Init(this, _game, _camera, _inputHandler);
			_gameUI.Init(_game.City, _city, _camera);
			_inputHandler.EscapePressed += OnEscapePressed;
			return _city.Create();
		}

		public void EnterMode(int mode) => EnterMode((GameMode) mode);

		public void EnterMode(GameMode mode)
		{
			if (CurrentMode == mode)
				return;

			CurrentMode = mode;
			GameModeChanged?.Invoke(mode);
		}

		private void OnEscapePressed()
		{
			if (CurrentMode == GameMode.Building)
				EnterMode(GameMode.Base);
		}

		private Game _game;
	}

	public enum GameMode
	{
		Base,
		Building
	}
}