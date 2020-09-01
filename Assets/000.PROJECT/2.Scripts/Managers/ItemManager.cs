using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnInventory.Editor;

public class ItemManager : MonoBehaviour, IPointerClickHandler,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private ItemManager itemManager;

    [System.Serializable]
    public enum Rarity
    {
        normal = 0,
        magic,
        unique,
        legendary,

        ALL
    }

    [System.Serializable]
    public enum ItemKind
    {
        use = 0,
        Glove,
        LRing,
        Belt,
        RRing,
        Foot,
        LHand,
        RHand,
        Body,
        Pants,
        Hat,

        ALL
    }


    [System.Serializable]
    public enum ItemSize
    {
        _1_1 = 1,
        _2_1,

        ALL
    }

    public bool isHpPotion;

    //아이템이 바닥에 드롭중
    private bool isDrop;

    //마우스 추적중
    public bool isMouseTracking;

    private UIManager uiManager;
    private InventoryManager inventoryManager;

    //리스트상의 자신의 번호
    public int listNum;
    //아이템 코드 관련
    private int itemCount;

    //아이템이미지
    public Image itemImage;
    public Image itemImageBack;

    public int weaponNum;
    private int slotSize;

    public Transform uiPanel;

    [HideInInspector]
    public int xSize;

    [HideInInspector]
    public int ySize;

    [HideInInspector]
    public int xPosition;
    [HideInInspector]
    public int yPosition;

    //아이템 종류
    public ItemKind itemKind;
    //배열에서 차지하는 공간
    private ItemSize itemSize;
    //이미지의 크기
    private Vector2 itemImageSize;

    private RectTransform rectTr;

    private Vector3 centerPos;

    private int downSpace;
    private int leftSpace;

    public string itemName;

    public Rarity rarity;
    public int needLevel;
    public int damage;
    public int defense;

    public EquipmentManager equipmentManager;
    public bool isEquipment;
    private Transform inventoryTr;

    //아이템 UI인포관련
    bool isMouseOver;

    public GameObject uiInfo;
    private Text uiName;
    private Text uiRarity;
    private Text uiNeedLevel;
    private Text uiStatus;

    public GameObject equipmentInfo;

    private Text uiEqName;
    private Text uiEqRarity;
    private Text uiEqNeedLevel;
    private Text uiEqStatus;


    private ShopManager shopManager;
    private AcceptedManager acceptedManager;

    public int itemSellPrice;

    private ItemCreateManager itemCreater;
    private SoundManager soundManager;
    

    private void Awake()
    {
        uiPanel = GameObject.Find("UIPanel").transform;
        isEquipment = false;
        rectTr = GetComponent<RectTransform>();
        equipmentManager = GameObject.FindObjectOfType<EquipmentManager>();

        soundManager = GameObject.FindObjectOfType<SoundManager>();
        isMouseTracking = false;
        itemManager = this;
        inventoryTr = uiPanel.Find("InvenImg").transform;
        UIInfoDataInit();
        shopManager = uiPanel.Find("SellerImg").GetComponent<ShopManager>();
        acceptedManager = GameObject.Find("AcceptedManager").GetComponent<AcceptedManager>();
        itemImage = transform.GetChild(0).GetComponent<Image>();
        itemImageBack = GetComponent<Image>();
        itemCreater = GameObject.Find("ItemCreator").GetComponent<ItemCreateManager>();
    }

    private void Start()
    {
        UiInfoTrInit();
        uiManager = new UIManager();//instance로써 할당해야함 일단 구현안됐으니 이렇게
        inventoryManager = new InventoryManager(); // **


        AutoSizeInit();
        slotSize = uiManager.slotSize;
        ImageSizeInit();


        centerPos = itemImageSize / -2;
        downSpace = uiManager.inventoryDownSpaceSize;
        leftSpace = uiManager.inventoryLeftSpaceSize;


        ItemListAdd();

        InventoryAutoInput();

        ItemReturnPosition();

        ItemImageSet();

        ItemSellPriceSet();
        DontEquipmentItemImageSet();

    }

    void Update()
    {
        if (isMouseTracking)
        {
            MouseTracking();
            EventSystem.current.IsPointerOverGameObject();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton input = eventData.button;

        switch (input)
        {
            case PointerEventData.InputButton.Left:
                ItemLeftClick();
                break;
            case PointerEventData.InputButton.Right:
                ItemRightClick();
                break;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        uiInfo.SetActive(false);
        equipmentInfo.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData = null)
    {
        if (isMouseTracking)
            return;

        //throw new System.NotImplementedException();
        if (!isEquipment)
        {
            uiInfo.SetActive(true);
            UIInfoDataInit();
            //uiInfo.transform.SetAsLastSibling();

        }

        if (itemKind == ItemKind.use)
        {

            return;

        }

        RectTransform temp = equipmentManager.GetRectTr(itemKind);


        if (temp.childCount > 0)
        {

            temp.GetComponentInChildren<ItemManager>().SetUiEquipmentItemDataInit();

            equipmentInfo.SetActive(true);

        }

    }

    private void UiInfoTrInit()
    {
        GameObject temp = GameObject.FindGameObjectWithTag("UIPanel");
        GameObject _temp = temp.transform.Find("InvenImg").gameObject;

        uiInfo = _temp.transform.Find("Info").gameObject;
        equipmentInfo = _temp.transform.Find("EquipMentInfo").gameObject;

        uiName = uiInfo.transform.Find("Name").GetComponent<Text>();
        uiRarity = uiInfo.transform.Find("rarity").GetComponent<Text>();
        uiNeedLevel = uiInfo.transform.Find("level").GetComponent<Text>();
        uiStatus = uiInfo.transform.Find("status").GetComponent<Text>();

        uiEqName = equipmentInfo.transform.Find("Name").GetComponent<Text>();
        uiEqRarity = equipmentInfo.transform.Find("rarity").GetComponent<Text>();
        uiEqNeedLevel = equipmentInfo.transform.Find("level").GetComponent<Text>();
        uiEqStatus = equipmentInfo.transform.Find("status").GetComponent<Text>();
    }

    private void UIInfoDataInit()
    {
        uiName.text = itemName;

        uiNeedLevel.text = "요구 레벨 : " + needLevel.ToString();

        switch (rarity)
        {
            case Rarity.normal:
                uiRarity.text = "커먼";
                uiRarity.color = Color.white;
                break;

            case Rarity.magic:
                uiRarity.text = "언커먼";
                uiRarity.color = Color.blue;
                break;

            case Rarity.unique:
                uiRarity.text = "희귀";
                uiRarity.color = Color.yellow;
                break;

            case Rarity.legendary:
                uiRarity.text = "전설";
                uiRarity.color = new Color(1, 0.56f, 0);
                break;
        }

        if(itemKind == ItemKind.use)
        {
            uiNeedLevel.text = "요구 레벨 : 0";
            uiStatus.text = "회복량 : 30%";
            return;
        }

        if(damage > 0 && defense > 0)
        {
            Debug.LogError("힘과 방어도가 둘 다 존재함");
        }

        if(damage > 0)
        {
            uiStatus.text = "힘 " + damage.ToString();
        }
        else
        {
            uiStatus.text = "방어도 " + defense.ToString();
        }

        uiInfo.transform.position = new Vector2(Screen.width / 2 - slotSize, Screen.height / 5);
    }

    private void ItemSellPriceSet()
    {
        int temp = 0;

        switch (rarity)
        {
            case Rarity.normal:
                temp += 500;
                break;
            case Rarity.magic:
                temp += 1000;
                break;
            case Rarity.unique:
                temp += 1500;
                break;
            case Rarity.legendary:
                temp += 2000;
                break;

        }
        temp = Mathf.RoundToInt(temp * (1 + (needLevel / 10f)));

        if (itemKind == ItemKind.use)
            temp = 250;

        itemSellPrice = temp;
    }

    public bool InventoryAutoInput()
    {
        if(!inventoryManager.AutoItemInput(ref itemManager))
        {
            Debug.Log("더 이상 가질 수 없습니다.");
            return false;
        }
        return true;
    }

    void ItemListAdd()
    {
        itemCount = inventoryManager.GetItemCount();
    }

    void ItemListRemove()
    {
        inventoryManager.items.RemoveAt(listNum);
    }

    void AutoSizeInit()
    {
        switch (itemKind)
        {
            case ItemKind.use:
                itemSize = ItemSize._1_1;
                break;
            case ItemKind.Hat:
                itemSize = ItemSize._2_1;
                break;
            case ItemKind.Body:
                itemSize = ItemSize._2_1;
                break;
            case ItemKind.Belt:
                itemSize = ItemSize._1_1;
                break;
            case ItemKind.Pants:
                itemSize = ItemSize._2_1;
                break;
            case ItemKind.Foot:
                itemSize = ItemSize._2_1;
                break;
            case ItemKind.Glove:
                itemSize = ItemSize._2_1;
                break;
            case ItemKind.LRing:
            case ItemKind.RRing:
                itemSize = ItemSize._1_1;
                break;
            case ItemKind.LHand:
            case ItemKind.RHand:
                itemSize = ItemSize._2_1;
                break;
        }

        ArrSizeInit();
    }

    void ArrSizeInit()
    {
        switch (itemSize)
        {
            case ItemSize._1_1:
                ySize = 1;
                xSize = 1;
                break;
            case ItemSize._2_1:
                ySize = 2;
                xSize = 1;
                break;

        }
    }

    void ImageSizeInit()
    {
        switch (itemSize)
        {
            case ItemSize._1_1:
                itemImageSize = new Vector2(slotSize, slotSize);
                break;
            case ItemSize._2_1:
                itemImageSize = new Vector2(slotSize, slotSize * 2);
                break;

        }

        rectTr.sizeDelta = itemImageSize;
    }

    void MouseTracking()
    {
        rectTr.position = Input.mousePosition + centerPos;
    }

    private void ItemLeftClick()
    {
        //장비중인 아이템일때
        if (isEquipment)
        {
            //착용중인 아이템에서 제외
            inventoryManager.isItemDrag = true;
            isMouseTracking = true;
            transform.parent = inventoryTr;
            isEquipment = false;
            itemImageBack.enabled = false;

            equipmentManager.WeaponSet(weaponNum);
            equipmentManager.ItemStatusSet();
            return;
        }

        //아이템이 마우스를 추적중일때
        if (isMouseTracking && inventoryManager.isItemDrag)
        {
            if (!ItemDropGrid())
            {
                return;
            }
            InputArr();
            inventoryManager.isItemDrag = false;
            isMouseTracking = false;
            DontEquipmentItemImageSet();
            OnPointerEnter();
        }//인벤토리에 존재할 때
        else if (!isMouseTracking && !inventoryManager.isItemDrag)
        {
            transform.SetAsLastSibling();
            RemoveArr();
            inventoryManager.isItemDrag = true;
            isMouseTracking = true;
            uiInfo.SetActive(false);
            equipmentInfo.SetActive(false);
        }
    }

    //아이템의 이미지를 리소스 폴더에 불러와 적용
    private void ItemImageSet()
    {
        string path = "ItemImages\\";
        switch (itemKind)
        {
            case ItemKind.use:
                if (isHpPotion)
                    path += "lifepotion";
                //else
                //    path += "manapotion";
                break;

            case ItemKind.LHand:
            case ItemKind.RHand:
                path += "weapon" + weaponNum.ToString();
                break;

            case ItemKind.Hat:
                path += "hat" + RarityNum();
                break;

            case ItemKind.Body:
                path += "body" + RarityNum();
                break;

            case ItemKind.Belt:
                path += "belt" + RarityNum();
                break;

            case ItemKind.Pants:
                path += "pants" + RarityNum();
                break;

            case ItemKind.Foot:
                path += "foot" + RarityNum();
                break;

            case ItemKind.Glove:
                path += "glove" + RarityNum();
                break;

            case ItemKind.LRing:
            case ItemKind.RRing:
                path += "lRing" + Random.Range(0, 4).ToString();
                break;
        }
        itemImage.sprite = Resources.Load<Sprite>(path);
    }

    //착용중인 아이템의 테두리
    public void EquipmentItemImgSet()
    {
        itemImageBack.enabled = true;

        Color temp = Color.white;
        temp.a = 1;
        itemImageBack.color = temp;
    }

    //창고에 있는 아이템의 테두리
    public void DontEquipmentItemImageSet()
    {
        itemImageBack.enabled = true;
        Color temp = new Color();
        switch (rarity)
        {
            case Rarity.normal:
                temp = Color.white;
                break;
            case Rarity.magic:
                temp = Color.blue;
                break;
            case Rarity.unique:
                temp = Color.yellow;
                break;
            case Rarity.legendary:
                temp.r = 1;
                temp.g = 0.56f;
                temp.b = 0;

                break;
        }
        temp.a = 0.5f;

        itemImageBack.color = temp;
    }

    //리소스 불러오기용
    private string RarityNum()
    {
        switch (rarity)
        {
            case Rarity.normal:
                return 0.ToString();
            case Rarity.magic:
                return 1.ToString();
            case Rarity.unique:
                return 2.ToString();
            case Rarity.legendary:
                return 3.ToString();
        }
        return null;
    }

    //배열에서 제외 함수 호출
    public void RemoveArr()
    {
        inventoryManager.RemoveArr(xPosition, yPosition, xSize, ySize);
    }

    //배열에 넣는 함수 호출
    public void InputArr()
    {
        inventoryManager.ArrInput(xPosition, yPosition, xSize, ySize, ref itemManager);
    }

    public void InputArr(int _x, int _y)
    {
        inventoryManager.ArrInput(_x, _y, xSize, ySize, ref itemManager);
    }

    //아이템 우클릭
    void ItemRightClick()
    {
        if (isMouseTracking)
        {
            if (InventoryAutoInput())
            {
                isMouseTracking = false;
                inventoryManager.isItemDrag = false;
                ItemReturnPosition();
                DontEquipmentItemImageSet();
            }
            else
            {
                Debug.Log("빈공간이 부족해");
            }
        }
        else
        {
            if (!isEquipment)
            {
                RemoveArr();

                if (shopManager.isOn)
                {
                    ItemSellCheck();
                }
                else //아이템 우클릭 착용
                {
                    if(itemKind == ItemKind.use)
                    {
                        //소모품 먹는 로직 추가
                        UsePotion();
                        Destroy(this.gameObject);
                        return;
                    }

                    if (needLevel > PlayerManager.instance.Level) return;

                    equipmentManager.ClickEquipment(ref itemManager);
                    if (itemKind == ItemKind.LHand)
                        equipmentManager.WeaponSet(weaponNum);

                    equipmentManager.ItemStatusSet();
                    EquipmentItemImgSet();
                }

            }
            else
            {
                transform.parent = inventoryTr;
                if (inventoryManager.AutoItemInput(ref itemManager))  //아이템 우클릭 벗기
                {

                    ItemReturnPosition();
                    equipmentManager.ItemStatusSet();
                    isEquipment = false;
                    DontEquipmentItemImageSet();

                }
                else
                {
                    Debug.Log("아이템에 공간이 없다");
                }
            }
        }
    }

    //포지션 되돌리기
    public void ItemReturnPosition()
    {
        rectTr.anchoredPosition = new Vector2((xPosition * slotSize) + leftSpace, (yPosition * slotSize) + downSpace);

    }

    //아이템을 놓을때
    bool ItemDropGrid()
    {
        Vector2 pos = rectTr.anchoredPosition;

        //아이템이 인벤토리 밖에서 드랍될시(x좌표가 음수일때or 0보다 작을때) 
        if(pos.x <= 0 - leftSpace)
        {
            if(shopManager.isOn && ShopOverlapCheck())
            {
                ItemSellCheck();
                ItemSell(1);
            }
            else
            {
                ItemInvenOutCheck();
            }
            return false;
        }
        if (pos.y >= downSpace + (slotSize * 6))
        {
            //아이템 착용
            if (needLevel > PlayerManager.instance.Level)
                return false;

            equipmentManager.DropEquipment(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), xSize, ySize, ref itemManager);

            equipmentManager.ItemStatusSet();

            return false;
        }

        //x,y포지션 카운트 계산
        int _xPosition = Mathf.RoundToInt((pos.x - leftSpace) / slotSize);
        int _yPosition = Mathf.RoundToInt((pos.y - downSpace) / slotSize);

        //사이즈와 인벤 배열크기 비교 (초과하는지 확인) 후 예외처리 추가해야함
        if (!ItemArrOverCheck(_xPosition, _yPosition))
        {
            return false;
        }

        //배열에 아이템이 존재하는지 확인
        if (!inventoryManager.ArrInputCheck(_xPosition, _yPosition, xSize, ySize))
        {
            //존재 한다면 스왑을 시켜야함
            DoSwap(_xPosition, _yPosition);

            return false;
        }

        rectTr.anchoredPosition = new Vector2((_xPosition * slotSize) + leftSpace, (_yPosition * slotSize) + downSpace);

        //배열에 아이템이 존재하는지 확인 후 배열에 추가 로직 추가

        xPosition = _xPosition;
        yPosition = _yPosition;

        return true;
    }

    //아이템 스왑
    void DoSwap(int _xPos, int _yPos)
    {
        int _x = _xPos;
        int _y = _yPos;
        int _ToX = xSize + _xPos;
        int _ToY = ySize + _yPos;

        int temp = 0;
        int count = 0;
        ItemManager itemtemp = null;

        for(_x = _xPos; _x < _ToX; _x++)
        {
            for(_y = _yPos; _y < _ToY; _y++)
            {
                if(inventoryManager.itemArrs[_y, _x].isUse)
                {
                    if(temp != inventoryManager.itemArrs[_y, _x].itemInfo.itemCount)
                    {
                        temp = inventoryManager.itemArrs[_y, _x].itemInfo.itemCount;
                        Debug.Log(inventoryManager.itemArrs[_y, _x].itemInfo.itemCount);
                        itemtemp = inventoryManager.itemArrs[_y, _x].itemInfo;
                        count++;
                    }
                }
            }
        }
        if(count > 1)
        {
            //못할 때
        }
        else
        {
            //가능할떄
            itemtemp.isMouseTracking = true;
            itemtemp.RemoveArr();
            itemtemp.itemImageBack.enabled = false;

            isMouseTracking = false;
            InputArr(_xPos, _yPos);
            DontEquipmentItemImageSet();
            rectTr.anchoredPosition = new Vector2((_xPos * slotSize) + leftSpace, (_yPos * slotSize) + downSpace);

            // 배열에 아이템이 존재하는지 확인 후 배열에 추가 로직 구성
            xPosition = _xPos;
            yPosition = _yPos;
            itemtemp.transform.SetAsLastSibling();
        }
    }

    void UsePotion()
    {
        soundManager.PlaySfx(Resources.Load<AudioClip>("ItemSounds\\usePotion"));

        //if (isHpPotion)
        //    PlayerManager.instance.HpRecovery(30);
        //else
        //    PlayerManager.instance.MpRecovery(30);

    }

    //아이템 드롭시 상점 공간이내인지 확인
    bool ShopOverlapCheck()
    {
        if (shopManager.InSellArea(Input.mousePosition))
        {
            return true;
        }
        return false;
    }

    void ItemSellCheck()
    {
        acceptedManager.ShowSellConfirm();
        AcceptedManager.positive += this.ItemSell;
        AcceptedManager.negative += this.NegativeItemSell;
    }

    //확인 후 판매시
    void ItemSell()
    {
        AcceptedManager.positive -= this.ItemSell;
        AcceptedManager.negative -= this.NegativeItemSell;

        soundManager.PlaySfx(Resources.Load<AudioClip>("ItemSounds\\itemSell"));
        //인자로 아이템 가격을 건내줌
        shopManager.ItemSell(itemSellPrice);
        Destroy(this.gameObject);
    }

    //바로 판매
    void ItemSell(int i)
    {
        inventoryManager.isItemDrag = false;
        isMouseTracking = false;

        soundManager.PlaySfx(Resources.Load<AudioClip>("ItemSounds\\itemSell"));

        //인자로 아이템 가격을 건내줌
        shopManager.ItemSell(itemSellPrice);
        Destroy(this.gameObject);
    }

    void NegativeItemSell()
    {
        AcceptedManager.positive -= this.ItemSell;
        AcceptedManager.negative -= this.NegativeItemSell;
    }

    void ItemInvenOutCheck()
    {
        acceptedManager.ShowDropConfirm();
        //네거티브시 이벤트 빼기
        AcceptedManager.positive += this.ItemInvenOut;
        AcceptedManager.negative += this.NegativeItemOut;
    }

    void NegativeItemOut()
    {
        AcceptedManager.positive -= this.ItemInvenOut;
        AcceptedManager.negative -= this.NegativeItemOut;
    }

    void ItemInvenOut()
    {
        AcceptedManager.positive -= this.ItemInvenOut;
        AcceptedManager.negative -= this.NegativeItemOut;

        // 아이템을 던지는 로직 추가
        itemCreater.DropItemCreate(itemManager);

        inventoryManager.isItemDrag = false;
        isMouseTracking = false;

        Destroy(this.gameObject, 0.01f);
    }

    bool ItemArrOverCheck(int x, int y)
    {
        if (xSize + x > 10 || x < 0)
            return false;
        else if (ySize + y > 4 || y < 0)
            return false;

        return true;
    }

    public void SetUiEquipmentItemDataInit()
    {
        uiEqName.text = itemName;

        uiEqNeedLevel.text = "요구 레벨 : " + needLevel.ToString();

        switch (rarity)
        {
            case Rarity.normal:
                uiEqRarity.text = "커먼";
                uiEqRarity.color = Color.white;
                break;

            case Rarity.magic:
                uiEqRarity.text = "언커먼";
                uiEqRarity.color = Color.blue;
                break;

            case Rarity.unique:
                uiEqRarity.text = "희귀";
                uiEqRarity.color = Color.yellow;
                break;

            case Rarity.legendary:
                uiEqRarity.text = "전설";
                uiEqRarity.color = new Color(1, 0.56f, 0);
                break;
        }

        if (damage > 0 && defense > 0)
        {
            Debug.LogError("힘과 방어도가 둘 다 존재함");
        }

        if (damage > 0)
        {
            uiEqStatus.text = "힘 " + damage.ToString();
        }
        else
        {
            uiEqStatus.text = "방어도 " + defense.ToString();
        }

        uiInfo.transform.position = new Vector2(Screen.width / 2 - slotSize, Screen.height / 5);
    }

    void OnDisable()
    {
        if (isMouseTracking)
        {
            InventoryAutoInput();

            ItemReturnPosition();
        }

        inventoryManager.isItemDrag = true;
        isMouseTracking = false;
    }

    void OnDestroy()
    {
        RemoveArr();
    }












    //bool ItemArrOverCheck(int x, int y)
    //{
    //    if (xSize + x > 10 || x < 0)
    //        return false;
    //    else if (ySize + y > 4 || y < 0)
    //        return false;

    //    return true;


    //}

    //public bool InventoryAutoInput()
    //{
    //    if (!inventoryManager.AutoItemInput(ref itemManager))
    //    {
    //        Debug.Log("더이상 가질 수 없어");
    //        return false;
    //    }
    //    return true;
    //}



    ////아이템 스왑
    //void DoSwap(int _xPos, int _yPos)
    //{

    //    int _x = _xPos;
    //    int _y = _yPos;
    //    int _ToX = xSize + _xPos;
    //    int _ToY = ySize + _yPos;

    //    int temp = 0;
    //    int count = 0;
    //    ItemManager itemtemp = null;

    //    for (_x = _xPos; _x < _ToX; _x++)
    //    {

    //        for (_y = _yPos; _y < _ToY; _y++)
    //        {

    //            if (inventoryManager.itemArrs[_y, _x].isUse)
    //            {
    //                if (temp != inventoryManager.itemArrs[_y, _x].itemInfo.itemCount)
    //                {
    //                    temp = inventoryManager.itemArrs[_y, _x].itemInfo.itemCount;
    //                    Debug.Log(inventoryManager.itemArrs[_y, _x].itemInfo.itemCount);
    //                    itemtemp = inventoryManager.itemArrs[_y, _x].itemInfo;
    //                    count++;

    //                }
    //            }

    //        }
    //    }

    //    if (count > 1)
    //    {
    //        //못할때
    //    }
    //    else
    //    {
    //        //가능할때
    //        itemtemp.isMouseTracking = true;
    //        itemtemp.RemoveArr();
    //        itemtemp.itemImageBack.enabled = false;

    //        isMouseTracking = false;
    //        InputArr(_xPos, _yPos);
    //        DontEquipmentItemImageSet();
    //        rectTr.anchoredPosition = new Vector2((_xPos * slotSize) + leftSpace, (_yPos * slotSize) + downSpace);

    //        //배열에 아이템이 존재하는지 확인 후 배열에 추가 로직 추가

    //        xPosition = _xPos;
    //        yPosition = _yPos;
    //        itemtemp.transform.SetAsLastSibling();
    //    }

    //}


    //public void SetUiEquipmentItemDataInit()
    //{
    //    uiEqName.text = itemName;

    //    uiEqNeedLevel.text = "레벨 제한 : " + needLevel.ToString();

    //    switch (rarity)
    //    {
    //        case Rarity.normal:
    //            uiEqRarity.text = "흔함";
    //            uiEqRarity.color = Color.white;
    //            break;
    //        case Rarity.magic:
    //            uiEqRarity.text = "평범함";
    //            uiEqRarity.color = Color.blue;
    //            break;
    //        case Rarity.unique:
    //            uiEqRarity.text = "희귀함";
    //            uiEqRarity.color = Color.yellow;
    //            break;
    //        case Rarity.legendary:

    //            uiEqRarity.text = "전설적";
    //            uiEqRarity.color = new Color(1, 0.56f, 0);
    //            break;



    //    }

    //    if (damage > 0 && defense > 0)
    //    {
    //        Debug.LogError("공격력과 방어력이 둘 다 존재함");
    //    }

    //    if (damage > 0)
    //    {

    //        uiEqStatus.text = "공격력 " + damage.ToString();

    //    }
    //    else
    //    {
    //        uiEqStatus.text = "방어력 " + defense.ToString();
    //    }


    //    equipmentInfo.transform.position = new Vector2(Screen.width / 2 - slotSize, Screen.height / 2);



    //}

    //void OnDisable()
    //{

    //    if (isMouseTracking)
    //    {
    //        InventoryAutoInput();

    //        ItemReturnPosition();

    //    }

    //    inventoryManager.isItemDrag = false;
    //    isMouseTracking = false;


    //}

    //void OnDestroy()
    //{

    //    RemoveArr();
    //}
}
