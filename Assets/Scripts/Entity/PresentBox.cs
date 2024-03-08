using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentBox : MonoBehaviour
{
    public GameObject endParticle;
    
    public float speed;

    public int passCount;
    
    public void StartMoving()
    {
        speed = 4f;
    }
    
    private void Update()
    {
        transform.position += Time.deltaTime * speed * Vector3.down;
        
        if(GameManager.Inst.IsGameOver)
            Destroy(gameObject);
    }
    
    private void Destroy()
    {
        speed = 0f;
        
        // 파티클 생성후 사라짐 
        Instantiate(endParticle, transform.position, Quaternion.identity);
        
        // 카메라 모드 변경
        Camera.main.GetComponent<CameraCtrl>().SetNormalMode();
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        // 1. 바닥이면
        if (other.gameObject.CompareTag("bottom"))
        {
            Destroy();
            return;
        }

        passCount++;
        other.gameObject.GetComponent<Animator>().SetTrigger("doPump");
        GameManager.Inst.AddBonusScoreUI(passCount);
        
        // 시작 도넛은 다르게 판정
        if (other.gameObject.CompareTag("StartDropEntity"))
            return;

        if (other.gameObject.GetComponent<DropEntity>().score != 10)
            Destroy();
    }
}
