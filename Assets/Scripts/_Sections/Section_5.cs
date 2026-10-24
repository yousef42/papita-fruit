using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using System;
using Spine.Unity;
using UnityEngine.UI;


public class Section_5 : MonoBehaviour, ISection
{
    [SerializeField] bool isTutorialScene = false;
    [SerializeField] SkeletonAnimation skelAnim;
    [SerializeField] GameObject blockTouchScreen;
    [SerializeField] Button tutButton;
    bool tutorialIsInPlaying;

    [Space(20)]
    [SerializeField] List<Transform> startPoints = new List<Transform>();
    [SerializeField] float yForce = 15;
    [SerializeField] Transform topBubbleTransform;
    [SerializeField] TrailRenderer trail;

    [SerializeField, Range(0.2f, 1f)] float topShapeScale = 0.7f;
    [SerializeField, Range(0.2f, 1f)] float throwingShapeScale = 0.4f;
    [SerializeField, Range(0.2f, 2f)] float timeBetweenShapeCreation = 1f;
    [SerializeField] int incorrectCount = 12;
    [SerializeField] int correctCount = 8;
    [SerializeField] ParticleSystem confettiExplosion;
    [SerializeField] ParticleSystem linearConfetti;
    [SerializeField] Transform[] effectPositions;
    [SerializeField] Transform topPosition4LinearPaticles;


    List<Transform> tempTransformList = new List<Transform>();
    List<Fruit> createdObjectInScene = new List<Fruit>();
    List<Fruit> throwingShapes = new List<Fruit>();


    bool pressed = false;
    bool winned = false;
    bool gameStarted;
    Fruit fruit;
    int throwedShapesCount;
    public Action onSectionCompleted { get; set; }


    [Space(10)]
    [Header("--- Sounds --------------")]
    [SerializeField] AudioClip correctTouchedSound;
    [SerializeField] AudioClip incorrectTouchedSound;
    [SerializeField] AudioClip effectExplosionSound;
    [SerializeField] AudioClip winSound;


    [Header("-----------------")]
    [SerializeField] List<Fruit> differentThrowingShapes = new List<Fruit>();


    public void InitilizeSection(FruitType fruitType)
    {
        this.fruit = fruitType.GetFruit();
    }

    public void EnableSection(Action onSectionComplete)
    {
        this.onSectionCompleted = onSectionComplete;
        if(isTutorialScene) HideNinjaTutorial(); //اگه سین آموزش نباشه این اجسام داخلش نیستند یعنی اجسام داخل این تابع فقط تو سین آموزش هستند

        TouchController.Instance.onPressed += OnPressed;
        TouchController.Instance.onUpdated += OnUpdated;
        TouchController.Instance.onReleased += OnReleased;

        StartGame();
    }

    public void DisableSection()
    {
        TouchController.Instance.onPressed -= OnPressed;
        TouchController.Instance.onUpdated -= OnUpdated;
        TouchController.Instance.onReleased -= OnReleased;
    }

    void StartGame() => StartCoroutine(StartGame_Co());
    IEnumerator StartGame_Co()
    {
        if (isTutorialScene) 
        {
            WaitAndDo(() => PlayNinjaTutorial(), .4f);
            tutorialIsInPlaying = true;
            yield return new WaitUntil(() => !tutorialIsInPlaying);
        }

        StartCoroutine(SetShapeInTopBubble(() =>
        {
            StartCoroutine(StartNewWave(null));

            trail.gameObject.SetActive(false);
            trail.emitting = false;
            gameStarted = true;

            InvokeRepeating("CheckGame", 0.1f, 1f);
        }));
    }

    IEnumerator SetShapeInTopBubble(Action onCompleted)
    {
        float timeToShowShape = 0.6f;

        Fruit curShape = Instantiate(fruit, topBubbleTransform.position, Quaternion.identity);
        curShape.transform.localScale = Vector2.zero;
        curShape.InitializeForNinja();

        Destroy(curShape.transform.GetComponent<Rigidbody2D>());
        Destroy(curShape.transform.GetComponent<Collider2D>());

        bool showCompleted = false;
        curShape.transform.DOScale(Vector3.one * topShapeScale, timeToShowShape).SetEase(Ease.OutBack).OnComplete(() =>
        {
            showCompleted = true;
        });
        yield return new WaitUntil(() => showCompleted);
        yield return new WaitForSeconds(0.2f);
        onCompleted?.Invoke();
    }
       
    IEnumerator StartNewWave(Action onComplete)
    {
        for (int i = 0; i < incorrectCount; i++)
        {
            throwingShapes.Add(differentThrowingShapes[UnityEngine.Random.Range(0, differentThrowingShapes.Count)]);
        }
        Fruit correctFruit = fruit.fruitType.GetFruit();
        for (int i = 0; i < correctCount; i++)
        {
            throwingShapes.Add(correctFruit);
        }

        throwingShapes = throwingShapes.MixMe();

        for (int i = 0; i < throwingShapes.Count; i++)
        {
            Vector2 startPoint = GetRandomTransform().position;
            int rndIndex = UnityEngine.Random.Range(0, throwingShapes.Count);
            Fruit shGo = Instantiate(throwingShapes[rndIndex], startPoint, Quaternion.Euler(0, 0, UnityEngine.Random.Range(-10, 10) * 4));
            shGo.transform.localScale *= throwingShapeScale;
            createdObjectInScene.Add(shGo);
            shGo.InitializeForNinja();
            shGo.ShootShape(GetUpPointDirection(startPoint) * yForce);
            throwedShapesCount++;

            yield return new WaitForSeconds(timeBetweenShapeCreation);
        }
        yield return new WaitUntil(() => createdObjectInScene.Count == 0);//وایسا تا همه ی آبجکتهای این سین از بین  برن
        onComplete?.Invoke();
    }

