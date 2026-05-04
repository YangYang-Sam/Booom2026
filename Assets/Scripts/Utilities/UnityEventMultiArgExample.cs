using UnityEngine;
using UnityEngine.Events;

namespace JTUtility
{
    /// <summary>
    /// 演示：用「一般 UnityEvent」在编辑器中间接调用多参数方法。
    /// 做法：在 Inspector 里把普通 UnityEvent 的监听设为「无参包装方法」（如 OnTriggerMultiArgMethod），
    /// 在包装方法内部再调用真正的多参数方法（参数可写死或从上下文取）。
    /// </summary>
    public class UnityEventMultiArgExample : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEvent;

        /// <summary>
        /// 在 Inspector 里把 onEvent 的监听选成这个方法即可。
        /// 事件触发时不会传参，但这里内部会调用带多参数的方法。
        /// </summary>
        public void OnTriggerMultiArgMethod()
        {
            // 参数来自代码或上下文，例如：
            DoSomethingWithMultipleArgs(1, 2, "hello");
        }

        /// <summary>
        /// 实际的多参数方法，由包装方法 OnTriggerMultiArgMethod 调用。
        /// </summary>
        public void DoSomethingWithMultipleArgs(int a, int b, string msg)
        {
            Debug.Log($"{name}: DoSomethingWithMultipleArgs({a}, {b}, \"{msg}\")");
        }

        /// <summary>
        /// 若需要从事件方传 2+ 个参数给监听方，应使用多参数事件类型（如 MultiArgUnityEvents.IntIntUnityEvent），
        /// 并在 Inspector 里直接绑定 DoSomethingWithMultipleArgs 这类多参数方法。
        /// </summary>
        [SerializeField] private MultiArgUnityEvents.IntIntUnityEvent onEventWithTwoInts;

        public void InvokeTwoInts(int x, int y)
        {
            onEventWithTwoInts?.Invoke(x, y);
        }
    }
}
