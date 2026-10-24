using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Spine.Unity;
using DG.Tweening;

public class Section_2 : MonoBehaviour, ISection
{
    [SerializeField] bool isTutorialScene = false;
    [SerializeField] List<TutorialData_2> tutorials = new List<TutorialData_2>();
    [SerializeField] SkeletonAnimation handHandler;
    bool tutorialFinnished = false;

    [Space(20)]
    [SerializeField] Transform kafListTransform;
    [SerializeField] Kaf kafPrefab;
    [SerializeField] GameObject doosheabGO;
    [SerializeField] GameObject skachGO;

    [Space(20)]
    [Header("-----------------"), Space]
    [SerializeField] AudioClip badChoiceAudio;
    [SerializeField] AudioClip kafSpawnSound;
    [SerializeField] AudioClip doosheAbSound;
    [SerializeField] AudioClip winSound;


    public Action onSectionCompleted { get; set; }
    Fruit dirtyFruit;

    bool canDrag;
    bool winned = false;
    List<Kaf> kafHaList = new List<Kaf>();

    bool canDragDoosheAb = false;//اول کار نمیتونه دوش رو بگیره باید اسکاچ رو بگیره و کف مالی کنه میوه رو

    public void EnableSection(Action onSectionComplete) => StartCoroutine(EnableSection_Co(onSectionComplete));
    IEnumerator EnableSection_Co(Action onSectionComplete)
    {
        this.onSectionCompleted = onSectionComplete;
        objectIsInStartPoint = false;

        float skach_startPosY = skachGO.transform.position.y;
        float doosh_startPosY = doosheabGO.transform.position.y;

        skachGO.transform.position -= Vector3.up * 10;
        doosheabGO.transform.position -= Vector3.up * 10;

        skachGO.transform.DOMoveY(skach_startPosY, 0.5f).SetEase(Ease.OutBack);
        doosheabGO.transform.DOMoveY(doosh_startPosY, 0.5f).SetEase(Ease.OutBack).SetDelay(0.2f).OnComplete(() => objectIsInStartPoint = true);

        if(isTutorialScene) HideTutorialTools();
        yield return new WaitUntil(() => objectIsInStartPoint);
        if (isTutorialScene)
        {
            StartCoroutine(SetTutorial(0));
        }

        TouchController.Instance.onPressed += OnPressed;
        TouchController.Instance.onUpdated += OnUpdated;
        TouchController.Instance.onReleased += OnReleased;
    }

    public void SeDirtytFruitGameObject(Fruit dirtyFruitGameObject)
    {
        this.dirtyFruit = dirtyFruitGameObject;
        //this.cleanFruitSprite = cleanFruitSprite;
    }

    public void DisableSection()
    {
        TouchController.Instance.onPressed -= OnPressed;
        TouchController.Instance.onUpdated -= OnUpdated;
        TouchController.Instance.onReleased -= OnReleased;
    }

