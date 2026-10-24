using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class checkVersion : MonoBehaviour
{

    public float publishVersion; //این باید هربار اپدیت میدیم با انتشار یکی باشه 
    private float NewVersion; // اینوباید تو سایت ببریم بالاتر از ورژنی که الان هست
    private string textFromWWW;

    
  

    //متن فورس رو از لینک بخونه
    //public string UrlMatnForceUpdate = string.Empty;
    // public string webText = string.Empty;
    // public Text guiText = null;

    //اینجا لینکه سایته که توش  فایل باید باشه با یه عدد فقط
#if UNITY_ANDROID
    private string urlversion = "http://papitafood.ir/service/checkmarket.php";
#endif
#if UNITY_IOS
    private string urlversion = "http://papitafood.ir/service/checkmarketios.php";
#endif




    // Use this for initialization
    void Start()
    {

        //میره از سایت رشته رو میخونه و تبدیلش میکنه به فلوت

        //UnityEngine.PlayerPrefs.DeleteKey("activation2");



        StartCoroutine(GetTextFromWWW());



    }

  

    IEnumerator GetTextFromWWW()
    {
#if UNITY_IOS
        string checkUrl = "http://papitafood.ir/service/checkmarketios.php";
#else
        string checkUrl = "http://papitafood.ir/service/checkmarket.php";
#endif
        WWW www = new WWW(checkUrl);
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

            if (publishVersion == NewVersion)

            {
                Debug.Log("ghofl baz shod");
           
                UnityEngine.PlayerPrefs.SetInt("activation2", 100);
               
                UnityEngine.PlayerPrefs.Save();

             

            }
            else
            {
                
                Debug.Log("ghofl baz nashoood");
                UnityEngine.PlayerPrefs.SetInt("activation2", -5);
           
                UnityEngine.PlayerPrefs.Save();

               
            }
        }
    }
}

