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

        public void GenerateMove(Match match, Board board)
        {
            // Lógica para generar un movimiento para la IA
            // 1. Obtener todas las celdas disponibles
            List<Cell> availableCells = GetAllAvailableCells(board);

            // 2. Si no hay celdas disponibles, terminar el turno de la IA
            if (availableCells.Count == 0)
            {
                Debug.Log("No hay celdas disponibles para la IA.");
                return;
            }

            // 3. Intentar formar una palabra
            List<Cell> word = FindWord(availableCells, board, match);

            // 4. Si se encuentra una palabra, enviarla
            if (word.Count > 0)
            {
                wordManager.ValidateWord(word, match.Player2Id, match);
                match.ClearPlayerWord(match.Player2Id);
                Debug.Log("Palabra encontrada por la IA: " + ConvertCellsToString(word));
                return;
            }

            // 5. Si no se encuentra una palabra, realizar un movimiento aleatorio
            Cell randomCell = availableCells[Random.Range(0, availableCells.Count)];
            match.AddPlayerWord(match.Player2Id, randomCell);
            Debug.Log("Movimiento aleatorio de la IA: " + randomCell.Letter);
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

        private List<Cell> FindWord(List<Cell> availableCells, Board board, Match match)
        {
            // Intenta encontrar una palabra válida en el tablero
            List<Cell> bestWord = new List<Cell>();

            // Todas las permutaciones posibles de las celdas disponibles
            foreach (var permutation in Permute(availableCells))
            {
                List<Cell> currentWord = new List<Cell>();
                foreach (var cell in permutation)
                {
                    if (IsAdjacentToLast(cell, currentWord, board))
                    {
                        currentWord.Add(cell);
                        string wordString = ConvertCellsToString(currentWord);
                        if (wordManager.ValidateWord(currentWord, match.Player2Id, match))
                        {
                            // Si la palabra actual es más larga que la mejor palabra encontrada hasta ahora, actualizar la mejor palabra
                            if (currentWord.Count > bestWord.Count)
                            {
                                bestWord = new List<Cell>(currentWord); // Crear una nueva lista para evitar referencias
                            }
                        }
                    }
                }
            }
            return bestWord;
        }

        private bool IsAdjacentToLast(Cell cell, List<Cell> currentWord, Board board)
        {
            //Comprueba si la celda es adyacente a la última celda de la palabra actual
            if (currentWord.Count == 0)
            {
                return true; // Cualquier celda es válida si la palabra está vacía
            }

            Cell lastCell = currentWord.Last();
            return board.GetAdjacentCells(lastCell).Contains(cell);
        }

        private string ConvertCellsToString(List<Cell> cells)
        {
            return new string(cells.Select(c => c.Letter).ToArray());
        }

        // Método para obtener todas las permutaciones de una lista
        private IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> sequence)
        {
            if (sequence == null)
            {
                yield break;
            }

            var list = sequence.ToList();

            if (!list.Any())
            {
                yield return Enumerable.Empty<T>();
            }
            else
            {
                var startingElementIndex = 0;

                foreach (var startingElement in list)
                {
                    var index = startingElementIndex;
                    var remainingItems = list.Where((e, i) => i != index);

                    foreach (var permutationOfRemainder in Permute(remainingItems))
                    {
                        yield return Concat(startingElement, permutationOfRemainder);
                    }

                    startingElementIndex++;
                }
            }
        }

        // Método auxiliar para concatenar un elemento a una secuencia
        private IEnumerable<T> Concat<T>(T firstElement, IEnumerable<T> secondSequence)
        {
            yield return firstElement;
            if (secondSequence == null)
            {
                yield break;
            }

            foreach (var item in secondSequence)
            {
                yield return item;
            }
        }
    }
}