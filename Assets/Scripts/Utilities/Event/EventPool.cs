using System;
using System.Collections.Generic;
using UnityEngine;

namespace JTUtility.Event
{
    public class EventPool
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public struct HandlerItem
        {
            public int Priority;
            public Action Delegate;
        }

        public class EventActions
        {
            public List<HandlerItem> Items = new List<HandlerItem>();
            private List<Action> rtActions = new List<Action>();
            private bool dirty = true;

            public void Add(int priority, Action action)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Delegate == action)
                        return;
                }
                Items.Add(new HandlerItem { Priority = priority, Delegate = action });
                dirty = true;
            }

            public void Remove(Action action)
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i].Delegate == action)
                    {
                        Items.RemoveAt(i);
                        dirty = true;
                        return;
                    }
                }
            }

            public void Invoke()
            {
                if (dirty)
                {
                    Items.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                    rtActions.Clear();
                    for (int i = 0; i < Items.Count; i++)
                        rtActions.Add(Items[i].Delegate);
                    dirty = false;
                }

                foreach (Action a in rtActions)
                {
                    try
                    {
                        a();
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class EventPool<T1>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public struct HandlerItem
        {
            public int Priority;
            public Action<T1> Delegate;
        }

        public class EventActions
        {
            public List<HandlerItem> Items = new List<HandlerItem>();
            private List<Action<T1>> rtActions = new List<Action<T1>>();
            private bool dirty = true;

            public void Add(int priority, Action<T1> action)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Delegate == action)
                        return;
                }
                Items.Add(new HandlerItem { Priority = priority, Delegate = action });
                dirty = true;
            }

            public void Remove(Action<T1> action)
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i].Delegate == action)
                    {
                        Items.RemoveAt(i);
                        dirty = true;
                        return;
                    }
                }
            }

            public void Invoke(T1 value1)
            {
                if (dirty)
                {
                    Items.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                    rtActions.Clear();
                    for (int i = 0; i < Items.Count; i++)
                        rtActions.Add(Items[i].Delegate);
                    dirty = false;
                }

                foreach (Action<T1> a in rtActions)
                {
                    try
                    {
                        a(value1);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class EventPool<T1, T2>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public struct HandlerItem
        {
            public int Priority;
            public Action<T1, T2> Delegate;
        }

        public class EventActions
        {
            public List<HandlerItem> Items = new List<HandlerItem>();
            private List<Action<T1, T2>> rtActions = new List<Action<T1, T2>>();
            private bool dirty = true;

            public void Add(int priority, Action<T1, T2> action)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Delegate == action)
                        return;
                }
                Items.Add(new HandlerItem { Priority = priority, Delegate = action });
                dirty = true;
            }

            public void Remove(Action<T1, T2> action)
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i].Delegate == action)
                    {
                        Items.RemoveAt(i);
                        dirty = true;
                        return;
                    }
                }
            }

            public void Invoke(T1 value1, T2 value2)
            {
                if (dirty)
                {
                    Items.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                    rtActions.Clear();
                    for (int i = 0; i < Items.Count; i++)
                        rtActions.Add(Items[i].Delegate);
                    dirty = false;
                }

                try
                {
                    foreach (Action<T1, T2> a in rtActions)
                    {
                        try
                        {
                            a(value1, value2);
                        }
                        catch (System.Exception e)
                        {
                            UnityEngine.Debug.LogException(e);
                        }
                    }
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.LogException(e);
                }
            }
        }
    }

    public class EventPool<T1, T2, T3>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public struct HandlerItem
        {
            public int Priority;
            public Action<T1, T2, T3> Delegate;
        }

        public class EventActions
        {
            public List<HandlerItem> Items = new List<HandlerItem>();
            private List<Action<T1, T2, T3>> rtActions = new List<Action<T1, T2, T3>>();
            private bool dirty = true;

            public void Add(int priority, Action<T1, T2, T3> action)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Delegate == action)
                        return;
                }
                Items.Add(new HandlerItem { Priority = priority, Delegate = action });
                dirty = true;
            }

            public void Remove(Action<T1, T2, T3> action)
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i].Delegate == action)
                    {
                        Items.RemoveAt(i);
                        dirty = true;
                        return;
                    }
                }
            }

            public void Invoke(T1 value1, T2 value2, T3 value3)
            {
                if (dirty)
                {
                    Items.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                    rtActions.Clear();
                    for (int i = 0; i < Items.Count; i++)
                        rtActions.Add(Items[i].Delegate);
                    dirty = false;
                }

                foreach (Action<T1, T2, T3> a in rtActions)
                {
                    try
                    {
                        a(value1, value2, value3);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }

    public class EventPool<T1, T2, T3, T4>
    {
        public static Dictionary<int, EventActions> DelegatePool = new Dictionary<int, EventActions>();

        public struct HandlerItem
        {
            public int Priority;
            public Action<T1, T2, T3, T4> Delegate;
        }

        public class EventActions
        {
            public List<HandlerItem> Items = new List<HandlerItem>();
            private List<Action<T1, T2, T3, T4>> rtActions = new List<Action<T1, T2, T3, T4>>();
            private bool dirty = true;

            public void Add(int priority, Action<T1, T2, T3, T4> action)
            {
                for (int i = 0; i < Items.Count; i++)
                {
                    if (Items[i].Delegate == action)
                        return;
                }
                Items.Add(new HandlerItem { Priority = priority, Delegate = action });
                dirty = true;
            }

            public void Remove(Action<T1, T2, T3, T4> action)
            {
                for (int i = Items.Count - 1; i >= 0; i--)
                {
                    if (Items[i].Delegate == action)
                    {
                        Items.RemoveAt(i);
                        dirty = true;
                        return;
                    }
                }
            }

            public void Invoke(T1 value1, T2 value2, T3 value3, T4 value4)
            {
                if (dirty)
                {
                    Items.Sort((a, b) => b.Priority.CompareTo(a.Priority));
                    rtActions.Clear();
                    for (int i = 0; i < Items.Count; i++)
                        rtActions.Add(Items[i].Delegate);
                    dirty = false;
                }

                foreach (Action<T1, T2, T3, T4> a in rtActions)
                {
                    try
                    {
                        a(value1, value2, value3, value4);
                    }
                    catch (System.Exception e)
                    {
                        UnityEngine.Debug.LogException(e);
                    }
                }
            }
        }
    }
}
