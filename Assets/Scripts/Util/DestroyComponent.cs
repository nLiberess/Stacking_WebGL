using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyComponent : MonoBehaviour
{
    [SerializeField] private float destroyTime = 3f;
    
    private void Start() => Destroy(gameObject, destroyTime);
}
