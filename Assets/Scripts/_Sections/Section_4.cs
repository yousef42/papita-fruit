using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using DG.Tweening;
using Spine.Unity;


public class Section_4 : MonoBehaviour, ISection
{
    [SerializeField] bool isTutorialScene = false;
    [SerializeField] List<TutorialData_4> tutorials = new List<TutorialData_4>();
    [SerializeField] SkeletonAnimation handHandler;
    bool tutorialFinnished = false;

    [Space(20)]
    [SerializeField] GameObject flatMap;
    [Range(0.1f, 2f), SerializeField] float minDistanceToAutoPlacement = 0.7f;
    [Range(0.1f, 1.4f)] [SerializeField] float durationToSetInIncorrectPositions = 0.7f;

    [Space(20)]
    [Header("--- Sounds --------------")]
    [SerializeField] AudioClip incorrectDroppedSound;
    [SerializeField] AudioClip winSound;



    [Space(20), Header("--------------")]
    [Space]
    public List<WordAndWordMap> wordAndWordMap = new List<WordAndWordMap>();
    [Space]
    [ReadOnly] public List<Vector2> correctPositions;
    [ReadOnly] public List<Vector2> incorrectPositions;

    List<WordPositionMatch> wordPositionMatchList = new List<WordPositionMatch>();

    GameObject selectedWordGameObject;

    AudioClip EnglishNameSound;
    AudioClip PersianNameSound;
    public Action onSectionCompleted { get; set; }

    public void EnableSection(Action onSectionComplete)
    {
        this.onSectionCompleted = onSectionComplete;

        if(isTutorialScene) HideTutorialTools();

        flatMap.SetActive(true);
        StartCoroutine(EnableSection_Co());
    }

    IEnumerator EnableSection_Co()
    {
        for (int i = 0; i < wordAndWordMap.Count; i++)
        {
            wordAndWordMap[i].word.transform.position = correctPositions[i];
            if (!wordAndWordMap[i].word.gameObject.activeInHierarchy) wordAndWordMap[i].word.gameObject.SetActive(true);
            wordAndWordMap[i].word.InitializeWord();

            Word_Spine word = wordAndWordMap[i].word;
            wordAndWordMap[i].wordFlatMap.flatMapID = i;
            wordPositionMatchList.Add(new WordPositionMatch(word.wordType, i, wordAndWordMap[i].wordFlatMap, word.transform.position));
        }

        yield return new WaitForSeconds(1);
        int countWords = -1;
        for (int i = 0; i < wordAndWordMap.Count; i++)
        {
            wordAndWordMap[i].word.transform.DOMove(incorrectPositions[i], durationToSetInIncorrectPositions).SetEase(Ease.OutBack).OnComplete(() =>
            {
                countWords++;
            });
            wordAndWordMap[i].word.transform.DORotate(Vector3.forward * UnityEngine.Random.Range(-30, 30) * 2, durationToSetInIncorrectPositions / 2f);
        }

        yield return new WaitUntil(() => countWords == wordAndWordMap.Count - 1);
        for (int i = 0; i < wordAndWordMap.Count; i++)
        {
            wordAndWordMap[i].word.PlayAnimation(AnimationMode.End);
        }

        ///////////////--Tutorial--////////////////
        if (isTutorialScene) CheckTutorial();
        ///////////////////////////////////////////
        

        TouchController.Instance.onPressed += OnPressed;
        TouchController.Instance.onUpdated += OnUpdated;
        TouchController.Instance.onReleased += OnReleased;
    }

    public void InitializeSection(AudioClip EnglishNameSound, AudioClip PersianNameSound)
    {
        this.EnglishNameSound = EnglishNameSound;
        this.PersianNameSound = PersianNameSound;
    }

    public void DisableSection()
    {
        flatMap.SetActive(false);
        foreach (var item in wordAndWordMap)
        {
            if (item.word.gameObject.activeInHierarchy) item.word.gameObject.SetActive(false);
        }

        TouchController.Instance.onPressed -= OnPressed;
        TouchController.Instance.onUpdated -= OnUpdated;
        TouchController.Instance.onReleased -= OnReleased;
    }

    Vector2? shiftAmount;
    //bool objectIsInStartPoint = true;
    Vector3? startObjectPosition;

    public void OnPressed(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position.GetWorldPosition(), -Vector3.forward, 100, LayerMask.GetMask("Word_Spiny", "BlockTouch"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlockTouch")) return;

            if (!hit.collider.GetComponent<Word_Spine>().isInCorrectPosition)
            {
                selectedWordGameObject = hit.collider.gameObject;
                startObjectPosition = hit.collider.gameObject.transform.position;
                selectedWordGameObject.GetComponent<Word_Spine>().SetDragOptions();

                shiftAmount = hit.collider.transform.position.ConvertToVector2() - position.GetWorldPosition();
            }
        }
    }

    public void OnUpdated(Vector2 position)
    {
        if (selectedWordGameObject != null)
        {
            selectedWordGameObject.transform.position = position.GetWorldPosition() + shiftAmount.Value;
        }
    }

