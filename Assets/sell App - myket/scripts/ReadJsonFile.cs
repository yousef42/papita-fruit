using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadJsonFile : MonoBehaviour
{
    public string MyJsonUrl;
   

    void Start()
    {
        StartCoroutine(getData());
    }

    IEnumerator getData()
    {
        WWW _www = new WWW(MyJsonUrl);
        yield return _www;
        if(_www.error == null)
        {
            processJsonData(_www.text);
            Debug.Log("my readed json : "+_www.text);
        }
    }
    void processJsonData( string _url)
    {
        inAppmessage jsnData = JsonUtility.FromJson<inAppmessage>(_url);


        foreach (inAppmessageData x in jsnData.interData)
        {

            if (x.status == "200")
            {

                UnityEngine.PlayerPrefs.SetString("matn", x.matn);
                UnityEngine.PlayerPrefs.SetString("matn2", x.matn2);
                UnityEngine.PlayerPrefs.SetString("matn3", x.matn3);
                UnityEngine.PlayerPrefs.SetInt("until", x.until);
                UnityEngine.PlayerPrefs.SetInt("fontSize", x.fontSize);
                UnityEngine.PlayerPrefs.SetInt("ShowAd", x.ShowAd);
                UnityEngine.PlayerPrefs.SetInt("fromAlphabet", x.fromAlphabet);
                UnityEngine.PlayerPrefs.SetInt("inappmessageTime", x.inappmessageTime);
                UnityEngine.PlayerPrefs.SetString("inappmessage", x.inappmessage);
                UnityEngine.PlayerPrefs.SetString("titleinappmessage", x.TitleInappMessage);
                UnityEngine.PlayerPrefs.SetString("PhoneNumberWhatsapp", x.PhoneNumberWhatsapp);
                UnityEngine.PlayerPrefs.SetInt("inapppurchase", x.inAppPurchase);
                UnityEngine.PlayerPrefs.SetInt("PlayTheGameOnce", x.PlaythegameOnce);
                UnityEngine.PlayerPrefs.SetInt("sellWhatsapp", x.sellWhatsapp);

            }

        }

    }
  
}