    enum GrabbedObject { DoosheAb, Skach, None }
    GrabbedObject grabbedObject = GrabbedObject.None;
    GameObject grabbbedGameObject;
    Vector3? grabbedGOPosition;
    Vector2? shiftAmount;
    Transform dooshabRayPointTransform;
    AudioSource doosheAbAudioSource;
    public void OnPressed(Vector2 position)
    {
        if (winned) return;
        canDrag = true;
        RaycastHit2D hit = Physics2D.Raycast(position.GetWorldPosition(), -Vector3.forward, 100,
                                             LayerMask.GetMask(new string[] { "DoosheAb", "Skach", "BlockTouch" }));
        if (hit.collider != null)
        {
            if (isTutorialScene && hit.collider.gameObject.layer == LayerMask.NameToLayer("BlockTouch")) return;

            string tag = hit.collider.tag;
            if (tag == "DoosheAb")
            {
                if (canDragDoosheAb)
                {
                    grabbedObject = GrabbedObject.DoosheAb;
                    grabbbedGameObject = hit.collider.gameObject;
                    grabbedGOPosition = hit.collider.gameObject.transform.position;
                    dooshabRayPointTransform = hit.collider.gameObject.GetComponent<DoosheAb>().GetRayPointTransform();

                    hit.collider.GetComponent<DoosheAb>().StartDrag();

                    doosheAbAudioSource = AudioController.instance.PlaySound(doosheAbSound, true);
                }
                else
                {
                    PlayBadChoiceAnim(hit.collider.transform);
                    AudioController.instance.PlaySound(badChoiceAudio, 0);
                }
            }
            else if (tag == "Skach")
            {
                grabbedObject = GrabbedObject.Skach;
                grabbbedGameObject = hit.collider.gameObject;
                grabbedGOPosition = hit.collider.gameObject.transform.position;

                hit.collider.GetComponent<Skach>().StartDrag();
            }

            objectIsInStartPoint = false;
            shiftAmount = hit.collider.transform.position.ConvertToVector2() - position.GetWorldPosition();
        }
    }

    Vector2 endPosition;

    public void OnUpdated(Vector2 position)
    {
        if (!canDrag) return;
        if (grabbbedGameObject != null)
        {
            Vector2 touchPosition = position.GetWorldPosition();

            grabbbedGameObject.transform.position = touchPosition + shiftAmount.Value;

            if (winned) return;//نمیخوتد کاری کنی ولی بالا شی رو جابجا میکنه

            switch (grabbedObject)
            {
                case GrabbedObject.DoosheAb:

                    RaycastHit2D hitt = Physics2D.CircleCast(dooshabRayPointTransform.position, 0.6f, -Vector3.forward, 100, LayerMask.GetMask("Kaf"));

                    if (hitt.collider != null)
                    {
                        Kaf kaf = hitt.collider.gameObject.GetComponent<Kaf>();
                        kaf.DestroyKaf();
                        kafHaList.Remove(kaf);

                        CheckKafHaList();
                    }

                    break;
                case GrabbedObject.Skach:

                    RaycastHit2D hit = Physics2D.Raycast(touchPosition, -Vector3.forward, 100, LayerMask.GetMask(new string[] { "Fruit", "Kaf" }));

                    if (hit.collider != null)
                    {
                        string tag = hit.collider.gameObject.tag;
                        if (tag == "Fruit")
                        {
                            if (Vector2.Distance(endPosition, touchPosition) <= 0.05f) return;
                            endPosition = touchPosition;
                            SpawnKaf(touchPosition);//کف رو بالاتر میوه قرار دادیم
                        }
                        else if (tag == "Kaf")
                        {
                            //print("kaf");
                        }
                    }
                    break;
            }
        }
    }

    public void OnReleased(Vector2 position)
    {
        canDrag = false;
        ReleaseGrabbedObject(null);
    }

    void SpawnKaf(Vector3 position)
    {
        Kaf kaf = Instantiate(kafPrefab, position - Vector3.forward * 2, Quaternion.identity, kafListTransform);
               
        kaf.PlayStart();

        kafHaList.Add(kaf);

        if (kafHaList.Count % 2 == 0) AudioController.instance.PlayOneShot(kafSpawnSound, position);


        if (kafHaList.Count > 12)
        {
            canDragDoosheAb = true;
        }
    }

    void PlayBadChoiceAnim(Transform tra)
    {
        tra.DOPunchPosition(Vector3.right * 1.4f, 0.6f, 10, 0.6f);
    }

    void CheckKafHaList()
    {
        if (winned) return;
        if (kafHaList.Count <= 0)
        {
            winned = true;
            ReleaseGrabbedObject(null);

            WaitAndDo(() =>
            {
                dirtyFruit.ChangeDirty2Clean();

                AudioController.instance.PlaySound(winSound, 0);

                StartCoroutine(WinTheSection());
            }, 0.6f);
        }
    }

