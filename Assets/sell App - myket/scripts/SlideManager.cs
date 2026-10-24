using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;
using System;


public class SlideManager : MonoBehaviour
{
    // Start is called before the first frame update
    public string application_id;
    public bool inappSection;
    private GameObject azkoja, btnNum, buyBtn, Buttonbazar18, buttonmyket18;

    public GameObject nextbtn, nextbtn2, closeBtn, justthisAfterBuy;

    public GameObject[] term;
    public string[] mylinkGoogle, myLinkBazar, MylinkMyket, mypackagename;
    public Image[] appsimage;
    public GameObject[] apps;
    public GameObject[] terms, buyBtns;
    public string[] appsName;
    AudioSource m_MyAudioSource;
    public MainAppController manager;
    public GameObject kpi, keyboard;
    public Sprite imageBuy;
    public enum Slider
    {
        slide1,
        slide2,
        slide3,
        slide4,
        slide5,
        slide6,
        manager,

    }
    public Slider mySliding = new Slider();

    void Awake()
    {


        Screen.orientation = ScreenOrientation.LandscapeLeft;



    }
    void Start()
    {
        if (GameObject.Find("section Buy"))
            m_MyAudioSource = GameObject.Find("section Buy").GetComponent<AudioSource>();

        manager = GameObject.Find("manager").GetComponent<MainAppController>();



        if (mySliding == Slider.slide1)
        {
            m_MyAudioSource = GetComponent<AudioSource>();


            nextbtn = GameObject.Find("nextBtn");
            nextbtn.SetActive(false);



        }
        else if (mySliding == Slider.slide2)
        {

            // kpi = GameObject.Find("my kpi");
            //  kpi.SetActive(false);
            keyboard = GameObject.Find("my keyboard");
            keyboard.SetActive(true);

            m_MyAudioSource = GetComponent<AudioSource>();
            nextbtn = GameObject.Find("nextBtn");
            nextbtn.SetActive(false);
            nextbtn2 = GameObject.Find("nextBtn2");
            nextbtn2.SetActive(false);
            //  azkoja = GameObject.Find("back az koja");
            //  azkoja.SetActive(false);
            btnNum = GameObject.Find("back num + btn");
            GameObject.Find("sabt shomare").SetActive(false);
            //   btnNum.GetComponent<Button>().interactable = false;

        }
        else if (mySliding == Slider.slide3)
        {
            m_MyAudioSource = GetComponent<AudioSource>();
            nextbtn = GameObject.Find("nextBtn");
            manager.GiftController();
            manager.videoController();

        }
        else if (mySliding == Slider.slide4)
        {
            m_MyAudioSource = GetComponent<AudioSource>();
            nextbtn = GameObject.Find("nextBtn");
            nextbtn.SetActive(false);
            cando = true;
        }
        else if (mySliding == Slider.slide5)
        {

            manager.videoController();
            manager.GiftController();

            Buttonbazar18 = term[0];
            buttonmyket18 = term[1];

            closeBtn.SetActive(false);


            Buttonbazar18.SetActive(false);
            buttonmyket18.SetActive(false);

            m_MyAudioSource = GetComponent<AudioSource>();
            if (UnityEngine.PlayerPrefs.GetInt("played narition") == 0)
            {
                if (UnityEngine.PlayerPrefs.GetInt("inapppurchase") == 0)
                {
                    m_MyAudioSource.Play();
                    UnityEngine.PlayerPrefs.SetInt("played narition", 1);
                }

            }


            if (UnityEngine.PlayerPrefs.GetInt("store") == 1)
            {
                for (int i = 0; i < buyBtns.Length; i++)
                {
                    if (i < 2)
                    {
                        buyBtns[i].SetActive(true);
                    }
                    if (i > 2)
                    {
                        buyBtns[i].SetActive(false);
                    }


                }
            }
            else if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
            {
                for (int i = 0; i < buyBtns.Length; i++)
                {
                    if (i < 3)
                    {
                        buyBtns[i].SetActive(false);
                    }
                    if (i > 2 & i < 6)
                    {
                        buyBtns[i].SetActive(true);
                    }
                    if (i > 5)
                    {
                        buyBtns[i].SetActive(false);
                    }

                }
            }

            else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
            {
                m_MyAudioSource = GetComponent<AudioSource>();
                for (int i = 0; i < buyBtns.Length; i++)
                {
                    if (i < 6)
                    {
                        buyBtns[i].SetActive(false);
                    }
                    if (i > 5 & i < 9)
                    {
                        buyBtns[i].SetActive(true);
                    }
                    if (i > 8)
                    {
                        buyBtns[i].SetActive(false);
                    }

                }
            }





            // همین بازی رو فقط خریده
            if (UnityEngine.PlayerPrefs.GetInt("status") == 1 | UnityEngine.PlayerPrefs.GetInt("activation") > 0)
            {
                buyBtns[2].SetActive(false);
                buyBtns[5].SetActive(false);
                buyBtns[8].SetActive(false);
                terms[0].SetActive(false);
                terms[2].SetActive(false);
                closeBtn.SetActive(true);
                Debug.Log("hamin bazi faghat brash faale");
            }



            if (UnityEngine.PlayerPrefs.GetInt("status") != 0)
            {
                terms[0].SetActive(false);
                terms[2].SetActive(false);
                if (GameObject.Find("just this after buy") != null)
                {
                    GameObject.Find("just this after buy").transform.GetChild(1).gameObject.GetComponent<Text>().text = appsName[Convert.ToInt32(application_id) - 1];
                    GameObject.Find("just this after buy").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = apps[Convert.ToInt32(application_id) - 1].GetComponent<Image>().sprite;
                }

            }
            else
            {
                justthisAfterBuy.SetActive(false);
                if (!inappSection)
                {
                    terms[0].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = apps[Convert.ToInt32(application_id) - 1].GetComponent<Image>().sprite;

                }
                // تنظیمات دیدن دکمه مشاهده بازی

                int seefree = UnityEngine.PlayerPrefs.GetInt("seefree");
                if (seefree == 1)
                {

                    // GameObject.Find("Button see").SetActive(false);
                    if (GameObject.Find("slide 5") != null)
                    {
                        GameObject.Find("slide 5").GetComponent<AudioSource>().Stop();
                    }

                }
                else
                {
                    Debug.Log("bar avval bbine ? : " + UnityEngine.PlayerPrefs.GetString("freeseeAtFirstTime"));
                    // از سمت سرور باز شده یا نه برای بار اول
                    if (UnityEngine.PlayerPrefs.GetString("freeseeAtFirstTime") == "1")
                    {

                        buyBtns[2].SetActive(false);
                        buyBtns[5].SetActive(false);
                        buyBtns[8].SetActive(false);
                        terms[2].SetActive(false);

                    }


                }
            }



            m_MyAudioSource = GetComponent<AudioSource>();

            GameObject.Find("old price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("priceOld2");
            GameObject.Find("new price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price2");
            terms[3].GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price3");

            // اگه قبلا یه بسته مارو خریده بود قیمت بسته کاملش فرق کنه

            if (UnityEngine.PlayerPrefs.GetInt("status") == 14 | UnityEngine.PlayerPrefs.GetInt("status") == 15
                | UnityEngine.PlayerPrefs.GetInt("status") == 16 | UnityEngine.PlayerPrefs.GetInt("status") == 17)
            {
                if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
                {
                    Buttonbazar18.SetActive(true);
                }
                else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
                {
                    buttonmyket18.SetActive(true);
                }

                GameObject.Find("old price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("priceOld18");
                GameObject.Find("new price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price18");
                GameObject.Find("dico 79").SetActive(false);
                GameObject.Find("baste 49").SetActive(false);
                GameObject.Find("dico 49").SetActive(false);


                if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
                {
                    for (int i = 0; i < buyBtns.Length; i++)
                    {
                        if (i == 15)
                        {
                            buyBtns[i].SetActive(true);
                        }
                        else
                        {
                            buyBtns[i].SetActive(false);
                        }
                    }

                }
                if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
                {
                    for (int i = 0; i < buyBtns.Length; i++)
                    {
                        if (i == 16)
                        {
                            buyBtns[i].SetActive(true);
                        }
                        else
                        {
                            buyBtns[i].SetActive(false);
                        }
                    }
                }



            }
            else
            {
                if (GameObject.Find("old price 49") != null)
                {
                    GameObject.Find("old price 49").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("priceOld");
                    GameObject.Find("new price 49").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price");
                }


            }


            if (UnityEngine.PlayerPrefs.GetString("showOldprice") == "0")
            {
                if (GameObject.Find("old price 49") != null)
                {
                    GameObject.Find("old price 49").SetActive(false);
                    GameObject.Find("new price 49").GetComponent<Text>().fontSize = 50;
                    GameObject.Find("new price 49").transform.position = GameObject.Find("pos new price 49").transform.position;
                }

            }
            if (UnityEngine.PlayerPrefs.GetString("showOldprice2") == "0")
            {
                if (GameObject.Find("old price 79") != null)
                {
                    GameObject.Find("old price 79").SetActive(false);
                    GameObject.Find("new price 79").GetComponent<Text>().fontSize = 50;
                    GameObject.Find("new price 79").transform.position = GameObject.Find("pos new price 79").transform.position;
                }

            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif") == "0")
            {
                GameObject dic49 = GameObject.Find("dico 49");
                if (dic49 != null)
                    dic49.SetActive(false);
            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif2") == "0")
            {
                GameObject dic79 = GameObject.Find("dico 79");
                if (dic79 != null)
                    dic79.SetActive(false);
            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif3") == "0")
            {
                GameObject dic = GameObject.Find("dico");
                if (dic != null)
                    dic.SetActive(false);
            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif3") == "1" | UnityEngine.PlayerPrefs.GetString("showtakhfif") == "1" | UnityEngine.PlayerPrefs.GetString("showtakhfif") == "1")
            {
                GameObject.Find("all btns").GetComponent<Animator>().enabled = true;
            }
            Debug.Log("khmosh shodan baste ha " + UnityEngine.PlayerPrefs.GetInt("status"));
            // نمایش بسته های پیشنهادی که با شماره تماسش فهمیدیم
            if (!inappSection)
            {
                if (UnityEngine.PlayerPrefs.GetInt("status") == 1)
                {

                    terms[0].SetActive(false);
                    terms[1].SetActive(true);
                }
                if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
                {

                    terms[0].SetActive(true);
                    terms[1].SetActive(false);
                }
            }




        }
        else if (mySliding == Slider.slide6)
        {
            manager.videoController();
            manager.GiftController();
            m_MyAudioSource = GetComponent<AudioSource>();
            if (GameObject.Find("just this after buy") != null)
            {
                GameObject.Find("just this after buy").transform.GetChild(1).gameObject.GetComponent<Text>().text = appsName[Convert.ToInt32(application_id) - 1];
                GameObject.Find("just this after buy").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = apps[Convert.ToInt32(application_id) - 1].GetComponent<Image>().sprite;
            }

           

            for (int a = 0; a < apps.Length; a++)
            {
                apps[a].SetActive(false);
            }
            counter = 0;
            if (Coroutine)
            {
                StartCoroutine(coroutineA());

            }

            if (GameObject.Find("narition 1") != null)
            {
                if (UnityEngine.PlayerPrefs.GetInt("status") == 1)
                {
                    GameObject.Find("narition 1").GetComponent<AudioSource>().Play();
                }
                else if (UnityEngine.PlayerPrefs.GetInt("status") == 13 | UnityEngine.PlayerPrefs.GetInt("status") == 18)
                {
                    GameObject.Find("narition 3").GetComponent<AudioSource>().Play();
                }
                else if (UnityEngine.PlayerPrefs.GetInt("status") == 14 | UnityEngine.PlayerPrefs.GetInt("status") == 15 | UnityEngine.PlayerPrefs.GetInt("status") == 16 | UnityEngine.PlayerPrefs.GetInt("status") == 17)
                {
                    GameObject.Find("narition 2").GetComponent<AudioSource>().Play();
                }
            }




        }
    }

    void OnEnable()
    {

        if (mySliding == Slider.slide5)
        {
            Buttonbazar18 = term[0];
            buttonmyket18 = term[1];

            closeBtn.SetActive(false);

            Buttonbazar18.SetActive(false);
            buttonmyket18.SetActive(false);

            m_MyAudioSource = GetComponent<AudioSource>();

            if (UnityEngine.PlayerPrefs.GetInt("store") == 1)
            {
                for (int i = 0; i < buyBtns.Length; i++)
                {
                    if (i < 2)
                    {
                        buyBtns[i].SetActive(true);
                    }
                    if (i > 2)
                    {
                        buyBtns[i].SetActive(false);
                    }


                }
            }
            else if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
            {
                for (int i = 0; i < buyBtns.Length; i++)
                {
                    if (i < 3)
                    {
                        buyBtns[i].SetActive(false);
                    }
                    if (i > 2 & i < 6)
                    {
                        buyBtns[i].SetActive(true);
                    }
                    if (i > 5)
                    {
                        buyBtns[i].SetActive(false);
                    }

                }
            }

            else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
            {
                m_MyAudioSource = GetComponent<AudioSource>();
                for (int i = 0; i < buyBtns.Length; i++)
                {
                    if (i < 6)
                    {
                        buyBtns[i].SetActive(false);
                    }
                    if (i > 5 & i < 9)
                    {
                        buyBtns[i].SetActive(true);
                    }
                    if (i > 8)
                    {
                        buyBtns[i].SetActive(false);
                    }

                }
            }





            // همین بازی رو فقط خریده
            if (UnityEngine.PlayerPrefs.GetInt("status") == 1 | UnityEngine.PlayerPrefs.GetInt("activation") > 0)
            {
                buyBtns[2].SetActive(false);
                buyBtns[5].SetActive(false);
                buyBtns[8].SetActive(false);
                terms[0].SetActive(false);
                terms[2].SetActive(false);
                closeBtn.SetActive(true);
                Debug.Log("hamin bazi faghat brash faale");
            }



            if (UnityEngine.PlayerPrefs.GetInt("status") != 0)
            {
                terms[0].SetActive(false);
                terms[2].SetActive(false);
            }
            else
            {
                justthisAfterBuy.SetActive(false);

                // تنظیمات دیدن دکمه مشاهده بازی

                int seefree = UnityEngine.PlayerPrefs.GetInt("seefree");
                if (seefree == 1)
                {

                    GameObject slide = GameObject.Find("slide 5");
                    if (slide != null)
                    {
                        GameObject.Find("slide 5").GetComponent<AudioSource>().Stop();
                        GameObject.Find("Button see").SetActive(false);
                    }

                }
                else
                {
                    Debug.Log("bar avval bbine ? : " + UnityEngine.PlayerPrefs.GetString("freeseeAtFirstTime"));
                    // از سمت سرور باز شده یا نه برای بار اول
                    if (UnityEngine.PlayerPrefs.GetString("freeseeAtFirstTime") == "1")
                    {

                        buyBtns[2].SetActive(false);
                        buyBtns[5].SetActive(false);
                        buyBtns[8].SetActive(false);
                        terms[2].SetActive(false);

                    }


                }
            }



            m_MyAudioSource = GetComponent<AudioSource>();
            if (GameObject.Find("old price 79") != null)
            {
                GameObject.Find("old price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("priceOld2");
                GameObject.Find("new price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price2");
            }


            // اگه قبلا یه بسته مارو خریده بود قیمت بسته کاملش فرق کنه

            if (UnityEngine.PlayerPrefs.GetInt("status") == 14 | UnityEngine.PlayerPrefs.GetInt("status") == 15
                | UnityEngine.PlayerPrefs.GetInt("status") == 16 | UnityEngine.PlayerPrefs.GetInt("status") == 17)
            {
                if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
                {
                    Buttonbazar18.SetActive(true);
                }
                else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
                {
                    buttonmyket18.SetActive(true);
                }
                if (GameObject.Find("old price 79") != null)
                {
                    GameObject.Find("old price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("priceOld18");
                    GameObject.Find("new price 79").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price18");
                    GameObject.Find("dico 79").SetActive(false);
                    GameObject.Find("baste 49").SetActive(false);
                    GameObject.Find("dico 49").SetActive(false);
                }




            }
            else
            {
                if (GameObject.Find("old price 49") != null)
                {
                    GameObject.Find("old price 49").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("priceOld");
                    GameObject.Find("new price 49").GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price");
                }


            }


            if (UnityEngine.PlayerPrefs.GetString("showOldprice") == "0")
            {
                if (GameObject.Find("old price 49") != null)
                {

                    GameObject.Find("old price 49").SetActive(false);
                    GameObject.Find("new price 49").GetComponent<Text>().fontSize = 50;
                    GameObject.Find("new price 49").transform.position = GameObject.Find("pos new price 49").transform.position;
                }

            }
            if (UnityEngine.PlayerPrefs.GetString("showOldprice2") == "0")
            {
                if (GameObject.Find("old price 49") != null)
                {
                    GameObject.Find("old price 79").SetActive(false);
                    GameObject.Find("new price 79").GetComponent<Text>().fontSize = 50;
                    GameObject.Find("new price 79").transform.position = GameObject.Find("pos new price 79").transform.position;
                }

            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif") == "0")
            {

                GameObject dic49 = GameObject.Find("dico 49");
                if (dic49 != null)
                    dic49.SetActive(false);
            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif2") == "0")
            {
                GameObject dic79 = GameObject.Find("dico 79");
                if (dic79 != null)
                    dic79.SetActive(false);
            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif3") == "0")
            {
                GameObject dic = GameObject.Find("dico");
                if (dic != null)
                    dic.SetActive(false);
            }
            if (UnityEngine.PlayerPrefs.GetString("showtakhfif3") == "1" | UnityEngine.PlayerPrefs.GetString("showtakhfif") == "1" | UnityEngine.PlayerPrefs.GetString("showtakhfif") == "1")
            {
                GameObject.Find("all btns").GetComponent<Animator>().enabled = true;
            }

            // نمایش بسته های پیشنهادی که با شماره تماسش فهمیدیم
            if (UnityEngine.PlayerPrefs.GetInt("status") == 1)
            {
                terms[0].SetActive(false);
                terms[1].SetActive(true);
            }
            if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
            {

                terms[0].SetActive(true);
                terms[1].SetActive(false);
            }
            terms[3].GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("price3");

        }
        else if (mySliding == Slider.slide6)
        {
           



        }

    }
    int anim = 0;
    public void closeBuy()
    {
        UnityEngine.PlayerPrefs.SetInt("closeBuy", 1);
        if (anim == 0)
        {
            anim = 1;
            GetComponent<Animator>().SetBool("bool1", true);
        }
        else
        {
            GetComponent<Animator>().SetBool("bool3", true);
        }
        GetComponent<AudioSource>().Stop();
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
        {

            GameObject.Find("manager").GetComponent<MainAppController>().buyBtn.SetActive(false);
        }
        else
        {
            GameObject.Find("manager").GetComponent<MainAppController>().buyBtn.SetActive(true);

        }
        UnityEngine.PlayerPrefs.SetInt("slideCounter", 5);
        Invoke("showSlide6", 0.9f);
    }
    void showSlide6()
    {
        GetComponent<Animator>().SetBool("bool1", false);
        GetComponent<Animator>().SetBool("bool3", false);
        GameObject.Find("manager").GetComponent<MainAppController>().Slides[5].SetActive(true);
        GameObject.Find("just this after buy").transform.GetChild(1).gameObject.GetComponent<Text>().text = appsName[Convert.ToInt32(application_id) - 1];
        GameObject.Find("just this after buy").transform.GetChild(0).gameObject.GetComponent<Image>().sprite = apps[Convert.ToInt32(application_id) - 1].GetComponent<Image>().sprite;
    }
    public void BuyBtn()
    {
        GameObject.Find("manager").GetComponent<MainAppController>().Slides[4].GetComponent<Animator>().SetBool("bool2", true);
        GameObject.Find("manager").GetComponent<MainAppController>().Slides[4].SetActive(true);

        GameObject.Find("manager").GetComponent<MainAppController>().Slides[5].SetActive(false);
        Invoke("setbool3Off", 0.9f);
        GameObject.Find("manager").GetComponent<MainAppController>().buyBtn.SetActive(false);

    }
    void setbool3Off()
    {
        GameObject.Find("manager").GetComponent<MainAppController>().Slides[4].GetComponent<Animator>().SetBool("bool2", false);
    }
    bool Coroutine = true;
    int counter = 0;
    void OnApplicationFocus()
    {




        if (mySliding == Slider.slide6)
        {
            Coroutine = true;
            counter = 0;
            for (int a = 0; a < apps.Length; a++)
            {
                apps[a].SetActive(false);
            }


            if (UnityEngine.PlayerPrefs.GetInt("store") == 1)
            {
                if (Coroutine)
                {
                    StartCoroutine(coroutineA());

                }


            }

            else if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
            {
                if (Coroutine)
                {
                    StartCoroutine(coroutineA());

                }
            }

            else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
            {
                if (Coroutine)
                {
                    StartCoroutine(coroutineA());

                }
            }


        }


    }
    IEnumerator coroutineA()
    {

        witchApp = counter;
        //  LaunchApp(mypackagename[counter], MylinkMyket[counter]);

        apps[counter].SetActive(true);
        counter++;
        yield return new WaitForSeconds(0.3f);
        if (counter < 9)
        {
            yield return StartCoroutine(coroutineA());
        }
        else
        {
            checkbuyedApps();
        }


    }

    void checkbuyedApps()
    {
        Debug.Log("checking ..." + UnityEngine.PlayerPrefs.GetInt("status"));
        if (UnityEngine.PlayerPrefs.GetInt("status") == 13)
        {
            Debug.Log("hama ro kharide");
            // همه رو خریده همشون روشنن

            for (int i = 0; i < apps.Length; i++)
            {
                appsimage[i] = apps[i].GetComponent<Image>();
                appsimage[i].color = new Color(1, 1, 1, 1f);
            }
        }
        else if (UnityEngine.PlayerPrefs.GetInt("status").ToString() == application_id | UnityEngine.PlayerPrefs.GetInt("activation") > 0)
        {
            //فقط همین اپ رو خریده
            for (int i = 0; i < apps.Length; i++)
            {
                if (i == Convert.ToInt32(application_id) - 1)
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(appsimage[i].color.r, appsimage[i].color.g, appsimage[i].color.b, 1f);
                }
                else
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
                }
            }
        }
        else if (UnityEngine.PlayerPrefs.GetInt("status") == 14)
        {
            //ترم یک رو خریده
            Debug.Log("term 1 ro khairde taarefe " + UnityEngine.PlayerPrefs.GetString("tarriff_id"));
            for (int i = 0; i < apps.Length; i++)
            {
                if (i < 3)
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(appsimage[i].color.r, appsimage[i].color.g, appsimage[i].color.b, 1f);
                }
                else
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
                }
            }
        }
        else if (UnityEngine.PlayerPrefs.GetInt("status") == 15)
        {
            Debug.Log("term 2 ro khairde");
            //ترم دوم رو خریده
            for (int i = 0; i < apps.Length; i++)
            {
                if (i < 3)
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
                }
                else if (i > 5)
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
                }
                else
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(appsimage[i].color.r, appsimage[i].color.g, appsimage[i].color.b, 1f);
                }

            }

        }
        else if (UnityEngine.PlayerPrefs.GetInt("status") == 16)
        {
            //ترم سوم رو خریده
            Debug.Log("term 3 ro khairde");
            for (int i = 0; i < apps.Length; i++)
            {
                if (i < 5)
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
                }
                else
                {
                    appsimage[i] = apps[i].GetComponent<Image>();
                    appsimage[i].color = new Color(appsimage[i].color.r, appsimage[i].color.g, appsimage[i].color.b, 1f);
                }
            }
        }
        else if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
        {

            Debug.Log("hichi nakharide");
            for (int i = 0; i < apps.Length; i++)
            {
                appsimage[i] = apps[i].GetComponent<Image>();
                appsimage[i].color = new Color(0.7f, 0.7f, 0.7f, 0.4f);
            }
          

        }


    }
    int witchApp;
    private bool cando = true;
    // Update is called once per frame
    void Update()
    {



    }


    public void answer(int a)
    {
        string Edit = "";

        switch (a)
        {
            case 0:
                UnityEngine.PlayerPrefs.SetInt("kpi", 1);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﻥﺎﯾﺎﻨﺷﺁ ﻭ ﻥﺎﺘﺳﻭﺩ ﯽﻓﺮﻌﻣ";

                break;
            case 1:
                UnityEngine.PlayerPrefs.SetInt("kpi", 2);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﻞﮔﻮﮔ چﺮﺳ";
                break;
            case 2:
                UnityEngine.PlayerPrefs.SetInt("kpi", 3);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﻡﺍﺮﮔﺎﺘﺴﻨﯾﺍ";
                break;
            case 3:
                UnityEngine.PlayerPrefs.SetInt("kpi", 4);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﺖﻠﺒﺗ ﻭ ﻞﯾﺎﺑﻮﻣ ﺕﺎﻐﯿﻠﺒﺗ";
                break;
            case 4:
                UnityEngine.PlayerPrefs.SetInt("kpi", 5);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﻡﺍﺮﮕﻠﺗ";
                break;
            case 5:
                UnityEngine.PlayerPrefs.SetInt("kpi", 6);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "پﺍ ﺲﺗﺍﻭ";
                break;
            case 6:
                UnityEngine.PlayerPrefs.SetInt("kpi", 7);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﻥﻮﯾﺯﻮﻠﺗ ﺕﺎﻐﯿﻠﺒﺗ";
                break;
            case 7:
                UnityEngine.PlayerPrefs.SetInt("kpi", 8);
                GameObject.Find("answer").GetComponent<Text>().text = Edit;
                GameObject.Find("answer").GetComponent<Text>().text = "ﺎﻫ ﺖﯾﺎﺴﺑﻭ ﺕﺎﻐﯿﻠﺒﺗ";
                break;
            default:

                break;
        }
    }

    public void InitCod()
    {
        UnityEngine.PlayerPrefs.SetString("cod_moaaref", GameObject.Find("text code").GetComponent<Text>().text);
        GameObject.Find("code intered (moarefi)").GetComponent<Text>().text = GameObject.Find("text code").GetComponent<Text>().text + " : ﻩﺪﺷ ﺩﺭﺍﻭ ﯽﻓﺮﻌﻣ ﺪﮐ";
        // GameObject.Find("code intered (moarefi)").transform.GetChild(0).gameObject.SetActive(true);
        GameObject.Find("kod moaref").SetActive(false);
    }

    public void termex(int a)
    {

        GameObject.Find("slide 4").GetComponent<AudioSource>().Stop();
        for (int b = 0; b < term.Length; b++)
        {
            if (a == b)
            {
                term[b].GetComponent<AudioSource>().Play();
                term[b].transform.localScale = new Vector3(1.1f, 1.1f, 1.1f);
            }
            else
            {
                term[b].GetComponent<AudioSource>().Stop();
                term[b].transform.localScale = new Vector3(0.85f, 0.85f, 0.85f);
            }
        }


    }

    /* public void LaunchApp(string package, string storeLink)
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
                 appsimage[witchApp] = apps[witchApp].GetComponent<Image>();
                 appsimage[witchApp].color = new Color(appsimage[witchApp].color.r, appsimage[witchApp].color.g, appsimage[witchApp].color.b, 0.4f);
             }
             else
             {
                 appsimage[witchApp] = apps[witchApp].GetComponent<Image>();
                 appsimage[witchApp].color = new Color(appsimage[witchApp].color.r, appsimage[witchApp].color.g, appsimage[witchApp].color.b, 1f);
             }

         }
         catch (System.Exception)
         {
             fail = true;

         }

         if (fail && storeLink != null)
         { //open app in store

         }


     }

 */


    public void changeDicoBtnsBuy(string witch)
    {

        if (UnityEngine.PlayerPrefs.GetInt("store") == 2)
        {
            if (witch == "1")
            {
                buyBtns[4].SetActive(false);
                buyBtns[10].SetActive(true);
            }
            else if (witch == "2")
            {
                buyBtns[3].SetActive(false);
                buyBtns[9].SetActive(true);
            }
            else if (witch == "3")
            {
                buyBtns[5].SetActive(false);
                buyBtns[11].SetActive(true);
            }
        }
        else if (UnityEngine.PlayerPrefs.GetInt("store") == 3)
        {

            if (witch == "1")
            {
                buyBtns[7].SetActive(false);
                buyBtns[13].SetActive(true);
            }
            else if (witch == "2")
            {
                buyBtns[6].SetActive(false);
                buyBtns[12].SetActive(true);
            }
            else if (witch == "3")
            {
                buyBtns[8].SetActive(false);
                buyBtns[14].SetActive(true);
            }
        }


    }

}
