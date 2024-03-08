using System.Collections;
using System.Collections.Generic;
using Consts;
using UnityEngine;
using UnityEngine.UI;

public class ShopScript : MonoBehaviour
{
    public List<int> GetItem = new List<int>();

    public Transform Content;

    public Text GetItemValue;
    public Text DelayText, CoinText;

    public GameObject timerObject;

    private void Start()
    {
        GetItem[1] = PlayerPrefs.GetInt("OneGetItem");

        GetItemCheck();
    }

    private void Update()
    {
        CoinText.text = AdsSaveManager.Inst.Coin.ToString();

        if (AdsSaveManager.Inst.timer > 0)
        {
            timerObject.SetActive(true);
            if(GetSecond(AdsSaveManager.Inst.timer) >= 10)
            DelayText.text = GetMinute(AdsSaveManager.Inst.timer).ToString() + " : " + GetSecond(AdsSaveManager.Inst.timer);
            else if(GetSecond(AdsSaveManager.Inst.timer) < 10)
                DelayText.text = GetMinute(AdsSaveManager.Inst.timer).ToString() + " : 0" + GetSecond(AdsSaveManager.Inst.timer);
        }
        else
        {
            timerObject.SetActive(false);
        }
    }

    public void GoShop(bool valueGet) => this.gameObject.SetActive(valueGet);
    public void SelectThisItem(int value)
    {
        if(GetItem[value] == 1)
        {
            for(int i = 0; i < Content.GetChild(value).transform.childCount; i++)
            {
                Content.GetChild(value).transform.GetChild(i).gameObject.SetActive(false);
            }

            Content.GetChild(value).GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            return;
        }
    }


    public void GetCoinRewardSuccess()
    {
        AdsSaveManager.Inst.timer += 3600f;
        timerObject.SetActive(true);
    }
    public Animator dontmoneyanimator;
    public Text dontMoneyText;
    public void GetItemBuy()
    {
        if(AdsSaveManager.Inst.Coin >= 999 && GetItem[1] == 0)
        {
            AdsSaveManager.Inst.Coin -= 999;
            
            GetItem[1] = 1;

            PlayerPrefs.SetInt("OneGetItem", GetItem[1]);

            GetItemCheck();
        }
        else if(AdsSaveManager.Inst.Coin < 999)
        {
            dontmoneyanimator.SetTrigger("DontMoney");
        }
        else if(GetItem[1] == 1)
        {
            dontmoneyanimator.SetTrigger("DontMoney");
            dontMoneyText.text = "Coming Soon !";
        }
    }

    public void GetItemCheck()
    {
        if(GetItem[1] == 1)
        {
            Content.GetChild(1).transform.GetChild(2).gameObject.SetActive(false);
            Content.GetChild(1).transform.GetChild(1).gameObject.SetActive(true);
        }
        else if(GetItem[1] == 0)
        {
            Content.GetChild(1).transform.GetChild(2).gameObject.SetActive(true);
            Content.GetChild(1).transform.GetChild(1).gameObject.SetActive(false);
        }

        Content.GetChild(AdsSaveManager.Inst.skinCount).transform.GetChild(1).gameObject.SetActive(false);
        Content.GetChild(AdsSaveManager.Inst.skinCount).transform.GetChild(0).gameObject.SetActive(true);

    }

    public void ButtonContent(int value)
    {
        int valueHave;

        valueHave = value == 0 ? 1 : 0;

        if (GetItem[value] == 1)
        {
            Content.GetChild(value).transform.GetChild(1).gameObject.SetActive(false);
            Content.GetChild(value).transform.GetChild(0).gameObject.SetActive(true);

            Content.GetChild(valueHave).transform.GetChild(1).gameObject.SetActive(true);
            Content.GetChild(valueHave).transform.GetChild(0).gameObject.SetActive(false);

            AdsSaveManager.Inst.skinCount = value;
            PlayerPrefs.SetInt("SaveShopCount", AdsSaveManager.Inst.skinCount);
        }
        else if (GetItem[value] == 0)
        {
            Content.GetChild(value).transform.GetChild(1).gameObject.SetActive(true);
            Content.GetChild(value).transform.GetChild(0).gameObject.SetActive(false);

            Content.GetChild(valueHave).transform.GetChild(1).gameObject.SetActive(false);
            Content.GetChild(valueHave).transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public int GetMinute(float time)
    {
        return (int)((time / 60) % 60);
    }

    public int GetSecond(float time)
    {
        return (int)(time % 60);
    }
}