    public void OnReleased(Vector2 position)
    {
        if (selectedWordGameObject != null)
        {
            RaycastHit2D hit = Physics2D.Raycast(position.GetWorldPosition(), -Vector3.forward, 100, LayerMask.GetMask("Word_FlatMap"));

            if (hit.collider != null)
            {
                Word_FlatMap hwfm = hit.collider.gameObject.GetComponent<Word_FlatMap>();

                if (hwfm.wordType == selectedWordGameObject.GetComponent<Word_Spine>().wordType && !hwfm.correctWordLocated)
                {
                    hwfm.correctWordLocated = true;
                    selectedWordGameObject.transform.position = wordPositionMatchList[hwfm.flatMapID].wordPosition;

                    selectedWordGameObject.GetComponent<Word_Spine>().SetMode(true);
                    
                    selectedWordGameObject.GetComponent<Word_Spine>().isLocated = true;//برا آموزش میخواهیمش

                    selectedWordGameObject.transform.rotation = Quaternion.identity;
                    selectedWordGameObject.GetComponent<Word_Spine>().SetReleaseOptions(true);
                    Collider2D collider = selectedWordGameObject.GetComponent<Collider2D>();

                    if (collider != null)
                    {
                        Destroy(collider);
                    }
                }
                else
                {
                    selectedWordGameObject.GetComponent<Word_Spine>().SetReleaseOptions(false);
                    if(isTutorialScene) selectedWordGameObject.transform.position = startObjectPosition.Value;//اگه آموزش بود برگردانش سرجاش تا بره تو کادر

                    AudioController.instance.PlayOneShot(incorrectDroppedSound, position.GetWorldPosition());

                    startObjectPosition = null;
                }
            }
            else
            {
                selectedWordGameObject.GetComponent<Word_Spine>().SetReleaseOptions(false);
                if (isTutorialScene) selectedWordGameObject.transform.position = startObjectPosition.Value;//اگه آموزش بود برگردانش سرجاش تا بره تو کادر

                AudioController.instance.PlayOneShot(incorrectDroppedSound, position.GetWorldPosition());

                startObjectPosition = null;
            }
            
            
            ///////////////--Tutorial--////////////////
            if (isTutorialScene) CheckTutorial();
            ///////////////////////////////////////////
            

            selectedWordGameObject = null;
            shiftAmount = null;
            CheckWords();
        }
    }

    bool winned = false;
    void CheckWords()
    {
        if (winned) return;
        if (!wordAndWordMap.Any(x => !x.word.isInCorrectPosition))
        {
            winned = true;
            WinSection();
        }
    }

    void WinSection()
    {
        StartCoroutine(WinSection_Co());
    }

    IEnumerator WinSection_Co()
    {
        AudioController.instance.PlaySound(winSound, 1);

        flatMap.SetActive(false);
        foreach (var item in wordAndWordMap)
        {
            item.word.PlayAnimation(AnimationMode.Play, true);
        }
        //float effectDuration = GamePlayManager.instance.PlayParticlesEffect();
        yield return new WaitForSeconds(1.6f);
        AudioController.instance.PlaySound(EnglishNameSound, 0);
        yield return new WaitForSeconds(1.6f);
        AudioController.instance.PlaySound(PersianNameSound, 0);

        yield return new WaitForSeconds(3f);

        //yield return new WaitForSeconds(effectDuration);
        //GamePlayManager.instance.DestroyParticleEffect();
        onSectionCompleted?.Invoke();

        int coin = UnityEngine.PlayerPrefs.GetInt("coins");
        if (coin < 99999)
        {
            int newcoin = coin + 3;
            UnityEngine.PlayerPrefs.SetInt("coins", newcoin);
            Debug.Log(newcoin);
            // GameObject.Find("mycoin").GetComponent<coinController>().updatecoin();
        }
    }


    ///////////////--Tutorial Section--////////////////////

    Coroutine progressingTutorial;
    int currentTutIndex = 0;

    void HideTutorialTools()
    {
        foreach (var item in tutorials)
        {
            item.blockScreen.blockScreen.SetActive(false);
        }
        handHandler.gameObject.SetActive(false);
    }

    void CheckTutorial() => StartCoroutine(CheckTutorial_Co());
    IEnumerator CheckTutorial_Co()
    {
        yield return new WaitUntil(() => selectedWordGameObject == null);

        TutorialData_4 tut = tutorials.FirstOrDefault(x => !x.wordSpine.isLocated);
        if (tut != null)
        {
            if (progressingTutorial != null) StopCoroutine(progressingTutorial);
            progressingTutorial = StartCoroutine(ShowIndicatorAnimation(tut));
        }
        else
        {
            tutorialFinnished = true;
        }
    }

    IEnumerator ShowIndicatorAnimation(TutorialData_4 tutorial)
    {
        handHandler.transform.DOKill();
        tutorial.blockScreen.blockScreen.gameObject.SetActive(true);
        handHandler.transform.position = tutorial.blockScreen.startPoint.position;
        handHandler.gameObject.SetActive(true);
        handHandler.AnimationState.SetAnimation(0, "idle", true);
        Tween tween = handHandler.transform.DOMove(tutorial.blockScreen.endPoint.position, 1.4f).SetAutoKill(false).
                              OnStepComplete(() => {
                                  handHandler.transform.position = tutorial.blockScreen.startPoint.position;
                              }).SetLoops(-1);

        yield return new WaitUntil(() => selectedWordGameObject != null);
        tween.Kill();
        handHandler.gameObject.SetActive(false);
        tutorial.blockScreen.blockScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
}

[Serializable]
public class WordPositionMatch
{
    public WordType wordType;
    public int wordMapID;
    public Word_FlatMap wordFlatMap;
    public Vector2 wordPosition;

    public WordPositionMatch(WordType wordType, int wordMapID, Word_FlatMap wordMap, Vector2 wordPosition)
    {
        this.wordType = wordType;
        this.wordMapID = wordMapID;
        this.wordFlatMap = wordMap;
        this.wordPosition = wordPosition;
    }
}

[Serializable]
public class WordAndWordMap
{
    public Word_Spine word;
    public Word_FlatMap wordFlatMap;
}


[Serializable]
class TutorialData_4
{
    public Word_Spine wordSpine;
    public BlockScreen blockScreen;
}