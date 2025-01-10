using UnityEngine;
using System;
using System.Collections.Generic;
using MagicWords.Domain.Data.Models;

namespace MagicWords.Application.EventChannels
{
    [CreateAssetMenu(menuName = "Events/Game Events Channel")]
    public class GameEventsChannelSO : ScriptableObject
    {
        public event Action<string> OnMatchCreated;
        public event Action<Match> OnMatchStarted;
        public event Action<string> OnMatchEnded;
        public event Action<List<Cell>, string, int> OnWordValidated;
        public event Action<List<Cell>, string> OnWordNotValidated;
        public event Action<Cell, string> OnPlayerSelectedCell;


        public void RaiseMatchCreated(string matchId)
        {
            OnMatchCreated?.Invoke(matchId);
        }
        public void RaiseMatchStarted(Match match)
        {
            OnMatchStarted?.Invoke(match);
        }

        public void RaiseMatchEnded(string winnerPlayerId)
        {
            OnMatchEnded?.Invoke(winnerPlayerId);
        }

        public void RaiseWordValidated(List<Cell> word, string playerId, int score)
        {
            OnWordValidated?.Invoke(word, playerId, score);
        }

        public void RaiseWordNotValidated(List<Cell> word, string playerId)
        {
            OnWordNotValidated?.Invoke(word, playerId);
        }

        public void RaisePlayerSelectedCell(Cell cell, string playerId)
        {
            OnPlayerSelectedCell?.Invoke(cell, playerId);
        }
    }
}