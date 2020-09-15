using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour
{
    public GameObject nextEffect;
    public BoxCollider boxCollider;

    // Start is called before the first frame update
    Vector3 pos;

    void Start()
    {
        StartCoroutine(Destroy());

        pos = new Vector3(0, 0, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * Time.deltaTime * 3);
        boxCollider.center += pos;
        boxCollider.size += pos;
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
        yield return new WaitForSeconds(3.5f);
        Destroy();
    }

}
