using UnityEngine;

namespace CrazyPawn.Game.Gameplay
{
    public class BoardCreator : MonoBehaviour
    {
        [SerializeField] private CrazyPawnSettings _settings;

        public void CreateBoard()
        {
            CreateCheckerboard();
        }

        private void CreateCheckerboard()
        {
            var boardSize = _settings.CheckerboardSize;
            var cellSize = 1.5f; 
            
            var whiteColor = _settings.WhiteCellColor;
            var blackColor = _settings.BlackCellColor;
            
            for (var x = 0; x < boardSize; x++)
            {
                for (var z = 0; z < boardSize; z++)
                {
                    var cellPosition = new Vector3(x * cellSize - (boardSize * cellSize / 2), 0,
                        z * cellSize - (boardSize * cellSize / 2));

                    var cell = GameObject.CreatePrimitive(PrimitiveType.Plane);
                    cell.transform.SetParent(transform);
                    cell.transform.position = cellPosition;
                    cell.transform.rotation = Quaternion.Euler(0, 0, 0);
            
                    var isWhiteCell = (x + z) % 2 == 0;
                    cell.GetComponent<Renderer>().material.color = isWhiteCell ? whiteColor : blackColor;
                    
                    cell.transform.localScale = new Vector3(cellSize / 10f, 1, cellSize / 10f);
                }
            }
        }
    }
}