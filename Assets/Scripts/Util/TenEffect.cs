using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TenEffect : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    
    private bool isGone = false;
    
    private void Update()
    {
        float scaleChange = Time.deltaTime * speed;
        
        if (isGone)
        {
            Vector3 localScale = transform.localScale;
            transform.localScale = new Vector3(localScale.x, localScale.y - scaleChange, localScale.z);
            
            if (transform.localScale.y < 0.01f)
                Destroy(gameObject);
        }
        else
        {
            Vector3 scaleChangeVec = new Vector3(scaleChange, scaleChange, scaleChange);
            transform.localScale += scaleChangeVec;

            if (transform.localScale.x > 6f)
                isGone = true;
        }
    }
}
