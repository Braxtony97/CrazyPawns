using CrazyPawn.Game.Gameplay.Interfaces;
using CrazyPawns.Scripts.Game.Gameplay.Interfaces;
using DI;
using UnityEngine;

namespace CrazyPawn.Game.Gameplay
{
    
    public class Pawn : MonoBehaviour, IClickable, IMoveable
    {
        [SerializeField] private SpherePawn[] _spheres;
        private DIContainer _container;

        public void Initialize(DIContainer container)
        {
            _container = container;

            foreach (var sphere in _spheres)
            {
                sphere.Initialize(_container);
            }
        }
        
        public void OnClick()
        {
        }

        public void OnClickWithRay(Ray ray)
        {
        }

        public void Move(Ray ray)
        {
            if (Physics.Raycast(ray, out var hit))
            {
                transform.position = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            }
        }
    }
}