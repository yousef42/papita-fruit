using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class downloadLink : MonoBehaviour
{
    private string textFromWWW;
    public string link;
    // Use this for initialization
    void Start () {
        StartCoroutine(GetTextFromWWW());
        //http://www.papitafood.ir/service/LinkTelegram.php
        //http://www.papitafood.ir/service/LinkWhatsapp.php
    }

    IEnumerator GetTextFromWWW()
    {


        WWW www = new WWW(link);


        yield return www;

        if (www.error != null)
        {

        }
        else
        {
           
            textFromWWW = www.text;
         
        }
    }

    public void OpenURL()
    {
        Application.OpenURL(textFromWWW);
    }
}
