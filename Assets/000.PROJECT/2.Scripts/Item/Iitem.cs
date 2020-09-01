using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Iitem : MonoBehaviour
{
    //이스크립트는 아이템에 집어넣어 아이템이 떨어졌을때 세트빔이 올라가고 주울수 있게 해준다.

    public GameObject beamPaticle; //빔 파티클 활성화/비활성화 해주기 위해

    private TextMeshPro mText;

    private void Awake()
    {
        mText = GetComponentInChildren<TextMeshPro>();
    }
    private void Update()
    {
        transform.eulerAngles = new Vector3(90, 0, 0);
    }
    private void OnCollisionEnter(Collision col) //빔 파티클
    {
        if(col.gameObject.tag == "Ground") //아이템이 땅에 떨어졌을때
        {
            
            beamPaticle.SetActive(true); //빔 파티클 활성화
        }
    }
    private void OnTriggerEnter(Collider other) //아이템 습득
    {
        if(other.gameObject.CompareTag("Player")) //아이템과 닿은 오브젝트의 태그가 플레이어일 경우
        {
            //other.GetComponent<PlayerTestMove>().Item = true;
            beamPaticle.SetActive(false);
            ItemPooling.ReturnObject(this);
        }

    }
}
