using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checktakhfif : MonoBehaviour
{
    public static checktakhfif instance;
    public string takhfif=null; //این باید هربار اپدیت میدیم با انتشار یکی باشه 
    private float NewVersion; // اینوباید تو سایت ببریم بالاتر از ورژنی که الان هست
    private string textFromWWW;
  


#if UNITY_ANDROID
    private string urltakhfif = "http://papitafood.ir/service/TakhfifText.php";

#elif UNITY_IOS
    private string urltakhfif = "http://www.papitafood.ir/service/TakhfifText.php";
#else
    private string urltakhfif = "http://www.papitafood.ir/service/TakhfifText.php";
#endif




    // Use this for initialization
    void Start()
    {

        //میره از سایت رشته رو میخونه و تبدیلش میکنه به فلوت

        //  UnityEngine.PlayerPrefs.DeleteKey("activation2");
        UnityEngine.PlayerPrefs.DeleteKey("savetakhfif");

        if (!UnityEngine.PlayerPrefs.HasKey("Issayhello"))
        {
            UnityEngine.PlayerPrefs.SetInt("Issayhello", 0);
        }

        int domyjob = UnityEngine.PlayerPrefs.GetInt("Issayhello");

        if (domyjob == 0)
        {
            StartCoroutine(GetTextFromWWW());

        }


    }



    IEnumerator GetTextFromWWW()
    {

#if UNITY_ANDROID
        WWW www = new WWW(urltakhfif);

#elif UNITY_IOS
        WWW www = new WWW(urltakhfif);
#else
        WWW www = new WWW(urltakhfif);
#endif
        yield return www;

    

            //رشته داخل فایل تکست رو بخونه و تبدیلش کنه به فلوت
            textFromWWW = www.text;

            takhfif = textFromWWW;
            UnityEngine.PlayerPrefs.SetString("savetakhfif",takhfif);
            UnityEngine.PlayerPrefs.Save();
        //شرط اپدیت رو بررسی میکنه




    }
}
