using System;
using System.Collections;
using System.Collections.Generic;
using com.ootii.Messages;
using Consts;
using UnityEngine;

using FirebaseWebGL.Scripts.Objects;
using FirebaseWebGL.Scripts.FirebaseBridge;
using FirebaseWebGL.Scripts.FirebaseAnalytics;
using FirebaseUser = FirebaseWebGL.Scripts.Objects.FirebaseUser;

public partial class FirebaseMgr
{
    public void Init()
    {
        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            DisplayError("The code is not running on a WebGL build; as such, the Javascript functions will not be recognized.");
            //return;
        }
        
        if (isTest)
        {
            if (PlayerPrefs.HasKey("UserID"))
            {
                // 유저ID가 이미 존재하면, 그 값을 불러온다.
                UserId = PlayerPrefs.GetString("UserID");
            }
            else
            {
                UserId = System.Guid.NewGuid().ToString();
                PlayerPrefs.SetString("UserID", UserId);
            }
        }
        else
        {
            string url = Application.absoluteURL;
            if(!url.Contains("?userID"))
            {
                hostNullPanel.SetActive(true);
                return;
            }
            UserId = url.Split("?userID=")[1];
        }
        
        /*UserStatus user = new UserStatus();
        RestClient.Post(URL+"/"+"NewUsers"+"/"+UserId+".json", user);*/

        GetJSON(UserId);
        GetValue_Week("WEEK");
        GetValue_Month();

        string temp = DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd");
        ForRanking();
        PostSET(temp);
    }

    public void DisplayUserInfo(string user)
    {
        var parsedUser = StringSerialization.Deserialize(typeof(FirebaseUser), user) as FirebaseUser;
        if (parsedUser != null)
            DisplayData($"Email: {parsedUser.email}, UserId: {parsedUser.uid}, EmailVerified: {parsedUser.isEmailVerified}");
        
        FirebaseAnalytics.LogEvent("On_Login_Success");
        
        Dispatcher.Send(eMsg.OnLoginSuccess, parsedUser);
    }

    public void DisplayInfo(string info)
    {
        logTxt.color = Color.white;
        logTxt.text = info;
        Debug.Log(info);
    }
    
    public void DisplayErrorObject(string error)
    {
        var parsedError = StringSerialization.Deserialize(typeof(FirebaseError), error) as FirebaseError;
        if(parsedError != null)
            DisplayError(parsedError.message);
        else
            DisplayError(error);
    }

    public void DisplayError(string error)
    {
        logTxt.color = Color.red;
        logTxt.text = error;
        Debug.LogError(error);
    }
}
