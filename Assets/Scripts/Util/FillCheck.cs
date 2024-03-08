using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillCheck : MonoBehaviour
{
    private Image img;
    
    private void Start()
    {
        img = GetComponent<Image>();
        StartCoroutine(FillCheckCoroutine());
    }

    private IEnumerator FillCheckCoroutine()
    {
        float current = 0;
        float percent = 0;
        float start = 1;
        float end = 0;

        while(percent < 1)
        {
            current += Time.deltaTime;
            percent = current / 5.0f;

            float timer = Mathf.Lerp(start, end, percent);
            img.fillAmount = timer;

            yield return null;
        }

        transform.parent.gameObject.SetActive(false);
    }
}
