﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class MonkSkill : PlayerManager
{
    public bool isMantraOfEvasion = false;
    public bool isCycloneStrike = false;
    public bool isDashingStrike = false;
    public bool isWaveOfLight = false;

    public GameObject Impact;
    public GameObject Phoenix;
    public GameObject pray;
    public GameObject lazyImpact;

    public static MonkSkill _instance;
    //private EquipmentManager equipmentManager;
    //public Transform weaponPos;
    //private Collider[] weaponColliders;
    //public bool colOnce;

    public Transform Impactpos;

    private void Awake()
    {
        _instance = this;
        //equipmentManager = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>();
        //weaponColliders = weaponPos.GetComponentsInChildren<Collider>();

        myRb = GetComponent<Rigidbody>();

        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;

        pv.synchronization = ViewSynchronization.UnreliableOnChange;

        anim = gameObject.GetComponentInChildren<Animator>();




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

    //private void Start()
    //{
    //    //WeaponOff();
    //    //equipmentManager.WeaponSet(0);
    //    //colOnce = false;
    //}

    void Update()
    {
        if (pv.isMine)
        {
            PlayerMovement();
            PlayerMovementJoyStick();
            PlayerMoveAnimation();

            if (Input.GetKey(KeyCode.X) && isMantraOfEvasion == false)
            {
                StartCoroutine(MantraOfEvasion());
            }

            if (Input.GetKey(KeyCode.E) && isCycloneStrike == false)
            {
                StartCoroutine(CycloneStrike());
            }

            if (Input.GetKey(KeyCode.R) && isDashingStrike == false)
            {
                StartCoroutine(DashingStrike());
            }

            if (Input.GetKey(KeyCode.Q) && isWaveOfLight == false)
            {
                StartCoroutine(WaveOfLight());
            }

        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, currPos, Time.deltaTime * 3.0f);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

    public IEnumerator MantraOfEvasion()
    {
        isMantraOfEvasion = true;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.15f);

        if (transform.eulerAngles.y > 135 && transform.eulerAngles.y < 225) //앞으로 발사
        {
            var FirePhoenix = Instantiate(Phoenix, Impactpos.position, Quaternion.Euler(0, 0, 0));
            FirePhoenix.GetComponent<EffectSettings>().MoveVector = Vector3.forward;
        }
        else if (transform.eulerAngles.y > 225 && transform.eulerAngles.y < 315) //오른쪽으로 -80~ -110 사이
        {
            var FirePhoenix = Instantiate(Phoenix, Impactpos.position, Quaternion.Euler(0, -90, 0));
            FirePhoenix.GetComponent<EffectSettings>().MoveVector = -Vector3.left;
        }
        else if (transform.eulerAngles.y > 45 && transform.eulerAngles.y < 135) //왼쪽으로
        {
            var FirePhoenix = Instantiate(Phoenix, Impactpos.position, Quaternion.Euler(0, 90, 0));
            FirePhoenix.GetComponent<EffectSettings>().MoveVector = Vector3.left;
        }

        else if ((transform.eulerAngles.y > 0 && transform.eulerAngles.y < 45) || (transform.eulerAngles.y > 315 && transform.eulerAngles.y < 360)) //뒤로
        {
            var FirePhoenix = Instantiate(Phoenix, Impactpos.position, Quaternion.Euler(0, 180, 0));
            FirePhoenix.GetComponent<EffectSettings>().MoveVector = -Vector3.forward;
        }

        yield return new WaitForSeconds(0.15f);
        //Invoke("PlayerAttackAnimation", 0.5f);
        anim.SetBool("isAttack", false);
        isMantraOfEvasion = false;
        yield return null;
    }


    public IEnumerator CycloneStrike()
    {
        if (isUseESkill == false)
        {
            isUseESkill = true;
            isCycloneStrike = true;
            anim.SetBool("isSkill1", true);


            yield return new WaitForSeconds(0.15f);
            Instantiate(Impact, Impactpos.position, Quaternion.Euler(90, 0, 0));

            StartCoroutine(CooltimeSkill2(5.0f));
            yield return new WaitForSeconds(0.15f);

            anim.SetBool("isSkill1", false);
            isCycloneStrike = false;
            yield return null;
        }
    }

    public IEnumerator DashingStrike()
    {
        if (isUseRSkill == false)
        {
            isUseRSkill = true;
            isDashingStrike = true;
            anim.SetBool("isSkill2", true);

            pray.SetActive(true);
            StartCoroutine(CooltimeSkill1(5.0f));
            yield return new WaitForSeconds(1.0f);
            pray.SetActive(false);

            anim.SetBool("isSkill2", false);
            isDashingStrike = false;
            yield return null;
        }
    }

    public IEnumerator WaveOfLight()
    {
        if (isUseQSkill == false)
        {
            isUseQSkill = true;
            isWaveOfLight = true;
            anim.SetBool("isSkill3", true);

            yield return new WaitForSeconds(0.35f);
            Instantiate(lazyImpact, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(90, 0, 0));

            StartCoroutine(CooltimeSkill3(5.0f));
            yield return new WaitForSeconds(0.5f);

            anim.SetBool("isSkill3", false);
            isWaveOfLight = false;
            yield return null;
        }
    }
    //private void WeaponOn()
    //{
    //    foreach (var it in weaponColliders)
    //    {
    //        it.enabled = true;
    //    }
    //}

    //private void WeaponOff()
    //{
    //    foreach (var it in weaponColliders)
    //    {
    //        it.enabled = false;
    //    }
    //}

}