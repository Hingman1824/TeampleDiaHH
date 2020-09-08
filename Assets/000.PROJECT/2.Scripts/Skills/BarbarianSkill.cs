using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BarbarianSkill : PlayerManager
{
    public bool isRend = false;
    public bool isBattleRage = false;
    public bool isFuriousCharge = false;
    public bool isWhirlWind = false;
   
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
            //PlayerMoveAnimation(); 아랫줄로 수정전  
            pv.RPC("PlayerMoveAnimation", PhotonTargets.AllBuffered, null);

            if (Input.GetKey(KeyCode.X) && isRend == false)
            {                
                StartCoroutine(Rend());                
            }

            if (Input.GetKey(KeyCode.E) && isBattleRage == false)
            {
                StartCoroutine(BattleRage());
            }

            if (Input.GetKey(KeyCode.R) && isFuriousCharge == false)
            {                
                StartCoroutine(FuriousCharge());                        
                
            }
            

            if (Input.GetKey(KeyCode.Q) && isWhirlWind == false)
            {
                // 어플라이루트모션을 항상켜두면 나머지 애니메이션들에도 각도상의 악 영향을 준다. 그래서 필요할 때만 켜둔다.
                anim.applyRootMotion = true;                
                StartCoroutine(WhirlWindOn());
            }
            else if (Input.GetKeyUp(KeyCode.Q) && isWhirlWind == true)
            {
                // 훨윈드가 끝나면 어플라이루트모션을 끈다.
                anim.applyRootMotion = false;
                StartCoroutine(WhirlWindOff());
                // 훨윈드 애니메이션이 끝나면 현재오브젝트 각도가 뒤틀리므로 강제로 원래 각도로 돌린다.
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                // 설상가상 바로 아래 자식 오브젝트는 위치도 이동하므로 강제로 원래 자리로 돌린다.
                transform.GetChild(0).gameObject.transform.localPosition = new Vector3(0, 0, 0);
                // 마찬가지로 바로 아래 자식 오브젝트는 각도도 뒤틀리므로 강제로 원래 각도로 돌린다.
                transform.GetChild(0).gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, currPos, Time.deltaTime * 3.0f);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

    public IEnumerator Rend()
    {
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.5f);
        //Invoke("PlayerAttackAnimation", 0.5f);
        anim.SetBool("isAttack", false);
    }


    private IEnumerator BattleRage()
    {
        isBattleRage = true;
        anim.SetBool("isSkill1", true);
        yield return new WaitForSeconds(2.5f);
        anim.SetBool("isSkill1", false);
        isBattleRage = false;
        yield return null;
    }

    private IEnumerator FuriousCharge()
    {
        isFuriousCharge = true;
        anim.SetBool("isSkill2", true);
        myRb.AddForce(-transform.forward * 175, ForceMode.Impulse);
        yield return new WaitForSeconds(2.0f);
        anim.SetBool("isSkill2", false);
        isFuriousCharge = false;
        yield return null;
    }

    private IEnumerator WhirlWindOn()
    {
        isWhirlWind = true;
        anim.SetBool("isSkill3", true);        
        yield return null;
        //anim.SetBool("isSkill3", false);
        //isSkill3 = false;
        //yield return null;
    }

    private IEnumerator WhirlWindOff()
    {
        anim.SetBool("isSkill3", false);
        isWhirlWind = false;        
        yield return null;
    }


}
