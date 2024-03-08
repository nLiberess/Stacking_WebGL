using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PrevDropEntity") || other.gameObject.CompareTag("DropEntity"))
            GameManager.Inst.failCount++;
    }
}
