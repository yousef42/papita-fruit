using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using System;


public class Fruit : MonoBehaviour
{
    [SerializeField] SkeletonAnimation skelAnim;
    [SerializeField] Rigidbody2D rb;

    public FruitType fruitType;
    MeshRenderer meshRenderer;

    /*[HideInInspector]*/ public bool isLocated = false;

    //For Ninja Mode
    public int shapId { get; set; }
    bool animIsPlaying = false;
    bool touched = false;
    //

    public void InitializeFruit()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.sortingOrder = 4;
        rb.bodyType = RigidbodyType2D.Static;
        PlayAnim(FruitAnimType.Idle_1);
    }

    void PlayAnim(FruitAnimType animType, bool loop = false, Action onComplete = null) =>
                    StartCoroutine(PlayAnim_Co(animType, loop, onComplete));
    IEnumerator PlayAnim_Co(FruitAnimType animType, bool loop, Action onComplete)
    {
        TrackEntry track = skelAnim.AnimationState.SetAnimation(0, GetAnimLayer(animType), loop);
        yield return new WaitForSpineAnimationComplete(track);
        onComplete?.Invoke();
    }
    
    string GetAnimLayer(FruitAnimType animType)
    {
        string animLayer = "";
        switch (animType)
        {
            case FruitAnimType.Black2DirtyAnim:
                animLayer = "play1";
                break;
            case FruitAnimType.Dirty2CleanAnim:
                animLayer = "play2";
                break;
            case FruitAnimType.GrowAnim:
                animLayer = "grow";
                break;
            case FruitAnimType.Idle_1:
                animLayer = "idle1";
                break;
            case FruitAnimType.Idle_3:
                animLayer = "idle3";
                break;
            case FruitAnimType.Play_3:
                animLayer = "play3";
                break;
            default:
                break;
        }
        return animLayer;
    }

    public void ChangeBlack2Dirty(Action onComplete)
    {
        PlayAnim(FruitAnimType.Black2DirtyAnim, false, onComplete);
    }
    public void ChangeDirty2Clean()
    {
        PlayAnim(FruitAnimType.Dirty2CleanAnim);
    }

    public void PlayGrowAnimation()
    {
        PlayAnim(FruitAnimType.GrowAnim);
    }

    public void SetDragOptions()
    {
        meshRenderer.sortingOrder = 8;
        PlayAnim(FruitAnimType.Play_3, true);
    }

    public void SetReleaseOptions()
    {
        meshRenderer.sortingOrder = 6;
        PlayAnim(FruitAnimType.Idle_3);
    }


    //For Ninja Mode
    public void InitializeForNinja()
    {
        skelAnim.AnimationState.ClearTrack(0);
        PlayAnim(FruitAnimType.Idle_3);
    }

    public void ShootShape(Vector2 force)
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.AddForce(force, ForceMode2D.Impulse);
    }

    public void ShapeTouched(bool isCorrectShape, AudioClip correctSound, AudioClip incorrectSound, Action onAnimComplete)
    {
        if (touched) return;
        touched = true;
        StartCoroutine(ShapeTouched_Co(isCorrectShape, onAnimComplete));

        if (isCorrectShape)
        {
            AudioController.instance.PlayOneShot(correctSound);
        }
        else
        {
            AudioController.instance.PlayOneShot(incorrectSound);
        }
    }

    IEnumerator ShapeTouched_Co(bool isCorrectShape, Action onAnimComplete)
    {
        string animName = isCorrectShape ? "touch-t" : "touch-f";
        skelAnim.AnimationState.ClearTrack(0);
        TrackEntry track = skelAnim.AnimationState.SetAnimation(0, animName, false);
        yield return new WaitForSpineAnimationComplete(track);
        onAnimComplete?.Invoke();
    }
    //

}