    public void OnPressed(Vector2 position)
    {
        if (winned) return;

        if (isTutorialScene && tutorialIsInPlaying)
        {
            return;
        }

        pressed = true;

        trail.transform.position = position.GetWorldPosition();
        trail.gameObject.SetActive(true);
        trail.emitting = true;
    }

    public void OnReleased(Vector2 position)
    {
        if (winned) return;
        pressed = false;
        trail.Clear();
        trail.gameObject.SetActive(false);
        trail.emitting = false;
    }

    public void OnUpdated(Vector2 position)
    {
        if (winned) return;
        if (!pressed) return;

        Vector2 worldPosition = position.GetWorldPosition();
        RaycastHit2D hit = Physics2D.CircleCast(worldPosition, 0.5f, -Vector3.forward, 100, LayerMask.GetMask("Fruit"));

        if (hit.collider != null)
        {
            Fruit touchedShape = hit.collider.gameObject.GetComponent<Fruit>();
            bool isCorrectShape = touchedShape.fruitType == fruit.fruitType ? true : false;

            touchedShape.ShapeTouched(isCorrectShape, correctTouchedSound, incorrectTouchedSound, null);
        }

        trail.transform.position = worldPosition;
    }

    Transform GetRandomTransform()
    {
        if (tempTransformList.Count < 2)
        {
            tempTransformList.Clear();
            tempTransformList = startPoints.Select(x => x).ToList();
        }

        Transform randTr = tempTransformList[UnityEngine.Random.Range(0, tempTransformList.Count)];
        tempTransformList.Remove(randTr);

        return randTr;
    }

    //راستای پرتاب جسم
    Vector2 GetUpPointDirection(Vector2 startPoint)
    {
        //float xSign = Mathf.Sign(startPoint.x);
        Vector2 point = new Vector2(Mathf.Clamp(startPoint.x + UnityEngine.Random.Range(-4, 4), -7, 7), 0);
        return (point - startPoint).normalized;
    }

    void Update()
    {
        if (!gameStarted || winned) return;
        
        if (createdObjectInScene.Count > 0)
        {
            foreach (var item in createdObjectInScene.ToList())
            {
                if (item.transform.position.y < -11)
                {
                    createdObjectInScene.Remove(item);
                    Destroy(item.gameObject);
                }
            }
        }
    }


    //InvokeRepeating
    void CheckGame()
    {
        if (winned) return;
        if (throwedShapesCount >= throwingShapes.Count && createdObjectInScene.Count == 0)
        {
            winned = true;

            StartCoroutine(WinTheGame());
        }
    }

    IEnumerator WinTheGame()
    {
        ParticleSystem linearParticles = Instantiate(linearConfetti, topPosition4LinearPaticles.transform.position, Quaternion.identity, topPosition4LinearPaticles.parent) as ParticleSystem;
        linearParticles.Play(true);
        print("1");
        AudioController.instance.PlaySound(winSound, 0);

        float winSoundDuration = (winSound != null) ? winSound.length : 2;//اگه صدای شادی نبود زمان توقف رو دو ثانیه فرض کن

        foreach (var item in effectPositions)
        {
            ParticleSystem partSys = Instantiate(confettiExplosion, item.transform.position, Quaternion.identity, item.parent) as ParticleSystem;
            partSys.Play(true);
            print("2");
            AudioController.instance.PlayOneShot(effectExplosionSound, item.transform.position);
            yield return new WaitForSeconds(0.4f);
            winSoundDuration -= 0.4f;
        }

        if (winSoundDuration > 0)
            yield return new WaitForSeconds(winSoundDuration);

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

    public void PlayClickSound()
    {
        AudioController.instance.PlaySound(SoundType.Click);
    }

    void WaitAndDo(Action action, float delay = 1) => StartCoroutine(WaitAndDo_Co(action, delay));
    IEnumerator WaitAndDo_Co(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }


    ///////////-- tutorial --//////////////////
    void HideNinjaTutorial()
    {
        tutorialIsInPlaying = false;
        skelAnim.gameObject.SetActive(false);
        blockTouchScreen.SetActive(false);
        tutButton.gameObject.SetActive(false);
    }
    void PlayNinjaTutorial()
    {
        blockTouchScreen.SetActive(true);
        skelAnim.gameObject.SetActive(true);
        WaitAndDo(() => { 
            tutButton.onClick.RemoveAllListeners();
            tutButton.onClick.AddListener(() => { HideNinjaTutorial(); });
            tutButton.gameObject.SetActive(true);
            tutButton.transform.DOPunchScale(Vector3.right * .6f, 0.4f, 1, 1);
        }, 4);
    }
    ///////////////////////////////////////////
}