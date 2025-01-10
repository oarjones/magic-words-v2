using System.Collections.Generic;
using MagicWords.Application.EventChannels;
using MagicWords.Application.Managers;
using MagicWords.Domain.Data.Models;
using MagicWords.Application.Interfaces;
using System;

namespace MagicWords.Application.Services
{
    public class LocalGameService : IGameService
    {
        private GameManager gameManager;
        private GameEventsChannelSO gameEventsChannel;
        private WordManager wordManager;
        private AlgorithmManager algorithmManager;
        private Match currentMatch;

        
        public LocalGameService(GameManager gameManager, GameEventsChannelSO gameEventsChannel, WordManager wordManager, AlgorithmManager algorithmManager)
        {
            this.gameManager = gameManager;
            this.gameEventsChannel = gameEventsChannel;
            this.wordManager = wordManager;
            this.algorithmManager = algorithmManager;
            
        }



        public void StartGame(string player1Id, GameMode gameMode, string objective, float maxTime, string boardSetup)
        {
            // Crear una partida local
            string matchId = Guid.NewGuid().ToString(); // Generar un ID único para la partida
            currentMatch = new Match(matchId, player1Id, gameMode, objective, maxTime, boardSetup);
            gameEventsChannel.RaiseMatchCreated(matchId);
            gameEventsChannel.RaiseMatchStarted(currentMatch);
        }

        public void HandleCellSelection(Cell cell, string playerId)
        {
            //Gestionar la celda seleccionada localmente (añadir a la palabra actual del jugador)
            currentMatch.AddPlayerWord(playerId, cell);

            //Comprobar si el movimiento es válido
            if (currentMatch.PlayerWords[playerId].Count > 1)
            {
                Cell lastCell = currentMatch.PlayerWords[playerId][currentMatch.PlayerWords[playerId].Count - 2];
                if (!wordManager.IsValidMove(lastCell, cell))
                {
                    currentMatch.ClearPlayerWord(playerId);
                }
            }
            if (playerId == currentMatch.Player2Id && currentMatch.Mode == GameMode.PvA)
            {
                // Si el jugador es la IA, generar un movimiento después de que haya seleccionado una celda
                algorithmManager.GenerateMove(currentMatch, currentMatch.Board);
            }
        }

        public void CreateBoard(string boardSetup)
        {
            // Crear el tablero localmente
            //Ya se crea el tableto en GameManager
            if (currentMatch.Mode == GameMode.PvA)
            {
                currentMatch.SetPlayer2("AIGamePlayer");
            }
        }

        public void ValidateWord(List<Cell> word, string playerId)
        {
            // Validar la palabra localmente
            wordManager.ValidateWord(word, playerId, currentMatch);
        }

        public void EndGame()
        {
            // Finalizar la partida localmente
            gameManager.FinishMatch();
        }
    }
}