using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Advertisements;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

[Serializable]
public class Player
{

    public string playerId;
    public string playerLoc;
    public string playerNick;
}

[Serializable]
public class ServerList
{
    public List<string> serversList;
}


public class startcheck : MonoBehaviour
{
    public bool isBinto;
    public static string advertisingIdentifier;
    public static startcheck instance;
    public string orderId = null, takh = null;

    public GameObject lockkk,phoneNumber,contactus,toastCopy;
    public Text takhtext;
    public GameObject setting, btn,btnbazar, boxkharid ,boxTakhfif,Btn, BoxAfter100, boxAfter50, loading;


    private string webTextTakhfif = string.Empty, dico, witchClick;
    public Text guiTextTakhfif = null;

    public Text orderrr;

    private string json_path;

    //private string list_ip;
    private string url = "http://www.papitafood.ir/service/paymenttakhfif.php?clientid=";

    // private string url = "";
    private string urltakhfif = "";
    private string url2 = "/service/paymenttakhfif.php?orderid=";
    private string urlt = "/service/paymenttakhfif.php?orderid=";
#if UNITY_ANDROID
    private string urlcheck2 = "http://www.papitafood.ir/service/checkPayment.php?clientid=";

#elif UNITY_IOS
    private string urlcheck2 = "http://www.papitafood.ir/service/checkPayment.php?clientid=";
#else
    private string urlcheck2 = "http://www.papitafood.ir/service/checkPayment.php?clientid=";

#endif

    private string urlcheck = "";
    private string urlping = "/service/checkServer.php";
    private string textFromWWW1, textFromServer, textFromWWW2;
    public string serverdevise, serverdeviseios;
    public Text imeiii;

    int first = 0;
    int first2 = 0;

#if UNITY_ANDROID
    private string checkPaymentPath = "/service/checkPayment.php?clientid=";

#elif UNITY_IOS
    private string checkPaymentPath = "/service/checkPayment.php?clientid=";

#else
    private string checkPaymentPath = "/service/checkPayment.php?clientid=";
#endif

    private string serverListUrl =
        "http://www.amulaaki.ir/service/serverList.php?access=f050e93d4af4038ac1dc5aedb0deff6d";

    public int activation;
    public int activation2;

    private ServerList m_fetchedServerList = new ServerList();
    private bool m_urlsFetched = false;
    private int m_currentServerListIndex = 0, witchBuy;
    private bool m_keepCheckingServer = false;

    private bool m_isCurrentUrlChecked = false;
    private bool m_isCheckingUrl = false;

    private string m_serial = "";

    public string SerialNum
    {
        get { return m_serial; }
    }

    private string m_currentValidDomain = "";

    public string CurrentValidDomain
    {
        get { return m_currentValidDomain; }
    }

    private string[] STATIC_URL =
    {

        "http://www.www.papitafood.ir/",
        "http://www.www.papitafood.ir/"

    };


    [SerializeField] private int tst = 0;
    // Use this for initialization

