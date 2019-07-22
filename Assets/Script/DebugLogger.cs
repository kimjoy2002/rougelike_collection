using UnityEngine;
using UnityEditor;

public class DebugLogger
{
    /// <summary>
    /// Debug.Log를 랩핑하였음. FIXME 나중에 debug일때 행동을 피하도록 해봐야지
    /// </summary>
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(object message)
    {
        Debug.Log(message);
    }
    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(string format, params object[] args)
    {
        Debug.Log(string.Format(format, args));
    }
}