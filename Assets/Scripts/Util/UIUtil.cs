using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum UtilState { ScaleUp, AlphaColor, ScaleDown , PosY, PosX , CameraShake}
public enum UIComponentState { Text, Image }
public enum DoState { OutCirc, Elastic}

public class UIUtil : MonoBehaviour
{
    public UtilState Util_State;
    public UIComponentState UI_Util_State;
    public DoState doState;

    [SerializeField] float Timer = 3f;
    [SerializeField] float Max_Direction = 1f;

    private float power = 0.2f;

    Vector3 startPosition;

    private void OnEnable() => StartCoroutine(UtilCoroutine(Max_Direction, Timer));

    IEnumerator UtilCoroutine(float direction, float Timer)
    {
        float current = 0;
        float percent = 0;

        float start, end;
        switch (Util_State)
        {
            case UtilState.ScaleDown:
                 start = direction; 
                 end = 0f;
                 break;

            case UtilState.PosY:
                RectTransform rtPos = GetComponent<RectTransform>();
                switch (doState)
                {
                    case DoState.OutCirc:
                        rtPos.DOAnchorPosY(Max_Direction, Timer).SetEase(Ease.OutCirc);
                        break;
                    case DoState.Elastic:
                        rtPos.DOAnchorPosY(Max_Direction, Timer).SetEase(Ease.OutElastic);
                        break;
                }
                start = 0;
                end = 0;
                StopCoroutine(nameof(UtilCoroutine));
                break;
            
            case UtilState.CameraShake:
                start = 0;
                end = 0;
                break;
            
            default:
                 start = 0;
                 end = direction;
                 break;
        }

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / Timer;

            float utilCount = Mathf.Lerp(start, end, percent);

            if (Util_State == UtilState.ScaleDown ||Util_State == UtilState.ScaleUp)
                    transform.localScale = new Vector3(utilCount, utilCount, utilCount);
            
            switch (Util_State)
            {
                case UtilState.AlphaColor:
                    Color alphaColor;

                    switch (UI_Util_State)
                    {
                        case UIComponentState.Image:
                            Image imagealpha = GetComponent<Image>();
                            alphaColor = new Color(imagealpha.color.r, imagealpha.color.g, imagealpha.color.b, utilCount);
                            imagealpha.color = alphaColor;
                            break;
                        
                        case UIComponentState.Text:
                            Text textalpha = GetComponent<Text>();
                            alphaColor = new Color(textalpha.color.r, textalpha.color.g, textalpha.color.b, utilCount);
                            textalpha.color = alphaColor;
                            break;
                    }
                    break;
            }

            yield return null; 
        }

        enabled = false;
    }
    
    private void OnDisable() => StopAllCoroutines();
}
