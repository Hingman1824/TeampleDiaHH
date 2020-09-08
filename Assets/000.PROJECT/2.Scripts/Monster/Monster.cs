using System.Collections;
using UnityEngine;
using Enemy;
using UnityEngine.AI;
using UnityEngine.UI;

[System.Serializable]
public class Anim
{
    public AnimationClip Idle; //기본
    public AnimationClip Attack; //공격
    public AnimationClip Move; //이동
    public AnimationClip Dead; //사망
    public AnimationClip Show; //첫 등장
    public AnimationClip Hit;
}
[RequireComponent(typeof(AudioSource))]


public class Monster : MonsterManager
{
    /* 이스크립트는 Enemy의 AI이다.
     * 현재 애니메이션 완료, 이동 완료,
     * 추가해야 할 것
     * 스탯 
     * Atk함수, Hit함수
     * 
     * 플레이어 화면에 보여졌을 때 플레이어를 향해 이동하고 그 외에는 idle 상태로 정지...
     * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * * 
     * 애니메이션 관련 오류가 났을 경우 모델 오브젝트에서 Rig->Animation Type을 Legacy로 변경
     */

    [Space(3)]
    [Header("몬스터 스탯")]
    [Tooltip("몬스터 체력")]
    public float monsterHp;
    public float monsterCurHp;  //현재 피
    public Image hpBar;

    [Tooltip("몬스터 이동속도")]
    [Range(0, 10)] public float monsterSpeed = 1f;
    [Tooltip("몬스터 공격속도")]
    [Range(0, 10)] public float monsterAttackSpeed = 1.5f;
    [Tooltip("몬스터 대미지")]
    [Range(0, 9999)] public float monsterDamage = 10f;
    [Space(3)]
    [Header("몬스터 상태")]
    public bool isAttack = false;
    public bool isHit = false;
    public bool life = true;
    public float moveSpeed = 2f;

    public Anim anims; //애니메이션 클립
    private Animation _anim; //자신의 애니메이션 컴포넌트
    private int animn;

    public GameObject[] players; //플레이어 (배열선언 파티원들도 찾기위해)
    public Transform playerTarget;
    public float dist1;
    public AudioClip hitSound, deadSound, Groul;
    public GameObject bloodEff;
    public GameObject damageTxt;
    public GameObject explosion;

    private NavMeshAgent nvAgent;
    private AudioSource myAudio;
    private PlayerManager player;

    public enum MODE_STATE { idle = 1, move, attack, die }; //현재 상태정보 저장
    public enum MODE_KIND { Skeleton, Gargoyle, Arachne, Diablo };

    public MODE_STATE enemyMode = MODE_STATE.idle; //Enemy의 상태 셋팅
    public MODE_KIND enemyKind;  //Enemy의 종류 셋팅


    public GameObject Box;
    //네트워크 관련 변수

    //PhotonView 컴포넌트를 할당할 레퍼런스 
    PhotonView pv = null;
    PlayerManager playerManager;
    //위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정 
    Vector3 currPos = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    int net_Aim = 1;

    void Awake()
    {
        monsterHp = 100f;
        player = FindObjectOfType<PlayerManager>();
        damageTxt.SetActive(false);
        _anim = GetComponent<Animation>();
        myAudio = GetComponent<AudioSource>();
        nvAgent = GetComponent<NavMeshAgent>();

        

        //  네트워크 추가w
        pv = GetComponent<PhotonView>();
        pv.ObservedComponents[0] = this;
        pv.synchronization = ViewSynchronization.UnreliableOnChange;
        if (!PhotonNetwork.isMasterClient) //자신이 네트워크 객체가 아닐때 (마스터클라이언트가 아닐때)
        {
        }
        currPos = transform.position;
        currRot = transform.rotation;


    }

