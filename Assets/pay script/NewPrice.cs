using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewPrice : MonoBehaviour
{


    public string PHPname;
    public Text guitext;



    void Start()
    {
     
            StartCoroutine(GetTextFromWebsite());
 
    }

    public IEnumerator GetTextFromWebsite()
    {



     WWW webRequest = new WWW("http://papitaplus.ir/service/NewBuy/"+ PHPname);




        // wait on the return
        yield return webRequest;

        guitext.text = webRequest.text;

    }
    void OnApplicationFocus()
    {
        StartCoroutine(GetTextFromWebsite());
    }


}
