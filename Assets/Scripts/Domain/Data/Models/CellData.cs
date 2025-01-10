using MagicWords.Domain.Data.Models.Enums;
using System.Collections.Generic;

namespace MagicWords.Domain.Data.Models
{
    /// <summary>
    /// Representa la información de una celda en el tablero hexagonal,
    /// sin dependencia de MonoBehaviour.
    /// </summary>
    public class CellData
    {
        public int Level;
        public int TileNumber;

        public float X;  // Coordenadas "hex" personalizadas
        public float Y;

        public char Letter;
        public bool IsFrozen;
        public float FrozenTimeRemaining;

        public bool IsObjectiveTile;
        public bool IsCurrentPlayerTile;
        public bool IsCurrentOpponentTile;

        // Estado: Bloqueada, seleccionada, unselected, etc.
        public GameTileState TileState;

        // Vecinos (hexagonales)
        // Podríamos almacenar una lista o un diccionario con sus 6 vecinos
        public List<CellData> Neighbors = new List<CellData>();

        // Constructor básico
        public CellData(int level, int tileNumber, float x, float y, char letter)
        {
            Level = level;
            TileNumber = tileNumber;
            X = x;
            Y = y;
            Letter = letter;
            TileState = GameTileState.Unselected;
            IsFrozen = false;
            FrozenTimeRemaining = 0;
        }
    }
}