    // Start is called before the first frame update
    IEnumerator Start() //생성되면
    {
        //네트워크 추가
        if (PhotonNetwork.isMasterClient) //마스터클라이언트 일때
        {
            //일정 간격으로 주변의 가장 가까운 플레이어를 찾는 코루틴
            StartCoroutine(targetSetting()); //타겟을 찾음

            myAudio.PlayOneShot(Groul);
            //애니메이션 관리
            StartCoroutine(AnimationSet());
            StartCoroutine(StatusSet());
        }
        else    // 마스터 클라이언트가 아닐때 
        {
            StartCoroutine(this.NetAnim());  //네트워크 객체를 일정 간격으로 애니메이션을 동기화 하는 코루틴
        }
        yield return null;
    }

    // Update is called once per frame
    void Update()
    {
        //체력바
        if(enemyKind == MODE_KIND.Diablo)
        {
            hpBar.fillAmount = monsterCurHp / monsterHp;
        }
        
        //네트워크
        if (PhotonNetwork.isMasterClient) //or pv.isMine //자신이 마스터클라이언트일때 실행
        {
            if (!life) //죽었을 때
            {
                EnemyDie();
            }
            if (players.Length != 0) //플레이어가 있으면
            {

                if(10 < dist1 && dist1 < 500f) //플레이어와의 거리가 50이상이면 500이하 일 때
                {
                    transform.LookAt(playerTarget); //플레이어를 바라보고
                    nvAgent.SetDestination(playerTarget.position);

                    //monRb.velocity = (playerTarget.position - transform.position) * (Time.deltaTime * moveSpeed); //벽에 막히면 그대로 있음.

                    animn = 1; //애니메이션 변경
         
                    //보스라면
                    if(enemyKind == MODE_KIND.Diablo)
                    {
                        //거리가 50이하일때
                        if (dist1 < 50f)
                        {
                            //랜덤으로
                            int ai = Random.Range(1, 5);

                            //원거리공격
                            if (ai >= 4)
                            {
                                StartCoroutine(BossAttack(ai));
                            }
                        }
                    }
                }
                else if (dist1 < 10f) //플레이어와 거리가 3이하이면 공격애니메이션 재생
                {
                    if(enemyKind == MODE_KIND.Diablo)
                        StartCoroutine(BossAttack());
                    else
                        StartCoroutine(Attack());
                }
                else if (dist1 > 500f) //거리가 500이상이면
                {
                    animn = 0; //기본자세 애니메이션
                }
            }
        }
        else //자신이 마스터클라이언트가 아닐때 
        {
            //원격 몬스터의 아바타를 수신받은 위치까지 부드럽게 이동시키자
            transform.position = Vector3.Lerp(transform.position, currPos, Time.deltaTime * 3.0f);
            //원격 몬스터의 아바타를 수신받은 각도만큼 부드럽게 회전시키자
            transform.rotation = Quaternion.Slerp(transform.rotation, currRot, Time.deltaTime * 3.0f);
        }

    }

    IEnumerator Attack()
    {
        animn = 2;
        isAttack = true;
        yield return new WaitForSeconds(monsterAttackSpeed);
        isAttack = false;
        yield return null;
    }

    //보스 근접 공격
    IEnumerator BossAttack()
    {
        animn = Random.Range(2,3);
        isAttack = true;
        yield return new WaitForSeconds(monsterAttackSpeed);
        isAttack = false;
        if (animn == 2)
            Debug.Log(animn + "Left Punch");
        else if (animn == 3)
            Debug.Log(animn + "Right Punch");
        yield return null;
    }

    //인수를 전달받는 공격(특정공격을 호출할때 사용)
    IEnumerator BossAttack(int i)
    {
        animn = i;
        isAttack = true;
        yield return new WaitForSeconds(monsterAttackSpeed);
        isAttack = false;
        if (i == 4)
            Debug.Log(i + "Magic Area");
        else if (i == 5)
            Debug.Log(i + "Jump Attack");

        yield return null;
    }

