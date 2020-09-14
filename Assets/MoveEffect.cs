using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    public GameObject nextEffect;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Destroy());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * 3);
    }

    void OnCollisionEnter(Collision col)
    {
        if( col.gameObject.tag == "Player")
        {
            Instantiate(nextEffect, this.transform);
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Instantiate(nextEffect, this.transform);
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(10f);
        Destroy();
    }

}
