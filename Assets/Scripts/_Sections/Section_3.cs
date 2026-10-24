using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using DG.Tweening;
using System.Linq;
using Spine.Unity;


public class Section_3 : MonoBehaviour, ISection
{
    [SerializeField] bool isTutorialScene = false;
    [SerializeField] List<TutorialData_3> tutorials = new List<TutorialData_3>();
    [SerializeField] SkeletonAnimation handHandler;
    bool tutorialFinnished = false;
    [Header("-------------------")]


    [Space(10)]   
    [SerializeField] Sabad sabad;
    [SerializeField] Transform sabadPosition2CatchFruit;
    [SerializeField] Transform fruitNewPosition;
    [SerializeField] float backgroundNewYPos;
    [SerializeField] List<Transform> transformsList2SpawningFruits = new List<Transform>();
    [SerializeField] Transform section3Transform;
    [SerializeField] ParticleSystem confettiExplosion;
    [SerializeField] ParticleSystem linearConfetti;
    [SerializeField] Transform[] effectPositions;
    [SerializeField] Transform topPosition4LinearPaticles;

    [Header("----- Sounds ---------"),Space]
    [SerializeField] AudioClip spawninngAudio;
    [SerializeField] AudioClip draggedSound;
    [SerializeField] AudioClip correctDroppedSound;
    [SerializeField] AudioClip incorrectDroppedSound;
    [SerializeField] AudioClip winSound;
    [SerializeField] AudioClip effectExplosionSound;

    [Space(20)]
    [Header("ﺎﻫ ﻪﺸﻧ ﺏﺎﺨﺘﻧﺍ ﯽﻠﺻﺍ ﯼ ﻩﻮﯿﻣ .ﻦﮐ ﺏﺎﺨﺘﻧﺍ ﻭﺭ ﻥﻮﮔﺎﻧﻮﮔ ﯼ ﻩﻮﯿﻣ ﻉﻮﻧ ﺶﺷ ﺎﺗ ﺭﺎﻬﭼ ﺖﺴﯿﻟ ﻦﯾﺍ ﺭﺩ")]
            //در این لیست چهار تا شش نوع میوه ی گوناگون رو انتخاب کن. میوه ی اصلی انتخاب نشه ها
    [SerializeField] List<FruitType> OtherTypesFruitsToSpawnOnGround = new List<FruitType>();

    public Action onSectionCompleted { get; set; }
    Fruit currentFruit;
    GameObject background;
    Vector3 sabadStartPosition;


    List<Fruit> fruitsListOnGround = new List<Fruit>();
    bool gameStarted = false;
    bool canDrag = false;
    bool winned = false;



    public void SetFruit(Fruit fruit, GameObject background)
    {
        this.currentFruit = fruit;
        //currentFruit.InitializeFruit();
        this.background = background;
    }

    public void EnableSection(Action onSectionComplete)
    {
        this.onSectionCompleted= onSectionComplete;

        if(isTutorialScene) HideTutorialTools();

        foreach (var item in transformsList2SpawningFruits)
        {
            SpriteRenderer sp = item.GetComponent<SpriteRenderer>();
            if(sp != null) Destroy(sp);
        }

        if (OtherTypesFruitsToSpawnOnGround.Count < 3)
        {
            Debug.LogError("ﺶﺘﺴﻫ ﻢﮐ OtherFruits ﺖﺴﯿﻟ ﺭﺩ ﺩﻮﺟﻮﻣ ﯼﺎﻫ ﻩﻮﯿﻣ ﺭﺎﻤﺷ");
                         // کم هستش OtherFruits شمار میوه های موجود در لیست 
            return;
        }

        sabadStartPosition = sabad.transform.position;
        //sabadStartScale = sabad.transform.localScale;
        sabad.transform.position = sabadPosition2CatchFruit.position - Vector3.up * 10;
        sabad.gameObject.SetActive(true);

        currentFruit.gameObject.transform.DOScale(currentFruit.transform.localScale / 3f, 1f);
        currentFruit.gameObject.transform.DOMoveY(fruitNewPosition.position.y, 1f);

        background.transform.DOMoveY(backgroundNewYPos, 1f);

        sabad.transform.DOMove(sabadPosition2CatchFruit.position, 1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Vector3 newFruitPos = sabad.transform.TransformPoint(sabad.GetNextFruitTranform().position);
            currentFruit.transform.DOMove(newFruitPos, 0.44f).SetEase(Ease.InCirc).OnComplete(() =>
            {
                currentFruit.transform.SetParent(sabad.transform, true);
                sabad.transform.DOMove(sabadStartPosition, 0.8f).SetEase(Ease.InOutBack).OnComplete(() =>
                {
                    StartCoroutine(SpawnOtherFruits());
                });
            });
        });

