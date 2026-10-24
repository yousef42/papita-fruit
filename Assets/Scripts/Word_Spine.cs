using UnityEngine;
using Spine.Unity;

public enum AnimationMode { Idle, Play, End }
public class Word_Spine : MonoBehaviour
{
    [HideInInspector] public bool isInCorrectPosition { get; private set; } = false;
    public WordType wordType;

    [SerializeField] AudioClip nameAudio;
    [SerializeField] AudioClip fantasyAudio;

    SkeletonAnimation skelAnimation;
    MeshRenderer meshRenderer;
    [ReadOnly] public bool isLocated = false;


    public void SetMode(bool co)
    {
        isInCorrectPosition = co;
    }

    public void InitializeWord()
    {
        gameObject.tag = "Word_Spiny";
        meshRenderer = GetComponent<MeshRenderer>();
        SetNewSortingOrder(3);
        skelAnimation = GetComponent<SkeletonAnimation>();
        PlayAnimation(AnimationMode.End, true);
    }

    public void SetNewSortingOrder(int order)
    {
        meshRenderer.sortingOrder = order;
    }

    public void PlayAnimation(AnimationMode animationMode, bool loop = false)
    {
        string animationName = "";
        switch (animationMode)
        {
            case AnimationMode.Idle:
                animationName = "idle";
                break;
            case AnimationMode.Play:
                animationName = "play";
                break;
            case AnimationMode.End:
                animationName = "end";
                break;
            default:
                break;
        }
        skelAnimation.AnimationState.SetAnimation(0, animationName, loop);
    }

    public void SetDragOptions()
    {
        SetNewSortingOrder(4);
        PlayAnimation(AnimationMode.Play, true);
        AudioController.instance.PlayLoopSound(fantasyAudio);
    }

    public void SetReleaseOptions(bool inCorrectPosition)
    {
        //print("re");
        int sortingOrder = inCorrectPosition ? 2 : 3;
        SetNewSortingOrder(sortingOrder);
        PlayAnimation(AnimationMode.End);
        AudioController.instance.StopLoopSound();
        if (inCorrectPosition)
        {
            AudioController.instance.PlaySound(nameAudio, 0);
        }
    }
}
