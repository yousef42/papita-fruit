using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;



public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [SerializeField] SavedData savedData;

 [HideInInspector] public string currentSelectedMenu = "";


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);
    }

    void Start()
    {
        LoadSavedData();
    }

    void LoadSavedData()
    {
        FileStream file = null;

        try
        {
            if (!File.Exists(Application.persistentDataPath + "/SavedData.dat"))
            {
                print("No data available.");
                savedData = new SavedData();
                savedData.isMusicSFXOn = true;
                savedData.isBGMusicOn = true;
                savedData.tutorialShowed = false;
                SaveData();
            }
            else
            {
                BinaryFormatter bf = new BinaryFormatter();

                file = File.Open(Application.persistentDataPath + "/SavedData.dat", FileMode.Open);

                savedData = (SavedData)bf.Deserialize(file);
            }
        }
        catch (Exception)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    void SaveData()
    {
        FileStream file = null;

        try
        {
            BinaryFormatter bf = new BinaryFormatter();
            file = File.Create(Application.persistentDataPath + "/SavedData.dat");
            bf.Serialize(file, savedData);
        }
        catch (Exception)
        {

        }
        finally
        {
            if (file != null)
            {
                file.Close();
            }
        }
    }

    public void GoToMainMenuScene()
    {
        StartCoroutine(LoadScene("Main Menu"));
    }

    public void GoToCurrentScene(string sceneName)
    {
        FadingScreen.instance.LoadFadingScreen(() => SceneManager.LoadSceneAsync(sceneName), delay: 2.4f);
    }

    IEnumerator LoadScene(string sceneName, Action onLoadCompleted = null)
    {
        LoadingScreen.instance.ShowLoadingScreen(null, null, 0);
        yield return new WaitForSeconds(0.2f);
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        yield return new WaitForSeconds(2f);
        yield return new WaitUntil(() => async.isDone);
        LoadingScreen.instance.HideLoadingScreen();
    }

    public bool GetBGMusic()
    {
        return savedData.isBGMusicOn;
    }

    public bool GetSFXMusic()
    {
        return savedData.isMusicSFXOn;
    }

    public void SetBGMusic(bool isOn)
    {
        savedData.isBGMusicOn = isOn;
        SaveData();
        AudioController.instance.UpdateBGAudio(isOn);
    }

    public void SetSFXMusic(bool isOn)
    {
        savedData.isMusicSFXOn = isOn;
        SaveData();
        AudioController.instance.UpdateSfxAudio(isOn);
    }

    public void TutorialShowed()
    {
        savedData.tutorialShowed = true;
        SaveData();
    }
    public bool IsTutorialShowed()
    {
        return savedData.tutorialShowed;
    }
}

[Serializable]
public class SavedData
{
    public bool isMusicSFXOn;
    public bool isBGMusicOn;
    public bool tutorialShowed;
}