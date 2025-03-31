using System.Collections;
using System.Collections.Generic;
using CrazyPawn.Game.Gameplay.Interfaces;
using CrazyPawns.Scripts.Base;
using CrazyPawns.Scripts.Game.Gameplay.Interfaces;
using DI;
using UnityEngine;

public class SpherePawn : MonoBehaviour, IClickable, IReleasable
{
    public Renderer Renderer;
    
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _activeMaterial;
    
    private DIContainer _container;
    private EventAggregator _eventAggregator;

    public void Initialize(DIContainer container)
    {
        _container = container;

        _eventAggregator = _container.Resolve<EventAggregator>();
        _eventAggregator.Publish(new EventsProvider.RegisterSphereEvent(this));
    }

    public void OnClick()
    {
        _eventAggregator.Publish(new EventsProvider.ClickSphereEvent(this));
    }

    public void OnRelease()
    {
    }
}
