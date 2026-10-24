using System.Collections;
using UnityEngine;
using System;
using Spine.Unity;
using UnityEngine.UI;
using DG.Tweening;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen instance;


    [SerializeField] SkeletonGraphic papitaAnimation;
    [SerializeField] GameObject loadingScreenGameObject;
    [SerializeField] Image barImage;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);

        HideLoadingScreen();
    }
    private void Start()
    {
    }

    public void ShowLoadingScreen(Action onComplete, Action onHideComplete = null, float delay = 2f)
    {
        StartCoroutine(ShowLoadingScreen_Co(onComplete, onHideComplete, delay));
    }

    IEnumerator ShowLoadingScreen_Co(Action onComplete, Action onHideComplete = null, float delay = 0f)
    {
        // پروگرسور رو تو حالت صفر بزار و بعد

        ShowLoadingScreen();

        float watingTime = 4f;

        papitaAnimation.AnimationState.ClearTracks();
        papitaAnimation.AnimationState.SetAnimation(0, "animation2", true);

        DOTween.To(() => barImage.fillAmount, x => barImage.fillAmount = x, 1, watingTime + delay);
        yield return new WaitForSeconds(watingTime / 2);
        onComplete?.Invoke();
        //print("loading Complete");
        yield return new WaitForSeconds(watingTime + delay);

        onHideComplete?.Invoke();

        yield return new WaitForSeconds(0.5f);
        HideLoadingScreen();
    }

    void ShowLoadingScreen()
    {
        if (!loadingScreenGameObject.activeInHierarchy)
            loadingScreenGameObject.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        barImage.fillAmount = 0;
        if (loadingScreenGameObject.activeInHierarchy)
            loadingScreenGameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            //ShowLoadingScreen(null);
        }
    }
}