    void Start()
    {


     




        //Invoke("checkmarket", 2f);
        checkmarket();
        Invoke("checktakhfif", 2f);
    
        instance = this;


        m_currentValidDomain = "";


        first = UnityEngine.PlayerPrefs.GetInt("phoneee");
        first2 = UnityEngine.PlayerPrefs.GetInt("frun2");



        witchBuy = UnityEngine.PlayerPrefs.GetInt("witchBuy");


        tst = 0;

        m_urlsFetched = false;
        m_isCurrentUrlChecked = false;

         StartCoroutine(CheckIfUrlIsBlocked());

       // UnityEngine.PlayerPrefs.SetInt("activation",100);






        orderrr.text = "act: " + activation;

        int firstplay= UnityEngine.PlayerPrefs.GetInt("firstplay");
        if (firstplay == 1)
        {
            UnityEngine.PlayerPrefs.SetInt("activation2",-5);
        }
        else
        {
            UnityEngine.PlayerPrefs.SetInt("activation2", 100);
            UnityEngine.PlayerPrefs.SetInt("firstplay", 1);
        }


        activation = UnityEngine.PlayerPrefs.GetInt("activation");
        
        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");


        if (activation > 0)
        {

            lockkk.SetActive(false);
        



        }
        else if (activation2 > 0)
        {
            lockkk.SetActive(false);
            boxkharid.SetActive(false);
            phoneNumber.SetActive(false);
            contactus.SetActive(false);
            setting.SetActive(false);
            
            Debug.Log("ghofl az start baz shod");
        }


        else
        {
           
            lockkk.SetActive(true);
          

            this.orderId = UnityEngine.PlayerPrefs.GetString("orderId");

            //  Debug.Log("orderId: " + orderId);

            orderrr.text += "\n" + orderId;

            // آیا از درگاه داری بر میگردی ؟
            // و شناسه سفارش برات تولید شده ؟
            if (string.IsNullOrEmpty(orderId))
            {
                // دفعه اول که اپ باز میشه

                this.orderId = Guid.NewGuid().ToString();
                UnityEngine.PlayerPrefs.SetString("orderId", orderId);
                UnityEngine.PlayerPrefs.Save();

                orderrr.text += "\n" + orderId + "new!";


            }
            else
            {
                // بررسی سرور
                m_keepCheckingServer = true;
                // StartCoroutine(SendPostCoroutine());
            }



        }

        //در اینجا کد گوشی رو به متن اختصاص میدیم
#if UNITY_ANDROID
        string serial = SystemInfo.deviceUniqueIdentifier;
        if (string.IsNullOrEmpty(serial)) //اگه کاربر اجازه دسترسی به شناسش نده موقع نصب
        {
            serial = orderId;
            imeiii.text = "OR" + serial;
            m_serial = imeiii.text;
        }
        else
        {
            imeiii.text = "SN" + serial;
            m_serial = imeiii.text;
        }



#elif UNITY_IOS

        string serial = Device.advertisingIdentifier;
          if (string.IsNullOrEmpty(serial))
         {
            serial = orderId;
            imeiii.text = "OR" + serial;
            m_serial = imeiii.text;
          }
          else{
            imeiii.text = "SN" + serial;
            m_serial = imeiii.text;
          }

#else
           string serial = SystemInfo.deviceUniqueIdentifier;
        if (string.IsNullOrEmpty(serial))
        {
            serial = orderId;
            imeiii.text = "OR" + serial;
            m_serial = imeiii.text;
        }
        else
        {
            imeiii.text = "SN" + serial;
            m_serial = imeiii.text;
        }


#endif

        UnityEngine.PlayerPrefs.SetString("clintid", m_serial);

        StartCoroutine(checkServerForpayAgain());
        
        
    }

    public void checkstartmarket()
    {
        
            lockkk.SetActive(false);
            boxkharid.SetActive(false);
            phoneNumber.SetActive(false);
            contactus.SetActive(false);
            setting.SetActive(false);

         
       
    }

    public void touchlock()
    {
        activation = UnityEngine.PlayerPrefs.GetInt("activation");

        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");

        if (activation > 0)
        {

            lockkk.SetActive(false);

        }
        else if (activation2 > 0)
        {
            lockkk.SetActive(false);
        }
        else
        {
            boxkharid.SetActive(true);
        }
    }

  

    public IEnumerator GetServersList()
    {

        // UnityWebRequest www = UnityWebRequest.Get(serverListUrl);
        UnityWebRequest www = UnityWebRequest.Get(serverListUrl + "&ttt=" + UnityEngine.Random.Range(0, 10000));
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {

        }
        else
        {
            m_urlsFetched = true;
            try
            {
                m_fetchedServerList = JsonUtility.FromJson<ServerList>(www.downloadHandler.text);

                if (m_fetchedServerList.serversList.Count > 0)
                {
                    m_currentServerListIndex = 0;
                    url = m_fetchedServerList.serversList[m_currentServerListIndex] + url2;
                    urltakhfif = m_fetchedServerList.serversList[m_currentServerListIndex] + urlt;
                    urlcheck = m_fetchedServerList.serversList[m_currentServerListIndex] + checkPaymentPath;
                    m_isCheckingUrl = false;
                    m_isCurrentUrlChecked = false;
                    //StartCoroutine(CheckIfUrlIsBlocked());
                }

            }
            catch (Exception)
            {

                // Debug.Log("Problem parsing servers list json");
            }



        }
    }

