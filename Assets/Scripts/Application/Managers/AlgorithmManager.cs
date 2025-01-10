using System.Collections.Generic;
using MagicWords.Domain.Data.Models;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using MagicWords.Application.Managers;
using MagicWords.Domain.Logic.Services;

namespace MagicWords.Application.Managers
{
    public class AlgorithmManager : MonoBehaviour
    {
        [SerializeField] private WordManager wordManager;
        [SerializeField] private DictionaryService dictionaryService;

        private List<string> currentWord = new List<string>();
        private List<string> bestWord = new List<string>();
        private List<Cell> exploredCells = new List<Cell>();

        public void GenerateMove(Match match, Board board)
        {
            //Reiniciar las variables para la búsqueda de palabras
            currentWord.Clear();
            bestWord.Clear();
            exploredCells.Clear();
            // 1. Obtener todas las celdas disponibles
            List<Cell> availableCells = GetAllAvailableCells(board);

            // 2. Si no hay celdas disponibles, terminar el turno de la IA
            if (availableCells.Count == 0)
            {
                Debug.Log("No hay celdas disponibles para la IA.");
                return;
            }

            // 3. Buscar la mejor palabra en el tablero
            foreach (Cell cell in availableCells)
            {
                CheckCell(cell, board, match);
            }

            // 4. Si se encuentra una palabra, enviarla
            if (bestWord.Count > 0)
            {
                List<Cell> wordCells = bestWord.Select(cellLetter => availableCells.FirstOrDefault(c => c.Letter.ToString() == cellLetter)).ToList();
                wordManager.ValidateWord(wordCells, match.Player2Id, match);
                match.ClearPlayerWord(match.Player2Id);
                Debug.Log("Palabra encontrada por la IA: " + ConvertCellsToString(wordCells));
                return;
            }

            // 5. Si no se encuentra una palabra, realizar un movimiento aleatorio
            Cell randomCell = availableCells[Random.Range(0, availableCells.Count)];
            match.AddPlayerWord(match.Player2Id, randomCell);
            Debug.Log("Movimiento aleatorio de la IA: " + randomCell.Letter);
        }
        private void CheckCell(Cell cell, Board board, Match match)
        {
            //Comprueba si la celda ya ha sido explorada
            if (exploredCells.Contains(cell))
            {
                return;
            }

            //Añadir la letra de la celda actual a la palabra actual
            currentWord.Add(cell.Letter.ToString());
            exploredCells.Add(cell);

            //Comprobar si la palabra actual es una palabra válida
            string currentWordString = string.Join("", currentWord);
            if (dictionaryService.IsValidWord(currentWordString))
            {
                //Si es una palabra válida y es más larga que la mejor palabra encontrada hasta ahora, actualizar la mejor palabra
                if (currentWord.Count > bestWord.Count)
                {
                    bestWord = new List<string>(currentWord);
                }
            }

            //Comprobar si la palabra actual es un prefijo válido
            if (dictionaryService.IsValidPrefix(currentWordString))
            {
                // Si es un prefijo válido, seguir buscando en las celdas adyacentes
                List<Cell> adjacentCells = board.GetAdjacentCells(cell);
                foreach (Cell adjacentCell in adjacentCells)
                {
                    CheckCell(adjacentCell, board, match); // Llamada recursiva
                }
            }

            //Retroceder: eliminar la última letra y marcar la celda como no explorada
            currentWord.RemoveAt(currentWord.Count - 1);
            exploredCells.Remove(cell);
        }


        private List<Cell> GetAllAvailableCells(Board board)
        {
            //Devuelve una lista de todas las celdas que no estén bloqueadas
            List<Cell> availableCells = new List<Cell>();
            foreach (var row in board.Cells)
            {
                foreach (var cell in row.Value)
                {
                    if (cell.Value.State != CellState.Blocked)
                    {
                        availableCells.Add(cell.Value);
                    }
                }
            }
            return availableCells;
        }

        private string ConvertCellsToString(List<Cell> cells)
        {
            return new string(cells.Select(c => c.Letter).ToArray());
        }
    }
}