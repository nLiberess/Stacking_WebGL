using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Consts;
using com.ootii.Messages;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{
    public static UIManager Inst { get; private set; }
    
    [HorizontalLine(color:EColor.Red), BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject mainPanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject webViewPanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    public GameObject bestScorePanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject inGameUI;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject gameOverPanel;
    public void SetActiveGameOverPanel(bool active) => gameOverPanel.SetActive(active);
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject exitPanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject settingPanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject ciBannerPanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    public GameObject bgPanel;
    
    [BoxGroup("## Handle Panel UI ##"), SerializeField]
    private GameObject exceededUsagePanel;

    [HorizontalLine(color: EColor.Orange), BoxGroup("## Handle Ranking UI ##"), SerializeField]
    private GameObject rankPanel;
    
    [BoxGroup("## Handle Ranking UI ##"), SerializeField]
    private Text rankTxt;
    
    [BoxGroup("## Handle Ranking UI ##"), SerializeField]
    private Text[] arrStatusTxt;
    
    [BoxGroup("## Handle Ranking UI ##"), SerializeField]
    private Transform[] arrTs;

    [HorizontalLine(color: EColor.Yellow), BoxGroup("## Handle Infomation UI ##"), SerializeField]
    private Text remainPlayTxt;

    [BoxGroup("## Handle Infomation UI ##"), SerializeField]
    private Text remainClearTxt;

    [BoxGroup("## Handle Infomation UI ##"), SerializeField]
    private Text bestScoreTxt;
    
    [BoxGroup("## Handle Infomation UI ##"), SerializeField]
    private Text inGameBestScoreTxt;

    [BoxGroup("## Handle Infomation UI ##"), SerializeField]
    public Text coinTxt;
    
    [HorizontalLine(color:EColor.Green), BoxGroup("## Handle InGame UI ##"), SerializeField]
    private Canvas canvas;

    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    public Text countTxt;
    
    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    private GameObject scoreUIPrefab;
    
    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    private GameObject bonusScoreUIPrefab;
    
    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    public Text scoreTxt;
    
    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    public Text comboTxt;
    
    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    private Button retryBtn;

    [BoxGroup("## Handle InGame UI ##"), SerializeField]
    private GameObject handUI;
    
    [HorizontalLine(color:EColor.Blue), BoxGroup("## Handle Main Title Entity ##"), SerializeField]
    private GameObject overviewEntity;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        retryBtn.onClick.AddListener(CallBackRetry);
        Dispatcher.Add(eMsg.OnUpdateCoinUI, OnUpdateCoinUI);
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if(settingPanel.activeSelf)
                    settingPanel.SetActive(false);
                else if(exitPanel.activeSelf)
                    exitPanel.SetActive(false);
                else
                    Exit();
            }
        }
    }

    private void OnDestroy()
    {
        Dispatcher.Remove(eMsg.OnUpdateCoinUI, OnUpdateCoinUI);
    }

    private void OnUpdateCoinUI(IMessage imsg)
    {
        coinTxt.text = AdsSaveManager.Inst.Coin.ToString();
    }

    public void StartCheckResetTime() => StartCoroutine(CheckResetTime());

    /// <summary>
    /// 도전 횟수 초기화 확인
    /// </summary>
    private IEnumerator CheckResetTime()
    {
        UpdateRemainPlayUI();
        
        WaitForSeconds wait = new WaitForSeconds(1f);
        while (true)
        {
            DateTime now = DateTime.Now;

            // 다음 날 0시(자정)을 한국 시간 기준으로 계산
            DateTime midnight = now.Date.AddDays(1);
            
            // 남은 시간을 계산
            TimeSpan remaining = midnight - now;
            
            remainClearTxt.text = $"도전 횟수 초기화까지 {remaining.Hours:D2}시간 {remaining.Minutes:D2}분 {remaining.Seconds:D2}초 남았습니다.";

            if (remaining.TotalSeconds <= 0)
            {
                remainClearTxt.text = "재접속을 하시면 도전 횟수가 초기화됩니다.";
                break;
            }
            
            // 1초 동안 대기
            yield return wait;
        }
    }

    public void UpdateRemainPlayUI()
    {
        remainPlayTxt.text = $"ㆍ 하루에 ({AdsSaveManager.Inst.setValueCount}/{Util.GetInitCount()})회 플레이 할 수 있어요.";
    }

    public void WebViewNone()
    {
        AdsSaveManager.Inst.setValueCount--;
        
        // 횟수 제한
        if(AdsSaveManager.Inst.setValueCount < 0)
        {
            AdsSaveManager.Inst.setValueCount = 0;
            SetActiveAnim(true);
        }
        else
        {
            webViewPanel.SetActive(false);
            FirebaseMgr.PostJSON(AdsSaveManager.Inst.bestScore, AdsSaveManager.Inst.setValueCount);
            FirebaseMgr.PostDayCount();
        }
        
        UpdateRemainPlayUI();
    }
    
    public void SetActiveAnim(bool active) => exceededUsagePanel.SetActive(active);
    
    public void CallBackTapToStart()
    {
        ciBannerPanel.SetActive(false);
        overviewEntity.GetComponent<OverviewEntity>().CallBackTapToStart();

        Invoke(nameof(StartGame), 0.3f);
        mainPanel.SetActive(false);
        inGameUI.SetActive(true);
    }

    private void StartGame()
    {
        DropEntitySpawner.Inst.RegenDropEntity();
        countTxt.text = DropEntitySpawner.Inst.DropEntityCount.ToString();
    }

    public void CallBackRetry()
    {
        AdsSaveManager.Inst.Coin += (GameManager.Inst.WholeScore / 10);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
    }

    public void Exit()
    {
        exitPanel.SetActive(true);
    }

    public void GameExit()
    {
        Application.Quit();
    }

    public void DontExit()
    {
        exitPanel.SetActive(false);
    }
    
    public void SettingButtonCallBack()
    {
        settingPanel.SetActive(true);
    }

    public void SettingOutButtonCallBack()
    {
        settingPanel.SetActive(false);
    }
    
    public void BestScoreCheck()
    {
        var bestScoreStr = AdsSaveManager.Inst.bestScore.ToString();
        bestScoreTxt.text = bestScoreStr;
        inGameBestScoreTxt.text = bestScoreStr;
    }

    public void CreateScoreUI(int score)
    {
        GameObject ui = Instantiate(scoreUIPrefab);
        if (score >= 10)
        {
            comboTxt.gameObject.SetActive(true);
            ui.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            comboTxt.gameObject.SetActive(false);
        }
        
        ui.transform.SetParent(canvas.transform);
        ui.GetComponent<Text>().text = "+" + score;
    }

    public void UpdateComboUI(int count)
    {
        comboTxt.text = string.Concat("+", count, " COMBO");
    }
    
    public void CreateBonusScoreUI(int score)
    {
        GameObject ui = Instantiate(bonusScoreUIPrefab);
        ui.transform.SetParent(canvas.transform);
        ui.GetComponent<Text>().text = "+" + score;
    }

    public void DestoryHandUI()
    {
        if(handUI != null)
            Destroy(handUI);
    }

    public void HandleRankPanel(bool active)
    {
        rankPanel.SetActive(active);
        if (active)
            UpdateRank();
    }
    
    public void UpdateRank()
    {
        Debug.Log("UIMgr::UpdateRank, 랭킹 표시 추후 수정");
        return;
        
        var myStatus = FirebaseMgr.Inst.ArrUserStatus.Last();
        
        rankTxt.text = myStatus.rank > 0 ? $"현재 순위 : {myStatus.rank}등" : "현재 순위 : -";
        arrStatusTxt[0].text = myStatus.rank > 0 ? $"{myStatus.rank}등" : "-";
        arrStatusTxt[1].text = myStatus.score > 0 ? $"{myStatus.score}점" : "-";

        int userStatusCount = FirebaseMgr.Inst.ArrUserStatus.Length - 1;
        
        for(int i = 0; i < arrTs.Length; i++)
        {
            var texts = arrTs[i].GetComponentsInChildren<Text>();
            texts[0].text = (i + 1).ToString();
            
            arrTs[i].GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            
            if (i >= userStatusCount)
                continue;
            
            var status = FirebaseMgr.Inst.ArrUserStatus[i];
            if (status.score > 0)
                texts[1].text = status.score.ToString();
        }
    }

    public void OnClick_InitCheat()
    {
        // 마지막 플레이 날짜와 현재 날짜가 다르면 
        AdsSaveManager.Inst.setValueCount = Util.GetInitCount();
        UpdateRemainPlayUI();
        
        FirebaseMgr.PostJSON(AdsSaveManager.Inst.bestScore, AdsSaveManager.Inst.setValueCount);
        FirebaseMgr.PostDayCount();
                
        FirebaseMgr.Inst.DisplayStackLog($"\n도전 횟수 초기화: {AdsSaveManager.Inst.setValueCount}");
    }
}