    IEnumerator StatusSet()
    {
        yield return new WaitForSeconds(0.2f);

        switch (enemyKind)
        {
            case (MODE_KIND)1: //스켈레톤
                monsterHp = monsterHp * 1;
                monsterCurHp = monsterHp;
                break;
            case (MODE_KIND)2: //가고일
                monsterHp = (int)(monsterHp * 2);
                monsterCurHp = monsterHp;
                break;
            case (MODE_KIND)3: //아라크네
                monsterHp = (int)(monsterHp * 5);
                monsterCurHp = monsterHp;
                break;
            case (MODE_KIND)4: //디아블로
                monsterHp = (int)(monsterHp * 10);
                monsterCurHp = monsterHp;
                break;
        }
    }

    IEnumerator AnimationSet()
    {
        while (life) //살아 있을 때
        {
            yield return new WaitForSeconds(0.2f); //0.2초 기달렸다가
            net_Aim = animn; // 애니메이션 동기화
            if (animn == 0) //애니메이션들을 실행
            {
                _anim.CrossFade(anims.Idle.name, 0.3f);

            }
            else if (animn == 1)
            {
                _anim.CrossFade(anims.Move.name, 0.3f);

            }
            else if (animn == 2)
            {
                _anim.CrossFade(anims.Attack.name, 0.3f);
            }
            else if (animn == 3)
            {
                _anim.CrossFade(anims.Dead.name, 0.3f);
            }
            else if (animn == 4)
            {
                _anim.CrossFade(anims.Show.name, 0.3f);
            }
            else if (animn == 5)
            {
                _anim.CrossFade(anims.Hit.name, 0.3f);
                // 코루틴 함수를 빠져나감(종료)
                yield break;
            }
        }
    }

