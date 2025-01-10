using UnityEngine;
using TMPro;
using MagicWords.Domain.Data.Models;
using MagicWords.Application.Managers;

namespace MagicWords.Application.UI
{
    public class CellView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI letterText;

        private Cell cell;
        private GameManager gameManager;
        private string playerId;

        private void Start()
        {
            // Obtener una referencia al GameManager
            gameManager = FindFirstObjectByType<GameManager>();
            playerId = FindFirstObjectByType<FirebaseManager>().UserId;

            // Suscribirse al evento OnMouseDown del botón
            UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.AddListener(OnCellClicked);
            }
        }

        private void OnDestroy()
        {
            // Desuscribirse del evento OnMouseDown del botón
            UnityEngine.UI.Button button = GetComponent<UnityEngine.UI.Button>();
            if (button != null)
            {
                button.onClick.RemoveListener(OnCellClicked);
            }
        }

        public void SetCell(Cell cell)
        {
            this.cell = cell;
        }

        public void UpdateLetter(char letter)
        {
            letterText.text = letter.ToString();
        }

        public void UpdateColor(Color color)
        {
            // Aquí puedes implementar la lógica para cambiar el color de la celda
            // Puedes usar un SpriteRenderer, una Image, o cualquier otro componente visual
            // ...
        }

        private void OnCellClicked()
        {
            // Manejar el clic en la celda
            if (gameManager != null && cell != null)
            {
                gameManager.HandleCellSelection(cell, playerId);
            }
        }
    }
}