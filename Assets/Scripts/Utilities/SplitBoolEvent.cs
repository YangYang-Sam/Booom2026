using UnityEngine;
using UnityEngine.Events;

public class SplitBoolEvent : MonoBehaviour
{
    [SerializeField] UnityEvent onTrue;
    [SerializeField] UnityEvent onFalse;

    public void OnEvent(bool value)
    {
        if (value)
            onTrue.Invoke();
        else
            onFalse.Invoke();
    }
}