        TouchController.Instance.onPressed += OnPressed;
        TouchController.Instance.onUpdated += OnUpdated;
        TouchController.Instance.onReleased += OnReleased;
    }

    //لیست میوه هایی که میخواند رو زمین بوجود بیان

    IEnumerator SpawnOtherFruits()
    {
        int correctChoice = UnityEngine.Random.Range(4, 5);

        List<FruitType> fruitsTypes4Ground = new List<FruitType>();

        //از میوه ی داخل سین چندتا بساز
        for (int i = 0; i < correctChoice; i++)
        {
            fruitsTypes4Ground.Add(currentFruit.fruitType);
        }

        //تعداد رندومی از میوه های دیگه رو بساز
        int currentOtherFruitIndex = 0;
        while (fruitsTypes4Ground.Count <= transformsList2SpawningFruits.Count)
        {
            for (int i = 0; i < UnityEngine.Random.Range(2,4); i++)
            {
                if (fruitsTypes4Ground.Count > transformsList2SpawningFruits.Count) break;
                fruitsTypes4Ground.Add(OtherTypesFruitsToSpawnOnGround[currentOtherFruitIndex]);
            }
            currentOtherFruitIndex++;
            if (currentOtherFruitIndex >= OtherTypesFruitsToSpawnOnGround.Count)
            {
                currentOtherFruitIndex = 0;
            }
        }

        List<Fruit> fruitsList = new List<Fruit>();
        fruitsList = fruitsTypes4Ground.GetFruitsViaTypes();
        fruitsList = fruitsList.MixMe();

        AudioController.instance.PlaySound(spawninngAudio, 1);

        for (int i = 0; i < transformsList2SpawningFruits.Count; i++)
        {
            Fruit fruit = Instantiate(fruitsList[i], transformsList2SpawningFruits[i].position, Quaternion.identity, section3Transform);
            fruit.transform.localScale *= 0.4f;
            fruitsListOnGround.Add(fruit);
            fruit.InitializeFruit();
            fruit.PlayGrowAnimation();

            ///////////////--Tutorial--////////////////
            if (isTutorialScene)
            {
                tutorials[i].Init(transformsList2SpawningFruits[i], fruit);
            }
            ///////////////////////////////////////////            

            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(0.08f * transformsList2SpawningFruits.Count);
        gameStarted = true;


        ///////////////--Tutorial--////////////////
        if (isTutorialScene) CheckTutorial();
        ///////////////////////////////////////////        
    }

    public void DisableSection()
    {
        sabad.gameObject.SetActive(false);

        foreach (var item in transformsList2SpawningFruits)
        {
            Destroy(item.GetComponent<SpriteRenderer>());
        }

        TouchController.Instance.onPressed -= OnPressed;
        TouchController.Instance.onUpdated -= OnUpdated;
        TouchController.Instance.onReleased -= OnReleased;
    }

    Vector2? shiftAmount;
    GameObject selectedGameObject;
    Vector3? startPosition4selectedGO;
    bool objectIsInStartPoint = true;
    public void OnPressed(Vector2 position)
    {
        if (winned) return;
        if (!gameStarted) return;
        canDrag = true;
        RaycastHit2D hit = Physics2D.Raycast(position.GetWorldPosition(), -Vector3.forward, 100, LayerMask.GetMask("Fruit", "BlockTouch"));

        if (hit.collider != null)
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlockTouch")) return;

            selectedGameObject = hit.collider.gameObject;
            startPosition4selectedGO = hit.collider.transform.position;
            selectedGameObject.GetComponent<Fruit>().SetDragOptions();
            shiftAmount = hit.collider.transform.position.ConvertToVector2() - position.GetWorldPosition();
            AudioController.instance.PlayLoopSound(draggedSound);
            objectIsInStartPoint = false;
        }
    }

    public void OnUpdated(Vector2 position)
    {
        if (!gameStarted) return;
        if (!canDrag) return;

        if (selectedGameObject != null)
        {
            selectedGameObject.transform.position = position.GetWorldPosition() + shiftAmount.Value;
        }
    }

    public void OnReleased(Vector2 position)
    {
        if (!gameStarted) return;

        RaycastHit2D hit = Physics2D.Raycast(position.GetWorldPosition(), -Vector3.forward, 100, LayerMask.GetMask("Sabad", "BlockTouch"));

        if (selectedGameObject != null)
        {
            GameObject fg = selectedGameObject;
            if (hit.collider != null)
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("BlockTouch")) return;

                if (fg.GetComponent<Fruit>().fruitType == currentFruit.fruitType)
                {
                    Sabad sabad = hit.collider.transform.GetComponent<Sabad>();

                    Vector3 newFruitPos = sabad.transform.TransformPoint(sabad.GetNextFruitTranform().localPosition);
                    Fruit currFruit = fg.GetComponent<Fruit>();

                    fg.transform.DOMove(newFruitPos, 0.12f).OnComplete(() =>
                    {
                        fg.transform.SetParent(sabad.transform, true);
                        currFruit.SetReleaseOptions();
                        currFruit.isLocated = true;

                        fg.GetComponent<CircleCollider2D>().enabled = false;

                        objectIsInStartPoint = true;
                    });


                    fruitsListOnGround.Remove(currFruit);
                    AudioController.instance.PlayOneShot(correctDroppedSound, position.GetWorldPosition());

                    CheckList();
                }
                else
                {
                    fg.transform.DOMove(startPosition4selectedGO.Value, 0.4f).OnComplete(()=> objectIsInStartPoint = true);
                    fg.GetComponent<Fruit>().SetReleaseOptions();

                    AudioController.instance.PlayOneShot(incorrectDroppedSound, position.GetWorldPosition());
                }
            }
            else
            {
                fg.transform.DOMove(startPosition4selectedGO.Value, 0.4f).OnComplete(()=> objectIsInStartPoint = true);
                fg.GetComponent<Fruit>().SetReleaseOptions();

                AudioController.instance.PlayOneShot(incorrectDroppedSound, position.GetWorldPosition());
            }
            AudioController.instance.StopLoopSound();
        }

        ///////////////--Tutorial--////////////////
        if (isTutorialScene) CheckTutorial();
        ///////////////////////////////////////////

        canDrag = false;
        selectedGameObject = null;
        startPosition4selectedGO = null;
        shiftAmount = null;
    }

    void CheckList()
    {
        if (!gameStarted) return;
        if (winned) return;
        if (!fruitsListOnGround.Any(x=>x.fruitType == currentFruit.fruitType))
        {
            winned = true;
            win(() => onSectionCompleted?.Invoke());

            int coin = UnityEngine.PlayerPrefs.GetInt("coins");
            if (coin < 99999)
            {
                int newcoin = coin + 4;
                UnityEngine.PlayerPrefs.SetInt("coins", newcoin);
                Debug.Log(newcoin);
                // GameObject.Find("mycoin").GetComponent<coinController>().updatecoin();
            }
        }
    }

    void WaitAndDo(Action action, float delay = 1) => StartCoroutine(WaitAndDo_Co(action, delay));
    IEnumerator WaitAndDo_Co(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    void win(Action onPlayEffectComplete) => StartCoroutine(Win_Co(onPlayEffectComplete));
    IEnumerator Win_Co(Action onPlayEffectComplete)
    {
        ParticleSystem linearParticles = Instantiate(linearConfetti, topPosition4LinearPaticles.transform.position, Quaternion.identity, topPosition4LinearPaticles.parent) as ParticleSystem;
        linearParticles.Play(true);
        float baseDur = linearParticles.main.duration;

        AudioController.instance.PlaySound(winSound, 0);

        float winSoundDuration = (winSound != null)? winSound.length:2;//اگه صدای شادی نبود زمان توقف رو دو ثانیه فرض کن

        foreach (var item in effectPositions)
        {
            ParticleSystem partSys = Instantiate(confettiExplosion, item.transform.position, Quaternion.identity, item.parent) as ParticleSystem;
            partSys.Play(true);

            AudioController.instance.PlayOneShot(effectExplosionSound, item.transform.position);
            yield return new WaitForSeconds(0.4f);
            winSoundDuration -= 0.4f;
        }

        if(winSoundDuration > 0)
            yield return new WaitForSeconds(winSoundDuration);

        onPlayEffectComplete?.Invoke();
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
        yield return new WaitUntil(() => objectIsInStartPoint == true);

        TutorialData_3 tut = tutorials.FirstOrDefault(x => !x.fruit.isLocated && x.fruit.fruitType == currentFruit.fruitType);
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

    IEnumerator ShowIndicatorAnimation(TutorialData_3 tutorial)
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

        yield return new WaitUntil(() => selectedGameObject != null);
        tween.Kill();
        handHandler.gameObject.SetActive(false);
        tutorial.blockScreen.blockScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
}


[Serializable]
class TutorialData_3
{
    //public int fruitTransformIndex;
    public Transform objectPosition;
    public Fruit fruit;
    public BlockScreen blockScreen;

    public void Init(Transform objectPosition, Fruit fruit)
    {
        this.objectPosition = objectPosition;
        this.fruit = fruit;
    }
}