using System.Collections;
using CrazyPawn.Base;
using CrazyPawn.Game.SceneEntryPoint;
using CrazyPawn.Utils;
using CrazyPawns.Scripts.Base;
using DI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazyPawns.Scripts.Game.GameRoot
{
    public class GameBootstrapper
    {
        private readonly DIContainer _projectContainer = new();
        private readonly Coroutines _coroutines;
        private static GameBootstrapper _gameBootstrapper;
        private UIRootView _uiRootView;
        private InputManagerProvider _inputManagerProvider;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoad()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
                
            _gameBootstrapper = new GameBootstrapper();
            _gameBootstrapper.StartGame();
        }

        private GameBootstrapper()
        {
            _coroutines = new GameObject("[Coroutines]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines);
            
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRootView = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRootView.gameObject);
            _projectContainer.RegisterInstance(_uiRootView);
            
            var inputManager = Resources.Load<InputManagerProvider>("BaseInputManager");
            _inputManagerProvider = Object.Instantiate(inputManager);
            Object.DontDestroyOnLoad(_inputManagerProvider.gameObject);
            _projectContainer.RegisterInstance(_inputManagerProvider);
            _inputManagerProvider.Initialize(_projectContainer);
            
            _projectContainer.RegisterSingleton<EventAggregator>(eventAggregator => new EventAggregator());
        }

        private void StartGame()
        {
#if UNITY_EDITOR 
            var sceneName = SceneManager.GetActiveScene().name;

            if (sceneName == Constants.PAWNFIELD)
            {
                _coroutines.StartCoroutine(LoadAndStartGame());
                
                return;
            }
            
            if (sceneName != Constants.GAMEBOOTSTRAP)
            {
                return;
            }
#endif

            _coroutines.StartCoroutine(LoadAndStartGame());
        }

        private IEnumerator LoadAndStartGame()
        {
            _uiRootView.Show();
            
            yield return LoadScene(Constants.GAMEBOOTSTRAP);
            yield return LoadScene(Constants.PAWNFIELD);

            yield return new WaitForSeconds(0.5f);

            var sceneEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            var gameplaySceneContainer = new DIContainer(_projectContainer);
            sceneEntryPoint.Initialize(gameplaySceneContainer);
            
            _uiRootView.Hide();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}