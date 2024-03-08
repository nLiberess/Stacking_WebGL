using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverviewEntity : MonoBehaviour
{
    private bool isStart;
    private bool isUpdating;
    private float rotSpeed;

    private Transform childTs;
    private Quaternion targetRot;

    [SerializeField] private Consts.eDir dir;
    private Vector3 rotDir;

    private void Start()
    {
        childTs = transform.GetChild(0);
        targetRot = childTs.rotation;

        rotDir = Consts.Def.DirVecs[(int)dir];

        isStart = false;
        isUpdating = true;
        rotSpeed = 200f;
        
        DropEntitySpawner.Inst.ListDropEntity.Add(gameObject);
    }

    private void Update()
    {
        if(!isUpdating)
            return;
        
        if (isStart)
        {
            childTs.rotation = Quaternion.Lerp(childTs.rotation, targetRot, Time.deltaTime * rotSpeed);
            if (Quaternion.Angle(childTs.rotation, targetRot) <= 0.001f)
            {
                isUpdating = false;
                GetComponent<Rigidbody>().useGravity = true;
                childTs.rotation = Quaternion.Euler(0f, 0f, 90f);
            }
        }
        else
        {
            childTs.Rotate(rotDir, Time.deltaTime * rotSpeed);
        }
    }

    public void CallBackTapToStart()
    {
        isStart = true;
    }

    public void CallBackRetry()
    {
        GetComponent<Rigidbody>().useGravity = false;
        rotSpeed = 200f;
        transform.position = new Vector3(0f, 2.17f, 0f);
    }
}
