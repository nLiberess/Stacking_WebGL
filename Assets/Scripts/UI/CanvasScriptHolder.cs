using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScriptHolder : MonoBehaviour
{
    public GameObject toggleOn, toggleOff;
    public GameObject infoPopUp;

    public void GetCI() => Application.OpenURL("https://2h1z.app.link/aisoU3nX4zb");
    
    public void SoundCheck(bool active)
    {
        toggleOn.SetActive(false);
        toggleOff.SetActive(false);
        if(active)
        {
            toggleOn.SetActive(true);
            SoundManager.Inst.SetSfxVolume(1.0f);
        }
        else if(!active)
        {
            toggleOff.SetActive(true);
            SoundManager.Inst.SetSfxVolume(0.0f);
        }
    }

    public void InfoGet(bool active) => infoPopUp.SetActive(active);

    public void HandleRankPanel(bool active) => UIManager.Inst.HandleRankPanel(active);
}
