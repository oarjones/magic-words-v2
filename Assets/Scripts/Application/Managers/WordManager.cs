using System.Collections.Generic;
using MagicWords.Domain.Data.Models;
using MagicWords.Domain.Logic.Services;
using MagicWords.Application.EventChannels;
using UnityEngine;
using System.Linq;
using MagicWords.Application.Managers;

namespace MagicWords.Application.Managers
{
    public class WordManager : MonoBehaviour
    {
        [SerializeField] private DictionaryService dictionaryService;
        [SerializeField] private ScoreService scoreService;
        [SerializeField] private MoveValidator moveValidator;

        public void ValidateWord(List<Cell> word, string playerId, Match match)
        {
            string wordString = ConvertCellsToString(word);
            bool isValid = dictionaryService.IsValidWord(wordString);

            if (isValid)
            {
                int score = scoreService.CalculateScore(word);
                match.AddValidatedWord(playerId, word);
                match.UpdatePlayerScore(playerId, score);
                // gameEventsChannel.OnWordValidated(word, playerId, score);  //Lo comentamos temporalmente

            }
            else
            {
                // gameEventsChannel.OnWordNotValidated(word, playerId); // Evento para palabra no válida. Lo comentamos temporalmente
            }
            match.ClearPlayerWord(playerId);
        }

        public bool IsValidMove(Cell currentCell, Cell nextCell)
        {
            return moveValidator.IsValidMove(currentCell, nextCell);
        }

        private string ConvertCellsToString(List<Cell> cells)
        {
            return new string(cells.Select(c => c.Letter).ToArray());
        }
    }
}