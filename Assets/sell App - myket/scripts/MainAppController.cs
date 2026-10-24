using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine.SceneManagement;


public class MainAppController : MonoBehaviour
{
    // Start is called before the first frame update
    //MainMenuController
    public static MainAppController instance;
    public enum Store
    {
        googleplay,
        Bazar,
        Mayket,
        AppStore,
        toop

    }
    public bool clearData;

    public Color mycolor;
    public int application_id, witchTerm;
    public string scheme, host;


    public string mainMenuName, mainMenuIranStor;
    public Store mystore = new Store();
    string orderId, serverdevise, whereFrom, PhoneUrlPath = "/service/addPhone.php", m_sendPhoneNumberPath;
    string IPAddress;
    int SlideCounter;
    public GameObject[] Slides;
    private bool firstSendNum = true;

    public GameObject phoneObject, buyBtn, video, afterbuy, boxUpdate;
    private float mytimeofVideo;
    public addStatusBazar addStatus;
    [HideInInspector] public int witchProductForBuy;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }


    IEnumerator Start()
    {
        getGameObject();


        if (clearData)
            UnityEngine.PlayerPrefs.DeleteAll();


        StartCoroutine(checkAgain());

        buyBtn.SetActive(false);


        for (int i = 0; i < Slides.Length; i++)
        {
            Slides[i].SetActive(false);
        }


        // اینجا فقط بار اول اجرای بازی هستش
        if (UnityEngine.PlayerPrefs.GetInt("store") == 0)
        {
            getSerial();

        }
        checkStore();


        m_serial = UnityEngine.PlayerPrefs.GetString("m_sarial");
        // m_sendPhoneNumberPath = AppDomain + PhoneUrlPath;
        serverdevise = SystemInfo.deviceName + " ver :" + Application.version;


        whereFrom = UnityEngine.PlayerPrefs.GetString("M_store");


        //ثبت دیتای یوزر
        SetData();

        yield return new WaitForSeconds(4);
        InviteController();
        yield return new WaitForSeconds(1.5f);
        stopvideo();

    }
    void checkStore()
    {

        // اینجا هربار که بازی اجرا میشه
        if (mystore == Store.googleplay)
        {
            UnityEngine.PlayerPrefs.SetInt("store", 1);
            UnityEngine.PlayerPrefs.SetString("M_store", "1");
        }
        else if (mystore == Store.Bazar)
        {
            UnityEngine.PlayerPrefs.SetInt("store", 2);
            UnityEngine.PlayerPrefs.SetString("M_store", "2");
        }

        else if (mystore == Store.Mayket)
        {
            UnityEngine.PlayerPrefs.SetInt("store", 3);
            UnityEngine.PlayerPrefs.SetString("M_store", "3");
        }
        else if (mystore == Store.AppStore)
        {
            UnityEngine.PlayerPrefs.SetInt("store", 4);
            UnityEngine.PlayerPrefs.SetString("M_store", "4");
        }
    }
    void getGameObject()
    {

      
        buyBtn = GameObject.Find("buyBtn");
        Invoke("storecontroller", 4f);
        if (UnityEngine.PlayerPrefs.GetInt("he is in game") == 0)
        {
            video.SetActive(true);
            GameObject.Find("section Buy").GetComponent<AudioSource>().Stop();
            mytimeofVideo = 6;
        }
        else
        {
            mytimeofVideo = 0.01f;
            UnityEngine.PlayerPrefs.SetInt("he is in game", 0);
        }

        Debug.Log("the status of user : " + UnityEngine.PlayerPrefs.GetInt("status"));
    }
    void getSerial()
    {


#if UNITY_ANDROID
        string serial = SystemInfo.deviceUniqueIdentifier;


        if (string.IsNullOrEmpty(serial)) //اگه کاربر اجازه دسترسی به شناسش نده موقع نصب
        {
            this.orderId = Guid.NewGuid().ToString();
            UnityEngine.PlayerPrefs.SetString("orderId", orderId);
            UnityEngine.PlayerPrefs.Save();


            m_serial = orderId;
        }
        else
        {

            m_serial = "SN" + serial;
            UnityEngine.PlayerPrefs.SetString("m_sarial", m_serial);
        }



#elif UNITY_IOS

        string serial = Device.advertisingIdentifier;
        serverdevise = SystemInfo.deviceName + " ver :" + Application.version;
        if (string.IsNullOrEmpty(serial))
        {
            serial = orderId;
            m_serial = "OR" + serial;
        }
        else
        {
            m_serial = "SN" + serial;
            UnityEngine.PlayerPrefs.SetString("m_sarial", m_serial);
        }

#else
        string serial = SystemInfo.deviceUniqueIdentifier;
        if (string.IsNullOrEmpty(serial))
        {
            serial = orderId;
            m_serial = "OR" + serial;
        }
        else
        {
            m_serial = "SN" + serial;
            UnityEngine.PlayerPrefs.SetString("m_sarial", m_serial);
        }

#endif
    }


    void stopvideo()
    {
        Debug.Log("forceUpdate :" + UnityEngine.PlayerPrefs.GetInt("forceupdate"));
        if (UnityEngine.PlayerPrefs.GetInt("forceupdate") == 4)
        {
            video.SetActive(false);
            checkUpdate(UnityEngine.PlayerPrefs.GetInt("forceupdate"));
        }
        else
        {
            nextScene();
        }


    }
    public void SlideConter(int s)
    {
        UnityEngine.PlayerPrefs.SetInt("slideCounter", s);

    }
    public string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {

                IPAddress = ip.ToString();
                UnityEngine.PlayerPrefs.SetString("ipaddress", IPAddress);
                return ip.ToString();
            }
        }
        throw new System.Exception("No network adapters with an IPv4 address in the system!");
    }
    // Update is called once per frame
    void Update()
    {

    }

    public void QuitGame()
    {

        Application.Quit();


    }

    string phoneNumber;
    int sendeAmar;
    public void OnSendNumberButtonPressed()
    {

        GameObject.Find("response").GetComponent<Text>().text = "... ﯽﺳﺭﺮﺑ ﻝﺎﺣ ﺭﺩ" + "\n" + "ﺪﯿﯾﺎﻤﻧ ﺮﺒﺻ ﺎﻔﻄﻟ" + "\n" + "ﺪﻧﻭﺭ ﻦﯾﺍ ﻥﺪﺷ ﯽﻧﻻﻮﻃ ﺕﺭﻮﺻ ﺭﺩ" + "\n" + "ﺪﯿﯾﺎﻤﻧ ﮏﭼ ﺍﺭ ﺩﻮﺧ ﺖﻧﺮﺘﻨﯾﺍ" + "\n" + "ﺪﯿﻧﺰﺑ ﺍﺭ ﺖﺒﺛ ﻪﻤﮐﺩ ﺍﺩﺪﺠﻣ ﻭ";
        GameObject.Find("response").GetComponent<Text>().color = Color.red;

        phoneNumber = GameObject.Find("phone number").GetComponent<Text>().text;
        UnityEngine.PlayerPrefs.SetString("phoneNumber", phoneNumber);



        if (UnityEngine.PlayerPrefs.GetInt("activation") > 0)
        {
            //add status
            UnityEngine.PlayerPrefs.SetString("tarriff_id", application_id.ToString());
            UnityEngine.PlayerPrefs.SetString("token", "buyed Before");
            addStatus.SetStatus();


        }
        StartCoroutine(SendPhone());










    }

    IEnumerator SendPhone()
    {
        SetData();
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(SendPhoneNumberRequest(phoneNumber));
        yield return new WaitForSeconds(5f);

        if (UnityEngine.PlayerPrefs.GetInt("init phone") == 0)
        {
            GameObject.Find("response").GetComponent<Text>().text = "ﺪﯿﻧﺰﺑ ﻪﻤﮐﺩ ﯼﻭﺭ ﺍﺩﺪﺠﻣ ﻭ ﺪﯿﻨﮐ ﮏﭼ ﺍﺭ ﺩﻮﺧ ﺖﻧﺮﺘﻨﯾﺍ";

        }

    }

    IEnumerator SendPhoneNumberRequest(string phoneNumber)
    {





        string clientID = UnityEngine.PlayerPrefs.GetString("m_sarial");
        string mykpi = UnityEngine.PlayerPrefs.GetInt("kpi").ToString();

        //http://127.0.0.1:8000/service/account/set-phone?client_id=SN0485&mobile=0912346567

        string parameters = "?client_id=" + clientID + "&mobile=" + phoneNumber + "&acquaintanceship_id=" + "1" + "&invate_code=" + UnityEngine.PlayerPrefs.GetString("cod_moaaref");

        Debug.Log("https://papitadb.ir/service/account/set-phone" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/service/account/set-phone" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();


            Debug.Log(www.downloadHandler.text);

            processJsonData(www.downloadHandler.text);


        }
    }

    void processJsonData(string _url)
    {
        NumDataClass jsnData = JsonUtility.FromJson<NumDataClass>(_url);








        foreach (NumList x in jsnData.interData)
        {


            Debug.Log(x.status);
            Debug.Log(x.tariffs);
            UnityEngine.PlayerPrefs.SetInt("init phone", 1);
            // GameObject.Find("anim loading").SetActive(false);

            //کد دعوتی اختصاصی این کاربرو میگیریم

            UnityEngine.PlayerPrefs.SetString("my_invite_code", x.invate_code);

            if (x.status == "201")
            {
                GameObject.Find("response").GetComponent<Text>().text = "ﺪﯿﺘﺴﯿﻧ ﺩﻭﺭﻭ ﻪﺑ ﺯﺎﺠﻣ ﺎﻤﺷ" + "\n" + "ﺖﺳﺍ ﺯﺎﺠﻣ ﺪﺣ ﺯﺍ ﺶﯿﺑ ﺎﻤﺷ ﺩﻭﺭﻭ ﺕﺎﻌﻓﺩ" + "\n" + "ﺪﯿﻫﺪﺑ ﻡﺎﯿﭘ ﯽﻧﺎﺒﯿﺘﺸﭘ ﻪﺑ ﺮﺘﺸﯿﺑ ﺕﺎﻋﻼﻃﺍ ﯼﺍﺮﺑ";
                GameObject.Find("response").GetComponent<Text>().color = Color.red;
                UnityEngine.PlayerPrefs.SetInt("status", 0);
                // اینجا مجاز به ورود نیست

            }
            else
            {
                type.instance.mybtn2.SetActive(false);
                waitnig.instance.CancelInvoke();
                waitnig.instance.anim.SetActive(false);
                // تعرفه 27 و 19 زمانی هست که همه اپ ها رو خریده. تعرفه 27 رو وقتی بهش پیشنهاد دادیم که طرف یه ترم مارو داشته

                if (x.status == "200" & x.tariffs.Length > 0)
                {
                    if (x.tariffs.Length > 0)
                    {
                        // برای متن چک کردن خرید اینجا تعرفشو میدیم به حافظه
                        UnityEngine.PlayerPrefs.SetString("tarriff_id", x.tariffs.ToString());
                    }
                    pos = Array.IndexOf(x.tariffs, 13);
                    pos2 = Array.IndexOf(x.tariffs, 18);
                    if (pos > -1 | pos2 > -1)
                    {

                        Debug.Log("he buyed all app");
                        GameObject.Find("response").GetComponent<Text>().text = "ﺪﻨﺘﺴﻫ ﻝﺎﻌﻓ ﺎﻤﺷ ﯼﺍﺮﺑ ﺎﺘﯿﭘﺎﭘ ﯼﺎﻫ ﯼﺯﺎﺑ ﻡﺎﻤﺗ" + "\n" + "ﺪﯾﻮﺸﯿﻣ ﻞﻘﺘﻨﻣ ﯼﺯﺎﺑ ﻪﺑ ﻥﻮﻨﮐﺍ ﻢﻫ";
                        GameObject.Find("response").GetComponent<Text>().color = mycolor;
                        UnityEngine.PlayerPrefs.SetInt("status", 13);

                        //اینجا دیگه نباید اموزش ببینه مستقیم بره به اسلاید اخر
                        GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(true);
                        GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.SetActive(false);
                    }
                    else
                    {
                        // کامل نخریده بود حالا ترمی چک کنه
                        pos = Array.IndexOf(x.tariffs, witchTerm);
                        if (pos > -1)
                        {
                            Debug.Log("he buyed this term");

                            GameObject.Find("response").GetComponent<Text>().text = "ﻦﯾﺩﺮﮐ ﺪﯾﺮﺧ ﻭﺭ ﻡﺮﺗ ﻦﯾﺍ ﯼﺎﻫ ﯼﺯﺎﺑ ﺎﻤﺷ" + "\n" + "ﺪﯿﻧﺰﺑ ﺮﯾﺯ ﻪﻤﮐﺩ ﯼﻭﺭ ﻪﻣﺍﺩﺍ ﯼﺍﺮﺑ";
                            GameObject.Find("response").GetComponent<Text>().color = mycolor;
                            UnityEngine.PlayerPrefs.SetInt("status", witchTerm);
                            // اینجا فقط همین بازی رو فعال داره و تو صفحه خریدش باید دکمه مشاهده بازی بیاد
                            GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(true);
                            GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.SetActive(false);
                            Debug.Log("buyed just this game before");
                        }
                        else
                        {
                            // ترمی نخریده بود حالا خود اپ رو چک کنه
                            pos = Array.IndexOf(x.tariffs, application_id);
                            if (pos > -1)
                            {
                                Debug.Log("he buyed this app");

                                GameObject.Find("response").GetComponent<Text>().text = "ﻥﻮﺗﺪﻧﺯﺮﻓ ﺵﺯﻮﻣﺁ ﯼﺍﺮﺑ ﺎﺘﯿﭘﺎﭘ ﺏﺎﺨﺘﻧﺍ ﺯﺍ ﺮﮑﺸﺗ ﺎﺑ" + "\n" + "ﺪﯿﻧﺰﺑ ﺮﯾﺯ ﻪﻤﮐﺩ ﯼﻭﺭ ﻪﻣﺍﺩﺍ ﯼﺍﺮﺑ";
                                GameObject.Find("response").GetComponent<Text>().color = mycolor;
                                UnityEngine.PlayerPrefs.SetInt("status", 0);
                                GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(true);
                                GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.SetActive(false);

                            }
                            else
                            {
                                Debug.Log("he dident buy");
                                GameObject.Find("response").GetComponent<Text>().text = "ﻥﻮﺗﺪﻧﺯﺮﻓ ﺵﺯﻮﻣﺁ ﯼﺍﺮﺑ ﺎﺘﯿﭘﺎﭘ ﺏﺎﺨﺘﻧﺍ ﺯﺍ ﺮﮑﺸﺗ ﺎﺑ" + "\n" + "ﺪﯿﻧﺰﺑ ﺮﯾﺯ ﻪﻤﮐﺩ ﯼﻭﺭ ﻪﻣﺍﺩﺍ ﯼﺍﺮﺑ";
                                GameObject.Find("response").GetComponent<Text>().color = mycolor;
                                UnityEngine.PlayerPrefs.SetInt("status", 0);
                                // اینجا کاربره جدیده
                                GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(true);
                                Debug.Log("new user detected and he buy anther of our apps");

                                if (UnityEngine.PlayerPrefs.GetInt("comeForBuy") == 1)
                                {
                                    GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(false);
                                    GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.SetActive(true);
                                    GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.GetComponent<Image>().sprite = GameObject.Find("login 1").GetComponent<SlideManager>().imageBuy;
                                    GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.GetComponent<Image>().sprite = GameObject.Find("login 1").GetComponent<SlideManager>().imageBuy;

                                    GameObject.Find("response").GetComponent<Text>().text = "ﺪﯾﺍ ﻩﺪﺷ ﺩﻮﺧ ﺏﺎﺴﺣ ﺩﺭﺍﻭ ﺖﯿﻘﻓﻮﻣ ﺎﺑ" + "\n" + "ﺪﯿﻨﮐ ﯽﯾﺎﻬﻧ ﺍﺭ ﻥﺎﺗﺪﯾﺮﺧ ﺮﯾﺯ ﻪﻤﮐﺩ ﻥﺩﺯ ﺎﺑ";
                                    GameObject.Find("response").GetComponent<Text>().color = mycolor;
                                }
                            }
                        }
                    }

                }
                else if (x.status == "200")
                {
                    Debug.Log("he dident buy and maybe deactived by admin");
                    GameObject.Find("response").GetComponent<Text>().text = "ﻥﻮﺗﺪﻧﺯﺮﻓ ﺵﺯﻮﻣﺁ ﯼﺍﺮﺑ ﺎﺘﯿﭘﺎﭘ ﺏﺎﺨﺘﻧﺍ ﺯﺍ ﺮﮑﺸﺗ ﺎﺑ" + "\n" + "ﺪﯿﻧﺰﺑ ﺮﯾﺯ ﻪﻤﮐﺩ ﯼﻭﺭ ﻪﻣﺍﺩﺍ ﯼﺍﺮﺑ";
                    GameObject.Find("response").GetComponent<Text>().color = mycolor;
                    UnityEngine.PlayerPrefs.SetInt("status", 0);
                    // اینجا کاربره جدیده
                    GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(true);
                    if (UnityEngine.PlayerPrefs.GetInt("comeForBuy") == 1)
                    {
                        GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.SetActive(false);
                        GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.SetActive(true);
                        GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn2.GetComponent<Image>().sprite = GameObject.Find("login 1").GetComponent<SlideManager>().imageBuy;
                        GameObject.Find("login 1").GetComponent<SlideManager>().nextbtn.GetComponent<Image>().sprite = GameObject.Find("login 1").GetComponent<SlideManager>().imageBuy;

                        GameObject.Find("response").GetComponent<Text>().text = "ﺪﯾﺍ ﻩﺪﺷ ﺩﻮﺧ ﺏﺎﺴﺣ ﺩﺭﺍﻭ ﺖﯿﻘﻓﻮﻣ ﺎﺑ" + "\n" + "ﺪﯿﻨﮐ ﯽﯾﺎﻬﻧ ﺍﺭ ﻥﺎﺗﺪﯾﺮﺧ ﺮﯾﺯ ﻪﻤﮐﺩ ﻥﺩﺯ ﺎﺑ";
                        GameObject.Find("response").GetComponent<Text>().color = mycolor;
                    }

                }

            }

            FirebaseBootstrap.SafeLogEvent("confirm_phone_number", "phone_Num", 1);
        }

        LockLevelsManager.instance.checkBuy();

    }
    public void detectBuy(int a)
    {
        if (a == 5) // قبلا خریده
        {
            nextScene();
            SlideConter(4);
        }
        else if (a == 2) //بره توضیحات ببینه
        {

            SlideConter(a);
            Slides[1].SetActive(false);
            Slides[2].SetActive(true);
        }
        else if (a == 4) // توضیحات رو دید حالا یا بره به درون برنامه یا بره خرید بدون درون برنامه
        {
            if (UnityEngine.PlayerPrefs.GetInt("inapppurchase") == 1)
            {
                SlideConter(a);
                nextScene();
            }
            else
            {
                SlideConter(a);
                Slides[3].SetActive(false);
                Slides[4].SetActive(true);

            }
        }

    }
    public void nextScene()
    {
        SceneManager.LoadScene(mainMenuName);
        if (UnityEngine.PlayerPrefs.GetInt("PlayTheGameOnce") == 1)
        {
            UnityEngine.PlayerPrefs.SetInt("seefree", 1);
        }


    }
    string myStoreLink;
    public void launch(int select)
    {
        int witchstoreSelected = UnityEngine.PlayerPrefs.GetInt("store");
        string packagename = GameObject.Find("slide 6").GetComponent<SlideManager>().mypackagename[select];
        if (witchstoreSelected == 1)
        {
            myStoreLink = GameObject.Find("slide 6").GetComponent<SlideManager>().mylinkGoogle[select];
        }
        else if (witchstoreSelected == 2)
        {
            myStoreLink = GameObject.Find("slide 6").GetComponent<SlideManager>().myLinkBazar[select];
        }
        else if (witchstoreSelected == 3)
        {
            myStoreLink = GameObject.Find("slide 6").GetComponent<SlideManager>().MylinkMyket[select];
        }
        else
        {
            myStoreLink = GameObject.Find("slide 6").GetComponent<SlideManager>().mylinkGoogle[select];
        }

        LaunchApp(packagename, myStoreLink);
    }


    public void LaunchApp(string package, string storeLink)
    {
        bool fail = false;
        string bundleId = package; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");



        AndroidJavaObject launchIntent = null;
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleId);
            if (launchIntent == null)
            {
                Application.OpenURL(storeLink);
            }

        }
        catch (System.Exception)
        {
            fail = true;

        }

        if (fail && storeLink != null)
        { //open app in store
            Application.OpenURL(storeLink);
        }
        else //open the app
            ca.Call("startActivity", launchIntent);

        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
    }



    /////////////////////////////////////////////////////////////////////////////////////


    /////////////////////////////////////////////////////////////////////////////////////


    ////////////////////////////////// buy section /////////////////////////////////////

    private string dico, m_serial, buyType;



    public void clik(int buy)
    {
        //اینجا باید متغییر اینکه چه تعرفه ای میخواسته رو بفرستیم به لاک لول منیجر تا  بعدا از ثبت شماره  بهمون بگه چی میخواسته
        LockLevelsManager.instance.witchProductForBuy = buy;

        if (UnityEngine.PlayerPrefs.GetInt("init phone") == 0)
        {
            LockLevelsManager.instance.userBtn();
            UnityEngine.PlayerPrefs.SetInt("comeForBuy", 1);
        }
        else
        {
            // buyType = buy.ToString();


            if (buy == 1)
            {
                if (UnityEngine.PlayerPrefs.GetString("showtakhfif") == "1")
                {
                    dico = GameObject.FindWithTag("dico49").GetComponent<Text>().text;
                }
                if (application_id < 4)
                {
                    buyType = "14";
                }
                else if (application_id > 3 & application_id < 7)
                {
                    buyType = "15";
                }
                else if (application_id > 6)
                {
                    buyType = "16";
                }



            }
            else if (buy == 2)
            {
                if (UnityEngine.PlayerPrefs.GetString("showtakhfif2") == "1")
                {
                    dico = GameObject.FindWithTag("dico79").GetComponent<Text>().text;
                }
                if (UnityEngine.PlayerPrefs.GetInt("status") == 14 | UnityEngine.PlayerPrefs.GetInt("status") == 15
                    | UnityEngine.PlayerPrefs.GetInt("status") == 16 | UnityEngine.PlayerPrefs.GetInt("status") == 17)
                {

                    // یه ترم مارو قبلا خریده الان همه ترم ها رو رایگان بگیره
                    buyType = "18";
                }
                else
                {
                    buyType = "13";
                    Debug.Log(" injas" + buyType);
                }



            }
            else if (buy == 3)
            {
                if (UnityEngine.PlayerPrefs.GetString("showtakhfif3") == "1")
                {
                    dico = GameObject.FindWithTag("dico30").GetComponent<Text>().text;

                }
                buyType = application_id.ToString();

            }

            //خرید از داخل اپ
            else if (buy == 4)
            {

                buyType = application_id.ToString();

            }
            else if (buy == 5)
            {

                buyType = "13";

            }

            phoneNumber = UnityEngine.PlayerPrefs.GetString("phoneNumber");


            UnityEngine.PlayerPrefs.SetString("tarriff_id", buyType);

            StartCoroutine(SendbuyRequest());

        }


    }
    IEnumerator SendbuyRequest()
    {
        buyType = UnityEngine.PlayerPrefs.GetString("tarriff_id");
        string parameters = "?mobile=" + phoneNumber + "&tarriff_id=" + buyType + "&discount_code=" + dico + "&app_link=" + scheme + "://" + host + "/" + UnityEngine.PlayerPrefs.GetString("m_sarial");


        Debug.Log("https://papitadb.ir/service/account/create-bill" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/service/account/create-bill" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();


            Debug.Log(www.downloadHandler.text);



            processJsonDataForBuy(www.downloadHandler.text);
        }
    }


    void OnApplicationFocus()
    {
        if (UnityEngine.PlayerPrefs.GetString("M_store") == "2" | UnityEngine.PlayerPrefs.GetString("M_store") == "3")
        {
            // باید وایسته تا اطلاعات اداستاتوس توی سرور بشینه  بعد چک کنه
            Invoke("IranStoreCheck", 2);

        }
        else
        {
            if (UnityEngine.PlayerPrefs.GetInt("slideCounter") > 3)
            {
                StartCoroutine(checkAgain());
            }
        }




    }

    public void IranStoreCheck()
    {

        StartCoroutine(checkAgain());


    }
    string buyTypeDico;
    public void clikDico(int buy)
    {




        if (buy == 1)
        {
            buyTypeDico = witchTerm.ToString();
            UnityEngine.PlayerPrefs.SetString("buyTypeDico", buyTypeDico);
            dico = GameObject.FindWithTag("dico49").GetComponent<Text>().text;
            GameObject.Find("new price 49").GetComponent<Text>().text = "... ﯽﺳﺭﺮﺑ ﻝﺎﺣ ﺭﺩ";
        }
        else if (buy == 2)
        {
            buyTypeDico = "13";
            UnityEngine.PlayerPrefs.SetString("buyTypeDico", buyTypeDico);
            dico = GameObject.FindWithTag("dico79").GetComponent<Text>().text;
            GameObject.Find("new price 79").GetComponent<Text>().text = "... ﯽﺳﺭﺮﺑ ﻝﺎﺣ ﺭﺩ";
        }
        else if (buy == 3)
        {
            buyTypeDico = application_id.ToString();
            UnityEngine.PlayerPrefs.SetString("buyTypeDico", buyTypeDico);
            dico = GameObject.FindWithTag("dico30").GetComponent<Text>().text;
            GameObject.Find("new price just").GetComponent<Text>().text = "... ﯽﺳﺭﺮﺑ ﻝﺎﺣ ﺭﺩ";
        }
        UnityEngine.PlayerPrefs.SetString("myDicoCode", dico);

        StartCoroutine(SendDicoRequest());


    }

    IEnumerator SendDicoRequest()
    {
        buyTypeDico = UnityEngine.PlayerPrefs.GetString("buyTypeDico");
        string parameters = "?discount_code=" + dico + "&tariff_id=" + buyTypeDico;


        Debug.Log("https://papitadb.ir/service/account/check-discount-code" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/service/account/check-discount-code" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();


            Debug.Log(www.downloadHandler.text);



            processJsonDataCheckDico(www.downloadHandler.text);
        }

    }

    public IEnumerator checkAgain()
    {


        phoneNumber = UnityEngine.PlayerPrefs.GetString("phoneNumber");
        string parameters = "?mobile=" + phoneNumber;


        Debug.Log("https://papitadb.ir/service/account/get-purchases" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/service/account/get-purchases" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();


            Debug.Log(www.downloadHandler.text);



            processJsonDataCheckBuy(www.downloadHandler.text);
        }

    }

    void processJsonDataForBuy(string _url)
    {
        BuyDataClass jsnData = JsonUtility.FromJson<BuyDataClass>(_url);






        foreach (buyList x in jsnData.interData)
        {



            if (x.status == "200")
            {
                Debug.Log("sakht dargah");
                UnityEngine.PlayerPrefs.SetInt("goForBuy", 1);
                Application.OpenURL(x.link);
            }
            else
            {
                Debug.Log("khataye dargah");
            }

        }




    }
    void processJsonDataCheckDico(string _url)
    {
        checkDicoDataClass jsnData = JsonUtility.FromJson<checkDicoDataClass>(_url);






        foreach (CheckDicoList x in jsnData.interData)
        {



            if (x.status == "200")
            {
                UnityEngine.PlayerPrefs.SetInt("hasDicoUser", 1);
                Debug.Log("kod mootabar");
                if (GameObject.Find("slide 5") != null)
                {
                    GameObject.Find("slide 5").GetComponent<SlideManager>().changeDicoBtnsBuy(buyTypeDico);
                }


                float persent = float.Parse(x.discount);
                float newPriceUser = float.Parse(x.new_price);
                UnityEngine.PlayerPrefs.SetString("newPriceUser", x.new_price);
                if (buyTypeDico == witchTerm.ToString())
                {
                    if (GameObject.Find("new price 49") != null)
                    {
                        GameObject.Find("new price 49").transform.position = GameObject.Find("pos new price 49").transform.position;

                        GameObject.Find("new price 49").GetComponent<Text>().text = "ﺪﺷ ﺖﺒﺛ ﺪﮐ" + "\n" + "ﻥﺎﻣﻮﺗ " + newPriceUser.ToString();
                        GameObject.Find("old price 49").SetActive(false);
                        GameObject.Find("new price 49").GetComponent<Text>().color = Color.green;
                        if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
                        {
                            GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[10].SetActive(true);
                            GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[4].SetActive(false);
                        }
                        if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
                        {
                            GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[13].SetActive(true);
                            GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[7].SetActive(false);
                        }
                    }

                }
                else if (buyTypeDico == "13")
                {

                    if (GameObject.Find("new price 79") != null)
                    {
                        GameObject.Find("new price 79").transform.position = GameObject.Find("pos new price 79").transform.position;

                        GameObject.Find("new price 79").GetComponent<Text>().text = "ﺪﺷ ﺖﺒﺛ ﺪﮐ" + "\n" + "ﻥﺎﻣﻮﺗ " + newPriceUser.ToString();

                        GameObject.Find("new price 79").GetComponent<Text>().color = Color.green;

                        if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
                        {
                            if (GameObject.Find("slide 5") != null)
                            {
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[9].SetActive(true);
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[3].SetActive(false);
                            }
                        }
                        if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
                        {
                            if (GameObject.Find("slide 5") != null)
                            {
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[12].SetActive(true);
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[6].SetActive(false);
                            }
                        }
                        if (GameObject.Find("old price 79") != null)
                        {
                            GameObject.Find("old price 79").SetActive(false);
                        }

                    }

                }
                else if (buyTypeDico == application_id.ToString())
                {
                    if (GameObject.Find("new price just") != null)
                    {
                        GameObject.Find("new price just").GetComponent<Text>().text = "ﺪﺷ ﺖﺒﺛ ﺪﮐ" + "\n" + "ﻥﺎﻣﻮﺗ " + newPriceUser.ToString();
                        GameObject.Find("new price just").GetComponent<Text>().color = Color.green;


                        if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
                        {
                            if (GameObject.Find("slide 5") != null)
                            {
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[11].SetActive(true);
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[5].SetActive(false);
                            }
                        }
                        if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
                        {
                            if (GameObject.Find("slide 5") != null)
                            {
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[14].SetActive(true);
                                GameObject.Find("slide 5").GetComponent<SlideManager>().buyBtns[8].SetActive(false);
                            }
                        }
                    }


                }

            }
            else
            {
                Debug.Log("kod eshtebah ast");
                UnityEngine.PlayerPrefs.SetInt("hasDicoUser", 0);
                if (buyTypeDico == witchTerm.ToString())
                {
                    if (GameObject.Find("new price 49") != null & GameObject.Find("old price 49") != null)
                    {
                        GameObject.Find("new price 49").transform.position = GameObject.Find("pos new price 49").transform.position;
                        GameObject.Find("new price 49").GetComponent<Text>().text = "ﺖﺳﺍ ﻩﺎﺒﺘﺷﺍ ﺪﮐ" + "\n" + UnityEngine.PlayerPrefs.GetString("price");
                        GameObject.Find("old price 49").SetActive(false);
                        GameObject.Find("new price 49").GetComponent<Text>().color = Color.red;
                    }

                }
                else if (buyTypeDico == "13")
                {
                    if (GameObject.Find("new price 79") != null & GameObject.Find("old price 79") != null)
                    {
                        GameObject.Find("new price 79").transform.position = GameObject.Find("pos new price 79").transform.position;
                        GameObject.Find("new price 79").GetComponent<Text>().text = "ﺖﺳﺍ ﻩﺎﺒﺘﺷﺍ ﺪﮐ" + "\n" + UnityEngine.PlayerPrefs.GetString("price2");
                        GameObject.Find("old price 79").SetActive(false);
                        GameObject.Find("new price 79").GetComponent<Text>().color = Color.red;
                    }

                }
                else if (buyTypeDico == application_id.ToString())
                {
                    if (GameObject.Find("new price just") != null)
                    {
                        GameObject.Find("new price just").GetComponent<Text>().text = "ﺖﺳﺍ ﻩﺎﺒﺘﺷﺍ ﺪﮐ" + "\n" + UnityEngine.PlayerPrefs.GetString("price3");
                        GameObject.Find("new price just").GetComponent<Text>().color = Color.red;
                    }

                }

            }

        }




    }

    int pos, pos2;
    void processJsonDataCheckBuy(string _url)
    {
        checkBuyDataClass jsnData = JsonUtility.FromJson<checkBuyDataClass>(_url);



        foreach (CheckbuyList x in jsnData.interData)
        {






            if (x.status == "200")
            {


                //روش فهمیدن اینکه از یک ارایه چندبار تکرار شده
                //ایف های تو در تو برای اینکه اول از خرید 19 چک کنه تا برسه به خود اپ

                Debug.Log(x.tariffs.Length);
                if (x.tariffs.Length > 0)
                {
                    Debug.Log("taarefe dare");
                    AdiveryConfig.instance.hideBanner();

                    pos = Array.IndexOf(x.tariffs, 19);
                    pos2 = Array.IndexOf(x.tariffs, 27);
                    if (pos > -1 | pos2 > -1)
                    {
                        if (UnityEngine.PlayerPrefs.GetInt("Go shopping portal") == 1)
                        {
                            UnityEngine.PlayerPrefs.SetInt("Go shopping portal", 0);
                            FirebaseBootstrap.SafeLogEvent("buy", "buy_user", 1);
                        }
                        Debug.Log("he buyed all app");
                        UnityEngine.PlayerPrefs.SetInt("status", 19);
                        if (UnityEngine.PlayerPrefs.GetInt("slideCounter") > 4)
                        {
                            doAfterBuy();
                        }
                        Debug.Log("kharid movafagh status :" + UnityEngine.PlayerPrefs.GetInt("status"));

                        //این ایف برای اینه که بار اولی که از خرید برگشت اتومات انیمیشن صفحه خرید انجام بشه
                        if (UnityEngine.PlayerPrefs.GetInt("goForBuy") > 0)
                        {


                            Slides[4].GetComponent<SlideManager>().closeBuy();
                            UnityEngine.PlayerPrefs.SetInt("goForBuy", 0);
                            UnityEngine.PlayerPrefs.SetInt("he is in game", 1);
                            Scene scene = SceneManager.GetActiveScene();
                            SceneManager.LoadScene(scene.name);
                        }
                    }
                    else
                    {
                        // کامل نخریده بود حالا ترمی چک کنه
                        pos = Array.IndexOf(x.tariffs, witchTerm);
                        if (pos > -1)
                        {
                            Debug.Log("he buyed this term");
                            UnityEngine.PlayerPrefs.SetInt("status", witchTerm);

                            if (UnityEngine.PlayerPrefs.GetInt("slideCounter") > 4)
                            {
                                doAfterBuy();
                            }
                            Debug.Log("kharid movafagh status :" + UnityEngine.PlayerPrefs.GetInt("status"));

                            //این ایف برای اینه که بار اولی که از خرید برگشت اتومات انیمیشن صفحه خرید انجام بشه
                            if (UnityEngine.PlayerPrefs.GetInt("goForBuy") > 0)
                            {


                                Slides[4].GetComponent<SlideManager>().closeBuy();
                                UnityEngine.PlayerPrefs.SetInt("goForBuy", 0);
                                UnityEngine.PlayerPrefs.SetInt("he is in game", 1);
                                Scene scene = SceneManager.GetActiveScene();
                                SceneManager.LoadScene(scene.name);
                            }
                        }
                        else
                        {
                            // ترمی نخریده بود حالا خود اپ رو چک کنه
                            pos = Array.IndexOf(x.tariffs, application_id);
                            if (pos > -1)
                            {
                                Debug.Log("he buyed this app");
                                UnityEngine.PlayerPrefs.SetInt("status", 1);

                                if (UnityEngine.PlayerPrefs.GetInt("slideCounter") > 4)
                                {
                                    doAfterBuy();
                                }
                                Debug.Log("kharid movafagh status :" + UnityEngine.PlayerPrefs.GetInt("status"));

                                //این ایف برای اینه که بار اولی که از خرید برگشت اتومات انیمیشن صفحه خرید انجام بشه
                                if (UnityEngine.PlayerPrefs.GetInt("goForBuy") > 0)
                                {

                                    Slides[4].GetComponent<SlideManager>().closeBuy();
                                    UnityEngine.PlayerPrefs.SetInt("goForBuy", 0);
                                    UnityEngine.PlayerPrefs.SetInt("he is in game", 1);
                                    Scene scene = SceneManager.GetActiveScene();
                                    SceneManager.LoadScene(scene.name);
                                }
                            }
                            else
                            {
                                Debug.Log("he dident buy");
                            }
                        }
                    }


                }
                else
                {
                    Debug.Log("az server app gheyr faal shode bra user");

                    UnityEngine.PlayerPrefs.SetInt("status", 0);

                    //از مرحله ثبت شماره گذشته باشه
                    // تا اگه از سرور بستیمش قبلش فعال یا غیر فعال بودنش با اپدیت اپ لحاظ شده باشه
                    if (UnityEngine.PlayerPrefs.GetInt("slideCounter") > 3)
                    {
                        UnityEngine.PlayerPrefs.SetInt("activation", -5);
                    }


                }





            }
            else
            {


                Debug.Log("not buy !!!!!!!!!!!");
                Debug.Log("status :" + UnityEngine.PlayerPrefs.GetInt("status"));
                UnityEngine.PlayerPrefs.SetInt("status", 0);
                // buyBtn.SetActive(false);
                //  afterbuy.SetActive(false);



            }

        }




    }

    public void doAfterBuy()
    {

        if (UnityEngine.PlayerPrefs.GetInt("slideCounter") == 4 | UnityEngine.PlayerPrefs.GetInt("slideCounter") == 5)
        {





            if (UnityEngine.PlayerPrefs.GetInt("slideCounter") == 4)
            {
                if (GameObject.Find("just this before buy") != null)
                {
                    GameObject.Find("slide 5").GetComponent<SlideManager>().closeBtn.SetActive(true);

                    GameObject.Find("just this before buy").transform.GetChild(2).gameObject.SetActive(false);
                    GameObject.Find("just this before buy").transform.GetChild(3).gameObject.SetActive(false);
                    GameObject.Find("just this before buy").transform.GetChild(4).gameObject.SetActive(false);
                    GameObject.Find("just this before buy").transform.GetChild(5).gameObject.SetActive(false);
                    GameObject.Find("just this before buy").transform.GetChild(6).gameObject.SetActive(false);
                    GameObject.Find("just this before buy").transform.GetChild(7).gameObject.SetActive(false);
                }

            }



        }


    }


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////


    ///////////////////////////////////////   GIFT AND VIDEO CONTROLLER ///////////////////////////////////////


    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private GameObject btnGift, boxGift, video_Btn, btnGiftInBox;
    public void GiftController()
    {
        if (btnGift != null)
        {
            boxGift = btnGift.transform.GetChild(0).gameObject;

            btnGift.SetActive(false);
            if (UnityEngine.PlayerPrefs.GetString("show_gift") == "1")
            {
                btnGift.SetActive(true);
            }
        }


    }

    public void createGiftBox()
    {
        boxGift.SetActive(true);
        btnGiftInBox = GameObject.Find("Button_gift");
        btnGiftInBox.SetActive(false);

        // GameObject.Find("textGift").GetComponent<ArabicText>().Text= UnityEngine.PlayerPrefs.GetString("gift_text");
        Debug.Log("show button " + UnityEngine.PlayerPrefs.GetString("show_button_in_gift"));
        if (UnityEngine.PlayerPrefs.GetString("show_button_in_gift") == "1")
        {
            btnGiftInBox.SetActive(true);
        }

    }
    public void openLinkGif()
    {
        Application.OpenURL(UnityEngine.PlayerPrefs.GetString("gift_link"));
    }

    public void videoController()
    {
        if (video_Btn != null)
        {
            video_Btn.SetActive(false);
            if (UnityEngine.PlayerPrefs.GetString("show_video") == "1")
            {
                video_Btn.SetActive(true);
            }
        }

    }
    public void openLinkVideo()
    {

        Application.OpenURL(UnityEngine.PlayerPrefs.GetString("video_link"));

    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////// store controller ////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    void storecontroller()
    {
        Debug.Log(UnityEngine.PlayerPrefs.GetString("freeCodegoogle") + "=" + Application.version);
        if (UnityEngine.PlayerPrefs.GetString("M_store") == "1")
        {
            if (UnityEngine.PlayerPrefs.GetString("freeCodegoogle") == Application.version)
            {
                SceneManager.LoadScene(mainMenuName);
            }
        }
        /*     else if(UnityEngine.PlayerPrefs.GetString("M_store") == "2" | UnityEngine.PlayerPrefs.GetString("M_store") == "3")
             {
                 if (UnityEngine.PlayerPrefs.GetInt("freeSeeBazarCheck") == 0)
                 {
                     UnityEngine.PlayerPrefs.SetInt("freeSeeBazarCheck", 1);
                     SceneManager.LoadScene(mainMenuName);
                 }

                 if (UnityEngine.PlayerPrefs.GetString("freeCodebazar") == Application.version)
                 {
                     if (UnityEngine.PlayerPrefs.GetInt("freeSeeBazarCheck") == 0)
                     {
                         UnityEngine.PlayerPrefs.SetInt("freeSeeBazarCheck", 1);
                         SceneManager.LoadScene(mainMenuName);
                     }
                     else
                     {
                       //  SceneManager.LoadScene(mainMenuIranStor);
                     }

                 }

             }   */

        else if (UnityEngine.PlayerPrefs.GetString("M_store") == "4")
        {
            if (UnityEngine.PlayerPrefs.GetString("freeCodeIos") == Application.version)
            {
                SceneManager.LoadScene(mainMenuName);
            }
        }


    }


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////// set Data of user  ////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void SetData()
    {
        StartCoroutine(SendPhoneNumberRequest());
    }
    IEnumerator SendPhoneNumberRequest()
    {




        string market_id = UnityEngine.PlayerPrefs.GetString("M_store");
        string clientID = UnityEngine.PlayerPrefs.GetString("m_sarial");

        string mykpi = UnityEngine.PlayerPrefs.GetInt("kpi").ToString();
        string devise = SystemInfo.deviceName;
        string version = Application.version;



        string parameters = "?client_id=" + clientID + "&application_id=" + application_id + "&device=" + devise + "&currect_app_version=" + version + "&market_id=" + market_id;

        Debug.Log("https://papitadb.ir/service/account/init" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/service/account/init" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();

            /*    if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    Debug.Log(www.error);
                    if (sendeAmar == 1)
                    {
                        GameObject.Find("response").GetComponent<Text>().text = "ﺪﯿﻨﮐ ﮏﭼ ﺭﺍ ﺩﻮﺧ ﺖﻧﺮﺘﻨﯾﺍ ﻝﺎﺼﺗﺍ ﺎﻔﻄﻟ" + "\n" + "ﺪﯿﻧﺰﺑ ﺕﺎﻋﻼﻃﺍ ﺖﺒﺛ ﻪﻤﮐﺩ ﯼﻭﺭ ﺍﺩﺪﺠﻣ ﻭ";

                    }
                }
                else
                {   */
            Debug.Log(www.downloadHandler.text);





            processJsonDataInFirst(www.downloadHandler.text);





        }
    }

    void processJsonDataInFirst(string _url)
    {
        interDataClass jsnData = JsonUtility.FromJson<interDataClass>(_url);






        foreach (dataList m in jsnData.interData)
        {


            Debug.Log(m.status);

            if (m.status == "200")
            {
                //  UnityEngine.PlayerPrefs.SetInt("send", 1);
                Debug.Log("ba movafaghiyat sabt shod data user ");
            }
            else
            {
                UnityEngine.PlayerPrefs.SetInt("send", 0);
                Debug.Log("sabt nashod ");
            }


            JsonDataClass jsnDataOfapp = JsonUtility.FromJson<JsonDataClass>(_url);

            foreach (buttonsList x in jsnDataOfapp.applicationsData)
            {



                if (x.id == application_id.ToString())
                {

                    UnityEngine.PlayerPrefs.SetString("freeCodegoogle", x.free_code_google);
                    UnityEngine.PlayerPrefs.SetString("freeCodebazar", x.free_code_bazar);
                    UnityEngine.PlayerPrefs.SetString("freeCodeIos", x.free_code_ios);
                    UnityEngine.PlayerPrefs.SetString("price3", x.price);
                    //  UnityEngine.PlayerPrefs.SetString("priceOld", x.old_price);
                    //  UnityEngine.PlayerPrefs.SetString("showOldprice", x.show_old_price);
                    UnityEngine.PlayerPrefs.SetString("showtakhfif3", x.show_discount);
                    UnityEngine.PlayerPrefs.SetString("linkinsta", x.link_insta);
                    UnityEngine.PlayerPrefs.SetString("linkWhatsapp", x.link_whatsapp);
                    UnityEngine.PlayerPrefs.SetString("linktelegram", x.link_telegram);
                    UnityEngine.PlayerPrefs.SetString("freeseeAtFirstTime", x.free_see_at_first_time);
                    // تنظیمات گیفت و ویدیو
                    UnityEngine.PlayerPrefs.SetString("show_gift", x.show_gift);
                    UnityEngine.PlayerPrefs.SetString("show_button_in_gift", x.show_button_in_gift);
                    UnityEngine.PlayerPrefs.SetString("gift_text", x.gift_text);
                    UnityEngine.PlayerPrefs.SetString("gift_link", x.gift_link);
                    UnityEngine.PlayerPrefs.SetString("show_video", x.show_video);
                    UnityEngine.PlayerPrefs.SetString("video_link", x.video_link);
                    UnityEngine.PlayerPrefs.SetString("invite_Permission", x.number_of_invitations_to_be_free);

                    //  Debug.Log("show_button_in_gift  " + UnityEngine.PlayerPrefs.GetString("show_button_in_gift"));
                    if (x.app_status == "1")
                    {
                        //اپ غیرفعال 
                        UnityEngine.PlayerPrefs.SetInt("forceupdate", 1);
                    }
                    else if (x.app_status == "2")
                    {
                        //در حال بررسی
                        UnityEngine.PlayerPrefs.SetInt("forceupdate", 2);
                    }
                    else if (x.app_status == "3")
                    {
                        //فعال
                        UnityEngine.PlayerPrefs.SetInt("forceupdate", 3);
                    }
                    else if (x.app_status == "4")
                    {
                        //نیازمند اپدیت
                        UnityEngine.PlayerPrefs.SetInt("forceupdate", 4);
                        Debug.Log(" need to update");
                    }
                    else if (x.app_status == "5")
                    {
                        //مسدود
                        UnityEngine.PlayerPrefs.SetInt("forceupdate", 5);
                    }

                    if (boxUpdate != null)
                        checkUpdate(UnityEngine.PlayerPrefs.GetInt("forceupdate"));

                }
                if (x.id == "13")
                {
                    //بسته همه اپ ها
                    UnityEngine.PlayerPrefs.SetString("price2", x.price);
                    UnityEngine.PlayerPrefs.SetString("priceOld2", x.old_price);
                    UnityEngine.PlayerPrefs.SetString("showOldprice2", x.show_old_price);
                    UnityEngine.PlayerPrefs.SetString("showtakhfif2", x.show_discount);


                }
                if (x.id == "14")
                {
                    //بسته سه اپ
                    UnityEngine.PlayerPrefs.SetString("price", x.price);
                    UnityEngine.PlayerPrefs.SetString("priceOld", x.old_price);
                    UnityEngine.PlayerPrefs.SetString("showOldprice", x.show_old_price);
                    UnityEngine.PlayerPrefs.SetString("showtakhfif", x.show_discount);
                }
                if (x.id == "18")
                {
                    //  کسی که یکی از ترم های مارو قبلا خریده
                    UnityEngine.PlayerPrefs.SetString("price18", x.price);
                    UnityEngine.PlayerPrefs.SetString("priceOld18", x.old_price);
                    UnityEngine.PlayerPrefs.SetString("showOldprice18", x.show_old_price);
                    UnityEngine.PlayerPrefs.SetString("showtakhfif18", x.show_discount);
                }


            }



        }




    }



    /////////////////////////////////////////////////////////////////////////////////////////////////////

    //////////////////////////////////   force update   /////////////////////////////////////////////////
    /////////////////////////////////////////////////////////////////////////////////////////////////////

    void checkUpdate(int update)
    {
        if (update == 4)
        {
            boxUpdate.SetActive(true);
        }
        else
        {
            boxUpdate.SetActive(false);

        }
    }

    public void OpenLinkUpdate()
    {
        if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
        {
            Application.OpenURL(GetComponent<SlideManager>().MylinkMyket[application_id - 1]);
        }
        else if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
        {
            Application.OpenURL(GetComponent<SlideManager>().myLinkBazar[application_id - 1]);

        }
        else
        {
            Application.OpenURL(GetComponent<SlideManager>().mylinkGoogle[application_id - 1]);

        }


    }


    //////////////////////////////////////////////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////////  INVITE CONTROLLER ///////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public GameObject invite;
    void InviteController()
    {
        getInviteUser(0);

        if (UnityEngine.PlayerPrefs.GetString("invite_Permission") != "-1")
        {
            invite.SetActive(true);
            if (string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("invite_Count")) |
                string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("invite_Permission")))
            {
                //هنوز تازه بازی رو باز کرده
            }
            else
            {
                if (Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Count")) > Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Permission")) - 1)
                {
                    //تعداد دعوتی ها به حد مجاز رسیده براش باز کن


                }
            }

        }
        else
        {
            Debug.Log("cod moarrefi az server baste ast" + UnityEngine.PlayerPrefs.GetString("invite_Permission"));
            invite.SetActive(false);
        }


    }

    public void getInviteUser(int a)
    {
        StartCoroutine(getUserInvite());
        if (a == 1)
        {
            if (string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("my_invite_code")))
            {
                GameObject.Find("my code invite").GetComponent<Text>().text = "ﺪﯿﯾﺎﻤﻧ ﺖﺒﺛ ﺱﺎﻤﺗ ﻩﺭﺎﻤﺷ ﺍﺪﺘﺑﺍ" + ": ﻦﻣ ﺕﻮﻋﺩ ﺪﮐ";

            }
            else
            {
                GameObject.Find("my code invite").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("my_invite_code") + ": ﻦﻣ ﺕﻮﻋﺩ ﺪﮐ";
                GameObject.Find("number of invite").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("invite_Count") + ": ﺪﮐ ﻦﯾﺍ ﺎﺑ ﻩﺪﺷ ﺕﻮﻋﺩ ﺩﺍﺮﻓﺍ ﺩﺍﺪﻌﺗ";

                // اگه بازی براش غیر فعال بود بهش بگه با این روش میتونی فعال کنی

                if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
                {
                    if (Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Count")) > Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Permission")) - 1)
                    {
                        //تعداد دعوتی ها به حد مجاز رسیده 

                        GameObject.Find("information to user").GetComponent<Text>().text = "ﺪﯿﺳﺭ ﺏﺎﺼﻧ ﺪﺣ ﻪﺑ ﺎﻤﺷ ﯼﺎﻫ ﺕﻮﻋﺩ ﺩﺍﺪﻌﺗ ! ﮏﯾﺮﺒﺗ";

                    }
                    else
                    {
                        string userLeftOver = (Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Permission")) - Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Count"))).ToString();
                        GameObject.Find("information to user").GetComponent<Text>().text = userLeftOver + " ﺩﺍﺪﻌﺗ" + "\n" + "ﺪﯿﯾﺎﻤﻧ ﺐﺼﻧ ﻪﺑ ﺕﻮﻋﺩ ﺩﻮﺧ ﺪﮐ ﺎﺑ ﺍﺭ ﺮﮕﯾﺩ ﺮﺑﺭﺎﮐ"
                            + "\n" + "ﺩﻮﺷ ﻥﺎﮕﯾﺍﺭ ﺎﻤﺷ ﯼﺍﺮﺑ ﯼﺯﺎﺑ ﻦﯾﺍ ﺎﺗ" + "\n" + "ﺪﯿﻫﺩ ﻡﺎﺠﻧﺍ ﻪﺤﻔﺻ ﺭﺎﻨﮐ ﻝﺎﺳﺭﺍ ﻪﻤﮐﺩ ﻖﯾﺮﻃ ﺯﺍ ﺍﺭ ﺭﺎﮑﻨﯾﺍ";

                    }
                }
                else
                {
                    GameObject.Find("information to user").GetComponent<Text>().text = "ﺖﺳﺎﻤﺷ ﺭﺎﻨﮐ ﺭﺩ ﺎﺘﯿﭘﺎﭘ ﻪﻋﻮﻤﺠﻣ";

                }


            }

        }


    }

    IEnumerator getUserInvite()
    {




        string mobile = UnityEngine.PlayerPrefs.GetString("phoneNumber");




        string parameters = "?mobile=" + mobile + "&tariff_id=" + application_id;

        Debug.Log("https://papitadb.ir/service/account/check-number-of-invitations" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/service/account/check-number-of-invitations" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();

            /*    if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    Debug.Log(www.error);
                    if (sendeAmar == 1)
                    {
                        GameObject.Find("response").GetComponent<Text>().text = "ﺪﯿﻨﮐ ﮏﭼ ﺭﺍ ﺩﻮﺧ ﺖﻧﺮﺘﻨﯾﺍ ﻝﺎﺼﺗﺍ ﺎﻔﻄﻟ" + "\n" + "ﺪﯿﻧﺰﺑ ﺕﺎﻋﻼﻃﺍ ﺖﺒﺛ ﻪﻤﮐﺩ ﯼﻭﺭ ﺍﺩﺪﺠﻣ ﻭ";

                    }
                }
                else
                {   */
            Debug.Log(www.downloadHandler.text);





            processJsonDataInvention(www.downloadHandler.text);





        }
    }

    void processJsonDataInvention(string _url)
    {
        Invention jsnData = JsonUtility.FromJson<Invention>(_url);






        foreach (InviteData m in jsnData.interData)
        {


            Debug.Log(m.status);

            if (m.status == "200")
            {
                UnityEngine.PlayerPrefs.SetString("invite_Count", m.Number_of_invitations);
                Debug.Log("invite_Count:" + m.Number_of_invitations);

                if (string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("invite_Count")) |
               string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("invite_Permission")))
                {
                    //هنوز تازه بازی رو باز کرده
                }
                else
                {
                    if (Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Count")) > Convert.ToInt32(UnityEngine.PlayerPrefs.GetString("invite_Permission")) - 1)
                    {
                        //تعداد دعوتی ها به حد مجاز رسیئده براش باز کن
                        //  UnityEngine.PlayerPrefs.SetInt("status", 1);
                        UnityEngine.PlayerPrefs.SetInt("invite_permission", 1);

                        Debug.Log("yesss your invite has been completed");
                    }
                    else
                    {
                        Debug.Log("sorry !!!! your invite has not been completed");
                    }
                }


            }
            else
            {

                Debug.Log("invte dont recive from server");
            }




        }




    }
}

public class BypassCertificate : CertificateHandler
{
    protected override bool ValidateCertificate(byte[] certificateData)
    {
        //Simply return true no matter what
        return true;
    }
}