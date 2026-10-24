using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] GameObject page_1;
    [SerializeField] GameObject page_2;
    [SerializeField] MenuScroll menuScroll;
    [SerializeField] MenuData menuDataFruits;
    [SerializeField] MenuData menuDataVegetables;

    [SerializeField] GameObject settingsPanel;
    [SerializeField] GameObject exitPanel;
    [SerializeField] ToggleController sfxToggleButton, bgxToggleButton;
    [SerializeField] Button exitPanel_YesButton, exitPanel_NoButton;

    bool canPressBackButton = true;
    Action onBackButton = delegate { };

    public Text mytext;
    void Start()
    {
        mytext.text = "SN" + UnityEngine.PlayerPrefs.GetString("clintid");
        //تو کدام منو بره
        string currentSelectedMenu = GameController.Instance.currentSelectedMenu;

        switch (currentSelectedMenu)
        {
            case "":
                SetPage_1();
                break;
            case "Fruits":
                OpenScroll_Fruits();
                break;
            case "Vegetables":
                OpenScroll_Vegetables();
                break;
            default:
                SetPage_1();
                break;
        }

        AudioController.instance.PlayBGMusic(0);
    }

    void SetPage_1()
    {
        settingsPanel.SetActive(false);
        exitPanel.SetActive(false);
        page_2.SetActive(false);
        page_1.SetActive(true);

        onBackButton = () => OpenExitPanel();
    }

    void SetMenuScroll(MenuData menuData)
    {
        page_1.SetActive(false);
        page_2.SetActive(true);
        menuScroll.InitializeMenu(menuData);

        onBackButton = () =>
        {
            canPressBackButton = false;

            FadingScreen.instance.LoadFadingScreen(() => SetPage_1(), () => canPressBackButton = true);
        };
    }

    public void OpenScroll_Fruits()
    {
        GameController.Instance.currentSelectedMenu = "Fruits";
        SetMenuScroll(menuDataFruits);
    }

    public void OpenScroll_Vegetables()
    {
        GameController.Instance.currentSelectedMenu = "Vegetables";
        SetMenuScroll(menuDataVegetables);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton();
        }

        //Debug--////////////////////////
        if (Input.GetKeyDown(KeyCode.S))
        {
            menuScroll.ResetMenu();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            menuScroll.InitializeMenu(menuDataFruits);
        }
    }

    public void BackButton()
    {
        if (canPressBackButton) onBackButton?.Invoke();
    }

    public void OpenSettingsPanel()
    {
        settingsPanel.SetActive(true);

        sfxToggleButton.Initialize(GameController.Instance.GetSFXMusic(), x => { GameController.Instance.SetSFXMusic(x); });
        bgxToggleButton.Initialize(GameController.Instance.GetBGMusic(), x => { GameController.Instance.SetBGMusic(x); });


        onBackButton = () =>
        {
            HideSettingsPanel();
        };
    }

    public void HideSettingsPanel()
    {
        settingsPanel.SetActive(false);

        onBackButton = () => OpenExitPanel();
    }

    public void OpenExitPanel()
    {
        exitPanel_YesButton.onClick.RemoveAllListeners();
        exitPanel_NoButton.onClick.RemoveAllListeners();
        exitPanel_YesButton.onClick.AddListener(() => Application.Quit());
        exitPanel_NoButton.onClick.AddListener(() => HideExitPanel());
        exitPanel.SetActive(true);

        onBackButton = () =>
        {
            HideExitPanel();
        };
    }

    public void HideExitPanel()
    {
        exitPanel.SetActive(false);

        onBackButton = () => OpenExitPanel();
    }

    public void PlayClickSound()
    {
        AudioController.instance.PlaySound(SoundType.Click);
    }

    public void GoToTutorialScene()
    {
        GameController.Instance.GoToCurrentScene("TutScene");
    }
}