    void OnApplicationFocus()
    {
        StartCoroutine(checkAgain());

        activation = UnityEngine.PlayerPrefs.GetInt("activation");
        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");

  

        witchBuy = UnityEngine.PlayerPrefs.GetInt("witchBuy");

        if (activation > 0)
        {
            if (witchBuy == 100)
            {

                loading.SetActive(false);
                BoxAfter100.SetActive(true);

            }
            else if (witchBuy == 50)
            {
                loading.SetActive(false);
                boxAfter50.SetActive(true);

            }
            else
            {
                lockkk.SetActive(false);
                if (!isBinto)
                {
                    SceneManager.LoadScene("Main Menu");
                }



                boxkharid.SetActive(false);

                UnityEngine.PlayerPrefs.SetInt("frun2", 1);
                first2 = 1;
                UnityEngine.PlayerPrefs.Save();
            }


        }
        else if (activation2 > 0)
        {
            lockkk.SetActive(false);

            contactus.SetActive(false);
            boxkharid.SetActive(false);

            phoneNumber.SetActive(false);
        }
        else
        {

            lockkk.SetActive(true);

          

            // آیا از درگاه داری بر میگردی ؟
            // و شناسه سفارش برات تولید شده ؟
            if (string.IsNullOrEmpty(orderId))
            {
                // دفعه اول که اپ باز میشه

                this.orderId = Guid.NewGuid().ToString();
                UnityEngine.PlayerPrefs.SetString("orderId", orderId);
                UnityEngine.PlayerPrefs.Save();

                orderrr.text += "\n" + orderId + "new!";
            }
            else
            {
                // بررسی سرور
                m_keepCheckingServer = true;
                //  StartCoroutine(SendPostCoroutine());
            }
        }






    }

    public void clik()
    {
        StartCoroutine(CheckIfUrlIsBlocked());
        //از سایت خونده
        string takhfiff = takh.ToString();

        // از کاربر گرفته
        string takhfifff = takhtext.text.ToLower();

        string dico = takhfifff;

        Invoke("timer", 4);

#if UNITY_ANDROID
        Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico);

#elif UNITY_IOS
           Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdeviseios + "&dico=" + dico);
#else
            Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico);
#endif


     

    }

    public void clik100()
    {
        StartCoroutine(CheckIfUrlIsBlocked());

        witchClick = "100";
        string dico = "papita100";





#if UNITY_ANDROID
        Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico);

#elif UNITY_IOS
           Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdeviseios + "&dico=" + dico);
#else
            Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico);
#endif


    }

    public void clik50()
    {
        StartCoroutine(CheckIfUrlIsBlocked());

        witchClick = "50";
        string dico = "papita50";

        //با تبلیغات ببینن یا نه
        UnityEngine.PlayerPrefs.SetInt("showfree", 0);
        UnityEngine.PlayerPrefs.SetInt("statusAD", 0);

#if UNITY_ANDROID
        Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico);

#elif UNITY_IOS
           Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdeviseios + "&dico=" + dico);
#else
            Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico);
#endif





    }

    public void clikJust()
    {


        StartCoroutine(CheckIfUrlIsBlocked());


        string phoneNumber = UnityEngine.PlayerPrefs.GetString("phonenumber");

#if UNITY_ANDROID
        Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?clientid=" +
                            m_serial + "&dico=" + dico + "&mobile=" + phoneNumber + "&device=" + serverdevise);

#elif UNITY_IOS
            Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?orderid="  + orderId + "&clientid=" +
                            m_serial + "&device=" + serverdeviseios + "&dico=" + dico+ "&mobile=" + phoneNumber);
#else
            Application.OpenURL("http://www.papitafood.ir/service/paymenttakhfif.php?orderid=" + orderId + "&clientid=" +
                            m_serial + "&device=" + serverdevise + "&dico=" + dico+ "&mobile=" + phoneNumber);
