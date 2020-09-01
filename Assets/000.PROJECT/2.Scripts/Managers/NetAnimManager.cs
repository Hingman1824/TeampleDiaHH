using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rand = UnityEngine.Random;

[System.Serializable]
public class Animations
{
    public AnimationClip idle1;
    public AnimationClip move;
    public AnimationClip attack;
    public AnimationClip skill1;
    public AnimationClip skill2;
    public AnimationClip skill3;
    public AnimationClip hit;
    public AnimationClip die;
}

//컴포넌트 매뉴에 추가
[AddComponentMenu("NetworkTab/addNetAnim")]
[RequireComponent(typeof(PhotonView))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animation))]


public class NetAnimManager : MonoBehaviour
{
    private bool isDie = false;

    private PhotonView pv = null;
    private Rigidbody netRigidbody;

    Vector3 currPos = Vector3.zero;
    Quaternion currRot = Quaternion.identity;

    private Transform myTr;

    private Animator animator;
    private Animation _anim;
    private AnimationState animState;
    private float animTime;

    public enum CurrentState
    {
        idle = 1,
        move,
        attack,
        skill1,
        skill2,
        skill3,
        hit,
        die,


        ALL
    }
    public enum ThisKind
    {
        Player = 1,
        OtherPlayer,
        NPC,
        Enemy,
        Boss,

        ALL
    }


    [TextArea(7, 11)]
    public string memo = " 각 캐릭터와 적의 애니메이션 따로 연결하여 사용할 것\n" +
                         "만약 없는 모션이여도 채워놓을것\n" +
                         "추가 요소 있을시 꼭 메모 해주세요.";

    [Header("꼭 애니메이션을 연결 해주세요.")]
    [Space(20)]
    public Animations anims;

    [Header("기본 아이들로 지정 해주세요.")]
    [Space(30)]
    [Tooltip("현재 상태를 표시 해줍니다.")]
    public CurrentState current = CurrentState.idle;

    [Header("캐릭터 또는 몬스터의 종류를 지정 해주세요.")]
    [Tooltip("현재 캐릭터 또는 몬스터의 종류를 표시 해줍니다.")]
    public ThisKind kinds = ThisKind.ALL;

    [HideInInspector]
    public int net_anim = 0; //애니메이션 변경용

    private void Awake()
    {
        _anim = GetComponent<Animation>();
        animator = GetComponent<Animator>();
        myTr = GetComponent<Transform>();

        pv = GetComponent<PhotonView>();
        netRigidbody = GetComponent<Rigidbody>();

        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        

        if (!PhotonNetwork.isMasterClient)
        {
            netRigidbody.isKinematic = true;
            
        }
        else if (!pv.isMine)
        {
            _anim.enabled = true;
            animator.enabled = false;
        }
        currPos = myTr.position;
        currRot = myTr.rotation;
    }

    private IEnumerator Start()
    {
        if (!pv.isMine)
        {
            if (kinds == ThisKind.Player)
            {
                kinds = ThisKind.OtherPlayer;
                StartCoroutine(this.NetAnim());
            }
            else if (kinds == ThisKind.NPC)
            {
                _anim.clip = anims.idle1;
                _anim.Play();
            }
            else
            {
                _anim.clip = anims.idle1;
                _anim.Play();
                StartCoroutine(this.NetAnim());
            }
        }
        yield return null;
    }


    private void Update()
    {
        if (!pv.isMine)
        {
            myTr.position = Vector3.Lerp(myTr.position, currPos, Time.deltaTime * 3.0f);
            myTr.rotation = Quaternion.Slerp(myTr.rotation, currRot, Time.deltaTime * 3.0f);
        }
    }

    public void Die()
    {
        if (PhotonNetwork.isMasterClient && kinds == ThisKind.Enemy)
        {
            StartCoroutine(this.EnemyDie());
        }
        else if (pv.isMine && kinds == ThisKind.Player)
        {
            StartCoroutine(this.PlayerDie());
        }
    }

    private IEnumerator PlayerDie()
    {
        //대충 캐릭터 죽는모션
        yield return new WaitForSeconds(6.0f);
    }

    private IEnumerator EnemyDie()
    {
        isDie = true;
        _anim.CrossFade(anims.die.name, 0.3f);
        net_anim = 8;
        current = CurrentState.die;
        this.gameObject.tag = "Untagged";
        this.gameObject.transform.Find("Enemy").tag = "Untagged";

        foreach (Collider coll in gameObject.GetComponentsInChildren<Collider>())
        {
            coll.enabled = false;
        }

        yield return new WaitForSeconds(2.0f);
        PhotonNetwork.Destroy(gameObject);
    }

    private IEnumerator NetAnim()
    {
        while (!isDie)
        {
            yield return new WaitForSeconds(0.2f);
            if (!pv.isMine)
            {
                if (net_anim == 0)
                {
                    _anim.CrossFade(anims.idle1.name, 0.3f);
                }
                else if (net_anim == 1)
                {
                    _anim.CrossFade(anims.move.name, 0.3f);
                }
                else if (net_anim == 2)
                {
                    _anim.CrossFade(anims.attack.name, 0.3f);
                }
                else if (net_anim == 3)
                {
                    _anim.CrossFade(anims.skill1.name, 0.3f);
                }
                else if (net_anim == 4)
                {
                    _anim.CrossFade(anims.skill2.name, 0.3f);
                }
                else if (net_anim == 5)
                {
                    _anim.CrossFade(anims.skill3.name, 0.3f);
                }
                else if (net_anim == 6)
                {
                    _anim.CrossFade(anims.hit.name, 0.3f);
                }
                else if (net_anim == 7)
                {
                    _anim.CrossFade(anims.die.name, 0.3f);

                    yield break;
                }
                else if (net_anim == 8)
                {
                    _anim.CrossFade(anims.die.name, 0.3f);

                    yield break;
                }
            }
        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(myTr.position);
            stream.SendNext(myTr.rotation);
            stream.SendNext(net_anim);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            net_anim = (int)stream.ReceiveNext();
        }
    }

    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}
