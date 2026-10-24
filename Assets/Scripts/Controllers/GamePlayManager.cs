using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public GamePLayMode gamePlayMode = GamePLayMode.Sequential;

    [SerializeField] Section_1 section_1;
    [SerializeField] Section_2 section_2;
    [SerializeField] Section_3 section_3;
    [SerializeField] Section_4 section_4;
    [SerializeField] Section_5 section_5;

    [Space]
    [SerializeField] GameObject section_1_RootGO;
    [SerializeField] GameObject section_2_RootGO;
    [SerializeField] GameObject section_3_RootGO;
    [SerializeField] GameObject section_4_RootGO;
    [SerializeField] GameObject section_5_RootGO;
    [SerializeField] GameObject background;

    [Header("-----------------"), Space]
    [SerializeField] Fruit dirtyFuit;
    [SerializeField] AudioClip englishNameSound;
    [SerializeField] AudioClip persianNameSound;

    [Space]
    [SerializeField] bool isTutorialScene;
    [SerializeField] GameObject backButton;

    private void Start()
    {
        section_1.DisableSection();
        section_2.DisableSection();
        section_3.DisableSection();
        section_4.DisableSection();
        section_5.DisableSection();
        dirtyFuit.gameObject.SetActive(true);

        if (isTutorialScene) backButton.gameObject.SetActive(false);

#if UNITY_EDITOR
        switch (gamePlayMode)
        {
            case GamePLayMode.Section_1:
                section_1.EnableSection(() => print("Section 1 Winned."));
                section_1.SeDirtytFruit(dirtyFuit, englishNameSound, persianNameSound);
                break;
            case GamePLayMode.Section_2:
                section_2.EnableSection(() => print("Section 2 Winned."));
                section_2.SeDirtytFruitGameObject(dirtyFuit);
                break;
            case GamePLayMode.Section_3:
                section_3.SetFruit(dirtyFuit, background);
                section_3.EnableSection(() => print("Section 3 Winned."));
                break;
            case GamePLayMode.Section_4:
                dirtyFuit.gameObject.SetActive(false);
                section_4.EnableSection(() => print("Section 4 Winned."));
                section_4.InitializeSection(englishNameSound, persianNameSound);
                break;
            case GamePLayMode.Section_5:
                dirtyFuit.gameObject.SetActive(false);
                section_5.InitilizeSection(dirtyFuit.fruitType);
                section_5.EnableSection(() => print("Section 5 Winned."));
                break;
            case GamePLayMode.Sequential:

#endif
                section_1.EnableSection(() =>
                {
                    section_1.DisableSection();
                    section_1_RootGO.SetActive(false);

                    section_2_RootGO.SetActive(true);
                    section_2.EnableSection(() =>
                    {
                        section_2.DisableSection();
                        section_2_RootGO.SetActive(false);

                        section_3_RootGO.SetActive(true);
                        section_3.SetFruit(dirtyFuit, background);
                        section_3.EnableSection(()=> {
                            section_3.DisableSection();
                            section_3_RootGO.SetActive(false);

                            section_4_RootGO.SetActive(true);
                            dirtyFuit.gameObject.SetActive(false);
                            section_4.EnableSection(()=> {
                                section_4.DisableSection();
                                section_4_RootGO.SetActive(false);

                                section_5_RootGO.SetActive(true);
                                dirtyFuit.gameObject.SetActive(false);
                                section_5.InitilizeSection(dirtyFuit.fruitType);
                                section_5.EnableSection(() => 
                                {
                                    GameController.Instance.GoToMainMenuScene();
                                    if (isTutorialScene) GameController.Instance.TutorialShowed();
                                });
                            });
                            section_4.InitializeSection(englishNameSound, persianNameSound);
                        });
                    });
                    section_2.SeDirtytFruitGameObject(dirtyFuit);
                });
                section_1.SeDirtytFruit(dirtyFuit, englishNameSound, persianNameSound);
                if (!background.activeInHierarchy) background.SetActive(true);

#if UNITY_EDITOR
                break;
        }
#endif


        AudioController.instance.PlayBGMusic(0);
    }

    GamePLayMode? previousMode;
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (Application.isPlaying && EditorApplication.isPlaying) return;

        if (previousMode == null)
        {
            previousMode = gamePlayMode;
        }
        else
        {
            if (gamePlayMode == previousMode.Value) return;
            previousMode = gamePlayMode;
        }

        section_1_RootGO.SetActive(false);
        section_2_RootGO.SetActive(false);
        section_3_RootGO.SetActive(false);
        section_4_RootGO.SetActive(false);
        section_5_RootGO.SetActive(false);
        dirtyFuit.gameObject.SetActive(true);

        switch (gamePlayMode)
        {
            case GamePLayMode.Sequential:
            case GamePLayMode.Section_1:
                section_1_RootGO.SetActive(true);
                break;
            case GamePLayMode.Section_2:
                section_2_RootGO.SetActive(true);
                break;
            case GamePLayMode.Section_3:
                section_3_RootGO.SetActive(true);
                break;
            case GamePLayMode.Section_4:
                section_4_RootGO.SetActive(true);
                dirtyFuit.gameObject.SetActive(false);
                break;
            case GamePLayMode.Section_5:
                section_5_RootGO.SetActive(true);
                dirtyFuit.gameObject.SetActive(false);
                break;
            default:
                break;
        }
#endif
    }

    public void GoToMainScene()
    {
        GameController.Instance.GoToMainMenuScene();
    }
}