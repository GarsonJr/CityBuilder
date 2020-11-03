using CityBuilder.Config;
using CityBuilder.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CityBuilder.View
{
    public class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private TextAsset _configJson;
        [SerializeField] private LoadingBar _loadingBar;

        private void Awake()
        {
            var config = JsonUtility.FromJson<GameConfig>(_configJson.text);

            _game = new Game(config);
            _loadingBar.SetProgress(0);

            var loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);
            loading.completed += OnGameSceneLoaded;
        }

        private void OnGameSceneLoaded(AsyncOperation asyncOperation)
        {
            var gameManager = FindObjectOfType<GameView>();
            var progress = gameManager.Initialize(_game);
            _loadingBar.Subscribe(progress);
            _loadingBar.LoadingCompleted += UnloadBootstrapScene;
        }

        private void UnloadBootstrapScene()
        {
            SceneManager.UnloadSceneAsync(0);
        }

        private Game _game;
    }
}