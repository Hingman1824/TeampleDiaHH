using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class DevilHunterSkill : PlayerManager
{
    public bool isMultiShot = false;
    public bool isSmokeScreen = false;
    public bool isVault = false;
    public bool isCompanion = false;

    // 화살 프리팹
    public GameObject _arrowPf;

    // 발사 위치
    public Transform _arrowPoint; 
   
    //BoxCollider babaWeapon;

    private void Awake()
    {
        
        myRb = GetComponent<Rigidbody>();
        
        pv = GetComponent<PhotonView>();
        
        pv.ObservedComponents[0] = this;
        
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        
        anim = gameObject.GetComponentInChildren<Animator>();

        //babaWeapon = GameObject.FindWithTag("BabaWeapon").GetComponent<BoxCollider>();

        
        if (pv.isMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = camPivot;
        }
        else
        {            
            myRb.isKinematic = true; 
        }
        currPos = this.transform.position;
        currRot = this.transform.rotation;
    }
    void Update()
    {
        if (pv.isMine)
        {
            
            PlayerMovement();
            PlayerMoveAnimation();


            if (Input.GetKeyDown(KeyCode.X) && isMultiShot == false)
            {
                StartCoroutine(MultiShot());                
            }

            if (Input.GetKey(KeyCode.E) && isSmokeScreen == false)
            {
                StartCoroutine(SmokeScreen());
            }

            if (Input.GetKeyDown(KeyCode.R) && isVault == false)
            {
                StartCoroutine(VaultOn());
            }
            else if(Input.GetKeyUp(KeyCode.R) && isVault == true)
            {
                StartCoroutine(VaultOff());
                playerSpeed /= 4;
            }

            if (Input.GetKey(KeyCode.F) && isCompanion == false)
            {               
                StartCoroutine(Companion());
            }            
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, currPos, Time.deltaTime * 3.0f);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }


    public IEnumerator MultiShot()
    {
        anim.SetBool("isAttack", true);        
        yield return new WaitForSeconds(1.0f);
        Shoot();
        //Invoke("PlayerAttackAnimation", 0.5f);
        anim.SetBool("isAttack", false);
    }

    private IEnumerator SmokeScreen()
    {        
        isSmokeScreen = true;
        anim.SetBool("isSkill1", true);
        yield return new WaitForSeconds(1.0f);        
        anim.SetBool("isSkill1", false);      
        isSmokeScreen = false;
        yield return null;
    }

    private IEnumerator VaultOn()
    {
        isVault = true;
        anim.SetBool("isSkill2", true);
        yield return new WaitForSeconds(0.35f);
        // 빨리 움직이는 기술이므로 평소의 4배 속도로
        playerSpeed *= 4;
        // 플레이어가 움직이게 한다.
        PlayerMovement();
        anim.SetFloat("Speed", 1.0f);
    }

    private IEnumerator VaultOff()
    {
        anim.SetBool("isSkill2", false);
        isVault = false;
        yield return null;
    }

    private IEnumerator Companion()
    {
        isCompanion = true;
        anim.SetBool("isSkill3", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isSkill3", false);
        isCompanion = false;
        yield return null;
    }

    private void Shoot()
    {
        Instantiate(_arrowPf, _arrowPoint.position, _arrowPoint.rotation);
    }

}
