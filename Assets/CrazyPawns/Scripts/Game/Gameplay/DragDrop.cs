using CrazyPawn.Game.Gameplay.Interfaces;
using CrazyPawns.Scripts.Base;
using CrazyPawns.Scripts.Game.Gameplay.Interfaces;
using DI;
using UnityEngine;

namespace CrazyPawn.Game.Gameplay
{
    public class DragDrop : MonoBehaviour
    {
        private DIContainer _diContainer;
        private EventAggregator _eventAgregator;
        private bool _isDragging = false;
        private IMoveable  _draggingObject;
        private IClickable _clickableObject;
        private IReleasable _releasableObject;

        public void Initialize(DIContainer diContainer)
        {
            _diContainer = diContainer;

            _eventAgregator = _diContainer.Resolve<EventAggregator>();

            _eventAgregator.Subscribe<EventsProvider.MouseDownWithRayEvent>(HandleMouseDown) ;
            _eventAgregator.Subscribe<EventsProvider.MouseHoldWithRayEvent>(HandleMouseHold);
            _eventAgregator.Subscribe<EventsProvider.MouseUpEvent>(HandleMouseUp);
        }

        private void HandleMouseDown(EventsProvider.MouseDownWithRayEvent mouseDownEvent)
        {
            _isDragging = false;
            
            if (_releasableObject != null)
            {
                _releasableObject.OnRelease();
            }

            var ray = mouseDownEvent.Ray;

            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.gameObject.TryGetComponent<IClickable>(out var iClickable) && !hit.collider.gameObject.TryGetComponent<IMoveable>(out _))
                {
                    iClickable.OnClick();

                    if (hit.collider.gameObject.TryGetComponent<IReleasable>(out var iReleasable))
                    {
                        _releasableObject = iReleasable;
                    }
                    
                    return;
                }

                if (hit.collider.gameObject.TryGetComponent<IMoveable>(out var iMoveable))
                {
                    _draggingObject = iMoveable;
                }
                else
                {
                    var parentMoveable = hit.collider.transform.GetComponentInParent<IMoveable>();
                    
                    if (parentMoveable != null)
                    {
                        _draggingObject = parentMoveable;
                    }
                }
            }
        }

        private void HandleMouseHold(EventsProvider.MouseHoldWithRayEvent mouseHoldWithRayEvent)
        {
            if (_draggingObject == null) 
                return;

            if (!_isDragging)
            {
                _isDragging = true; 
                return;
            }

            _draggingObject.Move(mouseHoldWithRayEvent.Ray); 
        }
        
        private void HandleMouseUp(EventsProvider.MouseUpEvent mouseUpEvent)
        {
            _draggingObject = null;
            _isDragging = false;
        }
    }
}