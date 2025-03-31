using System.Collections.Generic;
using CrazyPawns.Scripts.Base;
using DI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrazyPawn.Game.Gameplay
{
    public class SphereManager : MonoBehaviour
    {
        [SerializeField] private LineRenderer _linePrefab;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private  Material _activeMaterial;

        private List<SpherePawn> _allSpheres = new List<SpherePawn>();
        private SpherePawn _selectedSphere;
        private DIContainer _container;
        private EventAggregator _eventAggregator;
        
        private Dictionary<(SpherePawn, SpherePawn), LineRenderer> _connections = new();

        public void Initialize(DIContainer container)
        {
            _container = container;
            
            _eventAggregator = _container.Resolve<EventAggregator>(); 
            
            _eventAggregator.Subscribe<EventsProvider.RegisterSphereEvent>(RegisterSphere);
            _eventAggregator.Subscribe<EventsProvider.ClickSphereEvent>(OnSphereClicked);
        }
        
        private void RegisterSphere(EventsProvider.RegisterSphereEvent registerSphereObject)
        {
            _allSpheres.Add(registerSphereObject.RegisterSphere);
        }

        private void OnSphereClicked(EventsProvider.ClickSphereEvent clickedSphereObject)
        {
            if (_selectedSphere == null)
            {
                _selectedSphere = clickedSphereObject.ClickedSphere;
                HighlightSpheres(true);
            }
            else if (_selectedSphere != clickedSphereObject.ClickedSphere)
            {
                CreateConnection(_selectedSphere, clickedSphereObject.ClickedSphere);
                HighlightSpheres(false);
                _selectedSphere = null;
            }
        }

        private void HighlightSpheres(bool highlight)
        {
            foreach (var sphere in _allSpheres)
            {
                if (sphere != _selectedSphere && sphere.transform.parent != _selectedSphere.transform.parent)
                {
                    sphere.Renderer.material = highlight ? _activeMaterial : _defaultMaterial;
                }
            }
        }

        private void CreateConnection(SpherePawn sphereA, SpherePawn sphereB)
        {
            if (sphereA.transform.parent == sphereB.transform.parent)
                return;
            
            var key = (sphereA, sphereB);
            var reverseKey = (sphereB, sphereA);
            
            if (_connections.ContainsKey(key) || _connections.ContainsKey(reverseKey)) 
                return;
            
            var line = Instantiate(_linePrefab, transform);
            line.material = new Material(Shader.Find("Unlit/Color")) { color = Color.white };
            line.startWidth = 0.07f;
            line.endWidth = 0.07f;
            line.useWorldSpace = true;

            line.SetPosition(0, sphereA.transform.position);
            line.SetPosition(1, sphereB.transform.position);

            _connections[key] = line;

            foreach (var conection in _connections)
            {
                
            }
        }

        private void LateUpdate()
        {
            foreach (var kvp in _connections)
            {
                var line = kvp.Value;
                var (sphereA, sphereB) = kvp.Key;
                if (line != null)
                {
                    line.SetPosition(0, sphereA.transform.position);
                    line.SetPosition(1, sphereB.transform.position);
                }
            }
        }
    }
}