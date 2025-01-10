using System.Collections.Generic;
using MagicWords.Application.EventChannels;
using MagicWords.Application.Managers;
using MagicWords.Domain.Data.Models;
using MagicWords.Application.Interfaces;
using System;

namespace MagicWords.Application.Services
{
    public class FirebaseGameService : IGameService
    {
        private FirebaseManager firebaseManager;
        private GameEventsChannelSO gameEventsChannel;

        public FirebaseGameService(FirebaseManager firebaseManager, GameEventsChannelSO gameEventsChannel)
        {
            this.firebaseManager = firebaseManager;
            this.gameEventsChannel = gameEventsChannel;
        }

        public void StartGame(string player1Id, GameMode gameMode, string objective, float maxTime, string boardSetup)
        {
            // Crea la partida en Firebase
            firebaseManager.CreateMatch(player1Id, gameMode, objective, maxTime, boardSetup);
        }

        public void CreateBoard(string boardSetup)
        {
            // Obtener la configuración del tablero de Firebase y crearlo
        }

        public void HandleCellSelection(Cell cell, string playerId)
        {
            // Enviar la celda seleccionada a Firebase
            firebaseManager.SendCellSelected(cell, playerId);
        }

        public void ValidateWord(List<Cell> word, string playerId)
        {
            // Enviar la palabra a validar a Firebase
            firebaseManager.SendWordValidated(word, playerId, 0); // El puntaje se calculará en la Cloud Function por seguridad
        }

        public void EndGame()
        {
            // Finalizar la partida en Firebase
            firebaseManager.EndMatch();
        }
    }
}