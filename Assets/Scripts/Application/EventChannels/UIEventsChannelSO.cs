using UnityEngine;
using System;

namespace MagicWords.Application.EventChannels
{
    [CreateAssetMenu(menuName = "Events/UI Events Channel")]
    public class UIEventsChannelSO : ScriptableObject
    {
        public event Action OnStartMatch;
        public event Action OnConnectionLost;
        public event Action OnConnectionRestored;
        public event Action OnConnectionTimeout;

        public void RaiseStartMatch()
        {
            OnStartMatch?.Invoke();
        }

        public void RaiseConnectionLost()
        {
            OnConnectionLost?.Invoke();
        }

        public void RaiseConnectionRestored()
        {
            OnConnectionRestored?.Invoke();
        }

        public void RaiseConnectionTimeout()
        {
            OnConnectionTimeout?.Invoke();
        }
    }
}