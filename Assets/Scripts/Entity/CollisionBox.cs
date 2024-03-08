using System;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using Consts;
using UnityEngine;

public class CollisionBox : MonoBehaviour
{
    private GameManager gameMgr;
    
    public Vector3 targetPos;
    public Vector3 originPos;
    public float speed;
    
    private void Start()
    {
        gameMgr = GameManager.Inst;
        
        targetPos = transform.position;
        originPos = transform.position;
        speed = 2f;
        
        Dispatcher.Add(eMsg.OnUpdateCollision, UpdateCollision);
    }

    private void Update()
    {
        if(gameMgr.IsGameOver)
            return;
        
        transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        Dispatcher.Remove(eMsg.OnUpdateCollision, UpdateCollision);
    }
    
    /// <summary>
    /// 콜리전 상승
    /// </summary>
    private void UpdateCollision(IMessage imsg)
    {
        if (imsg.Data is Vector3 data)
            targetPos = new Vector3(targetPos.x, data.y, targetPos.z);
    }

    public void SetTargetPos(float height)
    {
        targetPos += Vector3.up * height;
    }

    public void SetReturnPos(float height, int direction)
    {
        targetPos += Vector3.down * height * direction;
    }
    
    public void Retry()
    {
        targetPos = originPos;
        transform.position = originPos;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(gameMgr.IsGameOver)
            return;

        if (other.gameObject.CompareTag("PrevDropEntity"))
            gameMgr.SetGameOver();
    }
}
