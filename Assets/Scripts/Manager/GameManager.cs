using System;
using System.Collections.Generic;
using System.Collections;
using com.ootii.Messages;
using UnityEngine;
using Consts;
using FirebaseWebGL.Scripts.FirebaseAnalytics;
using NaughtyAttributes;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst { get; private set; }

    private UIManager uiMgr;
    private AdsSaveManager adsMgr;
    private DropEntitySpawner spawner;
    
    [SerializeField] private CameraCtrl camCtrl;
    [SerializeField] private CollisionBox colBox;
    
    [SerializeField] private ParticleSystem confettiPS;
    
    public GameObject tenEffect;
    
    // 10점 카운트
    private int tenCount;
    
    // 연속 10점시 이펙트를 위해
    [HideInInspector] public int prevScore;
    
    // 점수
    public int WholeScore { get; private set; }
    
    /// <summary>
    /// 게임이 플레이가 멈췄는지
    /// </summary>
    public bool IsPaused { get; private set; } = false;
    
    public bool IsGameOver { get; private set; } = false;

    // 이벤트 상태인가? -> 도넛 리젠안됨
    public bool IsEvent { get; private set; }

    // 특별도넛 연속 10점 -> 계속 특별도넛
    public bool IsSpecialFever { get; private set; }
    public void SetSpecialFever(bool isSpecial) => IsSpecialFever = isSpecial;

    [ReadOnly, SerializeField] private bool isMovable = true;

    /// <summary>
    /// 퍼펙트 콤보 수
    /// </summary>
    private int comboCount = 0;
    public int ComboCount
    {
        get => comboCount;
        set
        {
            comboCount = value;
            uiMgr.UpdateComboUI(comboCount);
        }
    }

    /// <summary>
    /// 도넛이 Limit에 닿은 횟수
    /// </summary>
    [ReadOnly] public int failCount = 0;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        uiMgr = UIManager.Inst;
        adsMgr = AdsSaveManager.Inst;
        spawner = DropEntitySpawner.Inst;

        if(camCtrl == null)
            camCtrl = FindObjectOfType<CameraCtrl>();
        if(colBox == null)
            colBox = FindObjectOfType<CollisionBox>();

        isMovable = true;
        
        failCount = 0;
        WholeScore = 0;
    }

    private void Update()
    {
        if (IsGameOver)
            return;

        InputKey();
    }

    private void InputKey()
    {
        if(IsGameOver)
            return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && spawner.movingDropEntity && isMovable)
        {
            spawner.movingDropEntity.GetComponent<DropEntity>().SetDown();
            spawner.SetCount();
            uiMgr.DestoryHandUI();
        }
    }

    public void UpdateTargetPos()
    {
        if(spawner.preDropEntity == null)
            return;
        
        DropEntity dropEntity = spawner.preDropEntity.GetComponent<DropEntity>();
        var meshRd = dropEntity.GetComponentInChildren<MeshRenderer>();
        float colPos = meshRd.bounds.size.y + 0.1f;
        float camPos = colPos + 0.1f;
        
        if (dropEntity.eType == eEntity.Bonus)
        {
            colPos /= 2f;
            camPos /= 2f;
        }
        
        spawner.ChangeKinematic();
        spawner.movingDropEntity = null;
        
        // 현재위치 증가
        spawner.nowHeight += colPos;
        
        // 카메라 & 충돌체 상승
        camCtrl.SetTargetPos(camPos);
        //colBox.SetTargetPos(colPos);
        
        Vector3 targetPos = spawner.preDropEntity.transform.position - new Vector3(0, meshRd.bounds.size.y, 0);
        Dispatcher.Send(eMsg.OnUpdateCollision, targetPos);
    }
    
    public void Retry()
    {
        IsGameOver = false;
        
        // 충돌체 아래로
        colBox.Retry();
        
        // regen
        //Invoke("RegenDropEntity", 0.3f);
        
        // 점수 초기화
        WholeScore = 0;
        UpdateScore(0);
        
        // 시작화면으로
    }
    
    public void SetGameOver()
    {
        IsGameOver = true;
        uiMgr.SetActiveGameOverPanel(true);
        
        camCtrl.BackTargetPos();

        //BestScorePanel.transform.parent = SceneManager.instance.GameOverPage.transform;
        RectTransform rect = uiMgr.bestScorePanel.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -400f);
        uiMgr.coinTxt.text = (WholeScore / 10).ToString();

        // 코인 업데이트 이벤트 발송
        Dispatcher.Send(eMsg.OnUpdateCoinUI);
        
        if (spawner.DropEntityCount >= adsMgr.bestStack)
        {
            adsMgr.bestStack = spawner.DropEntityCount;
            PlayerPrefs.SetInt("BestStack", adsMgr.bestStack);
        }

        if(WholeScore >= adsMgr.bestScore)
        {
            Invoke(nameof(GetConfetti), 1f);
            adsMgr.bestScore = WholeScore;
            //PlayerPrefs.SetInt("BestScore", AdsSaveManager.instance.BestScore);
        }

        Debug.Log("GameMgr::런처에도 데이터 전송");
        FirebaseMgr.PostJSON(adsMgr.bestScore, adsMgr.setValueCount);
        FirebaseAnalytics.LogEventParameter("PostBestScore", adsMgr.bestScore.ToString());

        uiMgr.BestScoreCheck();

        //int RandomCount = Random.Range(0, 2);
        //if(RandomCount == 1 && RewardButton != null)
        //{
        //    RewardButton.gameObject.SetActive(true);

        //    //RetryButton.onClick.AddListener(delegate { RewardButtonClick(RewardButton.gameObject); });
        //    RewardButton.onClick.AddListener(delegate { RewardButtonClick(RewardButton.gameObject); });
        //}
    }

    private void GetConfetti()
    {
        SoundManager.Inst.SfxPlay(eSfx.Clear);
        confettiPS.Play();
    }

    public void RewardGet()
    {
        float valueCC = spawner.CurPrefabIndex == 2 || spawner.CurPrefabIndex == 7 ? 1.5f : 1.0f;
        colBox.SetReturnPos(Def.DropEntityHeight * valueCC, failCount);
        camCtrl.SetReturnPos(Def.DropEntityHeight * valueCC, failCount);
        spawner.nowHeight -= Def.DropEntityHeight * failCount * valueCC;
        uiMgr.bgPanel.SetActive(false);
        failCount = 0;
    }

    private void RewardButtonClick(GameObject destroyObj)
    {
        //AdsManager.instance.Reward = true;
        //RetryButton.onClick.RemoveAllListeners();
        //RetryButton.onClick.AddListener(SceneManager.instance.CallBackRetry);

        Destroy(destroyObj);
    }
    
    private void UpdateScore(int score)
    {
        if (score == 0)
        {
            uiMgr.scoreTxt.text = WholeScore.ToString();
            return;
        }
        
        WholeScore += score;
        uiMgr.scoreTxt.text = WholeScore.ToString();
    }
    
    public void AddScoreUI(int score, bool isSpecial = false)
    {
        if (score >= 10)
            tenCount++;
        else
            tenCount = 0;

        if (isSpecial)
            score *= 2;
        
        int addScore = 0;
        if (tenCount > 0)
            addScore = tenCount - 1;

        score += addScore;

        if (score >= 10)
            ++ComboCount;
        else
            ComboCount = 0;
        
        uiMgr.CreateScoreUI(score);
        
        UpdateScore(score);
    }
    
    public void AddBonusScoreUI(int score)
    {
        Debug.Log("보너스 UI 생성");
        uiMgr.CreateBonusScoreUI(score);
        UpdateScore(score);
    }
    
    public void AddRibbonAndBox()
    {
        // 50개마다?
        // 0개는 없음
        //if (DropEntityCount % 50 == 0)
        //{
        //    Vector3 pos = new Vector3(0f, nowHeight + RIBBON_GAP, 0f);
        //    Instantiate(ribbon, pos, transform.rotation);
        //    //Instantiate(presentBox, pos, transform.rotation);
        //}
    }

    public void AddGiftBox()
    {
        //if (presentRegenTime < 10f)
        //{
        //    return;
        //}

        //presentRegenTime = 0f;
        
        //Debug.Log("선물상자 생성");
        //// 도넛리젠 안함 
        //bEvent = true;
        //Vector3 pos = DropEntityList[DropEntityList.Count - 1].transform.position;
        //pos += Vector3.up * 5f;
        //GameObject box = Instantiate(presentBox, pos, Quaternion.identity);
        //UnityEngine.Camera.main.GetComponent<Camera>().setFollowMode(box);
    }

    public void EventOut()
    {
        IsEvent = false;
        spawner.RegenDropEntity();
    }

    public void GetURL(URLComponent url)
    {
        Application.OpenURL(Def.URLs[(int)url.url]);
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            IsPaused = true;
            Time.timeScale = 0;
        }
        else
        {
            if(IsPaused)
            {
                IsPaused = false;
                Time.timeScale = 1;
            }
        }
    }
}
