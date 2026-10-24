using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class ForceUpdate : MonoBehaviour
{

    public float VersionName; //این باید هربار اپدیت میدیم ورژن بیلدمون رو با این یکی کنیم
    private float NewVersion; // اینوباید تو سایت ببریم بالا تا برای ورژن های قبلی اپدیت بیاد
    private string textFromWWW;
    public GameObject btnAndroid;
    public GameObject btnIos, btnJust;

    //متن فورس رو از لینک بخونه
    //public string UrlMatnForceUpdate = string.Empty;
    // public string webText = string.Empty;
    // public Text guiText = null;

    //اینجا لینکه سایته که توش  فایل باید باشه با یه عدد فقط
#if UNITY_ANDROID
    private string urlversion = "http://papitafood.ir/service/forceUpdate.php";
#endif
#if UNITY_IOS
    private string urlversion = "http://papitafood.ir/service/forceUpdateios.php";
#endif






    public GameObject forceupdate;


    // Use this for initialization
    void Start()
    {

        //میره از سایت رشته رو میخونه و تبدیلش میکنه به فلوت

        StartCoroutine(GetTextFromWWW());

#if UNITY_ANDROID
        btnAndroid.SetActive(true);
#endif
#if UNITY_IOS
    btnIos.SetActive(true);
#endif


        //غیرفعال کردن دکمه جاست
        StartCoroutine(GetTextFromJust());

    }


    void Update()
    {
        // متن فورس رو از لینک بخونه
        // StartCoroutine(GetTextFromWebsite());
    }

    /* public IEnumerator GetTextFromWebsite()
     {
         // web request object
         WWW webRequest = new WWW(UrlMatnForceUpdate);

         // wait on the return
         yield return webRequest;

         // Save text to local var if you need it elsewhere
         webText = webRequest.text;

         // Save text to a Text Object directly if needed
         guiText.text = webRequest.text;

     }*/

    // دکمه ها که دوتا دکمه داریم یکی بره از گوگل پلی آپدیت کنه یا بره از سایت

    public void buttengoogleplay()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=me.papitafruits.android");
    }
    public void buttenApple()
    {
        Application.OpenURL("https://www.papitaw.ir/");
    }

    //اینجا میتونی بجای این چک اینترنتی که گذاشتم چک آی پی خودتم بزاری  
    IEnumerator GetTextFromWWW()
    {
#if UNITY_IOS
        string forceUrl = "http://papitafood.ir/service/forceUpdateios.php";
#else
        string forceUrl = "http://papitafood.ir/service/forceUpdate.php";
#endif
        WWW www = new WWW(forceUrl);
        yield return www;

        if (www.error != null)
        {

        }
        else
        {
            //رشته داخل فایل تکست رو بخونه و تبدیلش کنه به فلوت
            textFromWWW = www.text;
            float NewVersion = float.Parse(textFromWWW);



            //شرط اپدیت رو بررسی میکنه

            if (VersionName < NewVersion)

            {
                Invoke("showAfterTime", 2f);
            }
            else
            {
                forceupdate.SetActive(false);
            }
        }
    }

    void showAfterTime()
    {
        forceupdate.SetActive(true);
    }

    IEnumerator GetTextFromJust()
    {
#if UNITY_IOS
        string justUrl = "http://papitaplus.ir/service/forceUpdateios.php";
#else
        string justUrl = "http://papitaplus.ir/service/NewBuy/btnJustThisAppFruit-Active.php";
#endif
        WWW www = new WWW(justUrl);
        yield return www;

        if (www.error != null)
        {

        }
        else
        {
            //رشته داخل فایل تکست رو بخونه و تبدیلش کنه به فلوت
            textFromWWW = www.text;
            float NewVersion = float.Parse(textFromWWW);



            //شرط اپدیت رو بررسی میکنه

            if (NewVersion == 1)

            {
                btnJust.SetActive(true);
            }
            else
            {
                btnJust.SetActive(false);
            }
        }
    }
}
