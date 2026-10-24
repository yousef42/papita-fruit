using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UPersian.Components;

public class LockLevelsManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static LockLevelsManager instance;
   
   
  
    public GameObject Lockbox, UserBox;

    private string myStoreLink;
  
    private String mytext ;
    public int witchProductForBuy;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    IEnumerator Start()
    {
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0)
        {
            

        }
        yield return new WaitForSeconds(0.5f);
        if (UnityEngine.PlayerPrefs.GetInt("comeFromAlphabet") == 1)
        {
            UnityEngine.PlayerPrefs.SetInt("comeFromAlphabet", 0);
          
                Lockbox.SetActive(true);
           
        }
        else
        {
            
            Lockbox.SetActive(false);
        }
     
       
       


       

       
    }
    void OnApplicationFocus()
    {
        Invoke("checkBuy", 1);
    }
 
    public void checkBuy()
    {
        if (UnityEngine.PlayerPrefs.GetInt("status") == 0 & UnityEngine.PlayerPrefs.GetInt("inapppurchase") == 1)
        {
           

        }
        else
        {
           
        }
    }
    public void OpenLockBox()
    {

       
            Lockbox.SetActive(true);
        
    }

    public void gameBuyEvent(string myevent)
    {
        FirebaseBootstrap.SafeLogEvent("buy_app_section", myevent, 1);
    }
    public void CloseLockBox()
    {
       
        Lockbox.SetActive(false);
      
    }

    public void gameBtnEvent(int a)
    {

       
    }
    public void moreAppBtn()
    {
     

        FirebaseBootstrap.SafeLogEvent("more_app", "more_App_btn", 1);
    }
    int phoneNumber;
    public void openWhatsApp()
    {
     

        String url = "https://api.whatsapp.com/send?phone=" + Convert.ToInt64(UnityEngine.PlayerPrefs.GetString("PhoneNumberWhatsapp")) + "&text=" + mytext;
       
        Application.OpenURL(url);
    }
    public void launch(int select)
    {
        

                int witchstoreSelected = UnityEngine.PlayerPrefs.GetInt("store");
        
        string packagename = this.gameObject.GetComponent<SlideManager>().mypackagename[select];
        if (witchstoreSelected == 1)
        {
            myStoreLink = this.gameObject.GetComponent<SlideManager>().mylinkGoogle[select];
        }
        else if (witchstoreSelected == 2)
        {
            myStoreLink = this.gameObject.GetComponent<SlideManager>().myLinkBazar[select];
        }
        else if (witchstoreSelected == 3)
        {
            myStoreLink = this.gameObject.GetComponent<SlideManager>().MylinkMyket[select];
        }
        else
        {
            myStoreLink = this.gameObject.GetComponent<SlideManager>().mylinkGoogle[select];
        }


        LaunchApp(packagename, myStoreLink);



    }
    public void userBtn()
    {
        UserBox.SetActive(true);
        Lockbox.GetComponent<AudioSource>().Stop();
       
        if (UnityEngine.PlayerPrefs.GetInt("init phone") == 1 & !String.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("city")))
        {
            // load user
            Debug.Log("active 1");
            Debug.Log(UnityEngine.PlayerPrefs.GetString("childName"));
            UserBox.GetComponent<AudioSource>().Stop();
            UserBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            UserBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UserBox.gameObject.transform.GetChild(2).gameObject.SetActive(true);
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(7).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("m_sarial");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(8).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("phoneNumber");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(9).GetComponent<RtlText>().text = UnityEngine.PlayerPrefs.GetString("childName");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(10).GetComponent<RtlText>().text = UnityEngine.PlayerPrefs.GetString("childAge");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(11).GetComponent<RtlText>().text = UnityEngine.PlayerPrefs.GetString("city");

        }
        else if (UnityEngine.PlayerPrefs.GetInt("init phone") == 1)
        {
            Debug.Log("active 2");
            UserBox.GetComponent<AudioSource>().Stop();
            UserBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            UserBox.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            UserBox.gameObject.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
            UserBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("active 3");
            UserBox.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            UserBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UserBox.gameObject.transform.GetChild(2).gameObject.SetActive(false);
        }

    }
    public void nextBtnLogin(int a)
    {
        UserBox.GetComponent<AudioSource>().Stop();
        if (a == 0)
        {

            UnityEngine.PlayerPrefs.SetInt("comeForBuy", 0);
            int witchstoreSelected = UnityEngine.PlayerPrefs.GetInt("store");

            if (witchstoreSelected == 1)
            {
                Buy(witchProductForBuy);
            }
            else
            {
                InAppStore.instance.purchaseProduct(witchProductForBuy);
            }




        }
        else if (a == 1) // next btn save name
        {
            if (String.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("city")))
            {
                UserBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                UserBox.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                UserBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
                UserBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
                UserBox.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(7).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("m_sarial");
                UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(8).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("phoneNumber");
                UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(9).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("childName");
                UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(10).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("childAge");
                UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(11).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("city");
            }
        }
        else
        {


            //save user
            UnityEngine.PlayerPrefs.SetString("childName", UserBox.gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<InputField>().text);
            UnityEngine.PlayerPrefs.SetString("childAge", UserBox.gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(3).GetComponent<InputField>().text);
            UnityEngine.PlayerPrefs.SetString("city", UserBox.gameObject.transform.GetChild(1).transform.GetChild(0).transform.GetChild(4).GetComponent<InputField>().text);

            UserBox.gameObject.transform.GetChild(0).gameObject.SetActive(false);
            UserBox.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            UserBox.gameObject.transform.GetChild(2).gameObject.SetActive(true);

            // load user
            Debug.Log(UnityEngine.PlayerPrefs.GetString("childName"));
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(7).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("m_sarial");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(8).GetComponent<Text>().text = UnityEngine.PlayerPrefs.GetString("phoneNumber");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(9).GetComponent<RtlText>().text = UnityEngine.PlayerPrefs.GetString("childName");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(10).GetComponent<RtlText>().text = UnityEngine.PlayerPrefs.GetString("childAge");
            UserBox.gameObject.transform.GetChild(2).transform.GetChild(0).transform.GetChild(11).GetComponent<RtlText>().text = UnityEngine.PlayerPrefs.GetString("city");
        }
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

    public void Buy(int a)
    {
        GameObject.Find("manager").GetComponent<MainAppController>().clik(a);
    }
    public void checkdico(int a)
    {
        GameObject.Find("manager").GetComponent<MainAppController>().clikDico(a);
    }
  
    public void confirmPhoneNumber()
    {
        GameObject.Find("manager").GetComponent<MainAppController>().OnSendNumberButtonPressed();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


}
