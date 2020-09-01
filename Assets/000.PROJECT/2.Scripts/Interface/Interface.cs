using System.Collections;
using UnityEngine;

namespace IPlayer
{
    interface IPlayerMove
    {
        float PlayerH { get; set; }
        float PlayerV { get; set; }
        void PlayerMovement();
    }
    interface IPlayerStats
    {
        float PlayerLife { get; set; }
        int PlayerLevel { get; }
        int PlayerDefense { get; }
        float PlayerExp { get; set; }

        void ShowNickName();
        void ShowOtherNickName();

        float PlayerSpeed { get; }
        float PlayerAttackSpeed { get; }
        float PlayerDamage { get; }
        //void PlayerAttack();
    }
    interface IPlayerAbility
    {
        float PlayerCooldown { get; }
        float PlayerSkillDamage { get; }
        void PlayerSkill();
    }
    interface IPlayerAnimation
    {
        Animator Net_Anim { get; }
        void PlayerMoveAnimation();

        //IEnumerator PlayerAttackAnimation();
    }

    interface ISkillAnimation
    {
        Animator Net_Skill_Anim { get; }

        void SkillAnimation();
    }

    interface IPlayerPhoton
    {
        void OnPhotonInstantiate(PhotonMessageInfo info);
        void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info);
    }
}

