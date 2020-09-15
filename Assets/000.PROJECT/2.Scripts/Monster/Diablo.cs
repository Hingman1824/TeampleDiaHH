using System.Collections;
using UnityEngine;
using Enemy;
using UnityEngine.AI;
using UnityEngine.UI;

public class Diablo : MonsterManager
{
    private SmoothFollow smoothFollow;

    public GameObject playerTransform;

    [Tooltip("몬스터 체력")]
    public float monsterHp;
    public float monsterCurHp;  //현재 피
    public Image hpBar;

    public float monsterAttackSpeed = 1.5f;
    
    public float monsterDamage = 10f;

    [Space(3)]
    [Header("몬스터 상태")]
    public bool isAttack = false;
    public bool move = false;
    public bool life = true;

    public bool hit = false;

    private Animator animator; //자신의 애니메이터 컴포넌트
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

    public GameObject JumpAttackEffect;
    public GameObject MagicAreaEffect;

    //public GameObject Box;
    //네트워크 관련 변수

    //PhotonView 컴포넌트를 할당할 레퍼런스 
    PhotonView pv = null;

    //위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정 
    Vector3 currPos = Vector3.zero;
    Quaternion currRot = Quaternion.identity;
    int net_Aim = 1;


    public enum KIND { Diablo1, Diablo2, Diablo3 };

    public GameObject spawn2;
    public GameObject spawn3;

    public int num;

    [SerializeField]
    KIND kind;


    public Transform magic;


    private MoveChoice MapMove;
    void Awake()
    {
        MapMove = GameObject.Find("AcceptBtn").GetComponent<MoveChoice>();
        num = 0;

        smoothFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>();

        monsterHp = 3000f;
        monsterCurHp = monsterHp;

        player = FindObjectOfType<PlayerManager>();
        damageTxt.SetActive(false);
        animator = GetComponent<Animator>();
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
            //StartCoroutine(AnimationSet());
        }
        else    // 마스터 클라이언트가 아닐때 
        {
            StartCoroutine(this.NetAnim());  //네트워크 객체를 일정 간격으로 애니메이션을 동기화 하는 코루틴
        }
        yield return new WaitForSeconds(1f);

        //Box.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        hpBar.fillAmount = monsterCurHp / monsterHp;
        
        if(kind == KIND.Diablo1)
        {
            if (monsterCurHp <= 1500)
            {
                if(num == 0)
                {
                    player.transform.position = spawn2.transform.position;
                    this.monsterCurHp = 3000;
                    MapMove.MapMove2();
                }
            }
        }
        if (kind == KIND.Diablo2)
        {
            if (monsterCurHp <= 100)
            {
                if(num == 0)
                {
                    player.transform.position = spawn3.transform.position;
                    //

                    this.monsterCurHp = 3000;
                    life = true;
                    MapMove.MapMove3();
                    this.enabled = true;
                }
            }
        }

