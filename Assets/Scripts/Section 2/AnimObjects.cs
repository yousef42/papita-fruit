using UnityEngine;
using System.Collections;
using Spine.Unity;
using Spine;
using DG.Tweening;

public abstract class AnimObjects : MonoBehaviour
{
    public SkeletonAnimation skelAnim;

    const string starDragAnimLayer = "start";
    const string dragAnimLayer = "play";
    const string idleAnimLayer = "idle";

    public virtual void StartDrag()
    {
        skelAnim.AnimationState.SetAnimation(0, starDragAnimLayer, false);
        skelAnim.AnimationState.AddAnimation(0, dragAnimLayer, true, 0);
    }

    public virtual void SetDragOptions()
    {

    }

    public virtual void SetReleaseOptions()
    {
        skelAnim.AnimationState.SetAnimation(0, idleAnimLayer, true);
    }
}