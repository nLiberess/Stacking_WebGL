using System;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using Consts;
using NaughtyAttributes;
using UnityEngine;

public class AdsSaveManager : MonoBehaviour
{
    public static AdsSaveManager Inst { get; private set; }

    [ReadOnly, SerializeField] private int coin;
    public int Coin
    {
        get => coin;
        set
        {
            coin = value;
            
            // 코인 업데이트 이벤트 발송
            Dispatcher.Send(eMsg.OnUpdateCoinUI);
        }
    }
    
    public int saveCount;
    public int bestScore;
    public int bestStack;
    
    /// <summary>
    /// DropEntity 외형 스킨 인덱스
    /// </summary>
    public int skinCount;
    public float timer;
    public int setValueCount;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        Dispatcher.Add(eMsg.OnUpdateCoinUI, OnUpdateCoinUI);
    }
    
    private void Update()
    {
        if (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            PlayerPrefs.SetFloat("timer", timer);
        }
    }

    private void OnDestroy()
    {
        Dispatcher.Remove(eMsg.OnUpdateCoinUI, OnUpdateCoinUI);
    }

    private void OnUpdateCoinUI(IMessage imsg)
    {
        PlayerPrefs.SetInt("Coin", coin);
    }

    public void StartDelayGo()
    {
        StartCoroutine(CorDelayGo());
    }

    private IEnumerator CorDelayGo()
    {
        yield return new WaitForSeconds(0.01f);
        
        if(FirebaseMgr.Score != "")
            bestScore = int.Parse(FirebaseMgr.Score);
        
        if(FirebaseMgr.Count != "")
            setValueCount = int.Parse(FirebaseMgr.Count);

        FirebaseMgr.NotifyMessage(eLauncherMsg.Loaded);
        LoadLastPlayDay();
        UIManager.Inst.BestScoreCheck();
    }

    private void LoadLastPlayDay()
    {
        // 마지막 플레이 날짜를 가져옴
        string lastPlayDayStr = PlayerPrefs.GetString("LastPlayDay", "null");
        
        if (lastPlayDayStr != "null")
        {
            // 마지막 플레이 날짜를 DateTime 객체로 변환
            DateTime lastPlayDay = DateTime.Parse(lastPlayDayStr);

            if (lastPlayDay.Date != DateTime.Now.Date)
            {
                // 마지막 플레이 날짜와 현재 날짜가 다르면 
                setValueCount = Util.GetInitCount();
                UIManager.Inst.UpdateRemainPlayUI();
        
                FirebaseMgr.PostJSON(bestScore, setValueCount);
                FirebaseMgr.PostDayCount();
                
                FirebaseMgr.Inst.DisplayStackLog($"\n마지막 플레이 날짜와 다르므로 도전 횟수 초기화: {setValueCount}");
            }
        }

        SaveLastPlayDay();
        
        // 코루틴 시작
        UIManager.Inst.StartCheckResetTime();
    }

    private void SaveLastPlayDay()
    {
        // 마지막 플레이 날짜를 현재 날짜로 갱신
        PlayerPrefs.SetString("LastPlayDay", DateTime.Now.ToString());
    }

    private void OnApplicationQuit()
    {
        SaveLastPlayDay();
    }
}
