using MagicWords.Domain.Data.Models;

namespace MagicWords.Domain.Logic.Services
{
    public class MoveValidator
    {
        public bool IsValidMove(Cell currentCell, Cell nextCell)
        {
            // Implementar la lógica de validación de movimientos aquí.
            // Por ejemplo, comprobar si las celdas son adyacentes y si la celda nextCell no está ocupada.

            if (currentCell == null || nextCell == null)
            {
                return false;
            }

            if (nextCell.State == CellState.Blocked || nextCell.State == CellState.TempBlocked)
            {
                return false;
            }

            // Comprobar si las celdas son adyacentes (esto ya está implementado en Board.GetAdjacentCells)
            //Podriamos usar Board, pero en ese caso no seria un servicio puro
            int x = currentCell.X;
            int y = currentCell.Y;

            // Coordenadas de las celdas adyacentes
            int[,] adjacentCoords = {
                { x, y + 1 }, { x + 1, y }, { x + 1, y - 1 },
                { x, y - 1 }, { x - 1, y }, { x - 1, y + 1 }
            };
            for (int i = 0; i < adjacentCoords.GetLength(0); i++)
            {
                if (adjacentCoords[i, 0] == nextCell.X && adjacentCoords[i, 1] == nextCell.Y)
                {
                    return true;
                }
            }

            return false;
        }
    }
}