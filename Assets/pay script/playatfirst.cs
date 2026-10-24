using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class playatfirst : MonoBehaviour {

		public GameObject phoneNumber;
    public GameObject exitInterNumber;
    public Text telephoneText, myphone;

		
		int first = 0,activation2;


	// Use this for initialization
	void Start ()
    {

        //لینک ارسال کد به واتس اپ

        StartCoroutine(GetTextFromWebsite());


        first = UnityEngine.PlayerPrefs.GetInt ("phone");
            activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");

        if (first == 1 )
        {

  
            exitInterNumber.SetActive(true);
            telephoneText.text = "ﺪﯿﯾﺎﻤﻧ ﺩﺭﺍﻭ ﺍﺭﺩﻮﺧ ﻩﺍﺮﻤﻫ ﻩﺭﺎﻤﺷ ﺯﺎﯿﻧ ﺕﺭﻮﺻ ﺭﺩ";
        }

      Invoke("domyjob",0.5f);
      /*  if (first == 1 || activation2 > 0)
        {


            phoneNumber.SetActive(false);
        }

        else
        {
            phoneNumber.SetActive(true);

        }
        */



    }

    // Update is called once per frame
   public void domyjob () {
        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");
        if (first == 1 )
        {
            
            phoneNumber.SetActive(false);
            exitInterNumber.SetActive(true);
            telephoneText.text = "ﺪﯿﯾﺎﻤﻧ ﺩﺭﺍﻭ ﺍﺭﺩﻮﺧ ﻩﺍﺮﻤﻫ ﻩﺭﺎﻤﺷ ﺯﺎﯿﻧ ﺕﺭﻮﺻ ﺭﺩ";
        }

        else
        {
            phoneNumber.SetActive(true);
         
        }

    }
    public IEnumerator GetTextFromWebsite()
    {


        // web request object
#if UNITY_ANDROID
        WWW webRequest = new WWW("http://papitaplus.ir/service/LinkWhatsappCode.php");

#elif UNITY_IOS
        WWW webRequest = new WWW("http://papitaplus.ir/service/introTextIOS.php");
#else
    WWW webRequest = new WWW("http://papitaplus.ir/service/introText.php");

#endif


        // wait on the return
        yield return webRequest;

        UnityEngine.PlayerPrefs.SetString("whatsCode", webRequest.text);


    }
    public void user(){
			
			UnityEngine.PlayerPrefs.SetInt ("phone",1);
	
			UnityEngine.PlayerPrefs.Save ();
        phoneNumber.SetActive (false);
			
		

		}



    public void showNum()
    {
        string phone = UnityEngine.PlayerPrefs.GetString("myphone");
        if (string.IsNullOrEmpty(phone))
        {
            myphone.text = "ﻩﺪﺸﻧ ﺖﺒﺛ ﺱﺎﻤﺗ ﻩﺭﺎﻤﺷ" + Environment.NewLine + "ﺪﯿﻧﺰﺑ ﺍﺭ ﺮﯾﺯ ﻪﻤﮐﺩ ﺖﺒﺛ ﺖﻬﺟ";
            myphone.color = Color.red;
            myphone.fontSize = 30;

        }
        else
        {
            myphone.text = phone;
            myphone.color = Color.black;
            myphone.fontSize = 40;

        }
    }

}
