using CrazyPawns.Scripts.InputManager;
using DI;
using UnityEngine;

namespace CrazyPawns.Scripts.Base
{
    public class InputManagerProvider : MonoBehaviour
    {
        public InputMouseManager InputMouseManager => _inputMouseManager;
        public InputKeyboardManager InputKeyboardManager => _inputKeyboardManager;
        
        [SerializeField] private InputMouseManager _inputMouseManager;
        [SerializeField] private InputKeyboardManager _inputKeyboardManager;
        
        private Camera _camera;
        private DIContainer _diContainer;
        private EventAggregator _eventAgregator;

        public void Initialize(DIContainer diContainer)
        {
            _diContainer = diContainer;
        }

        void Start()
        {
            _camera = FindObjectOfType<Camera>();
            _eventAgregator = _diContainer.Resolve<EventAggregator>();
            _inputMouseManager.Initialize(_diContainer, _camera, _eventAgregator);
        }
    }
}
