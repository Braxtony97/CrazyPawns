using UnityEngine;

namespace CrazyPawns.Scripts.Game.GameRoot
{
    public class UIRootView : MonoBehaviour
    {
        [SerializeField] private GameObject _loadingScreen;

        private void Awake()
        {
            _loadingScreen.SetActive(false);
        }

        public void Show()
        {
            _loadingScreen.SetActive(true);
        }

        public void Hide()
        {
            _loadingScreen.SetActive(false);
        }
    }
}