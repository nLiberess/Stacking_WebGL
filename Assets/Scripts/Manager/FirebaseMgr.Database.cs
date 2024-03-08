using System;
using System.Linq;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using Consts;
using FirebaseWebGL.Scripts.FirebaseAnalytics;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

using FirebaseWebGL.Scripts.FirebaseBridge;

[Serializable]
public class UserStatus
{
    public int rank = 0;
    public string idx = "";
    public int score = 0;
}

public partial class FirebaseMgr
{
    [SerializeField] private GameObject hostNullPanel;

    private static readonly string URL = "https://gogo-2b0c3-default-rtdb.firebaseio.com/"; 
    
    public static string UserId { get; private set; } = "";
    public static string PromotionKey { get; private set; } = "";
    public static string Score { get; private set; } = "";
    public static string Count { get; private set; } = "";
    public static string Rank { get; private set; } = "";
    public static string Day { get; private set; } = "";

    private Dictionary<string, object> dict = new Dictionary<string, object>();

    public UserStatus[] ArrUserStatus { get; private set; } = new UserStatus[11];
    
    private bool getRank = false;
    private bool isWeek, isMonth;
    public void SetWeek(string id) => isWeek = id == "TRUE";
    public void SetMonth(string id) => isMonth = id == "TRUE";

