using DI;
using UnityEngine;
using UnityEngine.Serialization;

namespace CrazyPawn.Game.Gameplay
{
    public class PawnSpawner : MonoBehaviour
    {
        [SerializeField] private CrazyPawnSettings _settings;
        [SerializeField] private GameObject _pawnPrefab;
        
        private DIContainer _container;

        public void Spawn(DIContainer container)
        {
            _container = container;
            
            SpawnPawns();
        }

        private void SpawnPawns()
        { 
            for (var i = 0; i < _settings.InitialPawnCount; i++)
            {
                var spawnPosition = GetRandomPositionInCircle(_settings.InitialZoneRadius);
                var newPawn = Instantiate(_pawnPrefab, spawnPosition, Quaternion.identity, transform);
                
                if (newPawn.TryGetComponent<Pawn>(out var pawn))
                {
                    pawn.Initialize(_container);
                }
            }
        }

        private Vector3 GetRandomPositionInCircle(float radius)
        {
            var angle = Random.Range(0f, Mathf.PI * 2);
            var distance = Mathf.Sqrt(Random.Range(0f, 1f)) * radius;

            var x = distance * Mathf.Cos(angle);
            var z = distance * Mathf.Sin(angle);
            return new Vector3(x, 0, z);
        }
    }
}