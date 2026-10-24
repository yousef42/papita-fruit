using System.Collections;
using UnityEngine;
using Spine.Unity;
using UnityEngine.SceneManagement;
using System;
using Spine;
using UnityEngine.UI;

public class IntroScene : MonoBehaviour
{
    [SerializeField] SkeletonGraphic skeletonAnim;
    [SerializeField] SkeletonGraphic playButtonSkeleton;
    Button playButton;

    IEnumerator Start()
    {
        playButton = playButtonSkeleton.gameObject.transform.GetComponent<Button>();
        playButtonSkeleton.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(ShowIntroAnim(null));

        AudioController.instance.PlayBGMusic(0);
    }

    IEnumerator ShowIntroAnim(Action onCompleteIntroAnim)
    {
        skeletonAnim.AnimationState.ClearTracks();
        TrackEntry introAnim = skeletonAnim.AnimationState.SetAnimation(0, "play", false);
        yield return new WaitForSpineAnimationComplete(introAnim);
        playButtonSkeleton.gameObject.SetActive(true);
        TrackEntry playButtonAnim = playButtonSkeleton.AnimationState.SetAnimation(0, "play", false);
        playButtonSkeleton.AnimationState.AddAnimation(0, "play2", true, 0);
        yield return new WaitForSeconds(0.5f);
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() => StartCoroutine(LoadMainMenuScene()));
        onCompleteIntroAnim?.Invoke();
    }

    IEnumerator LoadMainMenuScene()
    {

        if (!GameController.Instance.IsTutorialShowed())
        {
            GoToTutorialScene();
            yield break;
        }

        AsyncOperation async = SceneManager.LoadSceneAsync("Main Menu");
        async.allowSceneActivation = false;

        yield return new WaitUntil(() => async.progress >= 0.9f);
        async.allowSceneActivation = true;
    }

    public void GoToTutorialScene()
    {
        GameController.Instance.GoToCurrentScene("TutScene");
    }
}