    public static void InitializeGame(string language, string maxCount)
    {
        FirebaseDatabase.InitGame(UserId, PromotionKey, language, maxCount, Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public static void NotifyMessage(eLauncherMsg msg)
    {
        FirebaseDatabase.NotifyMessage(msg.ToString(), Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }
    
    public static void PostToLauncher(int score)
    {
        FirebaseDatabase.PostToLauncher(UserId, PromotionKey, score.ToString(), Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public static void PostJSON(int score, int valueCount)
    {
        PostToLauncher(score);
        
        string temp = string.Concat(score, "_", valueCount, "_", DateTime.Now.ToString("dd"));
        FirebaseDatabase.PostJSON(UserId, temp, Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public static void PostDayCount()
    {
        FirebaseDatabase.PostDayCount(UserId, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "TRUE", Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public static void PostSET(string tt)
    {
        FirebaseDatabase.PostSET("TIMESTAMP", tt + "_ADMIN" , Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public static void PostWeekAndMonth(string tt)
    {
        FirebaseDatabase.PostSET("MONTH", tt, Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }
    
    public static void GetJSON(string id)
    {
        FirebaseDatabase.GetJSON(id, Inst.gameObject.name, "DisplayData", "DisplayErrorObject");
    }

    public static void ForRanking()
    {
        FirebaseDatabase.ForRanking(Inst.gameObject.name, "DisplayRank", "DisplayErrorObject");
    }

    public static void GetDate(string id)
    {
        FirebaseDatabase.GetDate(id, Inst.gameObject.name, "DisplayDateFinder", "DisplayErrorObject");
    }

    public static void GetValue_Week(string id)
    {
        FirebaseDatabase.GetVALUE(id, Inst.gameObject.name, "SetWeek", "DisplayErrorObject");
    }

    public static void GetValue_Month()
    {
        FirebaseDatabase.GetVALUE("MONTH", Inst.gameObject.name, "SetMonth", "DisplayErrorObject");
    }

    public static void PostRank(string index, string id)
    {
        FirebaseDatabase.PostRANK(index, id, Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public static void PostDate(string dd, string dau)
    {
        FirebaseDatabase.PostDate(dd, dau, Inst.gameObject.name, "DisplayInfo", "DisplayErrorObject");
    }

    public void DisplayStackLog(string str)
    {
        stackLogStr += str;
        stackLogTxt.text = stackLogStr;
    }

    public void DisplayRank(string data)
    {
        dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

        Dictionary<string, int> Dic = new Dictionary<string, int>();

        foreach (KeyValuePair<string, object> obj in dict)
        {
            string tempCount = obj.Value.ToString().Split("_")[0];
            Dic.TryAdd(obj.Key, int.Parse(tempCount));
        }

        Dictionary<string, int> sortDic = Dic.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);

        for (int i = 0; i < ArrUserStatus.Length; i++)
            ArrUserStatus[i] = new UserStatus();
        
        int index = 0;
        foreach(KeyValuePair<string, int> dic in sortDic)
        {
            if(dic.Value <= 0)
                continue;
            
            if ((getRank || isTest) && index < 5)
                PostRank(index.ToString(), dic.Key);

            ArrUserStatus[index].rank = index + 1;
            ArrUserStatus[index].idx = dic.Key;
            ArrUserStatus[index].score = dic.Value;

            if(dic.Key == UserId)
                Rank = index.ToString();
            
            index++;
            
            if (index >= 10)
                break;
        }

        ArrUserStatus[10].rank = (int.Parse(Rank) + 1);
        ArrUserStatus[10].idx = UserId;
        ArrUserStatus[10].score = int.Parse(Score);
        
        UIManager.Inst.UpdateRank();
    }

    public void DisplayData(string data)
    {
        DisplayStackLog($"On_Login_Success::{UserId}");
        FirebaseAnalytics.LogEvent("On_Login_Success");
        NotifyMessage(eLauncherMsg.Load);
        
        logTxt.color = logTxt.color == Color.green ? Color.blue : Color.green;
        logTxt.text = data;

        DateTime now = DateTime.Now;
        string today = now.ToString("ddd", new CultureInfo("en-US")).ToUpper();
        if (string.IsNullOrEmpty(data) || data == "null")
        {
            int initCount = Util.GetInitCount();
            PostJSON(0, initCount);
            Score = "0";
            Count = initCount.ToString();
            ForRanking();
            
            if (today == "MON")
            {
                if (isWeek == false)
                {
                    PostDate("MON", "1");
                    PostDate("TUE", "0");
                    PostDate("WED", "0");
                    PostDate("THU", "0");
                    PostDate("FRI", "0");
                    PostDate("SAT", "0");
                    PostDate("SUN", "0");
                    PostDate("Week", "True");
                    return;
                }
            }
            else if (today != "MON")
            {
                PostDate("Week", "FALSE");
            }
            
            GetDate(today);
        }
        else
        {
            int initCount = Util.GetInitCount();
            
            string subData = data.Substring(1, data.Length - 1);
            string mainData = subData.Substring(0, subData.Length - 1);
            string[] temp = mainData.Split("_");
            Score = temp[0];
            Count = temp[1];
            
            if (now.ToString("dd") != temp[2])
            {
                if (now.ToString("dd") == "01")
                {
                    if (isMonth == false)
                    {
                        getRank = true;
                        PostWeekAndMonth("TRUE");
                    }
                }
                else
                {
                    PostWeekAndMonth("FALSE");
                }
                
                if (today == "MON")
                {
                    if (isWeek == false)
                    {
                        PostDate("MON", "1");
                        PostDate("TUE", "0");
                        PostDate("WED", "0");
                        PostDate("THU", "0");
                        PostDate("FRI", "0");
                        PostDate("SAT", "0");
                        PostDate("SUN", "0");
                        PostDate("Week", "True");

                        GetDate(today);
                        if (int.Parse(Count) <= initCount)
                            Count = initCount.ToString();

                        AdsSaveManager.Inst.StartDelayGo();
                        return;
                    }
                }
                else if (today != "MON")
                {
                    PostDate("Week", "FALSE");
                }
                
                GetDate(today);
                if(int.Parse(Count) <= initCount)
                    Count = initCount.ToString();
            }
        }
        
        AdsSaveManager.Inst.StartDelayGo();
    }
    
    public void DisplayDateFinder(string temp)
    {
        string subData = temp.Substring(1, temp.Length - 1);
        string mainData = subData.Substring(0, subData.Length - 1);
        PostDate(DateTime.Now.ToString("ddd", new CultureInfo("en-US")).ToUpper(), (int.Parse(mainData) + 1).ToString());
    }
}
