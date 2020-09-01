using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class WizardSkill : PlayerManager
{
    public bool isMagicMissile = false;
    public bool isExplosionBlast = false;
    public bool isTeleport = false;
    public bool isDiamondSkin = false;

    // 매직미사일 프리팹
    public GameObject _mmagicPf;

    // 발사 위치
    public Transform _magicPoint;


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

            if (Input.GetKeyDown(KeyCode.X) && isMagicMissile == false)
            {
                StartCoroutine(MagicMissile());                
            }

            if (Input.GetKey(KeyCode.E) && isExplosionBlast == false)
            {
                StartCoroutine(ExplosionBlast());
            }

            if (Input.GetKey(KeyCode.R) && isTeleport == false)
            {
                StartCoroutine(Teleport());
            }

            if (Input.GetKey(KeyCode.F) && isDiamondSkin == false)
            {
                StartCoroutine(DiamondSkin());
            }
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, currPos, Time.deltaTime * 3.0f);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

    public IEnumerator MagicMissile()
    {
        anim.SetBool("isAttack", true);
        // 애니메이션속도를 2로 설정해서 기다리는 시간도 0.5로 단축함
        yield return new WaitForSeconds(0.1f);
        Shoot();
        //Invoke("PlayerAttackAnimation", 0.5f);
        anim.SetBool("isAttack", false);
    }


    private IEnumerator ExplosionBlast()
    {
        isExplosionBlast = true;
        anim.SetBool("isSkill1", true);
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isSkill1", false);
        isExplosionBlast = false;
        yield return null;
    }

    private IEnumerator Teleport()
    {
        isTeleport = true;
        anim.SetBool("isSkill2", true);
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isSkill2", false);
        isTeleport = false;
        yield return null;
    }

    private IEnumerator DiamondSkin()
    {
        isDiamondSkin = true;
        anim.SetBool("isSkill3", true);
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isSkill3", false);
        isDiamondSkin = false;
        yield return null;
    }

    private void Shoot()
    {
        Instantiate(_mmagicPf, _magicPoint.position, _magicPoint.rotation);        
    }

}
