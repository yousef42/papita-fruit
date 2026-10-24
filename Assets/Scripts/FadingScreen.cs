using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using DG.Tweening;

public class FadingScreen : MonoBehaviour
{
    public static FadingScreen instance;

    [SerializeField] GameObject fadingCanvasGameObject;
    [SerializeField] Image fadingScreen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(this.gameObject);

        fadingCanvasGameObject.SetActive(false);
        fadingScreen.gameObject.SetActive(false);
    }

    public void LoadFadingScreen(Action onShowComplete, Action onHideComplete = null, float delay = 0)
    {
        fadingCanvasGameObject.SetActive(true);
        Color color = fadingScreen.color;
        fadingScreen.color = new Color(color.r, color.g, color.b, 0);
        if (!fadingScreen.gameObject.activeInHierarchy) fadingScreen.gameObject.SetActive(true);
        fadingScreen.DOColor(new Color(color.r, color.g, color.b, 1), 0.5f).OnComplete(() =>
        {
            onShowComplete?.Invoke();
            fadingScreen.DOColor(new Color(color.r, color.g, color.b, 0), 0.5f).SetDelay(delay).OnComplete(() =>
            {
                fadingCanvasGameObject.gameObject.SetActive(false);
                onHideComplete?.Invoke();
            });
        });
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            //LoadFadingScreen(null);
        }
    }
}
