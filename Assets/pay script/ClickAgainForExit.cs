using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ClickAgainForExit : MonoBehaviour {

    
    private bool clickedBefore = false;

    public GameObject quitobject;

    void Update()
    {
        //اولین بار که میزنه دکمه بک  رو
        if (Input.GetKeyDown(KeyCode.Escape) && !clickedBefore)
        {
         
           

            //به کاربر پیام نشون بده که دوباره بزن
            quitobject.SetActive(true);

            //تایمر منتظر وایستادن رو روشن کنه
            StartCoroutine(quitingTimer());
        }
    }

    IEnumerator quitingTimer()
    {
       
        yield return null;

        //تایمر رو ست میکنیم ثانیه هاشو
        const float timerTime = 3f;
        float counter = 0;

        while (counter < timerTime)
        {
          //بشمر تا 3ثانیه
            counter += Time.deltaTime;

            //در طی مدت شمارش اگه دوباره زد خارج شو از اپلیکیشن
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("Back Button pressed for the second time. EXITING.....");
                Quit();
            }

         
            yield return null;
        }

        Debug.Log("Timer finished...Back Button was NOT pressed for the second time within: '" + timerTime + "' seconds");

        //سه ثانیه تموم شد و دکمه خروجو نزد کاربر
        quitobject.SetActive(false);
       
        clickedBefore = false;
    }

    void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
   // System.Diagnostics.Process.GetCurrentProcess().Kill();
#endif
    }
}
