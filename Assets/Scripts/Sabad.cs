using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sabad : MonoBehaviour
{
    [SerializeField] List<Transform> transformsList = new List<Transform>();

    int nextTransformIndex = -1;

    /// <summary>
    /// Get One Point Inside Sabad In Sabad LocalWorld
    /// </summary>
    /// <returns></returns>
    public Transform GetNextFruitTranform()
    {
        nextTransformIndex++;
        if (nextTransformIndex < transformsList.Count)
        {
            return transformsList[nextTransformIndex];
        }
        return transformsList[0];
    }


}
