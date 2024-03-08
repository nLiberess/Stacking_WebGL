using System;
using System.Collections;
using System.Collections.Generic;
using Consts;
using UnityEngine;
using Random = UnityEngine.Random;

public static partial class Util
{
    /// <summary>
    /// list의 임의 index에 있는 아이템 반환
    /// </summary>
    public static bool TryGetItemOfIndex<T>(List<T> list, int index, out T target)
    {
        if (index >= 0 && index < list.Count)
        {
            target = list[index];
            return true;
        }

        target = default;
        return false;
    }
    
    public static int GetInitCount()
    {
        DateTime now = DateTime.Now;
        
        // 첫날 정보를 가져옴
        string firstDayStr = PlayerPrefs.GetString("FirstDay", "null");

        int initCount = 0;

        if (firstDayStr == "null")
        {
            PlayerPrefs.SetString("FirstDay", now.ToString());
            initCount = Def.FirstTryCount;
        }
        else
        {
            DateTime firstDay = DateTime.Parse(firstDayStr);
            initCount = firstDay.Date == now.Date ? Def.FirstTryCount : Def.InitTryCount;
        }

        return initCount;
    }
}
