using System.Collections;
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


    private void Awake()
    {      

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
    void Update()
    {
        if (pv.isMine)
        {
            
            PlayerMovement();
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

            if (Input.GetKey(KeyCode.F) && isWaveOfLight == false)
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
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        //Invoke("PlayerAttackAnimation", 0.5f);
        anim.SetBool("isAttack", false);
    }

    private IEnumerator CycloneStrike()
    {
        isCycloneStrike = true;
        anim.SetBool("isSkill1", true);
        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isSkill1", false);
        isCycloneStrike = false;
        yield return null;
    }

    private IEnumerator DashingStrike()
    {
        isDashingStrike = true;
        anim.SetBool("isSkill2", true);
        yield return new WaitForSeconds(3.0f);
        anim.SetBool("isSkill2", false);
        isDashingStrike = false;
        yield return null;
    }

    private IEnumerator WaveOfLight()
    {
        isWaveOfLight = true;
        anim.SetBool("isSkill3", true);
        yield return new WaitForSeconds(3.0f);
        anim.SetBool("isSkill3", false);
        isWaveOfLight = false;
        yield return null;
    }
}
