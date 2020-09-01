using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody rb;    
    private float arrowSpeed = 1000.0f;
   

    private void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * arrowSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        Invoke("arrowDestroy", 1.0f);            
    }

    void arrowDestroy()
    {
        Destroy(this.gameObject);
    }
}