#endif



    }


    void timer()
    {
       // loading.SetActive(false);
        btn.SetActive(true);
    }



    void Update()
    {
#if UNITY_IOS
         if (activation > 0)
        {

            lockkk.SetActive(false);
 

            boxkharid.SetActive(false);
           

            Destroy(CheckInternet);
            Instagram.SetActive(true);

        }
        //برای چک کردن گروه اپل
        else if(activation2 > 0)
        {
         contactus.SetActive(false);
            lockkk.SetActive(false);

        phoneNumber.SetActive(false);
          
            Instagram.SetActive(false);
            UserBox.SetActive(false);
         
        }

#endif


        if (m_keepCheckingServer && m_isCurrentUrlChecked)
            {
            Debug.Log("cheking   ............... ch");
                //if (m_fetchedServerList.serversList.Count > 0)
                {
                    StartCoroutine(SendPostCoroutine());
                    m_keepCheckingServer = false;
                }
            }

        







    }


    public IEnumerator SendPostCoroutine()
    {

       // Debug.Log(urlcheck + orderId);

        UnityWebRequest www =  UnityWebRequest.Get(urlcheck2 + m_serial);


        orderrr.text += "\nCheking...";

        yield return www.SendWebRequest();





        if (www.isNetworkError || www.isHttpError)
        {
            // اینجا نتونسته بره خرید باید شبکه وصل شه تا دوباره بتونه از سرور اوکی رو بگیره
            // اینجا باید پیغام فارسی به کاربر بدین که مثلا اینترنتش قطه


            UseNextUrlForChecking(true);

            orderrr.text += "\nError network: " + www.error;
        }
        else
        {

            if (www.downloadHandler.text.Contains("success"))
            {
                Debug.Log("successful!");

                UnityEngine.PlayerPrefs.SetInt("activation", 100);
                Btn.SetActive(false);
                loading.SetActive(false);
                lockkk.SetActive(false);
                witchBuy = UnityEngine.PlayerPrefs.GetInt("witchBuy");

                if (witchClick == "100")
                {
                    UnityEngine.PlayerPrefs.SetInt("witchBuy", 100);
                    loading.SetActive(false);
                    BoxAfter100.SetActive(true);
                }
                else if (witchClick == "50")
                {
                    UnityEngine.PlayerPrefs.SetInt("witchBuy", 50);
                    loading.SetActive(false);
                    boxAfter50.SetActive(true);
                }

                else
                {
                    SceneManager.LoadScene("Main Menu");
                }
              

                boxTakhfif.SetActive(false);
                guiTextTakhfif.color = Color.white;
                guiTextTakhfif.text = "ﺪﺷ ﻡﺎﺠﻧﺍ ﺖﯿﻘﻓﻮﻣ ﺎﺑ ﺎﻤﺷ ﺪﯾﺮﺧ" + Environment.NewLine + " ﻪﺑ ﺎﻤﺷ ﺩﺎﻤﺘﻋﺍ ﺯﺍ ﻥﻮﻨﻤﻣ " + Environment.NewLine + "ﺎﻫ ﻪﭽﺑ ﻩﺮﯾﺰﺟ ﻪﻋﻮﻤﺠﻣ";
                UnityEngine.PlayerPrefs.SetInt("frun2", 1);
                first2 = 1;
                UnityEngine.PlayerPrefs.Save();

                orderrr.text += "\nNet responce OK";
                m_keepCheckingServer = false;
            
            }
            else if (www.downloadHandler.text.Contains("error"))
            {

                orderrr.text += "\nNet response: " + www.downloadHandler.text;
          
                // سرور اوکی نداد
                // دوباره باید بره تو صفحه خرید

                m_keepCheckingServer = false;
            }


        }


    }

    public IEnumerator CheckIfUrlIsBlocked()
    {


        foreach (var static_url in STATIC_URL)
        {
            Debug.Log(static_url);
            using (var webRequest = UnityWebRequest.Get(static_url + urlping))
            {
                webRequest.downloadHandler = new DownloadHandlerBuffer();

                webRequest.timeout = 6;

                yield return webRequest.SendWebRequest();
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {

                    // network disable
                    //m_isCurrentUrlChecked = false;
                }
                else if (webRequest.isDone)
                {
                    //if (webRequest.downloadHandler.text == "OK")
                    if (webRequest.downloadHandler.text.ToLower().Contains("ok") &&
                        webRequest.downloadHandler.text.Length < 5 && !m_isCurrentUrlChecked)
                    {
                        // open payment.php

                        m_isCurrentUrlChecked = true;
                        m_currentValidDomain = static_url;
                        urlcheck = static_url + checkPaymentPath;
                        url = static_url + url2;
                        break;
                    }

                }

            }
        }

        if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(urlcheck))
        {
            yield return GetServersList(); // fetch servers

            foreach (var domain in m_fetchedServerList.serversList)
            {
                Debug.Log(domain);
                using (var webRequest = UnityWebRequest.Get(domain + urlping))
                {
                    webRequest.downloadHandler = new DownloadHandlerBuffer();

                    webRequest.timeout = 6;

                    yield return webRequest.SendWebRequest();
                    if (webRequest.isNetworkError || webRequest.isHttpError)
                    {

                        // network disable
                        //m_isCurrentUrlChecked = false;
                    }
                    else if (webRequest.isDone)
                    {
                        //if (webRequest.downloadHandler.text == "OK")
                        if (webRequest.downloadHandler.text.ToLower().Contains("ok") &&
                            webRequest.downloadHandler.text.Length < 5 && !m_isCurrentUrlChecked)
                        {
                            // open payment.php

                            m_isCurrentUrlChecked = true;
                            m_currentValidDomain = domain;
                            urlcheck = domain + checkPaymentPath;
                            url = domain + url2;
                            break;
                        }

                    }

                }
            }
        }

    }

    private void UseNextUrlForChecking(bool shouldKeepChecking)
    {
        if (m_fetchedServerList.serversList == null)
        {
            return;
        }

        m_currentServerListIndex++;
        if (m_currentServerListIndex < m_fetchedServerList.serversList.Count)
        {
            urlcheck = m_fetchedServerList.serversList[m_currentServerListIndex] + checkPaymentPath;
            url = m_fetchedServerList.serversList[m_currentServerListIndex] + url2;
            m_keepCheckingServer = shouldKeepChecking;
        }
        else if (m_fetchedServerList.serversList.Count > 0)
        {
            m_currentServerListIndex = 0;
            urlcheck = m_fetchedServerList.serversList[m_currentServerListIndex] + checkPaymentPath;
            url = m_fetchedServerList.serversList[m_currentServerListIndex] + url2;
            m_keepCheckingServer = shouldKeepChecking;
        }
    }

    //چک کنه سرور وصله و جواب میده یا نه که بعد بره خرید یا نخریدن یوزر رو چک کنه

    IEnumerator checkServerForpayAgain()
    {


        WWW www = new WWW("http://www.papitafood.ir/service/response.php");

        yield return www;

        if (www.error != null)
        {

        }
        else
        {

            textFromServer = www.text.ToString();
            // trim برای پاک کردن اسپیس یا چیزهای اضافی   
            if (textFromServer.Trim().Equals("ok"))

            {
                Debug.Log("server says " + textFromServer + "so we check again");
                StartCoroutine(checkAgain());
                UnityEngine.PlayerPrefs.SetInt("checkphone", 1);


            }
            else
            {
                Debug.Log("Server is not response or server says " + textFromServer);
            }
        }

    }

    // چک کردن دوباره خرید
    IEnumerator checkAgain()
    {



        WWW www = new WWW(urlcheck2 + m_serial);


        yield return www;

        if (www.error != null)
        {

        }
        else
        {

            textFromWWW1 = www.text;


            if (textFromWWW1.Contains("success"))

            {
                Debug.Log("successful in check again!");

                Debug.Log("successful!");

                UnityEngine.PlayerPrefs.SetInt("activation", 100);
                Btn.SetActive(false);
                loading.SetActive(false);
                lockkk.SetActive(false);
                witchBuy = UnityEngine.PlayerPrefs.GetInt("witchBuy");

                if (witchClick == "100")
                {
                    UnityEngine.PlayerPrefs.SetInt("witchBuy", 100);
                    loading.SetActive(false);
                    BoxAfter100.SetActive(true);
                }
                else if (witchClick == "50")
                {
                    UnityEngine.PlayerPrefs.SetInt("witchBuy", 50);
                    loading.SetActive(false);
                    boxAfter50.SetActive(true);
                }

                else
                {
                    SceneManager.LoadScene("Main Menu");
                }


                boxTakhfif.SetActive(false);
                guiTextTakhfif.color = Color.white;
                guiTextTakhfif.text = "ﺪﺷ ﻡﺎﺠﻧﺍ ﺖﯿﻘﻓﻮﻣ ﺎﺑ ﺎﻤﺷ ﺪﯾﺮﺧ" + Environment.NewLine + " ﻪﺑ ﺎﻤﺷ ﺩﺎﻤﺘﻋﺍ ﺯﺍ ﻥﻮﻨﻤﻣ " + Environment.NewLine + "ﺎﻫ ﻪﭽﺑ ﻩﺮﯾﺰﺟ ﻪﻋﻮﻤﺠﻣ";
                UnityEngine.PlayerPrefs.SetInt("frun2", 1);
                first2 = 1;
                UnityEngine.PlayerPrefs.Save();

                orderrr.text += "\nNet responce OK";
                m_keepCheckingServer = false;
            }
            else if (textFromWWW1.Contains("error"))
            {
                UnityEngine.PlayerPrefs.SetInt("activation", -5);
                UnityEngine.PlayerPrefs.GetInt("activation2");
                if (activation2 > 0)
                {
                    lockkk.SetActive(false);
                    contactus.SetActive(false);
                    phoneNumber.SetActive(false);
                }
                else
                {
                    lockkk.SetActive(true);
                  

                }


                UnityEngine.PlayerPrefs.SetInt("frun2", 2);
                first2 = 2;
                UnityEngine.PlayerPrefs.Save();

            }
        }
    }




    // برای اینکه گوشی ها  رو از سمت سرور آزاد کنیم که برن توش
    void checktakhfif()
    {
        takh = UnityEngine.PlayerPrefs.GetString("savetakhfif");

    }

    void checkmarket()
    {

        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");
        activation = UnityEngine.PlayerPrefs.GetInt("activation");



        if (activation > 0)
        {

            lockkk.SetActive(false);
          

            boxkharid.SetActive(false);



        }
        else if (activation2 > 0)
        {
            lockkk.SetActive(false);
            contactus.SetActive(false);
            phoneNumber.SetActive(false);
            boxkharid.SetActive(false);

       
            Debug.Log("ghofl az start baz shod");
        }
    }


    public void changePrice()
    {
        StartCoroutine(GetTextTakhfifFromWebsite());

    }

    public IEnumerator GetTextTakhfifFromWebsite()
    {

        dico = takhtext.text.ToLower();

       

        using (UnityWebRequest www = UnityWebRequest.Get("http://www.papitafood.ir/service/ckeckdico.php?dico=" + dico))
        {
            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.Send();

            if (www.isNetworkError)
          {
            guiTextTakhfif.text = "ﺖﺴﯿﻧ ﻞﺼﺘﻣ ﺎﻤﺷ ﺖﻧﺮﺘﻨﯾﺍ";
          }
        else
          {
            OnSendPhoneNumberSuccess(www.downloadHandler.text);
          }
        }
       // StartCoroutine(checkdico());


    }

    private void OnSendPhoneNumberSuccess(string text)
    {
   
        if (text.Contains("zero"))
        {
            guiTextTakhfif.text = "ﺪﺷﺎﺑ ﯽﻤﻧ ﺮﺒﺘﻌﻣ ﻩﺪﺷ ﺩﺭﺍﻭ ﺪﮐ";
                guiTextTakhfif.color = Color.red;
            guiTextTakhfif.fontSize = 30;
            btn.SetActive(true);
            btnbazar.SetActive(false);

        }
        else
        {
            guiTextTakhfif.text = " ﺖﻓﺎﯾ ﺮﯿﯿﻐﺗ ﻥﺎﻣﻮﺗ  " + text + " ﻪﺑ ﺎﻤﺷ ﺪﯾﺮﺧ ﻎﻠﺒﻣ ";
            guiTextTakhfif.fontSize = 30;
            guiTextTakhfif.color = Color.green;
            btn.SetActive(false);
            btnbazar.SetActive(true);
        }
    }
 
}
