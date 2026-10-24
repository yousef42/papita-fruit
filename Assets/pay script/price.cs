using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class price : MonoBehaviour
{



    public Text guitext ;

    void Start()
    {
        // Start the GetTextFromWebsite Coroutine
        StartCoroutine(GetTextFromWebsite());
    }

    public IEnumerator GetTextFromWebsite()
    {
      

        // web request object
       // WWW webRequest = new WWW("http://papitafood.ir/service/introText.php");

        // wait on the return
       // yield return webRequest;

      //  guitext.text = webRequest.text;

        UnityWebRequest www = UnityWebRequest.Get("https://www.papitafood.ir/service/introText.php");
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
           // Debug.Log(www.downloadHandler.text);
            guitext.text = www.downloadHandler.text;
            // Or retrieve results as binary data
            // byte[] results = www.downloadHandler.data;
        }

    }

    public void showagaintext()
    {
        StartCoroutine(GetTextFromWebsite());
    }
}
