namespace JTUtility.Event
{
    public readonly struct EventKey
    {
        public int Id { get; }

        public EventKey(int id)
        {
            Id = id;
        }
    }

    public readonly struct EventKey<T>
    {
        public int Id { get; }

        public EventKey(int id)
        {
            Id = id;
        }
    }

    public readonly struct EventKey<T1, T2>
    {
        public int Id { get; }

        public EventKey(int id)
        {
            Id = id;
        }
    }

    public readonly struct EventKey<T1, T2, T3>
    {
        public int Id { get; }

        public EventKey(int id)
        {
            Id = id;
        }
    }

    public readonly struct EventKey<T1, T2, T3, T4>
    {
        public int Id { get; }

        public EventKey(int id)
        {
            Id = id;
        }
    }
}
