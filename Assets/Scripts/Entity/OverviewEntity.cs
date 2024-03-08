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

    [SerializeField] private bool isParentRot = true;

    private Rigidbody rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        
        if (isParentRot)
        {
            targetRot = transform.rotation;
        }
        else
        {
            childTs = transform.GetChild(0);
            targetRot = childTs.rotation;
        }

        rotDir = Consts.Def.DirVecs[(int)dir];

        isStart = false;
        isUpdating = true;
        rotSpeed = 200f;

        DropEntitySpawner.Inst.ListDropEntity.Add(gameObject);
    }

    private void Update()
    {
        if (!isUpdating)
            return;

        Transform target = isParentRot ? transform : childTs;
        
        if (isStart)
            ResetToInitialRotation(target);
        else
            UpdateRotation(target);
    }
    
    private void ResetToInitialRotation(Transform target)
    {
        target.rotation = Quaternion.Lerp(target.rotation, targetRot, Time.deltaTime * rotSpeed);
        if (Quaternion.Angle(target.rotation, targetRot) <= 0.001f)
        {
            isUpdating = false;
            rigid.useGravity = true;
            target.rotation = targetRot;
        }
    }
    
    private void UpdateRotation(Transform target)
    {
        target.Rotate(rotDir, Time.deltaTime * rotSpeed);
    }

    public void CallBackTapToStart()
    {
        isUpdating = false;
        rigid.useGravity = true;
    }

    public void CallBackRetry()
    {
        rigid.useGravity = false;
        rotSpeed = 200f;
        transform.position = new Vector3(0f, 2.17f, 0f);
    }
}