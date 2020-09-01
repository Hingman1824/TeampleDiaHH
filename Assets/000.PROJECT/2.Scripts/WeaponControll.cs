//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class WeaponControll : MonoBehaviour
//{
//    private Rigidbody rb;
//    private PlayerTestMove player;
//    private bool isHit = false;

//    private void Awake()
//    {
//        rb = GetComponent<Rigidbody>();
//        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerTestMove>();

//    }

//    private void OnCollisionEnter(Collision collision)
//    {
//        if( collision.gameObject.tag == "Enemy" && isHit == false)
//        {
//            isHit = true;

//        }
//    }

//    private void Update()
//    {
//        if(isHit == true)
//        {
//            StartCoroutine(AttackTrue());
//        }
//    }

//    private IEnumerator AttackTrue()
//    {

//        yield return new WaitForSeconds(0.3f);
//        isHit = false;
//    }
//}
