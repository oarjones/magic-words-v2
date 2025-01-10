using System.Collections.Generic;
using MagicWords.Domain.Data.Models;
using MagicWords.Domain.Data.Models.Enums;
using UnityEngine;

namespace MagicWords.Domain.Logic.Services
{
    /// <summary>
    /// Servicio responsable de crear la estructura de celdas (CellData)
    /// para un tablero hexagonal.
    /// </summary>
    public class BoardGenerationService
    {
        public List<CellData> GenerateBoardData(
            float mapSize,
            float tileScale,
            float xOffset,
            float yOffset,
            float initialXpos,
            float initialYpos,
            IRandomLetterProvider letterProvider, // interfaz para obtener letras aleatorias
            GameType gameType
        )
        {
            var cells = new List<CellData>();

            // Lógica adaptada de tu código "GenerateBoard"
            // El "tileNum", "level", etc. se van a CellData, 
            // pero no instanciamos GameObjects aquí.

            for (int level = 0; level < mapSize; level++)
            {
                if (level == 0)
                {
                    // Solo una celda en el nivel 0
                    var cdata = new CellData(level, 0, 0f, 0f, letterProvider.GetRandomLetter());
                    cells.Add(cdata);
                }
                else
                {
                    float levelTilesNumber = level * 6f;

                    // Variables auxiliares como en tu código
                    float yIncrement = 0.5f;
                    float xIncrement = 1f;

                    float xMultipler = level * xIncrement;
                    float yMultipler = level * yIncrement;

                    int negativeXtiles = 0;
                    int positiveXtiles = 0;

                    for (int tileNum = 0; tileNum < levelTilesNumber; tileNum++)
                    {
                        // Calcula posición "teórica"
                        float xPos = (xMultipler * xOffset) * 1.03f;
                        float yPos = (yMultipler * yOffset) * 1.03f;

                        // Crea un CellData
                        var letter = letterProvider.GetRandomLetter();
                        var cdata = new CellData(level, tileNum, xPos, yPos, letter);

                        // Reglas de "estado" (p. ej. si es la celda actual del jugador):
                        bool isCurrentUserTile =
                            (gameType != GameType.Standalone && level == (mapSize - 1) && xMultipler == 0 && yMultipler == level)
                            || (gameType == GameType.Standalone && level == (mapSize - 1) && tileNum == 0);

                        bool isCurrentOpponentTile =
                            (gameType != GameType.Standalone && level == (mapSize - 1) && xMultipler == 0 && yMultipler == -level)
                            || (gameType == GameType.Standalone && level == (mapSize - 1) && tileNum == (levelTilesNumber / 3));

                        cdata.IsCurrentPlayerTile = isCurrentUserTile;
                        cdata.IsCurrentOpponentTile = isCurrentOpponentTile;

                        // Etc. (si el nivel es “EmptyLevel”, ponlo Bloqueado)
                        // Por ahora, dejaremos la lógica “isEmptyLevel” fuera para simplificar.

                        cells.Add(cdata);

                        // Ajuste de xMultipler / yMultipler
                        if (tileNum < level)
                        {
                            xMultipler -= xIncrement;
                            yMultipler += yIncrement;
                        }
                        else if (tileNum == level)
                        {
                            xMultipler -= xIncrement;
                            yMultipler -= yIncrement;
                        }
                        else
                        {
                            if (xMultipler < 0)
                            {
                                if (xMultipler == -level && negativeXtiles == 0)
                                {
                                    negativeXtiles++;
                                }

                                if (negativeXtiles == 1)
                                {
                                    yIncrement = 1;
                                }

                                if (negativeXtiles == 0)
                                {
                                    xMultipler -= xIncrement;
                                    yMultipler -= yIncrement;
                                }
                                else if (negativeXtiles > 0 && negativeXtiles < (level + 1))
                                {
                                    negativeXtiles++;
                                    yMultipler -= yIncrement;
                                }
                                else
                                {
                                    yIncrement = 0.5f;
                                    xMultipler += xIncrement;
                                    yMultipler -= yIncrement;
                                }
                            }
                            else
                            {
                                if (xMultipler == level && positiveXtiles == 0)
                                {
                                    positiveXtiles++;
                                }

                                if (positiveXtiles == 1)
                                {
                                    yIncrement = 1;
                                }

                                if (positiveXtiles == 0)
                                {
                                    xMultipler += xIncrement;
                                    yMultipler += yIncrement;
                                }
                                else if (positiveXtiles > 0 && positiveXtiles < (level + 1))
                                {
                                    positiveXtiles++;
                                    yMultipler += yIncrement;
                                }
                                else
                                {
                                    yIncrement = 0.5f;
                                    xMultipler -= xIncrement;
                                    yMultipler += yIncrement;
                                }
                            }
                        }
                    }
                }
            }

            // TODO: Si quieres identificar la "ObjectiveTile" (modo CatchLetter, etc.),
            // podrías hacerlo aquí, p.e. buscar la celda más lejana y marcar "IsObjectiveTile = true".

            return cells;
        }
    }

    
    /// <summary>
    /// Interfaz mínima para obtener letras aleatorias.
    /// Así abstraemos la fuente de las letras (podríamos usar un ScriptableObject, etc.).
    /// </summary>
    public interface IRandomLetterProvider
    {
        char GetRandomLetter();
    }
}
