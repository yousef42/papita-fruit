using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftController : MonoBehaviour {

#if UNITY_ANDROID
    private string urlversion = "http://www.papitafood.ir/service/Gift.php";

#elif UNITY_IOS
    private string urlversion = "http://www.papitafood.ir/service/Giftios.php";
#else
    private string urlversion = "http://www.papitafood.ir/service/Gift.php";

#endif

    private string textFromWWW, LinkFromWWW;
    public GameObject Gift;

    private int activation, domyjob;
    private string webText = string.Empty;
    public Text GiftText = null,giftBtnText;
    void Start () {


        if (!UnityEngine.PlayerPrefs.HasKey("Issayhello"))
        {
            UnityEngine.PlayerPrefs.SetInt("Issayhello", 0);
        }

        domyjob = UnityEngine.PlayerPrefs.GetInt("Issayhello");

        if (domyjob == 0)
        {
            StartCoroutine(GetTextFromWWW());
            StartCoroutine(GetLinkFromWWW());

            StartCoroutine(GetTextFromWebsite());
            StartCoroutine(GetBtnTextFromWWW());
           


            activation = UnityEngine.PlayerPrefs.GetInt("activation");
        }
      
    }


    IEnumerator GetTextFromWWW()
    {
     
#if UNITY_ANDROID
        WWW www = new WWW("http://www.papitafood.ir/service/Gift.php");

#elif UNITY_IOS
        WWW www = new WWW("http://www.papitafood.ir/service/Giftios.php");
#else
        WWW www = new WWW("http://www.papitafood.ir/service/Gift.php");
#endif
        yield return www;

        if (www.error != null)
        {

        }
        else
        {
            //رشته داخل فایل تکست رو بخونه و تبدیلش کنه به فلوت
            textFromWWW = www.text;
            float NewVersion = float.Parse(textFromWWW);

          

            if (NewVersion==1)

            {
                Debug.Log("you have gifttttt !!!!!  :)))");

               
               
                    Gift.SetActive(true);
                
                

            }
            else
            {
                Gift.SetActive(false);
                Debug.Log("you dont have gift :((((");

            }
        }
    }


    public IEnumerator GetTextFromWebsite()
    {



#if UNITY_ANDROID     // web request object
        WWW webRequest = new WWW("http://www.papitafood.ir/service/GiftText.php");

#elif UNITY_IOS
    WWW webRequest = new WWW("http://www.papitafood.ir/service/GiftText.php");
#else
        WWW webRequest = new WWW("http://www.papitafood.ir/service/GiftText.php");
#endif

        // wait on the return
        yield return webRequest;

        // Save text to local var if you need it elsewhere
        webText = webRequest.text;

        // Save text to a Text Object directly if needed
        GiftText.text = webRequest.text;


    }

    IEnumerator GetLinkFromWWW()
    {
     
#if UNITY_ANDROID
        WWW www = new WWW("http://www.papitafood.ir/service/GiftLink.php");

#elif UNITY_IOS
        WWW www = new WWW("http://www.papitafood.ir/service/GiftLink.php");
#else
        WWW www = new WWW("http://www.papitafood.ir/service/GiftLink.php");
#endif
        yield return www;

        if (www.error != null)
        {

        }
        else
        {
           
            LinkFromWWW = www.text;
            
            Debug.Log("your gift link is" + LinkFromWWW);
         
        }
    }

    public void openLinkGift()
    {
        Application.OpenURL(LinkFromWWW);
    }

    IEnumerator GetBtnTextFromWWW()
    {

#if UNITY_ANDROID     // web request object
        WWW webRequest = new WWW("http://www.papitafood.ir/service/GiftBtnText.php");

#elif UNITY_IOS
    WWW webRequest = new WWW("http://www.papitafood.ir/service/GiftBtnText.php");
#else
        WWW webRequest = new WWW("http://www.papitafood.ir/service/GiftBtnText.php");
#endif

        // wait on the return
        yield return webRequest;

        // Save text to local var if you need it elsewhere
        webText = webRequest.text;

        // Save text to a Text Object directly if needed
        giftBtnText.text = webRequest.text;
    }
}
