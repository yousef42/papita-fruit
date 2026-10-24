using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.IO;
using UnityEngine.UI;
using System;

public class myRate : MonoBehaviour
{
    // Start is called before the first frame update
    public int rateAt;
    public GameObject myObject;
    private bool internet;


    public enum Store
    {
       googleplay,
       Bazar,
       Mayket,
    
    }

    public Store mystore = new Store() ;
    void Start()
    {
        string HtmlText = GetHtmlFromUri("http://google.com");
        if (HtmlText == "")
        {
            internet = false;
        }
        else if (!HtmlText.Contains("schema.org/WebPage"))
        {
            //Redirecting since the beginning of googles html contains that 
            //phrase and it was not found
        }
        else
        {
            internet = true;
        }

        int rating = UnityEngine.PlayerPrefs.GetInt("rating");
        int remindrating = UnityEngine.PlayerPrefs.GetInt("rating");
        if(rateAt== rating)
        {
            if (internet)
            {
                myObject.SetActive(true);
            }
         
        }
        else
        {
            rating = rating + 1;
            UnityEngine.PlayerPrefs.SetInt("rating", rating);
        }
      

    }

 

    public void rateNow()
    {
        print("clicked");
        if (internet)
        {
            if (mystore == Store.googleplay)
            {
             
                Application.OpenURL("market://details?id=" + Application.identifier);


            }
            else if (mystore == Store.Bazar)
            {
                AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");

                AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

                AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");

                intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_EDIT"));

                intentObject.Call<AndroidJavaObject>("setData", uriClass.CallStatic<AndroidJavaObject>("parse", "bazaar://details?id=" + Application.identifier));

                intentObject.Call<AndroidJavaObject>("setPackage", "com.farsitel.bazaar");

                AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

                AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

                currentActivity.Call("startActivity", intentObject);
            }

            else if (mystore == Store.Mayket)
            {
                Application.OpenURL("myket://comment?id="+ Application.identifier);
            }

              
            UnityEngine.PlayerPrefs.SetInt("rating", 1000);
            myObject.SetActive(false);
        }

        
    }

    public void rateLater()
    {
        myObject.SetActive(false);
        UnityEngine.PlayerPrefs.SetInt("rating", 0);
    }

    public string GetHtmlFromUri(string resource)
    {
        string html = string.Empty;
        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
        try
        {
            using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
            {
                bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                if (isSuccess)
                {
                    using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                    {
                        //We are limiting the array to 80 so we don't have
                        //to parse the entire html document feel free to 
                        //adjust (probably stay under 300)
                        char[] cs = new char[80];
                        reader.Read(cs, 0, cs.Length);
                        foreach (char ch in cs)
                        {
                            html += ch;
                        }
                    }
                }
            }
        }
        catch
        {
            return "";
        }
        return html;
    }
}

