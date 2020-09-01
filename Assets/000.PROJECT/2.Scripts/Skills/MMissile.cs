using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMissile : MonoBehaviour
{
    public Rigidbody rb;
    private float missileSpeed = 500.0f;


    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * missileSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("missileDestroy", 1.5f);
    }

    void missileDestroy()
    {
        Destroy(this.gameObject);
    }
}