            //네트워크
            if (PhotonNetwork.isMasterClient) //or pv.isMine //자신이 마스터클라이언트일때 실행
        {
            if (!life) //죽었을 때
            {
                //EnemyDie();
            }
            else if (players.Length != 0) //플레이어가 있으면
            {
                if (100 <= dist1 && dist1 < 500f) //플레이어와의 거리가 50이상이면 500이하 일 때
                {
                    if (move == false)
                        MoveToPlayer();
                }
                //거리가 50이하일때
                else if (10 <= dist1 && dist1 < 100f) 
                {
                    //랜덤으로
                    int ai = Random.Range(2, 7);

                    //원거리공격
                    if (ai == 4 || ai == 5)
                    {
                        if (isAttack == false)
                            StartCoroutine(BossAttack(ai));
                    }
                    else
                    {
                        if (move == false)
                            MoveToPlayer();
                    }

                }
                else if (dist1 < 10f) //플레이어와 거리가 3이하이면 공격애니메이션 재생
                {
                    if (isAttack == false)
                        StartCoroutine(BossAttack());
                }
                else if (dist1 > 500f) //거리가 500이상이면
                {
                    animn = 0; //기본자세 애니메이션
                    animator.SetBool("Move", false);
                    animator.SetBool("Left Punch", false);
                    animator.SetBool("Right Punch", false);
                    animator.SetBool("Magic Area", false);
                    animator.SetBool("Jump Attack", false);
                    animator.SetBool("Hit", false);
                    animator.SetBool("Die", false);
                    if(this.transform.position.y >=1)
                    {
                        Vector3 pos = this.transform.position;
                        pos.y = 0;
                        this.transform.position = pos;
                    }

                    move = false;
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

    //플레이어에게 접근
    void MoveToPlayer()
    {
        if (move == false)
        {
            transform.LookAt(playerTarget); //플레이어를 바라보고
            nvAgent.SetDestination(playerTarget.position);

            //monRb.velocity = (playerTarget.position - transform.position) * (Time.deltaTime * moveSpeed); //벽에 막히면 그대로 있음.

            animn = 1; //애니메이션 변경
            animator.SetBool("Move", true);
            move = true;
        }
    }

    //보스 근접 공격
    IEnumerator BossAttack()
    {
        if (isAttack == true)
            yield return null;
        else if( isAttack == false)
        {
            isAttack = true;

            int ai = Random.Range(2, 4);

            animn = ai;

            animator.SetBool("Move", false);

            if (ai == 2)
            {
                animator.SetBool("Left Punch", true);
                animator.SetBool("Right Punch", false);
                animator.SetBool("Magic Area", false);
                animator.SetBool("Jump Attack", false);
                yield return new WaitForSeconds(monsterAttackSpeed);
                animator.SetBool("Left Punch", false);
                yield return null;
            }
            if (ai == 3)
            {
                
                animator.SetBool("Left Punch", false);
                animator.SetBool("Right Punch", true);
                animator.SetBool("Magic Area", false);
                animator.SetBool("Jump Attack", false);
                yield return new WaitForSeconds(monsterAttackSpeed);
                animator.SetBool("Right Punch", false);
                yield return null;
            }
            isAttack = false;
            move = false;
        }       
    }

    //인수를 전달받는 공격(특정공격을 호출할때 사용)
    IEnumerator BossAttack(int i)
    {
        if (isAttack == true)
            yield return null;
        else if (isAttack == false)
        {
            isAttack = true;

            animator.SetBool("Move", false);

            if (i == 2)
            {
                animator.SetBool("Left Punch", true);
                animator.SetBool("Right Punch", false);
                animator.SetBool("Magic Area", false);
                animator.SetBool("Jump Attack", false);
                yield return new WaitForSeconds(monsterAttackSpeed);
                animator.SetBool("Left Punch", false);
            }
            if (i == 3)
            {
                animator.SetBool("Left Punch", false);
                animator.SetBool("Right Punch", true);
                animator.SetBool("Magic Area", false);
                animator.SetBool("Jump Attack", false);
                yield return new WaitForSeconds(monsterAttackSpeed);
                animator.SetBool("Right Punch", false);
            }
            if (i == 4)
            {
                animator.SetBool("Left Punch", false);
                animator.SetBool("Right Punch", false);
                animator.SetBool("Magic Area", true);
                animator.SetBool("Jump Attack", false);

                yield return new WaitForSeconds(1.15f);
                var Effect = Instantiate(MagicAreaEffect, magic);

                Effect.transform.SetParent(this.transform.parent);

                yield return new WaitForSeconds(0.3f);
                animator.SetBool("Magic Area", false);

                yield return new WaitForSeconds(2f);
                Destroy(GameObject.FindGameObjectWithTag("Effect"));
            }
            if (i == 5)
            {
                animator.SetBool("Left Punch", false);
                animator.SetBool("Right Punch", false);
                animator.SetBool("Magic Area", false);
                animator.SetBool("Jump Attack", true);

                yield return new WaitForSeconds(1.5f);

                Transform tra = this.transform;
                Vector3 pos1 = this.transform.position;
                pos1.y += 1;
                tra.position = pos1;

                
                var Effect = Instantiate(JumpAttackEffect, tra) as GameObject;

                Effect.transform.parent = this.transform.parent.transform;
                yield return new WaitForSeconds(0.3f);

                //Destroy(GameObject.FindGameObjectWithTag("Effect"));

                if (this.transform.position.y >= 1)
                {
                    Vector3 pos2 = this.transform.position;
                    pos2.y = 0;
                    this.transform.position = pos2;
                }

                yield return new WaitForSeconds(0.2f);
                animator.SetBool("Jump Attack", false);
            }

            animn = i;

            isAttack = false;

            move = false;
            yield return null;
        }
            
        
    }

    //IEnumerator AnimationSet()
    //{
    //    while (life) //살아 있을 때
    //    {
    //        yield return new WaitForSeconds(0.2f); //0.2초 기달렸다가
    //        net_Aim = animn; // 애니메이션 동기화
    //        if (animn == 0) //애니메이션들을 실행
    //        {
    //            Debug.Log("여기타냐?");
    //            //_anim.CrossFade(anims.Idle.name, 0.3f);
    //            animator.SetBool("Move", false);
    //            animator.SetBool("Left Punch", false);
    //            animator.SetBool("Right Punch", false);
    //            animator.SetBool("Magic Area", false);
    //            animator.SetBool("Jump Attack", false);
    //            animator.SetBool("Hit", false);
    //            animator.SetBool("Die", false);
    //        }
    //        else if (animn == 1)
    //        {
    //            //_anim.CrossFade(anims.Move.name, 0.3f);
    //            animator.SetBool("Move", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Move", false);
    //        }
    //        else if (animn == 2)
    //        {
    //            //_anim.CrossFade(anims.Attack.name, 0.3f);
    //            animator.SetBool("Left Punch", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Left Punch", false);
    //        }
    //        else if (animn == 3)
    //        {
    //            //_anim.CrossFade(anims.Dead.name, 0.3f);
    //            animator.SetBool("Right Punch", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Right Punch", false);
    //        }
    //        else if (animn == 4)
    //        {
    //            //_anim.CrossFade(anims.Show.name, 0.3f);
    //            animator.SetBool("Magic Area", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Magic Area", false);
    //        }
    //        else if (animn == 5)
    //        {
    //            //_anim.CrossFade(anims.Hit.name, 0.3f);
    //            animator.SetBool("Jump Attack", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Jump Attack", false);
    //        }
    //        else if (animn == 6)
    //        {
    //            //_anim.CrossFade(anims.Hit.name, 0.3f);
    //            animator.SetBool("Hit", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Hit", false);
    //        }
    //        else if (animn == 7)
    //        {
    //            animator.SetBool("Die", true);
    //            yield return new WaitForSeconds(1f);
    //            animator.SetBool("Die", false);
    //            // 코루틴 함수를 빠져나감(종료)
    //            yield break;
    //        }
    //    }
    //}

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
        if (other.gameObject.CompareTag("Player") && isAttack) //태그가 플레이어이고, 공격모션을 취하고 있고, 공격중이라면
        {
            if(animn == 2 || animn == 3 || animn == 4 || animn == 5)
            {
                //other.GetComponent<PlayerTestMove>().playerHp -= (int)monsterDamage; 
                ///if(other.GetComponent<PlayerTestMove>().playerHp <= 0)
                {

                }
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if(life == true)
        {
            if (col.gameObject.tag == "Attack")
            {
                if (hit == false)
                {
                    hit = true;
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
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (life == true)
        {
            if (col.gameObject.tag == "Attack")
            {
                if (hit == false)
                {
                    hit = true;
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
        }
    }

    IEnumerator EnemyHit()
    {
        animn = 6;
        animator.SetBool("Move", false);
        animator.SetBool("Hit", true);
        myAudio.PlayOneShot(hitSound);
        Instantiate(bloodEff, transform.position, Quaternion.identity);
        damageTxt.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hit = false;
        yield return new WaitForSeconds(1.5f);
        animator.SetBool("Hit", false);
        damageTxt.SetActive(false);
        move = false;
        yield return null;
    }

    //몬스터 사망
    public void EnemyDie()
    {
        this.GetComponent<CapsuleCollider>().enabled = false;
        this.GetComponent<NavMeshAgent>().enabled = false;

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
        //life = true;
        // 포톤 추가
        if (pv.isMine)//마스터 클라이언트만 실행
        {
            StartCoroutine(this.Die());
        }
    }

    IEnumerator Die()
    {
        // Enemy의를 죽이자

        //죽는 애니메이션 시작
        
        animator.SetBool("Move", false);
        animator.SetBool("Left Punch", false);
        animator.SetBool("Right Punch", false);
        animator.SetBool("Magic Area", false);
        animator.SetBool("Jump Attack", false);
        animator.SetBool("Hit", false);
        animator.SetBool("Die", true);
        animn = 7;

        Instantiate(explosion, transform.position, Quaternion.identity);

        //4.5 초후 오브젝트 반환
        yield return new WaitForSeconds(1.5f);

        this.gameObject.SetActive(false);
        //Box.SetActive(true);

        StopAllCoroutines(); //객체 반환전 모든 코루틴을 정지
    }

    //점프어택시 이벤트 호출
    //IEnumerator JumpAttackEvent()
    //{
    //    Transform tra = this.transform;
    //    Vector3 pos = this.transform.position;
    //    pos.y = 0;
    //    tra.position = pos;
    //    //GameObject clone = JumpAttackEffect;
    //    smoothFollow.CameraShake();
    //    Instantiate(JumpAttackEffect, tra);
    //    yield return new WaitForSeconds(0.1f);
    //    smoothFollow.CameraShake();
    //    yield return new WaitForSeconds(0.1f);
    //    smoothFollow.CameraShake();
    //    yield return new WaitForSeconds(0.1f);
    //    smoothFollow.CameraShake();
    //    yield return new WaitForSeconds(0.1f);
    //    //Destroy(GameObject.FindGameObjectWithTag("Effect"));
    //}

    ////스킬공격시 이벤트 호출
    //IEnumerator MagicAreaEvent()
    //{
    //    Instantiate(MagicAreaEffect, this.transform);
    //    yield return new WaitForSeconds(1.5f);
    //    Destroy(GameObject.FindGameObjectWithTag("Effect"));
    //}



    //카메라 무빙, (카메라 스크립트가 카메라를 쥐고있어서 못함 -> 카메라 스크립트에 이 함수를 추가해서 사용하는 방식으로 전환)


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
                    //_anim.CrossFade(anims.Idle.name, 0.3f);
                }
                else if (net_Aim == 1)
                {
                    //_anim.CrossFade(anims.Move.name, 0.3f);
                }
                else if (net_Aim == 2)
                {
                    //_anim.CrossFade(anims.Attack.name, 0.3f);
                }
                else if (net_Aim == 3)
                {
                    //_anim.CrossFade(anims.Dead.name, 0.3f);
                }
                else if (net_Aim == 4)
                {
                    //_anim.CrossFade(anims.Show.name, 0.3f);
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
           // StartCoroutine(AnimationSet());
        }
        else    // 마스터 클라이언트가 아닐때 
        {
            StartCoroutine(this.NetAnim());  //네트워크 객체를 일정 간격으로 애니메이션을 동기화 하는 코루틴
        }
    }
}
