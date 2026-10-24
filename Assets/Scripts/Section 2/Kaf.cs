using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using System;

public class Kaf : MonoBehaviour
{
    [SerializeField] SkeletonAnimation skelAnim;
    [SerializeField] CircleCollider2D circleCollider;
    bool inDestroying;

    void PlayAnim(string animLayer, bool loop = false, Action onComplete = null) =>
        StartCoroutine(PlayAnim_Co(animLayer, loop, onComplete));

    IEnumerator PlayAnim_Co(string animLayer, bool loop = false, Action onComplete = null)
    {
        TrackEntry animTrack = skelAnim.AnimationState.SetAnimation(0, animLayer, loop);
        yield return new WaitForSpineAnimationComplete(animTrack);
        onComplete?.Invoke();
    }

    public void DestroyKaf()
    {
        if (inDestroying) return;
        inDestroying = true;
        circleCollider.enabled = false;
        PlayAnim("play2", onComplete: () => Destroy(gameObject));
    }

    public void PlayStart()
    {
        //PlayAnim("play");
        skelAnim.AnimationState.SetAnimation(0, "play", false);
        skelAnim.AnimationState.AddAnimation(0, "idle2", true, 0);
    }
}
