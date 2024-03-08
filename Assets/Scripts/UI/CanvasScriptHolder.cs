using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScriptHolder : MonoBehaviour
{
    public GameObject ToggleOn, ToggleOff;
    public GameObject InfoPopUp;

    public void GetCI() => Application.OpenURL("https://2h1z.app.link/aisoU3nX4zb");
    public void SoundCheck(bool B)
    {
        ToggleOn.SetActive(false);
        ToggleOff.SetActive(false);
        if(B)
        {
            ToggleOn.SetActive(true);
            SoundManager.Inst.SetSfxVolume(1.0f);
        }
        else if(!B)
        {
            ToggleOff.SetActive(true);
            SoundManager.Inst.SetSfxVolume(0.0f);
        }
    }

    public void InfoGet(bool B) => InfoPopUp.SetActive(B);

    public void rankPanel(bool B) => UIManager.Inst.HandleRankPanel(B);
}
