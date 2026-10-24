using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;

public class checkInternet : MonoBehaviour {
	
	
	public GameObject PhoneBox,CheckInternet;
   

    private int a = 0;
	// Use this for initialization
	public void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {
		
			if(Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
		
            PhoneBox.SetActive(false);
#if UNITY_IOS
            CheckInternet.SetActive(true);
#endif

        }

        else 
		{
            CheckInternet.SetActive(false);
        }
		
	}
	


}
