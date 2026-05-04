namespace JTUtility.Event
{
    public class EventDispatcher
    {
        public static void Dispatch(EventKey key)
        {
            if (Contains(key.Id))
            {
                EventPool.DelegatePool[key.Id].Invoke();
            }
        }

        private static bool Contains(int id)
        {
            return EventPool.DelegatePool.ContainsKey(id);
        }
    }

    public class EventDispatcher<T>
    {
        public static void Dispatch(EventKey<T> key, T value)
        {
            if (Contains(key.Id))
            {
                EventPool<T>.DelegatePool[key.Id].Invoke(value);
            }
        }

        private static bool Contains(int id)
        {
            return EventPool<T>.DelegatePool.ContainsKey(id);
        }
    }

    public class EventDispatcher<T1, T2>
    {
        public static void Dispatch(EventKey<T1, T2> key, T1 value1, T2 value2)
        {
            if (Contains(key.Id))
            {
                EventPool<T1, T2>.DelegatePool[key.Id].Invoke(value1, value2);
            }
        }

        private static bool Contains(int id)
        {
            return EventPool<T1, T2>.DelegatePool.ContainsKey(id);
        }
    }

    public class EventDispatcher<T1, T2, T3>
    {
        public static void Dispatch(EventKey<T1, T2, T3> key, T1 value1, T2 value2, T3 value3)
        {
            if (Contains(key.Id))
            {
                EventPool<T1, T2, T3>.DelegatePool[key.Id].Invoke(value1, value2, value3);
            }
        }

        private static bool Contains(int id)
        {
            return EventPool<T1, T2, T3>.DelegatePool.ContainsKey(id);
        }
    }

    public class EventDispatcher<T1, T2, T3, T4>
    {
        public static void Dispatch(EventKey<T1, T2, T3, T4> key, T1 value1, T2 value2, T3 value3, T4 value4)
        {
            if (Contains(key.Id))
            {
                EventPool<T1, T2, T3, T4>.DelegatePool[key.Id].Invoke(value1, value2, value3, value4);
            }
        }

        private static bool Contains(int id)
        {
            return EventPool<T1, T2, T3, T4>.DelegatePool.ContainsKey(id);
        }
    }
}
