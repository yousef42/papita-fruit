using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UPersian.Components;
using UnityEngine.Networking;
using System;
using System.IO;

public class leaderBoardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static leaderBoardManager instance;
    public bool justSetData;
    public Animator menuUI;
    public GameObject noor, thisUser, littleBox,coinsBoxAnim,coinsButton;
    public GameObject[] userLevels;
    public RtlText[] users;
    public Text scoreUI;
    bool showThisUser = true, showBox = false, showimageBox = false;
    string mySelfLevel;
    public InputField[] info;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        // PlayerPrefs.SetInt("coins", 150);
        
        if (!string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("city")))
        {

            SetData();

        }

        if (!justSetData)
        {
            StartCoroutine(menuManager());
            Screen.orientation = ScreenOrientation.LandscapeLeft;
            if (Screen.width > 2300)
            {
                menuUI.Play("leader menu 2400");
            }
            else
            {
                menuUI.Play("leader menu 1920");
            }
        }
        else
        {
            if (string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("city")))
            {

              //  noor.SetActive(true);

            }
            else
            {
                noor.SetActive(false);
            }

         
          
        }

        openCoinBox();
    }
    public void OpenLeaderBoard()
    {
        StartCoroutine(checkInternetConnection((isConnected) =>
        {
            if (isConnected)
            {

            }
            else
            {
              
                toast();
            }
        }));
        if (String.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("city")))
        {
            UnityEngine.PlayerPrefs.SetInt("comeForBuy", 0);
            LockLevelsManager.instance.userBtn();
        }
        else
        {
            SceneManager.LoadScene("leader board");
        }


    }
    // Update is called once per frame
    public void getCityName()
    {
        UnityEngine. PlayerPrefs.SetString("childName", info[0].text);
        UnityEngine.PlayerPrefs.SetString("childAge", info[1].text);
        UnityEngine.PlayerPrefs.SetString("city", info[2].text);
        SetData();
        
    }

    public void GoHome()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void SetData()
    {
        StartCoroutine(InitUserData());
    }
    IEnumerator menuManager()
    {

        yield return new WaitForSeconds(1.2f);
        noor.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        // if need
        if (showThisUser)
        {
            thisUser.SetActive(true);

        }

        if (string.IsNullOrEmpty(UnityEngine.PlayerPrefs.GetString("city")))
        {
            users[6].text = "0";
            users[8].text = "0";
            users[7].text = "اطلاعات کاربری را تکمیل کنید";
            thisUser.GetComponent<Button>().enabled = true;
            UnityEngine. PlayerPrefs.SetInt("comeFromLeaderBoard", 1);
        }
        else
        {

            //  users[7].text = PlayerPrefs.GetString("childName")+" " + PlayerPrefs.GetString("childAge") + " ساله از " + PlayerPrefs.GetString("city");
            if (users[7].text.Length < 16)
            {
                users[7].fontSize = 35;
            }
            else if (users[7].text.Length < 30)
            {
                users[7].fontSize = 30;
            }
            else if (users[7].text.Length < 45)
            {
                users[7].fontSize = 22;
            }
            else if (users[7].text.Length > 45)
            {
                users[7].fontSize = 15;
            }

        }
        yield return new WaitForSeconds(1.5f);
        // if need
        if (showThisUser)
        {
            userLevels[3].GetComponent<Animator>().Play("GoToGreen4");
        }
        if (mySelfLevel == "1")
        {
            userLevels[0].GetComponent<Animator>().Play("GoToGreen1");
        }
        if (mySelfLevel == "2")
        {
            userLevels[1].GetComponent<Animator>().Play("GoToGreen2");
        }
        if (mySelfLevel == "3")
        {
            userLevels[2].GetComponent<Animator>().Play("GoToGreen3");
        }
        if (showBox)
            littleBox.SetActive(true);

        if (showimageBox)
            giftImage.gameObject.SetActive(true);
    }

    IEnumerator InitUserData()
    {
        /*
         string name = UnityEngine.PlayerPrefs.GetString("childName");
         string mobile = UnityEngine.PlayerPrefs.GetString("phoneNumber");
         string age = UnityEngine.PlayerPrefs.GetString("childAge"); ;
         string city = UnityEngine.PlayerPrefs.GetString("city");
        // string applicationid = "1";



         WWWForm form = new WWWForm();
         form.AddField("name", name);
         form.AddField("mobile", mobile);
         form.AddField("age", age);
         form.AddField("city", city);
       //  form.AddField("app_id", applicationid);

         UnityWebRequest www = UnityWebRequest.Post("https://papitadb.ir/api/scores", form);
         yield return www.SendWebRequest();


         Debug.Log("Form upload complete!");
         Debug.Log(www.downloadHandler.text);
         processJsonData(www.downloadHandler.text);
          */

        string name = UnityEngine.PlayerPrefs.GetString("childName");
        string mobile = UnityEngine.PlayerPrefs.GetString("phoneNumber");
        string age = UnityEngine.PlayerPrefs.GetString("childAge"); ;
        string city = UnityEngine.PlayerPrefs.GetString("city");
        string score = UnityEngine.PlayerPrefs.GetInt("coins").ToString();
        string parameters = "?mobile=" + mobile + "&name=" + name + "&age=" + age + "&city=" + city + "&score=" + score + "&application_id=" +"1";


        Debug.Log("https://papitadb.ir/api/scores" + parameters);
        using (UnityWebRequest www = UnityWebRequest.Get("https://papitadb.ir/api/scores" + parameters))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            www.chunkedTransfer = false;
            www.certificateHandler = new BypassCertificate();
            yield return www.SendWebRequest();


            Debug.Log(www.downloadHandler.text);


           
                processJsonData(www.downloadHandler.text);
            

        }

    }
    string mounth, year;
    void processJsonData(string _url)
    {
        LeaderDataClass jsnData = JsonUtility.FromJson<LeaderDataClass>(_url);

        if (!justSetData)
        {
            foreach (LeaderList m in jsnData.interData)
            {



                if (m.status == "200")
                {

                    #region set data 1
                    users[6].text = m.mySelf;


                    if (m.mySelf == "1" || m.mySelf == "2" || m.mySelf == "3")
                    {
                        showThisUser = false;
                        mySelfLevel = m.mySelf;
                    }
                    // "date":"1401-08"
                    year = m.date.Substring(0, 4);
                    Debug.Log("date : " + m.date);
                    #region mounths
                    if (m.date.Contains("-01"))
                    {
                        mounth = "فروردین";
                    }
                    else if (m.date.Contains("-02"))
                    {
                        mounth = "اردیبهشت";
                    }
                    else if (m.date.Contains("-03"))
                    {
                        mounth = "خرداد";
                    }
                    else if (m.date.Contains("-04"))
                    {
                        mounth = "تیر";
                    }
                    else if (m.date.Contains("-05"))
                    {
                        mounth = "مرداد";
                    }
                    else if (m.date.Contains("-06"))
                    {
                        mounth = "شهریور";
                    }
                    else if (m.date.Contains("-07"))
                    {
                        mounth = "مهر";
                    }
                    else if (m.date.Contains("-08"))
                    {
                        mounth = "آبان";
                    }
                    else if (m.date.Contains("-09"))
                    {
                        mounth = "آذر";
                    }
                    else if (m.date.Contains("-10"))
                    {
                        mounth = "دی";
                    }
                    else if (m.date.Contains("-11"))
                    {
                        mounth = "بهمن";
                    }
                    else if (m.date.Contains("-12"))
                    {
                        mounth = "اسفند";
                    }
                    #endregion

                    users[9].text = mounth + " " + year;

                    // gift boxes
                    users[10].text = m.message;
                    users[10].fontSize = m.messageFont;
                    showBox = m.showBox;
                    showimageBox = m.showImageBox;
                    if (showimageBox)
                    {
                        imageURL = m.ImageUrl;
                        OnLoadImage();
                    }
                    #endregion



                }


            }
        }



        MyUserLeaderBoardDataClass jsnDatas = JsonUtility.FromJson<MyUserLeaderBoardDataClass>(_url);


        foreach (MyUserLeaderBoardList s in jsnDatas.user)
        {

            Debug.Log(s.score);
            if (!justSetData)
            {
                users[7].text = s.name + " " + (s.age).ToString() + " ساله از " + s.city;
                users[8].text = s.score;
                int sc = int.Parse(s.score);
                UnityEngine.PlayerPrefs.SetInt("coins", sc);
            }
            else
            {
                int sc = int.Parse(s.score);
                UnityEngine.PlayerPrefs.SetInt("coins", sc);
            }



        }



        if (!justSetData)
        {
            LeaderBoardDataClass jsnDataOfapp = JsonUtility.FromJson<LeaderBoardDataClass>(_url);
            foreach (LeaderBoardList x in jsnDataOfapp.data)
            {
                //ctrl + k + s
                #region setting nafarat
                // نفر اول
                if (x.num == 1)
                {

                    users[1].text = x.score;
                    users[0].text = x.name + " " + (x.age).ToString() + " ساله از " + x.city;
                    if (users[0].text.Length < 16)
                    {
                        users[0].fontSize = 30;
                    }
                    else if (users[0].text.Length < 30)
                    {
                        users[0].fontSize = 22;
                    }
                    else if (users[0].text.Length < 45)
                    {
                        users[0].fontSize = 20;
                    }
                    else if (users[0].text.Length > 45)
                    {
                        users[0].fontSize = 13;
                    }
                }


                // نفر دوم
                if (x.num == 2)
                {
                    users[3].text = x.score;
                    users[2].text = x.name + " " + (x.age).ToString() + " ساله از " + x.city;
                    if (users[2].text.Length < 16)
                    {
                        users[2].fontSize = 30;
                    }
                    else if (users[2].text.Length < 30)
                    {
                        users[2].fontSize = 22;
                    }
                    else if (users[2].text.Length < 45)
                    {
                        users[2].fontSize = 20;
                    }
                    else if (users[2].text.Length > 45)
                    {
                        users[2].fontSize = 13;
                    }
                }

                // نفر سوم
                if (x.num == 3)
                {
                    users[5].text = x.score;
                    users[4].text = x.name + " " + (x.age).ToString() + " ساله از " + x.city;
                    if (users[4].text.Length < 16)
                    {
                        users[4].fontSize = 30;
                    }
                    else if (users[4].text.Length < 30)
                    {
                        users[4].fontSize = 22;
                    }
                    else if (users[4].text.Length < 45)
                    {
                        users[4].fontSize = 20;
                    }
                    else if (users[4].text.Length > 45)
                    {
                        users[4].fontSize = 13;
                    }
                }

            }
            #endregion
        }



    }
    bool hasInternet = false;
   

    IEnumerator checkInternetConnection(Action<bool> action)
    {
        WWW www = new WWW("http://google.com");
        yield return www;
        if (www.error != null)
        {
            action(false);
        }
        else
        {
            action(true);
        }
    }

    public void toast()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        _ShowAndroidToastMessage("برای دیدن رتبه خود در نفرات برتر اینترنت خود را روشن کنید");
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void _ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
#endif





    ////////////////////////////////////////////////////   load & save image ////////////////////////////////////////////////////////////


    [SerializeField]
    Image giftImage;
    string imageURL;
    string fileName;


    public void OnLoadImage()
    {
        Debug.Log("image url : " + imageURL);
        StartCoroutine(LoadTextureFromWeb());
    }

    IEnumerator LoadTextureFromWeb()
    {
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(imageURL);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError("Error: " + www.error);
        }
        else
        {
            Texture2D loadedTexture = DownloadHandlerTexture.GetContent(www);
            giftImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
            giftImage.SetNativeSize();
        }
    }

    public void OnLoadImageFromDiskButtonClick()
    {
        if (!File.Exists(Application.persistentDataPath + fileName))
        {
            Debug.LogError("File Not Exist!");
            return;
        }

        LoadImageFromDisk();
    }
    // load texture bytes from disk and compose sprite from bytes
    private void LoadImageFromDisk()
    {
        byte[] textureBytes = File.ReadAllBytes(Application.persistentDataPath + fileName);
        Texture2D loadedTexture = new Texture2D(0, 0);
        loadedTexture.LoadImage(textureBytes);
        giftImage.sprite = Sprite.Create(loadedTexture, new Rect(0f, 0f, loadedTexture.width, loadedTexture.height), Vector2.zero);
        giftImage.SetNativeSize();

    }

    public void OnSaveImageButtonClick()
    {
        if (giftImage.sprite == null)
        {
            Debug.LogError("No Image to Save!");
            return;
        }

        WriteImageOnDisk();
    }

    private void WriteImageOnDisk()
    {
        byte[] textureBytes = giftImage.sprite.texture.EncodeToPNG();
        File.WriteAllBytes(Application.persistentDataPath + fileName, textureBytes);
        Debug.Log("File Written On Disk!");
    }

    /// <summary>
    /// click event - make image blank 
    /// </summary>
    public void OnBlankImageButtonClick()
    {
        giftImage.sprite = null;
    }

    /// <summary>
    /// //////////////////////////////////////////////////////// score showing //////////////////////////////////////////////////////
    /// </summary>
    int score, displayScore;
    float scoreTimer=0.02f;
    public void openCoinBox()
    {
        //coinsBoxAnim.SetActive(false);
        score = UnityEngine.PlayerPrefs.GetInt("coins");

        if (score < 100)
        {
            scoreTimer = 0.002f;
        }
        else if (score < 500)
        {
            scoreTimer = 0.0001f;
        }
        else
        {
            scoreTimer = 0.00005f;
        }
        displayScore = 0;
        scoreUI.text = displayScore.ToString();

        if (score > 0)
        {
            StartCoroutine(ScoreUpdater());
        }
   
       
    }

    private IEnumerator ScoreUpdater()
    {
        while (true)
        {
            if (displayScore < score)
            {
                displayScore++; //Increment the display score by 1
                scoreUI.text = displayScore.ToString(); //Write it to the UI
            }
            else
            {
              //  coinsBoxAnim.SetActive(true);
            }
            yield return new WaitForSeconds(scoreTimer); // I used .2 secs but you can update it as fast as you want
        }
    }
}

[System.Serializable]
public class LeaderDataClass
{
    public string status;
    public List<LeaderList> interData;

}

[System.Serializable]
public class LeaderList
{

    public string status;
    public string mySelf;
    public bool showBox;
    public string message;
    public string date;
    public int messageFont;
    public bool showImageBox;
    public string ImageUrl;
}
[System.Serializable]
public class LeaderBoardDataClass
{
    public string id;
    public List<LeaderBoardList> data;

}

[System.Serializable]
public class LeaderBoardList
{

    public string id;
    public int num;
    public string name;
    public string age;
    public string city;
    public string score;

}
[System.Serializable]
public class MyUserLeaderBoardDataClass
{
    public string name;
    public List<MyUserLeaderBoardList> user;

}

[System.Serializable]
public class MyUserLeaderBoardList
{


    public string name;
    public string age;
    public string city;
    public string score;

}