    bool objectIsInStartPoint = true;
    void ReleaseGrabbedObject(Action onMoveComplete)
    {
        if (grabbbedGameObject == null) return;
        GameObject fg = grabbbedGameObject;
        fg.transform.DOMove(grabbedGOPosition.Value, 0.4f).OnComplete(() => 
        {
            onMoveComplete?.Invoke();
            objectIsInStartPoint = true;
        });


        AnimObjects grObject = grabbbedGameObject.GetComponent<AnimObjects>();
        grObject.SetReleaseOptions();

        if (grObject is DoosheAb)
        {
            dooshabRayPointTransform = null;
            if (doosheAbAudioSource != null)
            {
                doosheAbAudioSource.Stop();
                Destroy(doosheAbAudioSource.gameObject);
            }


            ////Tutorial
            if (isTutorialScene && !tutorialFinnished)
            {
                if(kafHaList.Count > 0)
                {
                    StartCoroutine(SetTutorial(1));
                }
            }
            ////
        }
        else if (grObject is Skach)
        {

            ////Tutorial
            if (isTutorialScene && !tutorialFinnished)
            {
                if (kafHaList.Count < 12)
                {
                    StartCoroutine(SetTutorial(0));
                }
                else
                {
                    StartCoroutine(SetTutorial(1));
                }
            }
            ////
        }

        grabbbedGameObject = null;
        grabbedObject = GrabbedObject.None;
        grabbedGOPosition = null;
    }

    void HideTools()
    {
        skachGO.transform.DOMoveY(skachGO.transform.position.y - 10f, 0.5f).SetEase(Ease.InBack);
        doosheabGO.transform.DOMoveY(doosheabGO.transform.position.y - 10f, 0.5f).SetEase(Ease.InBack).SetDelay(0.2f);
    }

    void WaitAndDo(Action action, float delay = 1) => StartCoroutine(WaitAndDo_Co(action, delay));
    IEnumerator WaitAndDo_Co(Action action, float delay)
    {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    IEnumerator WinTheSection()
    {
        //زمان اتمام اهنگ
        yield return new WaitForSeconds(2f);
        //زمان پنهان سازی نیم ثانیه قرار داده شده است
        HideTools();
        yield return new WaitForSeconds(0.5f + 0.1f);
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
    void HideTutorialTools()
    {
        foreach (var item in tutorials)
        {
            item.blockScreen.blockScreen.SetActive(false);
        }
        handHandler.gameObject.SetActive(false);
    }

    IEnumerator SetTutorial(int tutIndex)
    {
        if(!objectIsInStartPoint) yield return new WaitUntil(() => objectIsInStartPoint == true);
        if (progressingTutorial != null)
        {
            StopCoroutine(progressingTutorial);
        }
        progressingTutorial = StartCoroutine(ShowIndicatorAnimation(tutorials[tutIndex]));
    }

    IEnumerator ShowIndicatorAnimation(TutorialData_2 tutorial)
    {
        tutorial.blockScreen.blockScreen.gameObject.SetActive(true);
        handHandler.transform.position = tutorial.blockScreen.startPoint.position;
        handHandler.gameObject.SetActive(true);
        handHandler.AnimationState.SetAnimation(0, "idle", true);
        Tween tween = handHandler.transform.DOMove(tutorial.blockScreen.endPoint.position, 1.4f).SetAutoKill(false).
                              OnStepComplete(() => handHandler.transform.position = tutorial.blockScreen.startPoint.position).SetLoops(-1);

        yield return new WaitUntil(() => grabbbedGameObject != null && (grabbedObject == GrabbedObject.Skach || grabbedObject == GrabbedObject.DoosheAb));
        tween.Kill();
        handHandler.gameObject.SetActive(false);
        tutorial.blockScreen.blockScreen.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.1f);
    }
}

[Serializable]
class TutorialData_2 
{
    public BlockScreen blockScreen;
}
