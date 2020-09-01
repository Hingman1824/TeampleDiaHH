using System.Collections;
using UnityEngine;
using Enemy;

public class MonsterManager : MonoBehaviour//, IMonsterAnimation, IMonsterAttack, IMonsterDead, IMonsterHit, IMonsterMove, IMonsterStatus
{
    //public int Animn { get; set; }
    //public int Net_Animn { get; set; }
    //public Animation _anim { get; set; }
    //public Anim anims { get; }

    //public float dist1 { get; set; }
    //public Rigidbody monRb { get; set; }
    //public GameObject[] players { get; set; }
    //public Transform playerTarget { get; set; }

    //public bool MonsterLife { get; set; }
    //public int MonsterHP { get; }
    //public float MonsterSpeed { get; }
    //public float MonsterAttackSpeed { get; }
    //public float MonsterDamage { get; }


    //public PhotonView pv = null;

    ////위치 정보를 송수신할 때 사용할 변수 선언 및 초기값 설정 
    //public Vector3 currPos = Vector3.zero;
    //public Quaternion currRot = Quaternion.identity;

    ////public void Awake()
    ////{
    ////    players = GameObject.FindGameObjectsWithTag("Player"); //태그로 모든 플레이어를 찾음
    ////    _anim = GetComponent<Animation>();
    ////    monRb = GetComponent<Rigidbody>();

    ////    pv = GetComponent<PhotonView>();
    ////    pv.ObservedComponents[0] = this;
    ////    pv.synchronization = ViewSynchronization.UnreliableOnChange;
    ////    if (!PhotonNetwork.isMasterClient) //자신이 네트워크 객체가 아닐때 (마스터클라이언트가 아닐때)
    ////    {
    ////    }
    ////    currPos = transform.position;
    ////    currRot = transform.rotation;
    ////}
    //public IEnumerator MonsterAnimation()
    //{
    //    while (MonsterLife)
    //    {
    //        Net_Animn = Animn;
    //        Debug.Log(Animn);
    //        yield return new WaitForSeconds(0.2f); //0.2초 기달렸다가
    //        if (Animn == 0) //애니메이션들을 실행
    //        {
    //            _anim.CrossFade(anims.Idle.name, 0.3f);
    //        }
    //        else if (Animn == 1)
    //        {
    //            _anim.CrossFade(anims.Move.name, 0.3f);
    //        }
    //        else if (Animn == 2)
    //        {
    //            _anim.CrossFade(anims.Attack.name, 0.3f);
    //        }
    //        else if (Animn == 3)
    //        {
    //            _anim.CrossFade(anims.Dead.name, 0.3f);
    //        }
    //        else if (Animn == 4)
    //        {
    //            _anim.CrossFade(anims.Show.name, 0.3f);
    //            // 코루틴 함수를 빠져나감(종료)
    //            yield break;
    //        }
    //    }
    //    yield return null;
    //}
    //public IEnumerator MonsterAnimationNetwork()
    //{
    //    while (MonsterLife)
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //        if (Net_Animn == 0)
    //        {
    //            _anim.CrossFade(anims.Idle.name, 0.3f);
    //        }
    //        else if (Net_Animn == 1)
    //        {
    //            _anim.CrossFade(anims.Move.name, 0.3f);
    //        }
    //        else if (Net_Animn == 2)
    //        {
    //            _anim.CrossFade(anims.Attack.name, 0.3f);
    //        }
    //        else if (Net_Animn == 3)
    //        {
    //            _anim.CrossFade(anims.Dead.name, 0.3f);
    //        }
    //        else if (Net_Animn == 4)
    //        {
    //            _anim.CrossFade(anims.Show.name, 0.3f);
    //            // 코루틴 함수를 빠져나감(종료)
    //            yield break;
    //        }
    //    }
    //    yield return null;
    //}

    //public void MonsterAttack()
    //{
    //}

    //public void MonsterDead()
    //{
    //}

    //public void MonsterHit()
    //{
    //}

    //public void MonsterMove()
    //{
    //    if (players.Length != 0) //플레이어가 있으면
    //    {

    //        if (dist1 > 10f && dist1 < 500f) //플레이어와의 거리가 3이상이면 500이하 일 때
    //        {
    //            transform.LookAt(playerTarget); //플레이어를 바라보고
    //                                            //monRb.AddForce((playerTarget.position-transform.position) * moveSpeed);
    //            monRb.velocity = (playerTarget.position - transform.position) * (Time.deltaTime * MonsterSpeed); //벽에 막히면 그대로 있음.
    //                                                                                                             //transform.Translate(transform.forward * (Time.deltaTime * moveSpeed), Space.World); //플레이어를 향해 이동
    //            Animn = 1; //애니메이션 변경
    //        }
    //        else if (dist1 < 10f) //플레이어와 거리가 3이하이면 공격애니메이션 재생
    //        {
    //            Animn = 2;
    //        }
    //        else if (dist1 > 500f) //거리가 500이상이면
    //        {
    //            Animn = 0; //기본자세 애니메이션
    //        }
    //    }
    //}
    //public IEnumerator TargetSetting()
    //{
    //    while (MonsterLife)
    //    {
    //        yield return new WaitForSeconds(0.2f);
    //        if (players.Length != 0) //플레이어가 있을 때
    //        {
    //            playerTarget = players[0].transform; //첫 번째 플레이어가 기본타겟
    //            dist1 = (playerTarget.position - transform.position).sqrMagnitude; //몬스터와 플레이어의 거리
    //            foreach (GameObject _players in players)
    //            {
    //                if ((_players.transform.position - transform.position).sqrMagnitude < dist1)//2~4번째 플레이어와 몬스터의거리가 1번째 플레이어보다 거리가 작다면
    //                {
    //                    playerTarget = _players.transform; //플레이어타겟을 바꾸고
    //                    dist1 = (playerTarget.position - transform.position).sqrMagnitude; //몬스터와 플레이어의 거리도 변경.
    //                }
    //            }
    //        }
    //    }
    //}
}
