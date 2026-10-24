using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class coinController : MonoBehaviour {

    public bool dakheli;
	// Use this for initialization
	void Start () {

        if (dakheli)
        {
                //سکه میگیره
                int coin = UnityEngine.PlayerPrefs.GetInt("coins");
                int newcoin = coin + 10;
                UnityEngine.PlayerPrefs.SetInt("coins", newcoin);
               
            
        }
        else
        {
            int coin = UnityEngine.PlayerPrefs.GetInt("coins");
            GetComponent<Text>().text = coin.ToString();
          //  Invoke("play", 1f);
        }
      

    }
	
	// Update is called once per frame
	void play () {

        GetComponent<AudioSource>().Play();

        int coin = UnityEngine.PlayerPrefs.GetInt("coins");
        GetComponent<Text>().text = coin.ToString();
        if (dakheli)
        {
            GetComponent<Animator>().enabled = true;
        }
     

    }
}
