using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace JTUtility
{
    public class SequenceEvent : MonoBehaviour
    {
        [System.Serializable]
        public class SequenceEventData
        {
            public string Label;
            public float BeforeTriggerDelay;
            public float AfterTriggerDelay;
            public UnityEvent Event;
            [TextArea(2, 8)] public string luaCodes;
        }

        [SerializeField] private bool startAtBeginning = false;
        [SerializeField] private bool manuallyExecute = false;
        [SerializeField] private bool canRepeat = true;
        [SerializeField] private bool unscaleTime = false;

        [SerializeField] private SequenceEventData[] sequences = new SequenceEventData[0];

        public bool Running { get; private set; }
        public int NextEventIndex { get; set; } = 0;

        private Action<string> luaHandler;

        public bool ManuallyExecute
        {
            get { return manuallyExecute; }
            set { manuallyExecute = value; }
        }

        private void Start()
        {
            if (startAtBeginning)
                Execute();
        }

        public void InjectLuaHandler(Action<string> luaHandler)
        {
            this.luaHandler = luaHandler;
        }

        public void Execute()
        {
            print(name + ": Execute");
            if (Running) return;

            if (canRepeat && NextEventIndex == -1)
                NextEventIndex = 0;

            if (NextEventIndex < 0)
                return;

            if (manuallyExecute)
            {
                try
                {
                    InvokeEvent(sequences[NextEventIndex]);
                    NextEventIndex++;
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }

                if (NextEventIndex >= sequences.Length)
                    NextEventIndex = -1;
            }
            else if (gameObject.activeInHierarchy)
            {
                StartCoroutine(RunningEvents(0));
            }
            else
            {
                Debug.LogWarning(name + ": GameObject is not active, skipping execution");
            }
        }

        public void AbortAndReset()
        {
            StopAllCoroutines();
            Running = false;
            NextEventIndex = 0;
        }

        public void Apply(bool running, int nextEventIndex)
        {
            Running = running;
            NextEventIndex = nextEventIndex;
            if (Running)
                StartCoroutine(RunningEvents(nextEventIndex));
        }

        private void InvokeEvent(SequenceEventData seq)
        {
            if (seq == null)
            {
                Debug.LogWarning(name + ": SequenceEventData is null, skipping event execution");
                return;
            }

            if (!string.IsNullOrEmpty(seq.luaCodes))
            {
                if (luaHandler != null)
                {
                    luaHandler(seq.luaCodes);
                }
                else
                {
                    Debug.LogWarning(name + ": Lua handler is not injected, skipping Lua code execution");
                }
            }

            if (seq.Event != null)
                seq.Event.Invoke();
        }

        private IEnumerator RunningEvents(int nextEventIndex)
        {
            Running = true;
            NextEventIndex = nextEventIndex;
            for (int i = NextEventIndex; i < sequences.Length; i++)
            {
                var seq = sequences[i];
                if (seq.BeforeTriggerDelay > 0)
                    if (unscaleTime)
                        yield return new WaitForSecondsRealtime(seq.BeforeTriggerDelay);
                    else
                        yield return new WaitForSeconds(seq.BeforeTriggerDelay);

                try
                {
                    InvokeEvent(seq);
                }
                catch (System.Exception e)
                {
                    Debug.LogException(e);
                }

                NextEventIndex = i + 1;

                if (seq.AfterTriggerDelay > 0)
                    if (unscaleTime)
                        yield return new WaitForSecondsRealtime(seq.AfterTriggerDelay);
                    else
                        yield return new WaitForSeconds(seq.AfterTriggerDelay);
            }
            Running = false;
            NextEventIndex = -1;
        }
    }
}