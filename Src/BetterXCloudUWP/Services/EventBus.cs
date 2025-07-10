using System;
using System.Collections.Generic;

namespace BetterXCloudUWP.Services
{
    public class EventBus
    {
        private static EventBus _instance;
        private readonly Dictionary<string, List<Action<object>>> _handlers = new Dictionary<string, List<Action<object>>>();

        private EventBus() { }

        public static EventBus Instance => _instance ?? (_instance = new EventBus());

        public void Subscribe(string eventName, Action<object> handler)
        {
            if (!_handlers.ContainsKey(eventName))
                _handlers[eventName] = new List<Action<object>>();
            _handlers[eventName].Add(handler);
        }

        public void Unsubscribe(string eventName, Action<object> handler)
        {
            if (_handlers.ContainsKey(eventName))
                _handlers[eventName].Remove(handler);
        }

        public void Emit(string eventName, object arg = null)
        {
            if (_handlers.ContainsKey(eventName))
                foreach (var handler in _handlers[eventName])
                    handler(arg);
        }
    }
}
