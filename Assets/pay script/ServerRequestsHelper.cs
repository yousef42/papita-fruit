using System.Collections;
using System.Collections.Generic;

using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerRequestsHelper : MonoBehaviour
{
    public Text m_phoneNumberInputfield;
    public string serverdevise, serverdeviseios;
    public string whereFrom; 
    public enum ValidationStates
    {
        EUnknown,
        EValid,
        EInvalid
    }

    private ValidationStates m_isAppVersionValid = ValidationStates.EUnknown;
    public ValidationStates IsAppVersionValid
    {
        get { return m_isAppVersionValid; }
    }

    private ValidationStates m_havePayedApplication = ValidationStates.EUnknown;
    public ValidationStates HavePayedApplication
    {
        get { return m_havePayedApplication; }
    }

    [SerializeField]
    private string m_checkVersionUriGet;

    [SerializeField]
    private string m_checkPaymentUriPost;
    public GameObject lockerr;
    public string orderidd;
    public GameObject manage;
    public GameObject p1;

    public int activation;

    private int sendeAmar = 0;

    private string phoneNumber;
    //private string m_sendPhoneNumberUriPost;
    private string m_sendPhoneNumberPath = "http://papitafood.ir/service/addPhone.php";


    public event System.Action<string> OnPaymentCheckSuccessCallback;
    public event System.Action OnPaymentCheckFailCallback;

    void Awake()
    {
        m_isAppVersionValid = ValidationStates.EUnknown;
        m_havePayedApplication = ValidationStates.EUnknown;

        OnPaymentCheckSuccessCallback = (string serverResponse) => { };
        OnPaymentCheckFailCallback = () => { };
    }
    void Start()
    {
        sendeAmar = 0;
        bool internetPossiblyAvailable;
        switch (Application.internetReachability)
        {
            case NetworkReachability.ReachableViaLocalAreaNetwork:
                internetPossiblyAvailable = true;
                break;
            case NetworkReachability.ReachableViaCarrierDataNetwork:
                //internetPossiblyAvailable = allowCarrierDataNetwork;
                internetPossiblyAvailable = true;
                break;
            default:
                internetPossiblyAvailable = false;
                break;
        }
        if (!internetPossiblyAvailable)
        {

            return;
        }

        CheckApplicationPaymentValidation();
        sendeAmar = UnityEngine.PlayerPrefs.GetInt("send");

        if (internetPossiblyAvailable)
        {
            if (sendeAmar == 1)
            {
                phoneNumber = m_phoneNumberInputfield.text;
            }
            else
            {
                UnityEngine.PlayerPrefs.SetInt("send", 1);

                phoneNumber = "0000000000";

                OnSendNumberButtonPressed();


            }
        }
      


    }

    #region VersionCheck

    /// <summary>
    /// You only have to call this method to check version. Do NOT call private methods beneath this one.
    /// </summary>
    public void CheckApplicationVersionValidation()
    {
        StartCoroutine(SendVersionCheckReq());
    }

    /// <summary>
    /// DO NOT CALL THIS METHOD. USE "CheckApplicationVersionValidation" INSTEAD.
    /// </summary>
    IEnumerator SendVersionCheckReq()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(m_checkVersionUriGet))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            if (www.isNetworkError)
            {
                OnVersionCheckFail(www.error);
            }
            else
            {
                OnVersionCheckSuccess(www.downloadHandler.text);
            }
        }
    }
    public void OnSendNumberButtonPressed()
    {
        SendPhoneNumber(phoneNumber);
        
        UnityEngine.PlayerPrefs.SetString("myphone", m_phoneNumberInputfield.text);

        sendeAmar = UnityEngine.PlayerPrefs.GetInt("send");
        if (sendeAmar == 1)
        {

            UnityEngine.PlayerPrefs.SetInt("phoneee", 1);
        }
       

    }
    IEnumerator SendPhoneNumberRequest(string phoneNumber)
    {
        //WWWForm form = new WWWForm();
        if (sendeAmar == 1)
        {
            phoneNumber = m_phoneNumberInputfield.text;
        }


       
        string clientID = "SN" + SystemInfo.deviceUniqueIdentifier;


        //http://www.papitaw.ir/service/addPhone.php?orderid=11113&mobile=09122023725&clientid=wwww

        Debug.Log(orderidd);
        //using (UnityWebRequest www = UnityWebRequest.Post(m_sendPhoneNumberUriPost, form))
        //using (UnityWebRequest www = UnityWebRequest.Post(startcheck.instance.CurrentValidDomain + m_sendPhoneNumberPath, form))
        //string parameters = "?orderid=" + orderidd + "&mobile=" + phoneNumber + "&clientid=" + clientID;
#if UNITY_ANDROID
        string parameters = "?clientid=" + clientID +"&mobile=" + phoneNumber + "&device=" + serverdevise + "&where=" + whereFrom;
#elif UNITY_IOS
        string parameters = "?clientid=" + clientID +"&mobile=" + phoneNumber + "&device=" + serverdeviseios + "&where=" + whereFrom;
#else
  string parameters = "?clientid=" + clientID +"&mobile=" + phoneNumber + "&device=" + serverdevise + "&where=" + whereFrom;
#endif
        Debug.Log(parameters);
        using (UnityWebRequest www = UnityWebRequest.Get( m_sendPhoneNumberPath + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            if (www.isNetworkError)
            {
                OnSendPhoneNumberFail(www.error);
            }
            else
            {
                OnSendPhoneNumberSuccess(www.downloadHandler.text);
            }
        }
    }
    /// <summary>
    /// On failure callback. Do what ever you want in case of failure.
    /// IMPORTANT: Failure at this stage just means that the server didn't respond.
    /// </summary>
    /// <param name="msg"></param>
    private void OnVersionCheckFail(string msg)
    {
        m_isAppVersionValid = ValidationStates.EUnknown;
        Debug.Log(msg);
    }

    /// <summary>
    /// On success callback. Do what ever you want in case of success.
    /// IMPORTANT: Success at this stage just means we now have the version number. You have to decide if the version number is update or not.
    /// </summary>
    /// <param name="text">data received from server</param>
    private void OnVersionCheckSuccess(string text)
    {
        // example:
        // Application.version == text;
        if (Application.version == text)
        {
            m_isAppVersionValid = ValidationStates.EInvalid;
        }
        else
        {
            m_isAppVersionValid = ValidationStates.EValid;
        }
        Debug.Log(text);
    }

#endregion

#region PaymentCheck


    /// <summary>
    /// You only have to call this method to check payment. Do NOT call private methods beneath this one.
    /// </summary>
    public void CheckApplicationPaymentValidation()
    {
        StartCoroutine(SendPaymentCheckReq());
    }

    /// <summary>
    /// DO NOT CALL THIS METHOD. USE "CheckApplicationPaymentValidation" INSTEAD.
    /// IMPORTANT: Fix "username" data.
    /// </summary>
    /// <returns></returns>
    IEnumerator SendPaymentCheckReq()
    {
        //WWWForm form = new WWWForm();

        //                              ^ could be any string. you decide.

        using (UnityWebRequest www = UnityWebRequest.Get(m_checkPaymentUriPost + "?t=" + Random.value))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.Send();

            if (www.isNetworkError)
            {
                OnPaymentCheckFail(www.error);
            }
            else
            {
                OnPaymentCheckSuccess(www.downloadHandler.text);
            }
        }
    }

    /// <summary>
    /// On failure callback. Do what ever you want in case of failure.
    /// IMPORTANT: Failure at this stage just means that the server didn't respond.
    /// </summary>
    /// <param name="msg"></param>
    private void OnPaymentCheckFail(string msg)
    {
        OnPaymentCheckFailCallback();
        Debug.Log(msg);
    }

    /// <summary>
    /// On success callback. Do what ever you want in case of success.
    /// IMPORTANT: Success at this stage just means that we now have the respond from server. 
    /// You have to decide if the player have payed or not by using the "text" parameter.
    /// </summary>
    /// <param name="text">data received from server</param>

    private void OnPaymentCheckSuccess(string text)
    {

        if (text == "unlock")
        {
           
         
           // UnityEngine.PlayerPrefs.SetInt("activation2", 0);
           // startcheck.instance.activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");
            


           // UnityEngine.PlayerPrefs.Save();
         
        }
        else if (text == "lock")
        {
      
          //  UnityEngine.PlayerPrefs.SetInt("activation2", 0);
           // startcheck.instance.activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");
           

           // UnityEngine.PlayerPrefs.Save();


        }
     
    }

#endregion

#region PhoneNumber


    /// <summary>
    /// You only have to call this method to send phone number. Do NOT call private methods beneath this one.
    /// </summary>
    public void SendPhoneNumber(string phoneNumber)
    {
       
        StartCoroutine(SendPhoneNumberRequest(phoneNumber));
    }

    /// <summary>
    /// DO NOT CALL THIS METHOD. USE "CheckApplicationPaymentValidation" INSTEAD.
    /// IMPORTANT: Fix "username" data.
    /// </summary>
    /// <returns></returns>

    /// <summary>
    /// On failure callback. Do what ever you want in case of failure.
    /// IMPORTANT: Failure at this stage just means that the server didn't respond.
    /// </summary>
    /// <param name="msg"></param>
    private void OnSendPhoneNumberFail(string msg)
    {
        Debug.Log(msg);
    }

    /// <summary>
    /// On success callback. Do what ever you want in case of success.
    /// IMPORTANT: Success at this stage means that we now have the client phone number. 
    /// </summary>
    /// <param name="text">data received from server</param>
    private void OnSendPhoneNumberSuccess(string text)
    {
        Debug.Log(text);
    }

#endregion
}
