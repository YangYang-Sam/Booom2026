using System;
using UnityEngine;
using UnityEngine.Events;

namespace JTUtility
{
    /// <summary>
    /// 可在 Editor/Inspector 中使用的多参数 UnityEvent 类型。
    /// Unity 无法直接序列化泛型 UnityEvent&lt;T0,T1&gt;，需使用此类中的 [Serializable] 具体类型。
    /// 需要其它参数组合时，按相同方式声明即可，例如：
    /// [Serializable] public class MyEvent : UnityEvent&lt;YourTypeA, YourTypeB&gt; { }
    ///
    /// 让「一般 UnityEvent」间接调用多参数方法：
    /// - 普通 UnityEvent 只能绑定无参方法，UnityEvent&lt;T&gt; 只能绑定 (T) 方法，无法在 Inspector 里直接选 2+ 参数方法。
    /// - 做法一：需要「事件触发时把 2 个及以上参数传给监听方」时，使用本类中的多参数事件类型（如 IntIntUnityEvent），
    ///   在 Inspector 中即可绑定并配置多参数方法。
    /// - 做法二：坚持用普通 UnityEvent 时，在 Inspector 里不选多参数方法，而是选一个「无参包装方法」，
    ///   在该包装方法内部再调用你的多参数方法（参数在代码里写死或从上下文/单例获取）。这样事件仍是无参的，但实际执行的是多参数逻辑。
    /// </summary>
    public static class MultiArgUnityEvents
    {
        // ---------- 2 参数 ----------

        [Serializable] public class IntIntUnityEvent : UnityEvent<int, int> { }
        [Serializable] public class IntFloatUnityEvent : UnityEvent<int, float> { }
        [Serializable] public class FloatFloatUnityEvent : UnityEvent<float, float> { }
        [Serializable] public class StringIntUnityEvent : UnityEvent<string, int> { }
        [Serializable] public class StringStringUnityEvent : UnityEvent<string, string> { }
        [Serializable] public class StringFloatUnityEvent : UnityEvent<string, float> { }
        [Serializable] public class BoolIntUnityEvent : UnityEvent<bool, int> { }
        [Serializable] public class BoolFloatUnityEvent : UnityEvent<bool, float> { }
        [Serializable] public class IntStringUnityEvent : UnityEvent<int, string> { }
        [Serializable] public class GameObjectIntUnityEvent : UnityEvent<GameObject, int> { }
        [Serializable] public class GameObjectFloatUnityEvent : UnityEvent<GameObject, float> { }
        [Serializable] public class GameObjectStringUnityEvent : UnityEvent<GameObject, string> { }

        // ---------- 3 参数 ----------

        [Serializable] public class IntIntIntUnityEvent : UnityEvent<int, int, int> { }
        [Serializable] public class IntIntFloatUnityEvent : UnityEvent<int, int, float> { }
        [Serializable] public class IntFloatFloatUnityEvent : UnityEvent<int, float, float> { }
        [Serializable] public class FloatFloatFloatUnityEvent : UnityEvent<float, float, float> { }
        [Serializable] public class StringIntIntUnityEvent : UnityEvent<string, int, int> { }
        [Serializable] public class StringIntFloatUnityEvent : UnityEvent<string, int, float> { }
        [Serializable] public class StringStringIntUnityEvent : UnityEvent<string, string, int> { }
        [Serializable] public class BoolIntIntUnityEvent : UnityEvent<bool, int, int> { }
        [Serializable] public class GameObjectIntIntUnityEvent : UnityEvent<GameObject, int, int> { }

        // ---------- 4 参数 ----------

        [Serializable] public class IntIntIntIntUnityEvent : UnityEvent<int, int, int, int> { }
        [Serializable] public class IntIntFloatFloatUnityEvent : UnityEvent<int, int, float, float> { }
        [Serializable] public class StringIntIntIntUnityEvent : UnityEvent<string, int, int, int> { }
    }
}
