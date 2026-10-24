using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour {

    public bool isIntro;
    int activation,firstTime =0 , activation2;
    public GameObject loadingScreen;
	public Slider slider;
    string sceneIndex;

    void Start()
    {
        if (isIntro)
        {
            Invoke("changeScene", 1.5f);
          
        }
    }

    void changeScene()
    {
        activation = UnityEngine.PlayerPrefs.GetInt("activation");
        activation2 = UnityEngine.PlayerPrefs.GetInt("activation2");
        firstTime = UnityEngine.PlayerPrefs.GetInt("first Time");

        if (firstTime == 0)
        {
            sceneIndex = "Main Menu";
            UnityEngine.PlayerPrefs.SetInt("first Time", 1);
            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
        else if (activation > 0 | activation2 > 0)
        {
            sceneIndex = "afterBuy";

            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
        else
        {
            sceneIndex = "intro";

            StartCoroutine(LoadAsynchronously(sceneIndex));
        }
    }

    public void LoadLevel (string sceneIndex)
	 
	 {
		 StartCoroutine(LoadAsynchronously(sceneIndex));
	 }
	 
	 IEnumerator LoadAsynchronously (string sceneIndex)
	 {
      
        AsyncOperation operation = SceneManager.LoadSceneAsync (sceneIndex);
		 
		 loadingScreen.SetActive(true);
		 
		 while (!operation.isDone)
		 {
			 float progress = Mathf.Clamp01(operation.progress / 0.9f);
			 
			 slider.value = progress;
			
			 
			 yield return null;
		 }
		 
	 }

    public void GoToTutorialScene()
    {
        SceneManager.LoadScene("TutScene");
    }


}
	
