using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class FirebaseMgr : MonoBehaviour
{
    public static FirebaseMgr Inst { get; private set; }

    [SerializeField] private Text logTxt;
    
    [SerializeField] private Text stackLogTxt;
    private string stackLogStr = "";

    [SerializeField] private bool isTest = true;

    private void Awake()
    {
        if (Inst == null)
            Inst = this;
        else
            Destroy(gameObject);
        
        Init();
    }
}