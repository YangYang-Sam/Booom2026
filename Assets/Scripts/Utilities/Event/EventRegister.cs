namespace JTUtility.Event
{
    public class EventRegister
    {
        public static void Register(EventKey key, Action action, int priority = 0)
        {
            EventPool.EventActions actions = GetEventActionsWithName(key.Id);
            actions.Add(priority, action);
        }

        public static void UnRegister(EventKey key, Action action)
        {
            if (EventPool.DelegatePool.ContainsKey(key.Id))
            {
                EventPool.EventActions actions = GetEventActionsWithName(key.Id);
                actions.Remove(action);
            }
        }

        private static EventPool.EventActions GetEventActionsWithName(int id)
        {
            if (!EventPool.DelegatePool.ContainsKey(id))
                EventPool.DelegatePool.Add(id, new EventPool.EventActions());

            return EventPool.DelegatePool[id];
        }
    }

    public class EventRegister<T>
    {
        public static void Register(EventKey<T> key, Action<T> action, int priority = 0)
        {
            EventPool<T>.EventActions actions = GetEventActionsWithName(key.Id);
            actions.Add(priority, action);
        }

        public static void UnRegister(EventKey<T> key, Action<T> action)
        {
            if (EventPool<T>.DelegatePool.ContainsKey(key.Id))
            {
                EventPool<T>.EventActions actions = GetEventActionsWithName(key.Id);
                actions.Remove(action);
            }
        }

        private static EventPool<T>.EventActions GetEventActionsWithName(int id)
        {
            if (!EventPool<T>.DelegatePool.ContainsKey(id))
                EventPool<T>.DelegatePool.Add(id, new EventPool<T>.EventActions());

            return EventPool<T>.DelegatePool[id];
        }
    }

    public class EventRegister<T1, T2>
    {
        public static void Register(EventKey<T1, T2> key, Action<T1, T2> action, int priority = 0)
        {
            EventPool<T1, T2>.EventActions actions = GetEventActionsWithName(key.Id);
            actions.Add(priority, action);
        }

        public static void UnRegister(EventKey<T1, T2> key, Action<T1, T2> action)
        {
            if (EventPool<T1, T2>.DelegatePool.ContainsKey(key.Id))
            {
                EventPool<T1, T2>.EventActions actions = GetEventActionsWithName(key.Id);
                actions.Remove(action);
            }
        }

        private static EventPool<T1, T2>.EventActions GetEventActionsWithName(int id)
        {
            if (!EventPool<T1, T2>.DelegatePool.ContainsKey(id))
                EventPool<T1, T2>.DelegatePool.Add(id, new EventPool<T1, T2>.EventActions());

            return EventPool<T1, T2>.DelegatePool[id];
        }
    }

    public class EventRegister<T1, T2, T3>
    {
        public static void Register(EventKey<T1, T2, T3> key, Action<T1, T2, T3> action, int priority = 0)
        {
            EventPool<T1, T2, T3>.EventActions actions = GetEventActionsWithName(key.Id);
            actions.Add(priority, action);
        }

        public static void UnRegister(EventKey<T1, T2, T3> key, Action<T1, T2, T3> action)
        {
            if (EventPool<T1, T2, T3>.DelegatePool.ContainsKey(key.Id))
            {
                EventPool<T1, T2, T3>.EventActions actions = GetEventActionsWithName(key.Id);
                actions.Remove(action);
            }
        }

        private static EventPool<T1, T2, T3>.EventActions GetEventActionsWithName(int id)
        {
            if (!EventPool<T1, T2, T3>.DelegatePool.ContainsKey(id))
                EventPool<T1, T2, T3>.DelegatePool.Add(id, new EventPool<T1, T2, T3>.EventActions());

            return EventPool<T1, T2, T3>.DelegatePool[id];
        }
    }

    public class EventRegister<T1, T2, T3, T4>
    {
        public static void Register(EventKey<T1, T2, T3, T4> key, Action<T1, T2, T3, T4> action, int priority = 0)
        {
            EventPool<T1, T2, T3, T4>.EventActions actions = GetEventActionsWithName(key.Id);
            actions.Add(priority, action);
        }

        public static void UnRegister(EventKey<T1, T2, T3, T4> key, Action<T1, T2, T3, T4> action)
        {
            if (EventPool<T1, T2, T3, T4>.DelegatePool.ContainsKey(key.Id))
            {
                EventPool<T1, T2, T3, T4>.EventActions actions = GetEventActionsWithName(key.Id);
                actions.Remove(action);
            }
        }

        private static EventPool<T1, T2, T3, T4>.EventActions GetEventActionsWithName(int id)
        {
            if (!EventPool<T1, T2, T3, T4>.DelegatePool.ContainsKey(id))
                EventPool<T1, T2, T3, T4>.DelegatePool.Add(id, new EventPool<T1, T2, T3, T4>.EventActions());

            return EventPool<T1, T2, T3, T4>.DelegatePool[id];
        }
    }
}
