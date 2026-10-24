using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class addStatusBazar : MonoBehaviour
{

    private string clientID;

    private string url = "https://papitadb.ir/service/account/store-payment";
    private string phoneNum, price, mydico, last_price, token;
    public int application_id;
    public int witchTerm;
    private string buyType, market_id;
    public MainAppController mainApp;

    void Start()
    {

        mainApp = GameObject.Find("manager").GetComponent<MainAppController>();
        //  StartCoroutine(SendDicoRequest());




    }

    public void SetStatus()
    {
        StartCoroutine(SendDicoRequest());
    }

    IEnumerator SendDicoRequest()
    {
        buyType = UnityEngine.PlayerPrefs.GetString("tarriff_id");
        // Debug.Log("buyType" + buyType);
        if (buyType == witchTerm.ToString())
        {
            last_price = UnityEngine.PlayerPrefs.GetString("price");
        }
        else if (buyType == "17")
        {
            last_price = UnityEngine.PlayerPrefs.GetString("price2");
        }
        else if (buyType == "27")
        {
            last_price = UnityEngine.PlayerPrefs.GetString("price18");
        }
        else if (buyType == application_id.ToString())
        {
            last_price = UnityEngine.PlayerPrefs.GetString("price3");
        }

        phoneNum = UnityEngine.PlayerPrefs.GetString("phoneNumber");

        mydico = UnityEngine.PlayerPrefs.GetString("myDicoCode");

        token = UnityEngine.PlayerPrefs.GetString("token");

        market_id = UnityEngine.PlayerPrefs.GetString("M_store");
        string parameters = "?mobile=" + phoneNum + "&tarriff_id=" + buyType + "&market_id=" + market_id + "&discount_code=" + mydico + "&last_price=" + last_price + "&transaction_id=" + token;


        Debug.Log(url + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get(url + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();


            Debug.Log(www.downloadHandler.text);



            processJsonDataMarket(www.downloadHandler.text);
        }

    }

    void processJsonDataMarket(string _url)
    {
        BuyInMarkets jsnData = JsonUtility.FromJson<BuyInMarkets>(_url);






        foreach (CheckListMarket x in jsnData.interData)
        {


            if (x.status == "200")
            {
                Debug.Log(" kharid bazar moshtri ba movafaghiyat sabt shod");
                mainApp.IranStoreCheck();
            }
            else
            {
                Debug.Log(" khataye sabt kharid moshtari dar bazar");
                StartCoroutine(SendDicoRequest());
            }


        }




    }

}
