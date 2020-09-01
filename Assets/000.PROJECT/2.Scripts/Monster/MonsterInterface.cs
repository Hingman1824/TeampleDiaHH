using System.Collections;
using UnityEngine;

namespace Enemy
{
    interface IMonsterStatus
    {
        bool MonsterLife { get; set; }
        int MonsterHP { get; }
        float MonsterSpeed { get; }
        float MonsterAttackSpeed { get; }
        float MonsterDamage { get; }
    }
    interface IMonsterAnimation
    {
        int Animn { get; set; }

        int Net_Animn { get; set; }
        Animation _anim { get; set; }

        Anim anims { get; }

        IEnumerator MonsterAnimation();
        IEnumerator MonsterAnimationNetwork();
    }
    interface IMonsterMove
    {
        float dist1 { get; set; }
        Rigidbody monRb { get; set; }

        GameObject[] players { get; set; }

        Transform playerTarget { get; set; }
        void MonsterMove(); //찾은 플레이어에게 이동
        IEnumerator TargetSetting(); //플레이어를 찾기 위해
    }
    interface IMonsterAttack
    {

        void MonsterAttack();
    }
    interface IMonsterDead
    {
        void MonsterDead();
    }
    interface IMonsterHit
    {
        void MonsterHit();
    }
}