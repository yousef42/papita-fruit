using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoDestroyPartSys : MonoBehaviour
{
    [SerializeField] ParticleSystem partSys;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(partSys.main.duration);
        Destroy(gameObject);
    }
}
