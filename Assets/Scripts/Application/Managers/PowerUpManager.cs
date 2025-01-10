using System.Collections.Generic;
using MagicWords.Application.EventChannels;
using MagicWords.Domain.Data.Models;
using MagicWords.Domain.Data.PowerUps;
using UnityEngine;

namespace MagicWords.Application.Managers
{
    public class PowerUpManager : MonoBehaviour
    {
        [SerializeField] private GameEventsChannelSO gameEventsChannel;
        [SerializeField] private UIEventsChannelSO uiEventsChannel;
        [SerializeField] private List<PowerUpSO> powerUps; // Lista de power-ups disponibles

        private Dictionary<string, PowerUpSO> playerPowerUps = new Dictionary<string, PowerUpSO>(); // Power-ups de cada jugador

        private void Start()
        {
            // Suscribirse a eventos relevantes
            gameEventsChannel.OnPlayerSelectedCell += HandleCellSelection;
        }

        private void OnDestroy()
        {
            // Desuscribirse de eventos
            gameEventsChannel.OnPlayerSelectedCell -= HandleCellSelection;
        }

        public void ActivatePowerUp(string playerId, PowerUpType type)
        {
            // Activar el power-up correspondiente
            switch (type)
            {
                case PowerUpType.ChangeCell:
                    // Lógica para cambiar de celda
                    break;
                case PowerUpType.ChangeLetter:
                    // Lógica para cambiar letra
                    break;
                case PowerUpType.IceTrap:
                    // Lógica para la trampa de hielo
                    break;
            }
        }

        private void HandleCellSelection(Cell cell, string playerId)
        {
            //Verificar si el jugador tiene una trampa de hielo activa en la celda seleccionada
            if (cell.State == CellState.TempBlocked)
            {
                // Lógica para la trampa de hielo si es necesario (bloquear al jugador, etc.)
            }
        }

        // Método para asignar power-ups a los jugadores (puedes llamarlo al inicio de la partida o cuando se obtengan nuevos power-ups)
        public void AssignPowerUpToPlayer(string playerId, PowerUpSO powerUp)
        {
            playerPowerUps[playerId] = powerUp;
            //Actualizar la UI para mostrar los power-ups del jugador
        }
    }
}