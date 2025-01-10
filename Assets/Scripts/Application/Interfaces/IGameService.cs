using System.Collections.Generic;
using MagicWords.Domain.Data.Models;

namespace MagicWords.Application.Interfaces
{
    public interface IGameService
    {
        void StartGame(string player1Id, GameMode gameMode, string objective, float maxTime, string boardSetup);
        void CreateBoard(string boardSetup);
        void HandleCellSelection(Cell cell, string playerId);
        void ValidateWord(List<Cell> word, string playerId);
        void EndGame();
    }
}