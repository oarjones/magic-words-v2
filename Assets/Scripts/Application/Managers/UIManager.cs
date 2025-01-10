using UnityEngine;
using MagicWords.Application.EventChannels;
using MagicWords.Domain.Data.Models;
using System.Collections.Generic;
using TMPro;
using MagicWords.Application.UI;

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
            //Crear un diccionario para almacenar las celdas de la UI
            Dictionary<int, Dictionary<int, CellView>> uiCells = new Dictionary<int, Dictionary<int, CellView>>();
            foreach (var row in board.Cells)
            {
                uiCells[row.Key] = new Dictionary<int, CellView>();
                foreach (var cell in row.Value)
                {
                    GameObject cellObject = Instantiate(CellPrefab, BoardParent);
                    CellView cellView = cellObject.GetComponent<CellView>();
                    cellView.SetCell(cell.Value);

                    // Configurar la posición de la celda en la UI
                    float xOffset = 0.8f; // Ajusta este valor según el espaciado horizontal deseado
                    float yOffset = 0.7f; // Ajusta este valor según el espaciado vertical deseado
                    float xPos = cell.Value.X * xOffset;
                    float yPos = cell.Value.Y * yOffset;

                    // Ajustar la posición para las filas con desplazamiento
                    if (cell.Value.X % 2 != 0)
                    {
                        yPos += yOffset / 2;
                    }

                    cellObject.transform.localPosition = new Vector3(xPos, yPos, 0);

                    // Actualizar el texto de la celda con la letra
                    cellView.UpdateLetter(cell.Value.Letter);

                    //Guardar la celda de la UI en el diccionario
                    uiCells[row.Key][cell.Key] = cellView;
                }
            }
        }
    }
}