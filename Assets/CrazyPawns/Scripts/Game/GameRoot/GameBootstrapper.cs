using System.Collections;
using CrazyPawn.Base;
using CrazyPawn.Game.SceneEntryPoint;
using CrazyPawn.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CrazyPawns.Scripts.Game.GameRoot
{
    public class GameBootstrapper
    {
        private readonly Coroutines _coroutines;
        private static GameBootstrapper _instance;
        private UIRootView _uiRootView;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void OnBeforeSceneLoad()
        {
            Application.targetFrameRate = 60;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
                
            _instance = new GameBootstrapper();
            _instance.StartGame();
        }

        private GameBootstrapper()
        {
            _coroutines = new GameObject("[Coroutines]").AddComponent<Coroutines>();
            Object.DontDestroyOnLoad(_coroutines);
            
            var prefabUIRoot = Resources.Load<UIRootView>("UIRoot");
            _uiRootView = Object.Instantiate(prefabUIRoot);
            Object.DontDestroyOnLoad(_uiRootView.gameObject);
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

            yield return new WaitForSeconds(2f);

            var sceneEntryPoint = Object.FindObjectOfType<GameplayEntryPoint>();
            sceneEntryPoint.Run();
            
            _uiRootView.Hide();
        }

        private IEnumerator LoadScene(string sceneName)
        {
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}