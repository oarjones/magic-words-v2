using UnityEngine;

namespace MagicWords.Application.EventChannels
{
    [CreateAssetMenu(menuName = "Events/UI Events Channel")]
    public class UIEventsChannelSO : ScriptableObject
    {
        // Se llenará más adelante con los eventos relacionados con la UI.
        public delegate void ConnectionStatusAction();
        public event ConnectionStatusAction OnConnectionLost;
        public event ConnectionStatusAction OnConnectionRestored;
        public event ConnectionStatusAction OnConnectionTimeout;
    }
}