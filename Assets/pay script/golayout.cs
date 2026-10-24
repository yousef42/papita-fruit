using UnityEngine;
using System.Collections;

public class golayout : MonoBehaviour {
//	public string lName;
	public Color loadToColor = Color.white;

	public void go2LevelAsync(string lName) 
	{

        Application.LoadLevel(lName); 
	
	}


	void Update () {
		gameObject.SetActive (true);

	}
}
