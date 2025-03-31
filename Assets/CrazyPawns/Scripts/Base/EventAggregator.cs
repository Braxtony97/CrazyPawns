using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CrazyPawns.Scripts.Base
{
    public class EventAggregator
    {
        private readonly Dictionary<Type, List<Delegate>> _subscribers = new();
        
        public void Subscribe<T>(Action<T> action)
        {
            if (!_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)] = new List<Delegate>();
            }

            _subscribers[typeof(T)].Add(action);
        }
        
        public void Unsubscribe<T>(Action<T> action)
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers[typeof(T)].Remove(action);

                if (_subscribers[typeof(T)].Count == 0)
                {
                    _subscribers.Remove(typeof(T));
                }
            }
        }
        
        public void Publish<T>(T eventData)
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                var actions = _subscribers[typeof(T)].Cast<Action<T>>().ToList();

                foreach (var action in actions)
                {
                    try
                    {
                        action(eventData);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error in subscriber for event {typeof(T)}: {ex.Message}");
                    }
                }
            }
        }
        
        public void RemoveEvent<T>()
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                _subscribers.Remove(typeof(T));
            }
        }

        public void RemoveAllEvents()
        {
            _subscribers.Clear();
        }
        
        public List<Delegate> GetSubscribers<T>()
        {
            if (_subscribers.ContainsKey(typeof(T)))
            {
                return _subscribers[typeof(T)].ToList();
            }
            else
            {
                Debug.LogWarning($"No subscribers found for event {typeof(T).Name}.");
                return new List<Delegate>();
            }
        }
    }
}