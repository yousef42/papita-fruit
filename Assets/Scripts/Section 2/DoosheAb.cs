using UnityEngine;
//using System.Collections;
//using Spine.Unity;
//using Spine;

public sealed class DoosheAb : AnimObjects
{
    [Space]
    [SerializeField] Transform rayPointPosition;

    public override void SetDragOptions()
    {
        base.SetDragOptions();

        //RaycastHit2D hit = Physics2D.Raycast(rayPointPosition.position, -Vector3.forward, 100, LayerMask.GetMask("Kaf"));
        //if (hit.collider != null)
        //{
        //    hit.collider.gameObject.GetComponent<Kaf>().DestroyKaf();
        //}
    }
    
    public Transform GetRayPointTransform()
    {
        return rayPointPosition;
    }
}