using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager instance;

    private PlayerManager playermanager; //디아블로3 플레이어 test???
    private UIManager uiMng;

    public Transform uipanel;
    private Transform invTr;

    public RectTransform glove;
    public RectTransform lRing;
    public RectTransform belt;
    public RectTransform rRing;
    public RectTransform foot;
    public RectTransform lHand;
    public RectTransform rHand;
    public RectTransform body;
    public RectTransform pants;
    public RectTransform hat;

    private int slotSize;

    public GameObject[] weapons;

    private void Awake()
    {
        instance = this;
        invTr = uipanel.Find("InvenImg").transform;
        playermanager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }
    
    void Start()
    {
        uiMng = UIManager.instance;
        slotSize = uiMng.slotSize;
    }

    public void DropEquipment(int x,int y, int xSize, int ySize, ref ItemManager item) 
    {
        int startX = 0;
        int startY = 0;
        int endX = 0;
        int endY = 0;
        bool pass = false;
        bool swap = false;

        RectTransform parentTr;
        parentTr = new RectTransform();

        int itemEndX = x + (slotSize * xSize);
        int itemEndY = y + (slotSize * ySize);

        switch (item.itemKind)
        {
            case ItemManager.ItemKind.use:
                return;

            case ItemManager.ItemKind.LHand:
                startX = uiMng.equipments.lHand.startX;
                startY = uiMng.equipments.lHand.startY;
                endX = uiMng.equipments.lHand.endX;
                endY = uiMng.equipments.lHand.endY;
                parentTr = lHand;

                break;
            case ItemManager.ItemKind.RHand:
                startX = uiMng.equipments.rHand.startX;
                startY = uiMng.equipments.rHand.startY;
                endX = uiMng.equipments.rHand.endX;
                endY = uiMng.equipments.rHand.endY;
                parentTr = rHand;

                break;

            case ItemManager.ItemKind.Hat:
                startX = uiMng.equipments.hat.startX;
                startY = uiMng.equipments.hat.startY;
                endX = uiMng.equipments.hat.endX;
                endY = uiMng.equipments.hat.endY;
                parentTr = hat;

                break;

            case ItemManager.ItemKind.Body:
                startX = uiMng.equipments.body.startX;
                startY = uiMng.equipments.body.startY;
                endX = uiMng.equipments.body.endX;
                endY = uiMng.equipments.body.endY;
                parentTr = body;

                break;

            case ItemManager.ItemKind.Belt:
                startX = uiMng.equipments.belt.startX;
                startY = uiMng.equipments.belt.startY;
                endX = uiMng.equipments.belt.endX;
                endY = uiMng.equipments.belt.endY;
                parentTr = belt;

                break;

            case ItemManager.ItemKind.Glove:
                startX = uiMng.equipments.glove.startX;
                startY = uiMng.equipments.glove.startY;
                endX = uiMng.equipments.glove.endX;
                endY = uiMng.equipments.glove.endY;
                parentTr = glove;

                break;

            case ItemManager.ItemKind.Pants:
                startX = uiMng.equipments.pants.startX;
                startY = uiMng.equipments.pants.startY;
                endX = uiMng.equipments.pants.endX;
                endY = uiMng.equipments.pants.endY;
                parentTr = pants;

                break;

            case ItemManager.ItemKind.RRing:
                if (x > ((uiMng.screenH / 9) * 7.375f) / 2)
                    //오른쪽일때
                    startX = uiMng.equipments.rRing.startX;
                    startY = uiMng.equipments.rRing.startY;
                    endX = uiMng.equipments.rRing.endX;
                    endY = uiMng.equipments.rRing.endY;
                    parentTr = rRing;

                    break;
            case ItemManager.ItemKind.LRing:
                startX = uiMng.equipments.lRing.startX;
                startY = uiMng.equipments.lRing.startY;
                endX = uiMng.equipments.lRing.endX;
                endY = uiMng.equipments.lRing.endY;
                parentTr = lRing;

                break;

            case ItemManager.ItemKind.Foot:
                startX = uiMng.equipments.foot.startX;
                startY = uiMng.equipments.foot.startY;
                endX = uiMng.equipments.foot.endX;
                endY = uiMng.equipments.foot.endY;
                parentTr = foot;

                break;
        }

        if ((x > startX && x < endX && y > startY && y < endY)
            || (x > startX && x < endX && itemEndY > startY && itemEndY < endY)
            || (itemEndX > startX && itemEndX < endX && itemEndY > startY && itemEndY < endY)
            || (itemEndX > startX && itemEndX < endX && y > startY && y < endY))
        {
            Debug.Log("착용이 가능");
            pass = true;

        }

        if (!pass) return;

        if(parentTr.childCount > 0)
        {
            ItemManager temp = parentTr.GetChild(0).GetComponent<ItemManager>();
            temp.isMouseTracking = true;
            temp.isEquipment = false;
            temp.transform.parent = invTr;
            temp.itemImageBack.enabled = false;
            swap = true;
        }

        item.transform.parent = parentTr;
        item.isMouseTracking = false;

        item.transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        item.isEquipment = true;
        item.EquipmentItemImgSet();

        if (!swap)
            InventoryManager.instance.isItemDrag = false;

        //ItemStatusSet(); 아래 함수구현 추후 주석 제거할 것!

    }

    //우클릭 착용시
    public void ClickEquipment(ref ItemManager item)
    {
        RectTransform parentTr;
        parentTr = new RectTransform();
        bool isRing = false;
        bool isWeapon = false;

        switch (item.itemKind)
        {
            case ItemManager.ItemKind.LHand:
            case ItemManager.ItemKind.RHand:
                parentTr = lHand;
                isWeapon = true;
                
                break;

            case ItemManager.ItemKind.Hat:
                parentTr = hat;

                break;
            case ItemManager.ItemKind.Body:
                parentTr = body;

                break;
            case ItemManager.ItemKind.Belt:
                parentTr = belt;

                break;
            case ItemManager.ItemKind.Glove:
                parentTr = glove;

                break;

            case ItemManager.ItemKind.Pants:
                parentTr = pants;

                break;

            case ItemManager.ItemKind.LRing:
                parentTr = lRing;
                isRing = true;

                break;

            case ItemManager.ItemKind.Foot:
                parentTr = foot;

                break;

            case ItemManager.ItemKind.use:
                
                return;
        }

        if (isRing)
        {
            if (lRing.childCount > 0)
            {
                parentTr = rRing;
            }
            else if (rRing.childCount > 0)
            {
                parentTr = lRing;
            }
        }

        if(parentTr.childCount > 0)
        {
            ItemManager temp = parentTr.GetChild(0).GetComponent<ItemManager>();
            if (!temp.InventoryAutoInput())
            {
                return;
            }
            temp.isEquipment = false;
            temp.transform.parent = invTr;

            temp.ItemReturnPosition();
            temp.DontEquipmentItemImageSet();
        }

        item.isEquipment = true;
        item.transform.parent = parentTr;
        item.transform.localPosition = Vector2.zero;
    }

    public void WeaponSet(int weaponNum = 0)
    {
        for(int i = 0; i < 3; i++)
        {
            weapons[i].SetActive(false);
        }

        if(lHand.childCount > 0)
        {
            weapons[weaponNum].SetActive(true);
        }
    }

    // 아이템 종류에 따른 부모 객체
    public RectTransform GetRectTr(ItemManager.ItemKind kinds)
    {
        RectTransform parentTr;
        parentTr = new RectTransform();

        switch (kinds)
        {
            case ItemManager.ItemKind.LHand:
                parentTr = lHand;
                break;

            case ItemManager.ItemKind.RHand:
                parentTr = rHand;
                break;

            case ItemManager.ItemKind.Hat:
                parentTr = hat;
                break;

            case ItemManager.ItemKind.Body:
                parentTr = body;
                break;

            case ItemManager.ItemKind.Pants:
                parentTr = pants;
                break;

            case ItemManager.ItemKind.Belt:
                parentTr = belt;
                break;

            case ItemManager.ItemKind.Foot:
                parentTr = foot;
                break;

            case ItemManager.ItemKind.Glove:
                parentTr = glove;
                break;

            case ItemManager.ItemKind.LRing:
                parentTr = lRing;
                break;
        }

        return parentTr;
    }

    public void ItemStatusSet()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("EquipmentSpace");
        Transform[] _tempTr = temp.GetComponentsInChildren<Transform>();

        int _tempAtt = 0;
        int _tempDef = 0;

        for (int i = 1; i < _tempTr.Length; i++)
        {
            //착용한게 없다면 컨티뉴
            if (_tempTr[i].childCount < 1) continue;

            _tempAtt += _tempTr[i].GetComponentInChildren<ItemManager>().damage;
            _tempDef += _tempTr[i].GetComponentInChildren<ItemManager>().defense;
        }

        //player.SetStatus(_tempAtt, _tempDef);
    }

}
