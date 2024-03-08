using System;
using System.Collections;
using UnityEngine;
using com.ootii.Messages;

// Dispatcher main
public partial class Dispatcher
{
    /// <summary>
    /// msg callback 연결,
    /// </summary>
    /// <typeparam name="T">Consts.eMsg</typeparam>
    /// <param name="func">callback</param>
    public static void Add<T>(T msg, MessageHandler func)
    {
        MessageDispatcher.AddListener(msg.ToString(), func);
    }
    /// <summary>
    /// callback 삭제
    /// </summary>
    /// /// <typeparam name="T">Consts.eMsg</typeparam>
    public static void Remove<T>(T msg, MessageHandler func)
    {
        MessageDispatcher.RemoveListener(msg.ToString(), func);
    }
    /// <summary>
    /// 콜백 호출
    /// </summary>
    /// <typeparam name="T">Consts.eMsg</typeparam>
    /// <param name="delay">대기 시간</param>
    public static void Send<T>(T msg, object data = null, float delay = 0f)
    {
        if (data == null)
            MessageDispatcher.SendMessage(msg.ToString(), delay);
        else
            MessageDispatcher.SendMessageData(msg.ToString(), data, delay);
    }
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    #region frame
    /// <summary>
    /// 다음 프레임에 호출
    /// </summary>
    public static void WaitFrame(Action call, float delay = 0f)
    {
        MessageDispatcher.Stub.StartCoroutine(WaitCoroutine(call, delay));
    }
    /// <summary>
    /// 다음 프레임에 호출
    /// </summary>]
    public static void WaitFrame<T>(Action<T> call, T val, float delay = 0f)
    {
        MessageDispatcher.Stub.StartCoroutine(WaitCoroutine(call, val, delay));
    }
    /// <summary>
    /// 다음 프레임에 호출
    /// </summary>]
    public static void WaitFrame<T1, T2>(Action<T1, T2> call, T1 val1, T2 val2, float delay = 0f)
    {
        MessageDispatcher.Stub.StartCoroutine(WaitCoroutine(call, val1, val2, delay));
    }
    /// <summary>
    /// delay 처음 대기 시간
    /// interval = 0 이면 매프레임 호출
    /// interval > 0 이면 ?초마다 호출
    /// return 값이 true면 멈춤
    /// </summary>
    public static void UpdateFrame(Func<bool> call, float interval, float delay = 0f)
    {
        MessageDispatcher.Stub.StartCoroutine(UpdateCoroutine(call, interval, delay));
    }
#endregion
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////
#region coroutine
    static IEnumerator WaitCoroutine(Action call, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        call();
    }
    static IEnumerator WaitCoroutine<T>(Action<T> call, T val, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        call(val);
    }
    static IEnumerator WaitCoroutine<T1, T2>(Action<T1, T2> call, T1 val1, T2 val2, float delay)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);
        else
            yield return null;

        call(val1, val2);
    }
    static IEnumerator UpdateCoroutine(Func<bool> call, float interval, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (delay > 0f)
        {
            // ?초마다 호출
            while (call() == false)
                yield return new WaitForSeconds(interval);
        }
        else
        {
            // 매 프레임마다 호출
            while (call() == false)
                yield return null;
        }
    }
    #endregion
}
