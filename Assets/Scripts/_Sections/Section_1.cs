using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Spine.Unity;

public class Section_1 : MonoBehaviour, ISection
{
    [SerializeField] bool isTutorialScene = false;
    [SerializeField] List<TutorialData_1> tutorials = new List<TutorialData_1>();
    [SerializeField] SkeletonAnimation handHandler;
    bool tutorialFinnished = false;

    [Space]
    [Header("--- Sounds ---------")]
    [SerializeField] AudioClip winSound;


    [Space]
    [Header("---- List ----------")]
    [SerializeField] List<WordMatch> wordMatchList = new List<WordMatch>();


    public Action onSectionCompleted { get; set; }
    Fruit dirtyFruit;
    AudioClip englishNameSound;
    AudioClip persianNameSound;



    public void DisableSection()
    {
        TouchController.Instance.onPressed -= OnPressed;
        TouchController.Instance.onUpdated -= OnUpdated;
        TouchController.Instance.onReleased -= OnReleased;
    }

    public void EnableSection(Action onSectionComplete)
    {
        this.onSectionCompleted = onSectionComplete;

        for (int i = 0; i < wordMatchList.Count; i++)
        {
            wordMatchList[i].word.wordID = i;
            wordMatchList[i].word.gameObject.SetActive(false);
            wordMatchList[i].flatMap.flatMapID = i;
        }

        if (isTutorialScene) StartCoroutine(PlayTutorial());

        TouchController.Instance.onPressed += OnPressed;
        TouchController.Instance.onUpdated += OnUpdated;
        TouchController.Instance.onReleased += OnReleased;
    }

    public void SeDirtytFruit(Fruit dirtyFruit, AudioClip englishNameSound, AudioClip persianNameSound)
    {
        this.dirtyFruit = dirtyFruit;
        this.dirtyFruit.InitializeFruit();

        this.englishNameSound = englishNameSound;
        this.persianNameSound = persianNameSound;
    }

    public void OnPressed(Vector2 position)
    {
        RaycastHit2D hit = Physics2D.Raycast(position.GetWorldPosition(), -Vector3.forward, 100, LayerMask.GetMask("Word_Cloudy", "BlockTouch"));
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlockTouch")) return;
            Word_Cloudy cloudyWord = hit.collider.GetComponent<Word_Cloudy>();
            IEnumerable<WordMatch> prClassLi = wordMatchList.Where(x => !x.word.isSolved && x.word.wordType == cloudyWord.wordType);
            if (prClassLi.Count() > 0)
            {
                StartCoroutine(ObjectMatched(cloudyWord, prClassLi.ElementAt(0).word,
                                            wordMatchList[prClassLi.ElementAt(0).word.wordID].flatMap));
            }
            else print("Not available");
        }
    }

    public void OnReleased(Vector2 position)
    {

    }

    public void OnUpdated(Vector2 position)
    {

    }

    IEnumerator ObjectMatched(Word_Cloudy wordCloudy, Word_Flat word, Word_FlatMap flatMap)
    {
        wordCloudy.transform.DOMoveY(wordCloudy.transform.position.y + 16, 1).SetEase(Ease.InBack).OnComplete(() => wordCloudy.gameObject.SetActive(false));
        wordCloudy.transform.DORotate(Vector3.up * 360, 0.8f, RotateMode.LocalAxisAdd);

        AudioController.instance.PlaySound(word.GetWordAudio(), 0);

        yield return new WaitForSeconds(0.4f);

        Vector3 wordScale = word.transform.localScale;
        word.transform.localScale = wordScale * 2f;
        word.gameObject.SetActive(true);
        word.SetMode(true);
        word.transform.DOScale(wordScale, 0.7f).SetEase(Ease.OutCubic).OnComplete(() => 
        {
            flatMap.gameObject.SetActive(false);
            word.located = true;
        });

        CheckList();
    }

    void CheckList()
    {
        if (wordMatchList.Any(c => !c.word.isSolved))
        {
            return;
        }
        else
        {
            StartCoroutine(CompeleteSection());
        }
    }

    IEnumerator CompeleteSection()
    {
        if (isTutorialScene) yield return new WaitUntil(() => tutorialFinnished == true);//وایسا تا آموزش تموم بشه

        float delay = 0.1f;
        yield return new WaitForSeconds(0.6f);


        AudioController.instance.PlaySound(winSound, 0);

        bool effectComplete = false;
        dirtyFruit.ChangeBlack2Dirty(() => effectComplete = true);
        yield return new WaitUntil(() => effectComplete);

        WaitAndDo(() => AudioController.instance.PlaySound(englishNameSound, 1), 1);

        yield return new WaitForSeconds(2.4f);

        foreach (var item in wordMatchList)
        {
            item.word.transform.DOMoveY(item.word.transform.position.y - 16, 0.6f).SetEase(Ease.InBack);
            yield return new WaitForSeconds(delay);
        }

        yield return new WaitForSeconds(wordMatchList.Count * delay + 1f);
        onSectionCompleted?.Invoke();
       int coin = UnityEngine.PlayerPrefs.GetInt("coins");
        if (coin < 99999)
        {
            int newcoin = coin + 4;
            UnityEngine.PlayerPrefs.SetInt("coins", newcoin);
            Debug.Log(newcoin);
            // GameObject.Find("mycoin").GetComponent<coinController>().updatecoin();
        }
    }

    void WaitAndDo(Action action, float delay = 1) => StartCoroutine(WaitAndDo_Co(action, delay));
    IEnumerator WaitAndDo_Co(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();

    }



    /////////////--Tutorial Section--////////////////
    
    IEnumerator PlayTutorial()
    {
        for (int i = 0; i < tutorials.Count; i++)
        {
            yield return StartCoroutine(ShowIndicatorAnimation(tutorials[i]));
        }
        tutorialFinnished = true;
    }

    void HideTutorialTools()
    {
        foreach (var item in tutorials)
        {
            item.blockScreen.blockScreen.SetActive(false);
        }
        handHandler.gameObject.SetActive(false);
    }
    IEnumerator ShowIndicatorAnimation(TutorialData_1 tutorial)
    {
        tutorial.blockScreen.blockScreen.gameObject.SetActive(true);
        handHandler.transform.position = tutorial.blockScreen.startPoint.position;
        handHandler.gameObject.SetActive(true);
        handHandler.AnimationState.SetAnimation(0, "play", true);
        yield return new WaitUntil(() => tutorial.wordFlat.isSolved == true);
        handHandler.gameObject.SetActive(false);
        yield return new WaitUntil(() => tutorial.wordFlat.located == true);
        yield return new WaitForSeconds(0.4f);
        tutorial.blockScreen.blockScreen.gameObject.SetActive(false);
    }
}

[Serializable]
public class WordPosition
{
    public WordType wordType;
    public int wordMapID;
    public Word_FlatMap wordFlatMap;

    public WordPosition(WordType wordType, int wordMapID, Word_FlatMap wordFlatMap)
    {
        this.wordType = wordType;
        this.wordMapID = wordMapID;
        this.wordFlatMap = wordFlatMap;
    }
}

[Serializable]
public class WordMatch
{
    public Word_Flat word;
    public Word_FlatMap flatMap;
}

[Serializable]
class TutorialData_1
{
    public Word_Flat wordFlat;
    public BlockScreen blockScreen;
}