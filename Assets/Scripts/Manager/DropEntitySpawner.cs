using System.Collections;
using System.Collections.Generic;
using Consts;
using UnityEngine;

public class DropEntitySpawner : MonoBehaviour
{
    public static DropEntitySpawner Inst { get; private set; }
    
    private GameManager gameMgr;
    
    private eEntity curEntity;

    [SerializeField] private GameObject[] arrDropEntityPrefab;
    [SerializeField] private GameObject[] arrSpecialDropEntityEffect;
    public GameObject GetDropEffect(eEffect eft) => arrSpecialDropEntityEffect[(int)eft];

    public List<DropEntity> DropEntities { get; private set; } = new List<DropEntity>();
    public List<GameObject> ListDropEntity { get; private set; } = new List<GameObject>();
    public int DropEntityCount => ListDropEntity.Count;

    [HideInInspector] public GameObject preDropEntity;
    [HideInInspector] public GameObject movingDropEntity;
    
    private Vector2 speedRange;

    [HideInInspector] public float nowHeight;
    
    /// <summary>
    /// 마지막으로 생성된 DropEntity의 Prefab 인덱스
    /// </summary>
    public int CurPrefabIndex { get; private set; } = 0;
    
    // 특별 도넛 관련
    private int specialDropEntityCnt;
    private int specialNum;

    // 선물상자 -> 도넛 리젠 안되도록
    public GameObject presentBox;
    public GameObject ribbon;
    
    // bonus 도넛 색상
    [SerializeField] private Color bonusColor;
    public Color BonusColor => bonusColor;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        gameMgr = GameManager.Inst;

