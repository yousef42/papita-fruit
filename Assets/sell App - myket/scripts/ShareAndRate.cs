using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;


public class ShareAndRate : MonoBehaviour
{

    public string subject;
    private string body ;

    public enum Store
    {
        googleplay,
        Bazar,
        Mayket,

    }

    public Store mystore = new Store();


#if UNITY_IPHONE
 
 [DllImport("__Internal")]
 private static extern void sampleMethod (string iosPath, string message);
 
 [DllImport("__Internal")]
 private static extern void sampleTextMethod (string message);
 
#endif



    void Start()
    {

        Debug.Log("https://play.google.com/store/apps/details?id=" + Application.identifier);
        subject = "بازی پاپیتا رو نصب کن با هم بازی کنیم";
        Invoke("setBtn", 4);
      

    }
    void setBtn()
    {
        if (UnityEngine.PlayerPrefs.GetInt("store") == 1)
        {

            body = "https://play.google.com/store/apps/details?id=" + Application.identifier;


        }
        else if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
        {
            body = "https://cafebazaar.ir/app/" + Application.identifier;

        }

        else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
        {
            body = "https://myket.ir/app/" + Application.identifier;

        }
        else
        {
            body = "https://play.google.com/store/apps/details?id=" + Application.identifier;

        }
    }

    string invitecode;
    public void OnAndroidTextSharingClick()
    {
      
        StartCoroutine(ShareAndroidText());

    }
    IEnumerator ShareAndroidText()
    {
        yield return new WaitForEndOfFrame();
        //execute the below lines if being run on a Android device
#if UNITY_ANDROID
        //Reference of AndroidJavaClass class for intent
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //Reference of AndroidJavaObject class for intent
        AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
        //call setAction method of the Intent object created
        intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        //set the type of sharing that is happening
        intentObject.Call<AndroidJavaObject>("setType", "text/plain");
        //add data to be passed to the other activity i.e., the data to be sent
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), subject);
        if (string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("my_invite_code")))
        {
            invitecode = " ";

        }
        else
        {
            invitecode = " کد دعوت من  " + UnityEngine.PlayerPrefs.GetString("my_invite_code");
        }
     
        intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), subject + " \n" + invitecode + " \n"  + " \n" + "\U0001F60D"+ "\U0001F60D" + "\U0001F60D" + " \n" +
           "اینم لینک بازیه"+ " \n"  + " \n" + body);
        //get the current activity
        AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
        //start the activity by sending the intent data
        AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "ارسال لینک بازی برای دوستان");
        currentActivity.Call("startActivity", jChooser);
#endif
    }


    public void OniOSTextSharingClick()
    {

#if UNITY_IPHONE || UNITY_IPAD
  string shareMessage = "Wow I Just Share Text ";
  sampleTextMethod (shareMessage);
  
#endif
    }

    public void RateUs()
    {
#if UNITY_ANDROID
        Application.OpenURL("market://details?id=YOUR_ID");
#elif UNITY_IPHONE
  Application.OpenURL("itms-apps://itunes.apple.com/app/idYOUR_ID");
#endif
    }

}