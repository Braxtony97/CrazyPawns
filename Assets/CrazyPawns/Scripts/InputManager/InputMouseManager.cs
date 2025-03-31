using CrazyPawn.Game.Gameplay;
using CrazyPawn.Game.Gameplay.Interfaces;
using CrazyPawns.Scripts.Base;
using DI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace CrazyPawns.Scripts.InputManager
{
    public class InputMouseManager : MonoBehaviour
    {
        private Camera _camera;
        private DIContainer _diContainer;
        private EventAggregator _eventAgregator;
        
        [SerializeField] private DragDrop _dragDrop;
        
        public void Initialize(DIContainer diContainer, Camera camera, EventAggregator eventAgregator)
        {
            _diContainer = diContainer;
            _camera = camera;
            _eventAgregator = eventAgregator;
        }
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                _eventAgregator.Publish(new EventsProvider.MouseDownEvent());
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit))
                {
                    _eventAgregator.Publish(new EventsProvider.MouseDownWithRayEvent(ray));
                }
            }
            
            if (Input.GetMouseButton(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                
                if (Physics.Raycast(ray, out var hit))
                {
                    _eventAgregator.Publish(new EventsProvider.MouseHoldEvent());
                    _eventAgregator.Publish(new EventsProvider.MouseHoldWithRayEvent(ray));
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _eventAgregator.Publish(new EventsProvider.MouseUpEvent());
            }
            
            
        }

        
    }
}