using UnityEngine;

namespace MagicWords.Domain.Data.PowerUps
{
    public enum PowerUpType
    {
        ChangeCell,
        ChangeLetter,
        IceTrap
    }

    [CreateAssetMenu(menuName = "PowerUps/PowerUp")]
    public class PowerUpSO : ScriptableObject
    {
        public PowerUpType type;
        public string description;
        public Sprite icon;
        // Otros atributos como duración, coste, etc.
    }
}