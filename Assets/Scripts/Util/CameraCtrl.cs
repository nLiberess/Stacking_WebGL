using System;
using UnityEngine;
using Consts;
using NaughtyAttributes;

public class CameraCtrl : MonoBehaviour
{
    /// <summary>
    /// 카메라의 현재 모드
    /// </summary>
    [ReadOnly, SerializeField] private eCamMode curMode;
    
    private Vector3 targetPos;
    private Vector3 originPos;
    private Vector3 prevPos;
    private Vector3 offset;
    
    /// <summary>
    /// 카메라가 움직이는 속도
    /// </summary>
    [SerializeField, Range(0, 5f)] private float moveVelocity = 2f;
    
    /// <summary>
    /// 카메라가 추적할 타겟
    /// </summary>
    [ReadOnly, SerializeField] private GameObject followObj;
    
    /// <summary>
    /// 카메라와 타겟의 위치를 체크하여, 위치에 도달했다면 카메라 이벤트 종료 -> 도넛 생성
    /// </summary>
    [ReadOnly, SerializeField] private bool isCheckPos;
    
    private void Start()
    {
        curMode = eCamMode.Normal;
        
        targetPos = transform.position;
        originPos = transform.position;
        offset = transform.position;
    }

    public void SetTargetPos(float height)
    {
        if (GameManager.Inst.IsGameOver)
            return;
        
        targetPos += Vector3.up * height;
    }

    public void SetReturnPos(float height, int direction)
    {
        targetPos += Vector3.down * height * direction;
    }

    public void BackTargetPos()
    {   
        targetPos += Vector3.back * 25.0f;
        targetPos += Vector3.up * 15.0f;
    }
    
    public void ReTargetPos()
    {
        targetPos += Vector3.forward * 25f;
        targetPos += Vector3.down * 15.0f;
    }

    public void SetFollowMode(GameObject target)
    {
        if (GameManager.Inst.IsGameOver)
            return;
        
        // 현재 카메라 위치 저장
        prevPos = transform.position;

        // 선물상자 위치로
        targetPos = new Vector3(targetPos.x, target.transform.position.y, targetPos.z);
        followObj = target;

        // 선물상자 위치로 도달후에 모드 변경
        curMode = eCamMode.Wait;
    }

    public void SetNormalMode()
    {
        if (GameManager.Inst.IsGameOver)
            return;
        
        followObj = null;
        Invoke(nameof(InvSetNormalMode), 1f);
    }

    public void InvSetNormalMode()
    {
        targetPos = new Vector3(targetPos.x, prevPos.y, targetPos.z);
        curMode = eCamMode.Normal;
        isCheckPos = true;
    }
    
    public void Retry()
    {
        targetPos = originPos;
        transform.position = originPos;
        curMode = eCamMode.Normal;
    }

    private void FixedUpdate()
    {
        if (curMode == eCamMode.Normal || curMode == eCamMode.Wait)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, moveVelocity * Time.deltaTime);
            
            if (GameManager.Inst.IsGameOver)
                return;
            
            // 이벤트 종류 후 카메라 위치 파악 후 도넛 생성해야함
            if (curMode == eCamMode.Normal && isCheckPos)
            {
                if (Mathf.Abs(targetPos.y - transform.position.y) < 0.01f)
                {
                    isCheckPos = false;
                    GameManager.Inst.EventOut();
                }
            }
            if (curMode == eCamMode.Wait && Mathf.Abs(targetPos.y - transform.position.y) < 0.01f)
            {
                curMode = eCamMode.Follow;
                followObj.GetComponent<PresentBox>().StartMoving();
            }
        }
        else if (curMode == eCamMode.Normal)
        {
            if (followObj != null)
            {
                // offset유지하며 선물상자 따라 이동
                transform.position = new Vector3(offset.x, followObj.transform.position.y, offset.z);
            }
        }
    }
}
