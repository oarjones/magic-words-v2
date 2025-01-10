using UnityEngine;
using MagicWords.Application.Managers;
using MagicWords.Domain.Data.Models;
using MagicWords.Application.EventChannels;
using MagicWords.Application.Interfaces;
using MagicWords.Application.Services;
using System.Collections.Generic;

namespace MagicWords.Application.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FirebaseManager firebaseManager;
        [SerializeField] private UIEventsChannelSO uiEventsChannel;
        [SerializeField] private GameEventsChannelSO gameEventsChannel;
        [SerializeField] private WordManager wordManager;
        [SerializeField] private AlgorithmManager algorithmManager;

        public GameMode gameMode; //PvP or PvA
        public float maxTime;
        public string objective;
        public string boardSetup;

        private Match currentMatch;
        private IGameService gameService;

        private void Start()
        {
            // Suscribirse a eventos de UI
            uiEventsChannel.OnStartMatch += HandleStartMatch;
            gameEventsChannel.OnMatchCreated += HandleMatchCreated;
            gameEventsChannel.OnMatchStarted += HandleMatchStarted;
        }

        private void OnDestroy()
        {
            // Desuscribirse de eventos
            uiEventsChannel.OnStartMatch -= HandleStartMatch;
            gameEventsChannel.OnMatchCreated -= HandleMatchCreated;
            gameEventsChannel.OnMatchStarted -= HandleMatchStarted;
        }

        private async void HandleStartMatch()
        {
            // Seleccionar el servicio adecuado según el modo de juego
            if (gameMode == GameMode.PvP)
            {
                gameService = new FirebaseGameService(firebaseManager, gameEventsChannel);
            }
            else
            {
                gameService = new LocalGameService(this, gameEventsChannel, wordManager, algorithmManager);
            }

            // Iniciar el juego usando el servicio seleccionado
            gameService.StartGame(firebaseManager.UserId, gameMode, objective, maxTime, boardSetup);
        }

        private void HandleMatchCreated(string matchId)
        {
            //Crear la partida local si es necesario
            if (gameMode == GameMode.PvP)
            {
                currentMatch = new Match(matchId, firebaseManager.UserId, gameMode, objective, maxTime, boardSetup);
            }
            gameService.CreateBoard(boardSetup);
        }

        public void HandleMatchStarted(Match match)
        {
            currentMatch = match;
            //Crear el tablero
            CreateBoard();
        }

        public void HandleCellSelection(Cell cell, string playerId)
        {
            if (gameService != null)
            {
                gameService.HandleCellSelection(cell, playerId);
            }
        }

        public void HandleValidateWord(List<Cell> word, string playerId)
        {
            if (gameService != null)
            {
                gameService.ValidateWord(word, playerId);
            }
        }

        public void PauseMatch()
        {
            // Pausar la partida
        }

        public void ResumeMatch()
        {
            // Reanudar la partida
        }

        public void FinishMatch()
        {
            // Finalizar la partida
            currentMatch.FinishMatch();
            if (gameService != null)
            {
                gameService.EndGame();
            }
        }

        private void CreateBoard()
        {
            // Lógica para crear el tablero de juego
            // Crear un tablero de ejemplo (puedes modificar esto para generar tableros aleatorios o personalizados)
            Dictionary<int, Dictionary<int, Cell>> cells = new Dictionary<int, Dictionary<int, Cell>>();

            // Ejemplo de creación de un tablero hexagonal
            for (int i = -2; i <= 2; i++)
            {
                cells[i] = new Dictionary<int, Cell>();
                for (int j = -2; j <= 2; j++)
                {
                    if (i == -2 && j == 2) continue;
                    if (i == -1 && j == 2) continue;
                    if (i == 2 && j == -2) continue;
                    if (i == 1 && j == -2) continue;
                    //Asignar una letra aleatoria a cada celda
                    char randomLetter = (char)('A' + Random.Range(0, 26));
                    cells[i][j] = new Cell(randomLetter, i, j);
                }
            }

            Board board = new Board(cells);
            currentMatch.SetBoard(board);

            // Notificar a UIManager que el tablero está listo
            FindFirstObjectByType<UIManager>().CreateBoard(board); // Asegúrate de que UIManager exista en la escena
        }

    }
}