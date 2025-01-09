using System;
using System.Collections.Generic;

namespace MagicWords.Domain.Data.Models
{
    public enum GameMode
    {
        PvP,
        PvA
    }

    [Serializable]
    public class Match
    {
        public string MatchId { get; set; }
        public Board Board { get; set; }
        public string Player1Id { get; set; } // ID del jugador 1
        public string Player2Id { get; set; } // ID del jugador 2 (o null si es PvA)
        public string CurrentPlayerId { get; set; } // ID del jugador que está realizando la acción
        public Dictionary<string, int> Scores { get; set; } // Puntuaciones de los jugadores
        public Dictionary<string, List<Cell>> PlayerWords { get; set; } // Palabras formadas por cada jugador
        public Dictionary<string, List<Cell>> ValidatedWords { get; set; } // Palabras válidas formadas por cada jugador
        public string LastPlayerId { get; set; }//Último jugador que ha realizado una acción
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public GameMode Mode { get; set; }
        public string WinnerPlayerId { get; set; }
        public string Objective { get; set; } //Objetivo de la partida
        public bool IsFinished { get; set; }//Booleano que indica si la partida ha terminado
        public float MaxTime { get; set; }//Tiempo máximo de la partida en caso de que se defina un objetivo de tiempo
        public string BoardSetup { get; set; } // Configuración inicial del tablero (letras, celdas especiales, etc.)
        // Otros datos relevantes para la partida

        public Match(string matchId, string player1Id, GameMode mode, string objective, float maxTime, string boardSetup)
        {
            MatchId = matchId;
            Player1Id = player1Id;
            CurrentPlayerId = player1Id;
            LastPlayerId = player1Id;
            Scores = new Dictionary<string, int>();
            PlayerWords = new Dictionary<string, List<Cell>>();
            ValidatedWords = new Dictionary<string, List<Cell>>();
            Mode = mode;
            Objective = objective;
            IsFinished = false;
            MaxTime = maxTime;
            BoardSetup = boardSetup;


            // Inicializar las puntuaciones y las listas de palabras para cada jugador
            Scores[player1Id] = 0;
            PlayerWords[player1Id] = new List<Cell>();
            ValidatedWords[player1Id] = new List<Cell>();

            if (mode == GameMode.PvP)
            {
                Scores[player1Id] = 0;
                PlayerWords[player1Id] = new List<Cell>();
                ValidatedWords[player1Id] = new List<Cell>();
            }
        }

        public void SetPlayer2(string player2Id)
        {
            Player2Id = player2Id;
            CurrentPlayerId = player2Id;
            Scores[player2Id] = 0;
            PlayerWords[player2Id] = new List<Cell>();
            ValidatedWords[player2Id] = new List<Cell>();
        }

        public void SetBoard(Board board)
        {
            Board = board;
        }

        public void StartMatch()
        {
            StartTime = DateTime.Now;
        }

        public void EndMatch()
        {
            EndTime = DateTime.Now;
        }
        public void FinishMatch()
        {
            IsFinished = true;
        }
        public void UpdateCurrentPlayer(string playerId)
        {
            CurrentPlayerId = playerId;
        }

        public void UpdateLastPlayer(string playerId)
        {
            LastPlayerId = playerId;
        }
        public void UpdatePlayerScore(string playerId, int score)
        {
            if (Scores.ContainsKey(playerId))
            {
                Scores[playerId] += score;
            }
        }
        public void AddPlayerWord(string playerId, Cell cell)
        {
            if (!PlayerWords.ContainsKey(playerId))
            {
                PlayerWords[playerId] = new List<Cell>();
            }
            PlayerWords[playerId].Add(cell);
        }

        public void AddValidatedWord(string playerId, List<Cell> word)
        {
            if (!ValidatedWords.ContainsKey(playerId))
            {
                ValidatedWords[playerId] = new List<Cell>();
            }
            ValidatedWords[playerId].AddRange(word);
        }

        public void ClearPlayerWord(string playerId)
        {
            if (PlayerWords.ContainsKey(playerId))
            {
                PlayerWords[playerId].Clear();
            }
        }

        public void SetWinner(string winnerPlayerId)
        {
            WinnerPlayerId = winnerPlayerId;
        }
    }
}