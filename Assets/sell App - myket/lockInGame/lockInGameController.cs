using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UPersian.Components;

public class lockInGameController : MonoBehaviour
{
    public static lockInGameController instance;

    private string currentSceneName;
    private bool hasSceneChanged;
    private Canvas m_canvas;
    Button m_button;
    public GameObject boxLock;
    public Button GoBuy;
    public AudioClip[] sounds;
    private void Awake()
    {



        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);

        currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    void Start()
    {
      
        m_button = this.gameObject.transform.GetChild(0).transform.GetChild(0).gameObject.GetComponent<Button>();
        m_button.onClick.AddListener(() => TouchLock());
        m_canvas = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Canvas>();
        m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        m_canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

        GoBuy.onClick.AddListener(() => GoForBuy());

        StartCoroutine(makeChanges());
    }

    public void TouchLock()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        if(sceneName== "video_1")
        {
            this.gameObject.GetComponent<AudioSource>().clip = sounds[0];
            boxLock.transform.GetChild(0).gameObject.GetComponent<RtlText>().text = "عزیزم برای دیدن ادامه فیلم از مامان یا بابا بخواه تا بازی رو برات فعال کنند";
        }
        else
        {
            this.gameObject.GetComponent<AudioSource>().clip = sounds[1];
            boxLock.transform.GetChild(0).gameObject.GetComponent<RtlText>().text = "عزیزم برای  ادامه بازی از مامان یا بابا بخواه تا بازی رو برات فعال کنند";
        }


        StartCoroutine(makeChange());

    }
    bool canPlay=false;
    IEnumerator makeChange()
    {

        boxLock.SetActive(true);
        DestOrDisable();
        yield return new WaitUntil(() => canPlay);
        canPlay = false;
        this.gameObject.GetComponent<AudioSource>().Play();
    }
    void GoForBuy()
    {
        boxLock.SetActive(false);
        UnityEngine.PlayerPrefs.SetInt("comeFromAlphabet", 1);
        SceneManager.LoadScene("Main Menu");
    }
    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        hasSceneChanged = true;
    }
    private void OnDestroy()
    {
        // Always clean up your listeners when not needed anymore
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    // Listener for sceneLoaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (hasSceneChanged)
        {
            Debug.Log("Scene has changed!");
            Debug.Log("Scene has changed to: " + scene.name);
            currentSceneName = scene.name;
            hasSceneChanged = false;
            StartCoroutine(makeChanges());
        }
    }
    IEnumerator makeChanges()
    {
        yield return new WaitForSeconds(0.1f);
        m_canvas.worldCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        string sceneName = SceneManager.GetActiveScene().name;
        Debug.Log(sceneName);

        Dictionary<string, bool> sceneColliderDict = new Dictionary<string, bool>()
    {
        { "Main Menu", true },
      
        { "leader board", true },
        { "slider", true }
    };

        if (sceneColliderDict.ContainsKey(sceneName) && sceneColliderDict[sceneName] || UnityEngine.PlayerPrefs.GetInt("status") > 0)
        {
            m_button.gameObject.SetActive(false);
        }
        else
        {

            Canvas exitCanvas = GameObject.Find("Back Button")?.GetComponent<Canvas>();
            if (exitCanvas != null)
            {
                if (exitCanvas.GetComponent<GraphicRaycaster>() == null)
                {
                    exitCanvas.gameObject.AddComponent<GraphicRaycaster>();
                }
                exitCanvas.overrideSorting = true;
                exitCanvas.sortingOrder = 2100;
            }

            Canvas reloadCanvas = GameObject.Find("home")?.GetComponent<Canvas>();
            if (reloadCanvas != null)
            {
                if (reloadCanvas.GetComponent<GraphicRaycaster>() == null)
                {
                    reloadCanvas.gameObject.AddComponent<GraphicRaycaster>();
                }
                reloadCanvas.overrideSorting = true;
                reloadCanvas.sortingOrder = 2100;
            }

            BoxCollider2D[] boxColliders = FindObjectsOfType<BoxCollider2D>();

            foreach (BoxCollider2D collider in boxColliders)
            {
                if (collider.enabled && collider.name !="exit")
                {
                    collider.enabled = false;
                }
            }

            m_button.gameObject.SetActive(true);
        }
    }

    void DestOrDisable()
    {
        BoxCollider2D[] boxColliders = FindObjectsOfType<BoxCollider2D>();

        foreach (BoxCollider2D collider in boxColliders)
        {
            if (collider.enabled)
            {
                collider.enabled = false;
            }
        }
       
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();

        foreach (AudioSource audioSource in audioSources)
        {
            if(audioSource.gameObject.tag !="Respawn")
               audioSource.Stop();
        }
      
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("(Clone)"))
            {
                Destroy(obj);
            }
        }

        canPlay = true;
    }


}
