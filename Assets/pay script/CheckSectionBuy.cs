using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class CheckSectionBuy : MonoBehaviour {

    // Use this for initialization
    public bool isBintro;
    public GameObject section1, section2;
    private string textFromWWW, notif, titleNotif;
    private int activation;


    void Start () {

        activation = UnityEngine.PlayerPrefs.GetInt("activation");

        if (isBintro)
        {
            if (activation > 0)
            {
               
               
                Invoke("notification", 3f);

                int MyIndex = UnityEngine.Random.Range(0, 5);

                if (MyIndex == 0)
                {
                    notif = "بیا با هم بازی کنیم و یاد بگیریم " + " \n" + "\U0001F60D" + "\U0001F60D";
                    titleNotif = "کجایی عزیزم؟";
                }
                else if (MyIndex == 1)
                {
                    notif = "دلم برات تنگ شده" + " \n" + "\U0001F60D" + "\U0001F60D";
                    titleNotif = "کجایی خوشگلم؟";
                }
                else if (MyIndex == 2)
                {
                    notif = "تو زمین برفیمون بازی کردی؟";
                    titleNotif = "دلم برات تنگ شده";

                }
                else if (MyIndex == 3)
                {
                    notif = "امروزبیا چنتا کلمه یاد بگیریم";
                    titleNotif = "چی یاد گرفتی از کلمه ها؟";
                }
                else
                {
                    notif = "اگه شماره تماستو توی بازی ثبت نکردی الان وقتشه" + " \n" + "\U0001F60D" + "\U0001F60D";
                    titleNotif = "کجایی عزیزم ؟";
                }


           
            }
            else
            {

                int MyIndex = UnityEngine.Random.Range(0, 5);

                if (MyIndex == 0)
                {
                    notif = "بیا با هم بازی کنیم و یاد بگیریم " + " \n" + "\U0001F973" + "\U0001F973";
                    titleNotif = "کجایی عزیزم؟";
                }
                else if (MyIndex == 1)
                {
                    notif = "منو فعال نمیکنی ؟";
                    titleNotif = "بیا با هم بازی کنیم";
                }
                else if (MyIndex == 2)
                {
                    notif = "قول میدم کلی چیز با هم یاد بگیریم" + " \n" + "\U0001F973" + "\U0001F973";
                    titleNotif = "اگه منو فعال کنی";

                }
                else if (MyIndex == 3)
                {
                    notif = "هرروز یه چیزی باهم تمرین میکنیم" + " \n" + "\U0001F973" + "\U0001F973";
                    titleNotif = "اگه منو فعال کنی";
                }
                else
                {
                    notif = "بازی پاپیتا رنگی منتظرته" + " \n" + "\U0001F973" + "\U0001F973";
                    titleNotif = "کجایی عزیزم ؟";
                }


           
            }
        

            StartCoroutine(GetTextFromWWW());
        }
        else
        {
           if( UnityEngine.PlayerPrefs.GetInt("section") == 1)
            {
                section2.SetActive(true);
                section1.SetActive(false);
            }
           else
            {
                section1.SetActive(true);
                section2.SetActive(false);
            }

        }
       
    }

    IEnumerator GetTextFromWWW()
    {

#if UNITY_ANDROID
        WWW www = new WWW("http://www.papitafood.ir/service/CheckSection.php");

#elif UNITY_IOS
        WWW www = new WWW("http://www.papitaplus.ir/service/Giftios.php");
#else
        WWW www = new WWW("http://www.papitaplus.ir/service/Gift.php");
#endif
        yield return www;

        if (www.error != null)
        {

        }
        else
        {
            //رشته داخل فایل تکست رو بخونه و تبدیلش کنه به فلوت
            textFromWWW = www.text;
            float NewVersion = float.Parse(textFromWWW);



            if (NewVersion == 1)

            {
                UnityEngine.PlayerPrefs.SetInt("section", 1);
            }
            else
            {
                UnityEngine.PlayerPrefs.SetInt("section", 0);
            }
        }
    }

    void notification()
    {
        // نوتیف دوم در صورت نیومدنش به بازی



        int MyIndex = UnityEngine.Random.Range(0, 5);

        if (MyIndex == 0)
        {
            notif = "چند روزه به پاپیتا سر نزدی" + " \n" + "\U0001F60D" + "\U0001F60D";
            titleNotif = "کجایی عزیزم؟";
        }
        else if (MyIndex == 1)
        {
            notif = "دلت برای پاپیتا تنگ نشده" + " \n" + "\U0001F60D" + "\U0001F60D";
            titleNotif = "کجایی خوشگلم؟";
        }
        else if (MyIndex == 2)
        {
            notif = "تو زمین برفیمون بازی کردی؟";
            titleNotif = "دلم برات تنگ شده";

        }
        else if (MyIndex == 3)
        {
            notif = "چند روزه نیومدی کلمه یادبگیریم با هم";
            titleNotif = "چی یاد گرفتی از کلمه ها؟";
        }
        else
        {
            notif = "بازی پاپیتا منتظرته" + " \n" + "\U0001F60D" + "\U0001F60D";
            titleNotif = "کجایی عزیزم ؟";
        }


    
    }
}
