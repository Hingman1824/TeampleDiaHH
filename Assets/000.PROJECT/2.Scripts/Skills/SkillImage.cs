using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillImage : MonoBehaviour
{
    public Sprite[] monkSkillImg;
    public Sprite[] BabaSkillImg;
    public Image[] SkillImg;
    public MonkSkill monk;
    public BarbarianSkill barba;
    PhotonView pv;
    public bool whiriwindoff = false;

    void Awake()
    {
        monk = GameObject.FindGameObjectWithTag("Player").GetComponent<MonkSkill>();
        barba = GameObject.FindGameObjectWithTag("Player").GetComponent<BarbarianSkill>();

        pv = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {        
        if (PhotonNetwork.player.UserId == "Monk")
        {
            for(int i = 0;i<3;i++)
                SkillImg[i].sprite = monkSkillImg[i];
        }
        else if (PhotonNetwork.player.UserId == "Babarian")
        {
            for (int i = 0; i < 3; i++)
                SkillImg[i].sprite = BabaSkillImg[i];
        }
    }

    public void OnClickQSkill()
    {
        if (PhotonNetwork.player.UserId == "Monk")
            StartCoroutine(monk.WaveOfLight());
        else if (PhotonNetwork.player.UserId == "Babarian")
            StartCoroutine(barba.WhirlWindOn());
        
    }

    public void OnClickESkill()
    {
        if (PhotonNetwork.player.UserId == "Monk")
            StartCoroutine(monk.CycloneStrike());
        else if (PhotonNetwork.player.UserId == "Babarian")
            StartCoroutine(barba.BattleRage());

    }

    public void OnClickRSkill()
    {
        if (PhotonNetwork.player.UserId == "Monk")
            StartCoroutine(monk.DashingStrike());
        else if(PhotonNetwork.player.UserId == "Babarian")
            StartCoroutine(barba.FuriousCharge());
    }

    public void OnClickAttack()
    {
        if (PhotonNetwork.player.UserId == "Monk")
        {
            StartCoroutine(monk.MantraOfEvasion());
        }
        else if (PhotonNetwork.player.UserId == "Babarian")
            StartCoroutine(barba.Rend());
    }

}