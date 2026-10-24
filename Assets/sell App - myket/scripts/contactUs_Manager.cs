

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contactUs_Manager : MonoBehaviour
{
    // Start is called before the first frame update
    public Sprite changebtn1, changebtn2;
    public GameObject guid, boxCaptcha, boxuser,btnUser, boxmatn,btnWhats, btncheck,saye;
    int change = 0,anim=0;

    public Text[] uitext;
    public string[] alphabet;
    string answer;
    public Text UserAnswer, userName;
    public Text titr;
    public MainAppController manager;
    
   
    void Start()
    {
        FirebaseBootstrap.WhenReady(() =>
        {
            Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeStatus()
    {
        if (change == 0)
        {
            change = 1;
            GetComponent<Image>().sprite = changebtn1;
            guid.SetActive(false);
            if (anim == 0)
            {
                GetComponent<Animator>().SetBool("bool1", true);
                anim = 1;
            }
            else
            {
                GetComponent<Animator>().SetBool("bool3", true);
            }

            saye.SetActive(true);
          
            Invoke("booOff", 0.9f);
        }
        else
        {
            change = 0;
            GetComponent<Image>().sprite = changebtn2;
            GetComponent<Animator>().SetBool("bool2", true);
            Invoke("booOff", 0.9f);
            saye.SetActive(false);
        }
    }

    void booOff()
    {
        GetComponent<Animator>().SetBool("bool1", false);
        GetComponent<Animator>().SetBool("bool2", false);
        GetComponent<Animator>().SetBool("bool3", false);
    }
    private int id , usedID;
    public void Btns(int id)
    {
       
        if (id == 1)
        {
            // Metrix.NewEvent("qazjk");
            FirebaseBootstrap.SafeLogEvent("contact_us", "instagram", 1);
            /*
            FirebaseBootstrap.SafeLogEvent(
  Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
  new Firebase.Analytics.Parameter[] {
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterItemName, "instagram"),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterValue, 1),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "contact_us"),
  }
);
*/
            Application.OpenURL(UnityEngine.PlayerPrefs.GetString("linkinsta"));
            usedID = 0;
        }
        else if (id == 2)
        {
            /*
            FirebaseBootstrap.SafeLogEvent(
Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
new Firebase.Analytics.Parameter[] {
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterItemName, "telegram"),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterValue, 1),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "contact_us"),
}
);
*/
          FirebaseBootstrap.SafeLogEvent("contact_us", "telegram", 1);
            Application.OpenURL(UnityEngine.PlayerPrefs.GetString("linktelegram"));
            usedID = 0;
        }
        else if (id == 3)
        {
            // Metrix.NewEvent("mpjnj");
          
            createBox(id);
            boxCaptcha.SetActive(true);
            usedID = 0;
        }
        else if (id == 4)
        {
            usedID = 0;
            /*
            FirebaseBootstrap.SafeLogEvent(
Firebase.Analytics.FirebaseAnalytics.EventSpendVirtualCurrency,
new Firebase.Analytics.Parameter[] {
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterItemName, "whatsapp"),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterValue, 1),
    new Firebase.Analytics.Parameter(
      Firebase.Analytics.FirebaseAnalytics.ParameterVirtualCurrencyName, "contact_us"),
}
);
*/
         FirebaseBootstrap.SafeLogEvent("contact_us", "whatsapp", 1);
            Application.OpenURL(UnityEngine.PlayerPrefs.GetString("linkWhatsapp"));
        }
        else if (id == 5)
        {
            usedID = 5;
            boxuser.SetActive(true);
            createBox(id);
            
           
        }

      
    }

    void createBox(int id)
    {
        btnUser.SetActive(false);
        btnWhats.SetActive(false);
        boxmatn.SetActive(true);

        btncheck.SetActive(true);

        if (id == 5)
        {
            //  titr.text = "ﺪﯿﯾﺎﻤﻧ ﺩﺭﺍﻭ ﺍﺭ ﺮﯾﺯ ﻦﺘﻣ ﺱﺎﻤﺗ ﻩﺭﺎﻤﺷ ﺾﯾﻮﻌﺗ ﺖﻬﺟ";
         
            userName.text = UnityEngine.PlayerPrefs.GetString("phoneNumber") + ": ﯼﺮﺑﺭﺎﮐ ﻡﺎﻧ";
            GameObject.Find("client_id").GetComponent<Text>().text = ": ﯼﺮﺑﺭﺎﮐ ﻪﺳﺎﻨﺷ" + "\n" + UnityEngine.PlayerPrefs.GetString("m_sarial");
            GameObject.Find("time").GetComponent<Text>().text = "version : " + Application.version;
            //GameObject.Find("icon app").GetComponent<Image>().sprite = manager.apps[manager.application_id - 1].GetComponent<Image>().sprite;
            if (UnityEngine.PlayerPrefs.GetInt("status") != 0)
            {
              
                //GameObject.Find("time").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("updated_at") + " : ﺎﻤﺷ ﺪﯾﺮﺧ ﺦﯾﺭﺎﺗ";
                if (GameObject.Find("manager").GetComponent<MainAppController>().application_id == UnityEngine.PlayerPrefs.GetInt("status"))
                {
                    GameObject.Find("status").GetComponent<Text>().text = "ﺖﺳﺍ ﻝﺎﻌﻓ ﯼﺯﺎﺑ ﻦﯿﻤﻫ ﺎﻬﻨﺗ ﺎﻤﺷ ﯼﺍﺮﺑ";

                }
                else if (UnityEngine.PlayerPrefs.GetInt("status")== 14)
                {
                    GameObject.Find("status").GetComponent<Text>().text = "ﺖﺳﺍ ﻝﺎﻌﻓ ﺎﻤﺷ ﯼﺍﺮﺑ ﮏﯾ ﻡﺮﺗ" ;

                }
                else if (UnityEngine.PlayerPrefs.GetInt("status") == 15)
                {
                    GameObject.Find("status").GetComponent<Text>().text = "ﺖﺳﺍ ﻝﺎﻌﻓ ﺎﻤﺷ ﯼﺍﺮﺑ ﻡﻭﺩ ﻡﺮﺗ";


                }
                else if (UnityEngine.PlayerPrefs.GetInt("status") == 16)
                {
                    GameObject.Find("status").GetComponent<Text>().text = "ﺖﺳﺍ ﻝﺎﻌﻓ ﺎﻤﺷ ﯼﺍﺮﺑ ﻡﻮﺳ ﻡﺮﺗ";


                }
                else if (UnityEngine.PlayerPrefs.GetInt("status") == 17)
                {
                    GameObject.Find("status").GetComponent<Text>().text = "ﺖﺳﺍ ﻝﺎﻌﻓ ﯽﺳﺭﺎﻓ ﻝﻭﺍ ﯼﺯﺎﺑﻭﺩ";


                }
                else if (UnityEngine.PlayerPrefs.GetInt("status") == 13 )
                {
                    GameObject.Find("status").GetComponent<Text>().text = "ﺖﺳﺍ ﻝﺎﻌﻓ ﺎﻤﺷ ﯼﺍﺮﺑ ﺎﻫ ﯼﺯﺎﺑ ﻡﺎﻤﺗ";

                }
            }

            if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
            {
                GameObject.Find("status").GetComponent<Text>().text = "ﺖﺴﯿﻧ ﻝﺎﻌﻓ ﯼﺯﺎﺑ";

            }

        }
        else
        {
            titr.text = "ﺪﯿﯾﺎﻤﻧ ﺩﺭﺍﻭ ﺍﺭ ﺮﯾﺯ ﻦﺘﻣ ﻁﺎﺒﺗﺭﺍ ﺖﻬﺟ";
            userName.text = "user_" + UnityEngine.PlayerPrefs.GetString("phoneNumber") + ": ﯼﺮﺑﺭﺎﮐ ﻡﺎﻧ";
            uitext[0].text = alphabet[Random.Range(0, 20)];
            uitext[1].text = alphabet[Random.Range(0, 20)];
            uitext[2].text = alphabet[Random.Range(0, 20)];
            uitext[3].text = alphabet[Random.Range(0, 20)];
            answer = uitext[0].text + uitext[1].text + uitext[2].text + uitext[3].text;
            Debug.Log(answer);

            btnUser.SetActive(false);
            btnWhats.SetActive(false);
            boxmatn.SetActive(true);

            btncheck.SetActive(true);

        }

   


    }

    public void checkAnswer()
    {
        Debug.Log(usedID);
        if (UserAnswer.text == answer)
        {
         
            boxmatn.SetActive(false);
        
            btncheck.SetActive(false);

            if (usedID == 5)
            {
                titr.text = "ﺩﻮﺑ ﺰﯿﻣﺁ ﺖﯿﻘﻓﻮﻣ" + "\n" + "ﺪﯿﻧﺰﺑ ﺍﺭ ﺮﯾﺯ ﻪﻤﮐﺩ ﺱﺎﻤﺗ ﻩﺭﺎﻤﺷ ﺮﯿﯿﻐﺗ ﯼﺍﺮﺑ";

                btnUser.SetActive(true);
            }
            else
            {
                titr.text = "ﺩﻮﺑ ﺰﯿﻣﺁ ﺖﯿﻘﻓﻮﻣﺮﯾﺯ ﻪﻤﮐﺩ ﻖﯾﺮﻃ ﺯﺍ ﺩﻮﺑ ﺰﯿﻣﺁ ﺖﻘﯿﻓﻮﻣ" + "\n" + "ﺪﯿﻨﮐ ﺡﺮﻄﻣ ﻥﺎﺒﯿﺘﺸﭘ ﺎﺑ ﺍﺭ ﺩﻮﺧ ﻞﮑﺸﻣ";
                btnWhats.SetActive(true);
            }
        }
        else
        {
            createBox(id);
            titr.text = "ﺪﯿﯾﺎﻤﻧ ﺵﻼﺗ ﺍﺩﺪﺠﻣ";
        }
    }

 
}
