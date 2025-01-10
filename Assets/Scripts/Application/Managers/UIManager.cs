using UnityEngine;
using MagicWords.Application.EventChannels;
using MagicWords.Domain.Data.Models;
using System.Collections.Generic;
using TMPro;

namespace MagicWords.Application.Managers
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private UIEventsChannelSO uiEventsChannel;
        [SerializeField] private GameEventsChannelSO gameEventsChannel;
        [SerializeField] private GameObject startPanel; // Panel de inicio
        [SerializeField] private TextMeshProUGUI scoreText; // Texto para mostrar la puntuación
        [SerializeField] private TextMeshProUGUI connectionStatusText; // Texto para mostrar el estado de la conexión
        [SerializeField] private GameObject CellPrefab;
        [SerializeField] private Transform BoardParent;

        private void Start()
        {
            // Suscribirse a eventos
            gameEventsChannel.OnWordValidated += UpdateScore;
            uiEventsChannel.OnConnectionLost += ShowConnectionLost;
            uiEventsChannel.OnConnectionRestored += ShowConnectionRestored;

        }

        private void OnDestroy()
        {
            // Desuscribirse de eventos
            gameEventsChannel.OnWordValidated -= UpdateScore;
            uiEventsChannel.OnConnectionLost -= ShowConnectionLost;
            uiEventsChannel.OnConnectionRestored -= ShowConnectionRestored;

        }

        // Llamado cuando se pulsa el botón de Start en la UI
        public void OnStartMatchButtonPressed()
        {
            uiEventsChannel.RaiseStartMatch(); //Se crea el evento en UIEventsChannelSO
        }

        private void UpdateScore(List<Cell> word, string playerId, int score)
        {
            scoreText.text = "Score: " + score;
        }

        private void ShowConnectionLost()
        {
            connectionStatusText.text = "Connection Lost";
        }

        private void ShowConnectionRestored()
        {
            connectionStatusText.text = "Connected";
        }
        public void CreateBoard(Board board)
        {
            foreach (var row in board.Cells)
            {
                foreach (var cell in row.Value)
                {
                    GameObject cellObject = Instantiate(CellPrefab, BoardParent);
                    // Configurar la posición de la celda en la UI
                    // ...
                    // Actualizar el texto de la celda con la letra
                    cellObject.GetComponentInChildren<TextMeshProUGUI>().text = cell.Value.Letter.ToString();
                }
            }
        }
    }
}