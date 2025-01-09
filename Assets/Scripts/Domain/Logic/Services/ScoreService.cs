using System.Collections.Generic;
using MagicWords.Domain.Data.Models;

namespace MagicWords.Domain.Logic.Services
{
    public class ScoreService
    {
        public int CalculateScore(List<Cell> word)
        {
            // Implementar la lógica de cálculo de puntuación aquí.
            // Por ejemplo, 1 punto por cada letra.
            // Considerar letras especiales o celdas especiales que multipliquen la puntuación.

            int score = 0;
            foreach (Cell cell in word)
            {
                score++;
                // Aquí puedes añadir más lógica para celdas especiales o letras especiales
            }
            return score;
        }
    }
}