    IEnumerator targetSetting() //가까운 플레이어를 찾는 함수
    {
        
        yield return new WaitForSeconds(0.5f); //0.5초 뒤에 몬스터는 생성되고
        players = GameObject.FindGameObjectsWithTag("Player"); //태그로 모든 플레이어를 찾음

        while (true) //반복으로 매번 가까운 플레이어를 찾아감
        {
            yield return new WaitForSeconds(0.3f);

            if (players.Length != 0) //플레이어가 있을 때
            {
                playerTarget = players[0].transform; //첫 번째 플레이어가 기본타겟
                dist1 = (playerTarget.position - transform.position).sqrMagnitude; //몬스터와 플레이어의 거리
                foreach (GameObject _players in players)
                {
                    if ((_players.transform.position - transform.position).sqrMagnitude < dist1)//2~4번째 플레이어와 몬스터의거리가 1번째 플레이어보다 거리가 작다면
                    {
                        playerTarget = _players.transform; //플레이어타겟을 바꾸고
                        dist1 = (playerTarget.position - transform.position).sqrMagnitude; //몬스터와 플레이어의 거리도 변경.
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other) //몬스터 공격범위내에 플레이어가 있을때
    {
        if (other.gameObject.CompareTag("Player") && animn == 2 && isAttack) //태그가 플레이어이고, 공격모션을 취하고 있고, 공격중이라면
        {
            //other.GetComponent<PlayerTestMove>().playerHp -= (int)monsterDamage; 
            ///if(other.GetComponent<PlayerTestMove>().playerHp <= 0)
            {

            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Attack")
        {
            monsterCurHp -= 100;
            StartCoroutine(EnemyHit());

            if (monsterCurHp <= 0)
            {
                life = false;
                myAudio.PlayOneShot(deadSound);
                EnemyDie();
            }
        }
    }

    IEnumerator EnemyHit()
    {
        animn = 5;
        myAudio.PlayOneShot(hitSound);
        Instantiate(bloodEff, transform.position, Quaternion.identity);
        damageTxt.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        damageTxt.SetActive(false);
        yield return null;
    }

    //몬스터 사망
    public void EnemyDie()
    {
        if (life == false)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].GetComponent<PlayerManager>().PlayerExp += 1.0f;
                players[i].GetComponent<PlayerManager>().expBar.fillAmount += players[i].GetComponent<PlayerManager>().PlayerExp * 0.001f;
                players[i].GetComponent<PlayerManager>().expText.text = players[i].GetComponent<PlayerManager>().PlayerExp + "%";
            }
        }
        //StartCoroutine(this.Die());
        life = true;
        // 포톤 추가
        if (pv.isMine)//마스터 클라이언트만 실행
        {
            StartCoroutine(this.Die());
        }
    }

    IEnumerator Die()
    {
        if (enemyKind == MODE_KIND.Diablo)
        {
            // Enemy의를 죽이자

            //죽는 애니메이션 시작
            animn = 7;

            Instantiate(explosion, transform.position, Quaternion.identity);

            //4.5 초후 오브젝트 반환
            yield return new WaitForSeconds(0.5f);

            this.gameObject.SetActive(false);
            Box.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z); //몬스터가 죽은 자리에서 스폰

            StopAllCoroutines(); //객체 반환전 모든 코루틴을 정지

        }
        else
        {

            // Enemy의를 죽이자

            //죽는 애니메이션 시작
            animn = 3;

            Instantiate(explosion, transform.position, Quaternion.identity);
            //Enemy에 추가된 모든 Collider를 비활성화(모든 충돌체는 Collider를 상속했음 따라서 다음과 같이 추출 가능)
            //foreach (Collider coll in gameObject.GetComponentsInChildren<Collider>())
            //{
            //    coll.enabled = false;
            //}

            //4.5 초후 오브젝트 반환
            yield return new WaitForSeconds(0.5f);
            int aaa = Random.Range(1, 10);
            if (aaa > 5)
            {
                var Mons = ItemPooling.GetItem();//오브젝트풀링에서 아이템을 빌려온다.
                Mons.transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z); //몬스터가 죽은 자리에서 스폰
            }
            StopAllCoroutines(); //객체 반환전 모든 코루틴을 정지
            EnemyPooling.ReturnObject(this); //몬스터가 죽으면 자신을 반환

        }
        // 포톤 추가
        // 자신과 네트워크상의 모든 아바타를 삭제

        //PhotonNetwork.Destroy(gameObject); 
    }

    // 마스터 클라이언트가 아닐때 애니메이션 상태를 동기화 하는 로직
    // RPC 로도 애니메이션 동기화 가능~!
    IEnumerator NetAnim()
    {
        while (life)
        {
            yield return new WaitForSeconds(0.2f);

            if (!PhotonNetwork.isMasterClient)
            {
                if (net_Aim == 0)
                {
                    _anim.CrossFade(anims.Idle.name, 0.3f);
                }
                else if (net_Aim == 1)
                {
                    _anim.CrossFade(anims.Move.name, 0.3f);
                }
                else if (net_Aim == 2)
                {
                    _anim.CrossFade(anims.Attack.name, 0.3f);
                }
                else if (net_Aim == 3)
                {
                    _anim.CrossFade(anims.Dead.name, 0.3f);
                }
                else if (net_Aim == 4)
                {
                    _anim.CrossFade(anims.Show.name, 0.3f);
                    // 코루틴 함수를 빠져나감(종료)
                    yield break;
                }
            }
        }
    }

    // 포톤 네트워크
    // PhotonView 컴포넌트의 Observe 속성이 스크립트 컴포넌트로 지정되면 PhotonView
    // 컴포넌트는 데이터를 송수신할 때, 해당 스크립트의 OnPhotonSerializeView 콜백 함수를 호출한다. 

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //로컬 플레이어의 위치 정보를 송신
        if (stream.isWriting)
        {
            //박싱
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(net_Aim);
        }
        //원격 플레이어의 위치 정보를 수신
        else
        {
            //언박싱
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            net_Aim = (int)stream.ReceiveNext();
        }

    }
    // 마스터 클라이언트가 변경되면 호출
    void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (PhotonNetwork.isMasterClient) //마스터클라이언트 일때
        {
            // 일정 간격으로 주변의 가장 가까운 플레이어를 찾는 코루틴 
            StartCoroutine(targetSetting()); //타겟을 찾음
            //애니메이션 관리
            StartCoroutine(AnimationSet());
        }
        else    // 마스터 클라이언트가 아닐때 
        {
            StartCoroutine(this.NetAnim());  //네트워크 객체를 일정 간격으로 애니메이션을 동기화 하는 코루틴
        }
    }
}
