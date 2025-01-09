using System.Collections.Generic;
using System.Linq;
using MagicWords.Domain.Logic.Services;

namespace MagicWords.Domain.Data.Models
{
    public class Board
    {
        public Dictionary<int, Dictionary<int, Cell>> Cells { get; private set; }

        public Board(Dictionary<int, Dictionary<int, Cell>> cells)
        {
            Cells = cells;
        }

        public List<Cell> GetAdjacentCells(Cell cell)
        {
            // Lógica para encontrar celdas adyacentes en un tablero hexagonal
            List<Cell> neighbors = new List<Cell>();
            int x = cell.X;
            int y = cell.Y;

            // Coordenadas de las celdas adyacentes
            int[,] adjacentCoords = {
                { x, y + 1 }, { x + 1, y }, { x + 1, y - 1 },
                { x, y - 1 }, { x - 1, y }, { x - 1, y + 1 }
            };

            for (int i = 0; i < adjacentCoords.GetLength(0); i++)
            {
                int adjX = adjacentCoords[i, 0];
                int adjY = adjacentCoords[i, 1];

                if (Cells.ContainsKey(adjX) && Cells[adjX].ContainsKey(adjY))
                {
                    neighbors.Add(Cells[adjX][adjY]);
                }
            }

            return neighbors;
        }

        public bool IsValidMove(Cell currentCell, Cell nextCell, MoveValidator moveValidator)
        {
            //Utiliza el MoveValidator para comprobar si el movimiento es válido
            return moveValidator.IsValidMove(currentCell, nextCell);
        }

        public Cell GetCell(int x, int y)
        {
            if (Cells.ContainsKey(x) && Cells[x].ContainsKey(y))
            {
                return Cells[x][y];
            }
            return null;
        }
    }
}