        Init();
    }

    private void Init()
    {
        nowHeight = Def.DropEntityHeight + Def.DiffHeight;

        speedRange = new Vector2(Def.SpeedOrigin, Def.SpeedOrigin);
        
        // 특별 도넛
        specialDropEntityCnt = 1;
        // 10~14
        specialNum = Random.Range(5, 8);
        // 특별도넛 턴 설정
        curEntity = eEntity.Bonus;

        preDropEntity = FindObjectOfType<OverviewEntity>().gameObject;
    }
    
    /// <summary>
    /// Entity index 중에서 랜덤하게 반환
    /// </summary>
    public static int GetRandomEntity(eEntity entity)
    {
        switch (entity)
        {
            case eEntity.Normal: return Random.Range(0, 2);
            case eEntity.Bonus: return Random.Range(2, 4);
            case eEntity.Obstacle: return 4;
        }

        return -1;
    }

    private void CreateDropEntity()
    {
        if (gameMgr.IsEvent)
            return;

        if (movingDropEntity != null)
            preDropEntity = movingDropEntity; 
        
        eEntity type = SetEntityType();

        if (type == eEntity.Bonus && AdsSaveManager.Inst.skinCount == 1)
        {
            CurPrefabIndex = GetRandomEntity(eEntity.Bonus);
            movingDropEntity = Instantiate(arrDropEntityPrefab[CurPrefabIndex],
                new Vector3(0f, nowHeight, 0f),
                new Quaternion(0f, 0f, 0f, 1f));
        }
        else
        {
            CurPrefabIndex = GetRandomEntity(type);
            movingDropEntity = Instantiate(arrDropEntityPrefab[CurPrefabIndex],
                new Vector3(0f, nowHeight, 0f),
                new Quaternion(0f, 0f, 0f, 1f));

            if (type == eEntity.Bonus) 
                movingDropEntity.GetComponent<DropEntity>().getBonusDropEntity = true;
        }
        
        movingDropEntity.GetComponent<DropEntity>().speed = Random.Range(speedRange.x, speedRange.y);
        movingDropEntity.GetComponent<DropEntity>().eType = type;

        movingDropEntity.name = $"DropEntity_{type.ToString()}_{DropEntityCount}";
        movingDropEntity.tag = "PrevDropEntity";
    }

    public void OnDownDropEntity()
    {
        SetCount();
        movingDropEntity.GetComponent<DropEntity>().SetDown();
        
        // 리스트에 도넛넣어서 3개빼고 아래는 키네틱
        ListDropEntity.Add(movingDropEntity);
        
        ChangeKinematic();
        movingDropEntity = null;
        
        // 현재위치 증가
        nowHeight += Def.DropEntityHeight;
    }

    public void OnRetry()
    {
        print(ListDropEntity.Count);
        
        // 도넛 삭제
        for(int i = ListDropEntity.Count - 1; i >= 0; i--)
        {
            Destroy(ListDropEntity[i]);
            ListDropEntity.Remove(ListDropEntity[i]);
        }
        
        if(movingDropEntity)
            Destroy(movingDropEntity);
        
        ListDropEntity.Clear();
        
        // 점수 초기화
        nowHeight = -0.0561738f;

        SetCount();
    }
    
    public void SetCount()
    {
        UIManager.Inst.countTxt.text = DropEntityCount.ToString();
        
        // 구간별 속도 설정
        SetSpeedByCount();
    }

    private void SetSpeedByCount()
    {
        Vector2[] multipliers =
        {
            new Vector2(1.1f, 1.3f), 
            new Vector2(1.5f, 2f),
            new Vector2(1.2f, 3f),
            new Vector2(1f, 4f)
        };
        
        int[] thresholds = new int[] { 21, 51, 101, int.MaxValue };

        for (int i = 0; i < thresholds.Length; i++)
        {
            if (DropEntityCount < thresholds[i])
            {
                speedRange = Def.SpeedOrigin * multipliers[i];
                break;
            }
        }
    }
    
    public void RegenDropEntity()
    {   
        Invoke(nameof(CreateDropEntity), 0.5f);
    }

    private eEntity SetEntityType()
    {
        // 스페셜 피버상태면 계속 보너스 도넛만 나오도록
        if (gameMgr.IsSpecialFever)
            return eEntity.Bonus;
        
        eEntity genIndex = eEntity.Normal;
        
        // 8 ~ 12개마다 보너스
        // 보너스 후 4 ~ 6개 후 타이어

        specialDropEntityCnt++;
        if (specialDropEntityCnt >= specialNum)
        {
            if (curEntity == eEntity.Bonus)
            {
                genIndex = eEntity.Bonus;
                curEntity = eEntity.Obstacle;
                specialDropEntityCnt = 0;
                specialNum = Random.Range(4, 7); // 4 ~ 6
            }
            else if (curEntity == eEntity.Obstacle)
            {
                genIndex = eEntity.Obstacle;
                curEntity = eEntity.Bonus;
                specialDropEntityCnt = 0;
                specialNum = Random.Range(7, 13); // 8 ~ 12
            }
        }

        return genIndex;
    }
    
    public void SortDropEntity()
    {
        StartCoroutine(CorSortDropEntity());
    }
    
    private IEnumerator CorSortDropEntity()
    {
        float speed = 0.5f / DropEntities.Count;

        for (int i = 0; i < DropEntities.Count; i++)
        {
            if(DropEntities[i] == null)
                continue;
            
            if(!DropEntities[i].Rigid.isKinematic)
                DropEntities[i].Rigid.velocity = Vector3.zero;
            DropEntities[i].Rigid.isKinematic = true;
        }
        
        for (int i = 0; i < DropEntities.Count; i++)
        {
            if(DropEntities[i] == null)
                continue;
            
            float start = DropEntities[i].transform.position.x;
            float current = 0;
            float percent = 0;
            float end = 0.0f;
            while (percent < 1)
            {
                current += Time.deltaTime;
                percent = current / speed;
                float lerpX = Mathf.Lerp(start, end, percent);
                DropEntities[i].transform.position = new Vector3(lerpX, DropEntities[i].transform.position.y, 0.0f);
                yield return null;
            }
        }

        if (Util.TryGetItemOfIndex(DropEntities, DropEntities.Count - 1, out var obj))
            obj.Rigid.isKinematic = false;
        DropEntities.Clear();
    }
    
    public void ChangeKinematic()
    {
        if (DropEntityCount - 8 > -1)
        {
            if(Util.TryGetItemOfIndex(ListDropEntity, DropEntityCount - 8, out var target))
                target.GetComponent<Rigidbody>().isKinematic = true;
        }

        if (DropEntityCount - 3 >= 0)
        {
            if(Util.TryGetItemOfIndex(ListDropEntity, DropEntityCount - 3, out var target))
                target.tag = "DropEntity";
        }
    }
}
