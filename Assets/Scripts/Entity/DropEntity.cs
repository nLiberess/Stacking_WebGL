using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Consts;

public class DropEntity : MonoBehaviour
{
    private DropEntitySpawner spawner;
    private GameManager gameMgr;

    // -4 ~ 4 왔다갔다
    private const float LEFTPOS = -2.5f;
    private const float RIGHTPOS = 2.5f;

    private eDir moveDir;
    public float speed = 3f;

    public Rigidbody Rigid { get; private set; }
    
    // langing 여부
    public bool isLanding;

    public bool getBonusDropEntity = false;
    
    // 타입 설정
    public eEntity eType;
    
    // 점수
    public int score;
    
    private static readonly int DoPump = Animator.StringToHash("doPump");

    private void Start()
    {
        Rigid = GetComponent<Rigidbody>();

        gameMgr = GameManager.Inst;
        spawner = DropEntitySpawner.Inst;
        
        int rand = Random.Range(1, 3);
        if (rand == 1)
        {
            moveDir = eDir.Left;
            transform.position += Vector3.right * LEFTPOS;
        }
        else
        {
            moveDir = eDir.Right;
            transform.position += Vector3.right * RIGHTPOS;
        }
    }

    public void SetDown()
    {
        moveDir = eDir.Down;
        Rigid.useGravity = true;
    }
    
    private void Update()
    {
        if(moveDir == eDir.Down)
            return;
        
        float direction = (moveDir == eDir.Left) ? -1 : 1;
        transform.Translate(direction * speed * Time.deltaTime, 0f, 0f);

        Vector3 pos = transform.position;
        if (moveDir == eDir.Left && pos.x < LEFTPOS)
        {
            transform.position = new Vector3(LEFTPOS, pos.y, pos.z);
            moveDir = eDir.Right;
        }
        else if (moveDir == eDir.Right && pos.x > RIGHTPOS)
        {
            transform.position = new Vector3(RIGHTPOS, pos.y, pos.z);
            moveDir = eDir.Left;
        }
    }
    
    private void CheckScore()
    {
        float prevCenter;
        
        if (spawner.DropEntityCount == 1)
            prevCenter = 0f;
        else
            prevCenter = spawner.ListDropEntity[spawner.DropEntityCount - 2].transform.position.x;

        float centerDiff = prevCenter - transform.position.x;
        float centerDiffAbs = Mathf.Abs(prevCenter - transform.position.x);
        if (centerDiffAbs < 0.12) 
        {
            // 10
            AddTenEffect();
            
            // 가운데로 이동
            transform.position += Vector3.right * centerDiff;
            score = 10;
        }
        else if (centerDiffAbs < 0.2)
        {
            score = 8;
        }
        else if (centerDiffAbs < 0.35)
        {
            score = 6;
        }
        else if (centerDiffAbs < 0.5)
        {
            score = 4;
        }
        else
        {
            score = 2;
        }

        // 특별 도넛이면 이펙트 추가 NORMAL은 이펙트 x
        if (eType != eEntity.Normal && AdsSaveManager.Inst.skinCount == 0)
        { 
            Instantiate(spawner.GetDropEffect((eEffect)(int)eType - 1), transform.position, transform.rotation);
        }
        else if (eType != eEntity.Normal && AdsSaveManager.Inst.skinCount == 1)
        { 
            Instantiate(spawner.GetDropEffect(eEffect.Round), transform.position, transform.rotation);
        }
        
        if (eType == eEntity.Bonus)
            gameMgr.AddScoreUI(score, true);
        else
            gameMgr.AddScoreUI(score);
        
        // 이전 도넛 10점인지 확인 & 이펙트 추가 생성
        if (gameMgr.prevScore == 10)
        {
            if(score == 10)
                Invoke(nameof(AddTenEffect), 0.15f);
            else
                gameMgr.prevScore = 0;
        }
        else
        {
            if (score == 10)
                gameMgr.prevScore = 10;
        }
        
        // bonus도넛 10점 -> 또 보너스 도넛
        // TenEffect 색상 핑크로
        if (eType == eEntity.Bonus && score == 10)
            gameMgr.SetSpecialFever(true);
        
        // 피버 해제 조건 설정
        if (gameMgr.IsSpecialFever)
        {
            if(score < 10)
                gameMgr.SetSpecialFever(false);
        }
        
        // 이벤트 아이템 생성
        //gameMgr.AddRibbonAndBox();

        // 착지 애니메이션
        GetComponent<Animator>().SetTrigger(DoPump);

        if (getBonusDropEntity && centerDiffAbs < 0.12)
        {
            gameObject.tag = "Untagged";
            spawner.SortDropEntity();
            spawner.ListDropEntity.Remove(gameObject);
            spawner.DropEntities.Remove(this);
            spawner.movingDropEntity = null;
            Destroy(gameObject);
        }
        else
        {
            // 리스트에 도넛넣어서 3개빼고 아래는 키네틱
            spawner.ListDropEntity.Add(spawner.movingDropEntity);
            spawner.DropEntities.Add(this);
            
            if (spawner.preDropEntity != null)
            {
                spawner.preDropEntity.tag = "DropEntity";
                spawner.preDropEntity = gameObject;
            }

            gameMgr.UpdateTargetPos();
        }
    }
    
    private void AddTenEffect()
    {
        GameObject effect = Instantiate(gameMgr.tenEffect, transform.position,new Quaternion(-1f,0f,0f,1f));
        if (eType == eEntity.Bonus)
            effect.GetComponent<SpriteRenderer>().color = spawner.BonusColor;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (isLanding || gameMgr.IsGameOver)
            return;
        
        if (other.gameObject.CompareTag("PrevDropEntity") || other.gameObject.CompareTag("DropEntity") ||other.gameObject.CompareTag("StartDropEntity"))
        {
            isLanding = true;
            
            SoundManager.Inst.SfxPlay(eSfx.Landing);
            DropEntitySpawner.Inst.RegenDropEntity();
            
            // 첫 동전이 튕겨나가는 느낌이 있어서 velocity zero로
            if (spawner.DropEntityCount <= 1)
            {
                if(!Rigid.isKinematic)
                    Rigid.velocity = Vector3.zero;
            }

            if(eType == eEntity.Bonus)
                spawner.ChangeKinematic();
            
            // 점수 판정
            CheckScore();
        }
    }
}
