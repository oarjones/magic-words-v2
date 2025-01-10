using UnityEngine;
using MagicWords.Application.Managers;
using MagicWords.Domain.Data.Models;
using MagicWords.Application.EventChannels;

namespace MagicWords.Application.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private FirebaseManager firebaseManager;
        [SerializeField] private UIEventsChannelSO uiEventsChannel;
        [SerializeField] private GameEventsChannelSO gameEventsChannel;

        public GameMode gameMode; //PvP or PvA
        public float maxTime;
        public string objective;
        public string boardSetup;

        private Match currentMatch;

        private void Start()
        {
            // Suscribirse a eventos de UI
            uiEventsChannel.OnStartMatch += HandleStartMatch;
            gameEventsChannel.OnMatchCreated += HandleMatchCreated;
        }

        private void OnDestroy()
        {
            // Desuscribirse de eventos
            uiEventsChannel.OnStartMatch -= HandleStartMatch;
            gameEventsChannel.OnMatchCreated -= HandleMatchCreated;
        }

        private async void HandleStartMatch()
        {
            //Obtener datos de la partida de la UI
            //Crear la partida en firebase
            firebaseManager.CreateMatch(firebaseManager.UserId, gameMode, objective, maxTime, boardSetup);
        }

        private void HandleMatchCreated(string matchId)
        {
            //Obtener el id de la partida creada
            currentMatch = new Match(matchId, firebaseManager.UserId, gameMode, objective, maxTime, boardSetup);
        }

        public void HandleMatchStarted(string matchId)
        {
            //Obtener la configuración del tablero de Firebase
            //Crear el tablero
            //Iniciar la partida
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
        }
    }
}