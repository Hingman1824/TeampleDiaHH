using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectCtr : MonoBehaviour
{
    private SphereCollider sphereCollider;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ColliderFalse());
    }

    IEnumerator ColliderFalse()
    {
        yield return new WaitForSeconds(0.3f);
        sphereCollider.enabled = false;
    }
}
