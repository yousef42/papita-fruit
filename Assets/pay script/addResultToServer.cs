using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class addResultToServer : MonoBehaviour
{
    private string clientID,FinalResult;
    private string url = "http://www.papitafood.ir/service/addresult.php";
    private string phoneNum;

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


    public int activation;
    //private string m_sendPhoneNumberUriPost;
    private string m_sendPhoneNumberPath = "/service/addresult.php";


    public event System.Action<string> OnPaymentCheckSuccessCallback;
    public event System.Action OnPaymentCheckFailCallback;

    void Awake()
    {
   
    }
    void Start()
    {

        //  string serial = SystemInfo.deviceUniqueIdentifier;
        //   clientID = "SN" + serial;

        phoneNum = UnityEngine.PlayerPrefs.GetString("myphone");
        Debug.Log(phoneNum);
#if UNITY_ANDROID
        string serial = SystemInfo.deviceUniqueIdentifier;
        if (string.IsNullOrEmpty(serial)) //اگه کاربر اجازه دسترسی به شناسش نده موقع نصب
        {
            serial = UnityEngine.PlayerPrefs.GetString("orderId");
            clientID = "OR" + serial;
          
        }
        else
        {
            clientID = "SN" + serial;
        }



#elif UNITY_IOS

        string serial = Device.advertisingIdentifier;
          if (string.IsNullOrEmpty(serial))
         {
            serial = UnityEngine.PlayerPrefs.GetString("orderId");
            clientID = "OR" + serial;
          }
          else{
            clientID = "SN" + serial;
          }

#endif



         FinalResult = UnityEngine.PlayerPrefs.GetInt("coins").ToString();

     

            OnSendNumberButtonPressed();



    }

    #region VersionCheck

 
    public void CheckApplicationVersionValidation()
    {
        StartCoroutine(SendVersionCheckReq());
    }

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
        SendPhoneNumber(clientID);
       

    }
    IEnumerator SendPhoneNumberRequest(string clientID)
    {
  


        string parameters = "?clientid=" + clientID + "&result=" + FinalResult + "&phone=" + phoneNum;
       
        Debug.Log(parameters);
        using (UnityWebRequest www = UnityWebRequest.Get(url + parameters))
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
    public void SendPhoneNumber(string clientID)
    {
        StartCoroutine(SendPhoneNumberRequest(clientID));
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

