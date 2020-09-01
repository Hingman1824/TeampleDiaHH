using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    public class UseItems
    {
        public GameObject hpPotion;
        //public GameObject MpPotion;
    }

    public UseItems useitemsPF;

    public bool isOn = false;

    public float sellSpaceStartX;
    public float sellSpaceStartY;
    public float sellSpaceEndX;
    public float sellSpaceEndY;

    private bool isDraging;
    public bool isMouseOver;

    public Vector2 clickPoint;
    private RectTransform uiTr;

    private int slotSize;
    private int screenH;
    private int screenW;

    public RectTransform[] sellItems;

    //private AcceptedManager acceptedManager; 아직 구현안됨

    private GameObject spawnItem;

    public Transform inventory;

    public InventoryManager invMng;

    //private NPCManager npc;

    void Awake()
    {
        uiTr = GetComponent<RectTransform>();
        //npc = GameObject.Find("NPCManager").GetComponent<NPCManager>();
        isOn = true;
        //acceptedManager = GameObject.Find("AcceptedManager").GetComponent<AcceptedManager>();
        invMng = GameObject.Find("InventoryManager").GetComponent<InventoryManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isMouseOver = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton ev = eventData.button;
        switch (ev)
        {
            case PointerEventData.InputButton.Left:
                ItemLeftClick(eventData);
                break;
            case PointerEventData.InputButton.Right:
                ItemRightClick(eventData);
                break;
        }

    }

    void Start()
    {
        isMouseOver = false;
        isDraging = false;

        slotSize = UIManager.instance.slotSize;
        screenH = UIManager.instance.screenH;
        screenW = UIManager.instance.screenW;

        //SellItemsInit();

        uiTr.anchoredPosition = new Vector2(screenW / 5f, screenH / -3f);
        uiTr.sizeDelta = new Vector2(slotSize * 5, slotSize * 6);
    }

    void Update()
    {
        if (isMouseOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                clickPoint = uiTr.position - Input.mousePosition;

                if (CanMove()) isDraging = true;

                // 아이템 구입 확인
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            PositionInit();
            InSellArea(Input.mousePosition);
        }
        if (isDraging)
        {
            ShopUiMove();
        }
    }

    void SellItemsInit()
    {
        int count;
        count = transform.childCount;

        sellItems = new RectTransform[count];

        for(int i = 0; i < count; i++)
        {
            sellItems[i] = transform.GetChild(i).GetComponent<RectTransform>();

            if(sellItems[i].transform.tag == "Untagged")
            {
                sellItems[i].sizeDelta = new Vector2(slotSize / 2f, slotSize / 2f);

                continue;
            }
            sellItems[i].sizeDelta = new Vector2(slotSize, slotSize);
            sellItems[i].localPosition = new Vector2((slotSize / 2), -slotSize * (i + 1));
        }
    }

    private void PositionInit()
    {
        Vector3 temp = transform.position;

        sellSpaceStartX = temp.x + (slotSize / 2f);
        sellSpaceEndX = sellSpaceStartX + (slotSize * 4f);
        sellSpaceStartY = temp.y - (slotSize);
        sellSpaceEndY = sellSpaceStartY - (slotSize * 4);
    }

    bool CanMove()
    {
        Vector2 temp = uiTr.position;
        Vector2 _click = Input.mousePosition;
        if(_click.x > temp.x + slotSize && _click.y > (temp.y - slotSize / 2))
        {
            return true;
        }

        return false;
    }

    void ShopUiMove()
    {
        uiTr.position = clickPoint + new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        LayoutScreenDontOut();
    }
    // 화면 밖으로 이동했다면 되돌림
    void LayoutScreenDontOut()
    {
        if(uiTr.position.x + (slotSize * 5) > screenW / 2)
        {
            uiTr.position = new Vector2((screenW / 2) - slotSize * 5, uiTr.position.y);
        }
        if(uiTr.position.x < 0)
        {
            uiTr.position = new Vector2(0, uiTr.position.y);
        }

        if(uiTr.position.y > screenH)
        {
            uiTr.position = new Vector2(uiTr.position.x, screenH);
        }

        if(uiTr.position.y - (slotSize * 6) < 0)
        {
            uiTr.position = new Vector2(uiTr.position.x, slotSize * 6);
        }
    }

    //좌표 값 받아와서 판매공간인지 확인
    public bool InSellArea(Vector2 pos)
    {
        if(pos.x >= sellSpaceStartX
            && pos.x <= sellSpaceEndX
            && pos.y >= sellSpaceEndY
            && pos.y <= sellSpaceStartY)
        {
            return true;
        }
        return false;
    }

    public void OnClickCloseButton()
    {
        gameObject.SetActive(false);
    }

    public void ItemSell(int price)
    {
        invMng.myGold += price;
        invMng.GoldUpdate();
    }

    void ItemLeftClick(PointerEventData eventData)
    {
        int _count = eventData.hovered.Count;

        if (_count == 0)
            return;

        ShopItemPrice.UseItems _temp = ShopItemPrice.UseItems.none;

        for(int i = 0; i < _count; i++)
        {
            if(eventData.hovered[i].tag == "ShopItem")
            {
                //구입 확인창으로 연결 && 해당하는 아이템
                ShopItemPrice temp = eventData.hovered[i].GetComponent<ShopItemPrice>();
                _temp = temp.useitems;
                switch (_temp)
                {
                    case ShopItemPrice.UseItems.hpPotion:
                        spawnItem = useitemsPF.hpPotion;
                        break;

                    //case ShopItemPrice.UseItems.mpPotion:
                    //    spawnItem = useitemsPF.mpPotion;
                    //    break;
                }

                ItemBuyCheck();
                return;
            }
        }
    }

    void ItemRightClick(PointerEventData eventData)
    {

    }

    void ItemBuyCheck()
    {
        //acceptedManager.ShowBuyConfirm();
        //AcceptedManager.positive += ItemBuy;
        //AcceptedManager.negative += NegativeItemBuy;
    }

    void NegativeItemBuy()
    {
        //AcceptedManager.positive -= ItemBuy;
        //AcceptedManager.negative -= NegativeItemBuy;
    }

    void ItemBuy()
    {
        //AcceptManager.positive -= ItemBuy;

        if(invMng.myGold <= 500)
        {
            Debug.Log("골드가 부족합니다.");
            return;
        }

        if (InventoryManager.instance.PotionBuyCheck())
        {
            StartCoroutine(ItemSpawn());
            invMng.myGold -= 500;
            invMng.GoldUpdate();
        }
    }

    void RightClickBuy(ShopItemPrice.UseItems _temp)
    {

    }

    IEnumerator ItemSpawn()
    {
        Instantiate(spawnItem, inventory);

        yield return null;
    }

    void OnEnable()
    {
        isOn = true;
    }

    void OnDisable()
    {
        isDraging = false;
        isOn = false;
        isMouseOver = false;
        //npc.isShopOpen = false;
    }

}
