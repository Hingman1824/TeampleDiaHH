using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    //타격감 프로토 타입
    private bool isAttack = false;

    private int attackCount;

    private Transform mainCamera;

    [Range(0, 30)]
    public float camShake = 0.0f;//무기 화면 흔들림 정도

    private void Awake()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    public void OnAttack()
    {
        isAttack = true;
    }
    public void OffAttack()
    {
        isAttack = false;
        attackCount = 0;
    }

    private void CameraShaking()
    {
        mainCamera.transform.position += new Vector3(0, 0, Mathf.Lerp(0, camShake, Time.deltaTime));
        mainCamera.transform.position -= new Vector3(0, 0, Mathf.Lerp(0, camShake, Time.deltaTime));

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isAttack)
        {
            if (collision.gameObject.tag == "Enemy" )
            {
                attackCount = 1;
                if (attackCount == 1)
                {
                    CameraShaking();
                }
                OffAttack();

            }
        }
    }
}
