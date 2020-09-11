using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class ItemCreateManager : MonoBehaviour
{
    //PF = Prefab

    public GameObject hpPotionPF;
    public GameObject glovePF;
    public GameObject beltPF;
    public GameObject bodyPF;
    public GameObject hatPF;
    public GameObject pantsPF;
    public GameObject footPF;
    public GameObject ringPF;
    public GameObject weapon0;
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    //public GameObject weapon5;

    //void Awake()
    //{
    //    itemCreateManager = GameObject.FindGameObjectWithTag("ItemCretor").GetComponent<ItemCreateManager>();
    //}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            RandItemCreate(PlayerManager.instance.transform.position);
            //itemCreator = new ItemCreator(RandItemCreate);
        }
    }

    // 해당 위치에 랜덤한 아이템 생성
    public void RandItemCreate(Vector3 pos)
    {
        StartCoroutine(RandItem(pos));
    }

    private IEnumerator RandItem(Vector3 pos)
    {
        ItemManager.ItemKind tempKind = (ItemManager.ItemKind)Random.Range(0, (int)ItemManager.ItemKind.ALL);

        if(tempKind == ItemManager.ItemKind.use)
        {
            DropItem dorps;
            //체력 포션
            GameObject temp = Instantiate(hpPotionPF, pos, Quaternion.identity);
            dorps = temp.GetComponent<DropItem>();
            dorps.isHpPotion = true;

            yield break;

        }

        ItemManager.Rarity tempRearity = (ItemManager.Rarity)Random.Range(0, (int)ItemManager.Rarity.ALL);
        GameObject tempPF = null;
        int tempLevel = PlayerManager.instance.Level;
        string tempName = null;
        float amplify = 0;
        //레벨은 현제 레벨보다 크거나 작거나 같도록 해야함
        tempLevel = Random.Range(0, 2) > 0 ? tempLevel + 5 : tempLevel;
        //템 레벨은 5렙단위로 끊어지도록
        tempLevel -= tempLevel % 5;
        //템 레벨의 최대치 70
        tempLevel = tempLevel > 70 ? 70 : tempLevel;

        //아이템 등급에 따른 배율 + -10% ~ +10간의 랜덤한 스텟
        switch (tempRearity)
        {
            case ItemManager.Rarity.normal:
                amplify = 1f + ((Random.Range(0, 21) - 10) / 100f);
                tempName = "커먼 ";
                break;
            case ItemManager.Rarity.magic:
                amplify = 1.1f + ((Random.Range(0, 21) - 10) / 100f);
                tempName = "언커먼 ";

                break;
            case ItemManager.Rarity.unique:
                amplify = 1.3f + ((Random.Range(0, 21) - 10) / 100f);
                tempName = "희귀 ";
                break;
            case ItemManager.Rarity.legendary:
                amplify = 1.5f + ((Random.Range(0, 21) - 10) / 100f);
                tempName = "전설 ";
                break;
        }

        switch (tempKind)
        {
            case ItemManager.ItemKind.LHand:
            case ItemManager.ItemKind.RHand:
                tempKind = ItemManager.ItemKind.LHand;
                //무기 생성
                switch (Random.Range(0, 4))
                {
                    case 0:
                        tempPF = weapon0;
                        break;

                    case 1:
                        tempPF = weapon1;
                        break;

                    case 2:
                        tempPF = weapon2;
                        break;

                    case 3:
                        tempPF = weapon3;
                        break;

                    //case 4:
                    //    tempPF = weapon5;
                    //    break;

                }

                tempName += "한손 무기";
                break;

            case ItemManager.ItemKind.Hat:
                tempPF = hatPF;
                tempName += "모자";
                break;

            case ItemManager.ItemKind.Body:
                tempPF = bodyPF;
                tempName += "갑옷";
                break;

            case ItemManager.ItemKind.Belt:
                tempPF = beltPF;
                tempName += "벨트";
                break;

            case ItemManager.ItemKind.Pants:
                tempPF = pantsPF;
                tempName += "바지";
                break;

            case ItemManager.ItemKind.Foot:
                tempPF = pantsPF;
                tempName += "신발";
                break;

            case ItemManager.ItemKind.Glove:
                tempPF = glovePF;
                tempName += "장갑";
                break;

            case ItemManager.ItemKind.LRing:
                tempPF = ringPF;
                tempName += "반지";
                break;
                
        }

        DropItem drop = Instantiate(tempPF, pos + (Vector3.up * 2f), Quaternion.identity).GetComponent<DropItem>();
        drop.needLevel = tempLevel;
        drop.rarity = tempRearity;
        if(tempKind == ItemManager.ItemKind.LHand)
        {
            drop.damage = Mathf.RoundToInt(tempLevel * 20 * amplify);
        }
        else
        {
            drop.defense = Mathf.RoundToInt(tempLevel * 10 * amplify);
        }
        drop.itemName = tempName;

        yield return null;

    }

    //아이템을 버릴 시엔..
    public void DropItemCreate(ItemManager item)
    {
        StartCoroutine(DropItem(item));
    }

    private IEnumerator DropItem(ItemManager item)
    {
        GameObject temp = null;

        switch (item.itemKind)
        {
            case ItemManager.ItemKind.use:

                if (item.isHpPotion)
                {
                    temp = hpPotionPF;
                }
                Instantiate(temp, PlayerManager.instance.transform.position,Quaternion.identity);

                yield break;

            case ItemManager.ItemKind.LHand:

                switch (item.weaponNum)
                {
                    case 0:
                        temp = weapon0;

                        break;
                    case 1:
                        temp = weapon1;

                        break;
                    case 2:
                        temp = weapon2;

                        break;
                    case 3:
                        temp = weapon3;

                        break;
                    //case 4:
                    //    temp = weapon5;

                    //    break;
                }

                break;
                //모자
            case ItemManager.ItemKind.Hat:
                temp = hatPF;
                break;
                //상의
            case ItemManager.ItemKind.Body:
                temp = bodyPF;
                break;
                //벨트
            case ItemManager.ItemKind.Belt:
                temp = beltPF;
                break;
                //바지
            case ItemManager.ItemKind.Pants:
                temp = pantsPF;
                break;
                //신발
            case ItemManager.ItemKind.Foot:
                temp = footPF;
                break;
                //장갑
            case ItemManager.ItemKind.Glove:
                temp = glovePF;
                break;
                //반지(반지도 일단 하나만 연결)
            case ItemManager.ItemKind.LRing:
                temp = ringPF;
                break;

                
        }

        DropItem drop = Instantiate(temp, PlayerManager.instance.transform.position, Quaternion.identity).GetComponent<DropItem>();

        drop.itemName = item.itemName;
        drop.damage = item.damage;
        drop.defense = item.defense;
        drop.weaponNum = item.weaponNum;
        drop.needLevel = item.needLevel;
        drop.rarity = item.rarity;
    }
}
