using System;
using CrazyPawn.Game.Gameplay;
using CrazyPawns.Scripts.Base;
using CrazyPawns.Scripts.Game.GameRoot;
using DI;
using UnityEngine;

namespace CrazyPawn.Game.SceneEntryPoint
{
    public class GameplayEntryPoint : MonoBehaviour
    {
        [SerializeField] private DragDrop _dragDrop;
        [SerializeField] private PawnSpawner _pawnSpawner;
        [SerializeField] private SphereManager _sphereManager; 
        [SerializeField] private BoardCreator _boardCreator; 

        public void Initialize(DIContainer gameplaySceneContainer)
        {
            EnterGame(gameplaySceneContainer);
        }

        private void EnterGame(DIContainer gameplaySceneContainer)
        {
            _dragDrop.Initialize(gameplaySceneContainer);
            _sphereManager.Initialize(gameplaySceneContainer);
            _pawnSpawner.Spawn(gameplaySceneContainer);
            _boardCreator.CreateBoard();
        }
    }
}