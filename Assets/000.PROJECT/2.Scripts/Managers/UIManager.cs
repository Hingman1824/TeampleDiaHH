using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EquipmentSpace
{
    public class Equipments
    {
        public RectTransform item;

        public int startX, startY;
        public int endX, endY;
        public int sizeX, sizeY;

        public Equipments()
        {
            item = new RectTransform();
        }

        public void FildSizeInit()
        {
            item.anchoredPosition = new Vector2(startX, startY);
        }
    }

    public Equipments glove;
    public Equipments lRing;
    public Equipments belt;
    public Equipments rRing;
    public Equipments foot;
    public Equipments lHand;
    public Equipments rHand;
    public Equipments body;
    public Equipments pants;
    public Equipments hat;

    public EquipmentSpace()
    {
        glove = new Equipments();
        lRing = new Equipments();
        belt = new Equipments();
        rRing = new Equipments();
        foot = new Equipments();
        lHand = new Equipments();
        rHand = new Equipments();
        body = new Equipments();
        pants = new Equipments();
        hat = new Equipments();
    }

    // 컴포넌트 불러운 뒤
    public void ComponentInit()
    {
        hat.item = GameObject.FindGameObjectWithTag("Hat").GetComponent<RectTransform>();
        body.item = GameObject.FindGameObjectWithTag("Body").GetComponent<RectTransform>();
        belt.item = GameObject.FindGameObjectWithTag("Belt").GetComponent<RectTransform>();
        pants.item = GameObject.FindGameObjectWithTag("Pants").GetComponent<RectTransform>();
        lHand.item = GameObject.FindGameObjectWithTag("LHand").GetComponent<RectTransform>();
        rHand.item = GameObject.FindGameObjectWithTag("RHand").GetComponent<RectTransform>();
        foot.item = GameObject.FindGameObjectWithTag("Foot").GetComponent<RectTransform>();
        glove.item = GameObject.FindGameObjectWithTag("Glove").GetComponent<RectTransform>();
        lRing.item = GameObject.FindGameObjectWithTag("LRing").GetComponent<RectTransform>();
        rRing.item = GameObject.FindGameObjectWithTag("RRing").GetComponent<RectTransform>();

    }


    // 각 각 아이템 사이즈
    public void XYSize()
    {
        hat.sizeX = 1;
        hat.sizeY = 2;
        body.sizeX = 1;
        body.sizeY = 2;
        pants.sizeX = 1;
        pants.sizeY = 2;
        foot.sizeX = 1;
        foot.sizeY = 2;
        glove.sizeX = 1;
        glove.sizeY = 2;
        belt.sizeX = 1;
        belt.sizeY = 1;
        lHand.sizeX = 1;
        lHand.sizeY = 2;
        rHand.sizeX = 1;
        rHand.sizeY = 2;
        lRing.sizeX = 1;
        lRing.sizeY = 1;
        rRing.sizeX = 1;
        rRing.sizeY = 1;


    }
    //시작 위치 지정이 필요함 그 후

    //시작 위치 기반 엔드까지
    public void FildSizeInit()
    {
        hat.FildSizeInit();    //투구
        body.FildSizeInit();   //상의갑옷
        pants.FildSizeInit();  //바지갑옷
        foot.FildSizeInit();   //신발
        glove.FildSizeInit();  //장갑
        belt.FildSizeInit();   // 벨트
        lHand.FildSizeInit();  //왼쪽무기
        rHand.FildSizeInit();  //오른쪽무기
        lRing.FildSizeInit();  //왼쪽 반지
        rRing.FildSizeInit();  //오른쪽 반지
    }
}

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    private List<GameObject> allUI;
    public RectTransform uiPanel;

    //public GameObject status; // 추후 추가
    public GameObject inventory; // 내가 만든 인벤토리
    public GameObject acceptedWindow; //추후 추가
    public GameObject shopWindow; // 상점 구현x

    [HideInInspector]
    public int slotSize; // 말 그대로 슬롯사이즈

    [HideInInspector]
    public int inventoryDownSpaceSize;
    [HideInInspector]
    public int inventoryLeftSpaceSize;

    // 스크린의 높이, 너비
    [HideInInspector]
    public int screenH;
    [HideInInspector]
    public int screenW;

    // 스크린 높이 기준 16:9 사이즈 너비 / 2크기
    private Vector2 invenSize;

    //장비칸에 빈공간
    public EquipmentSpace equipments;

    //골드
    public RectTransform goldSize;


    void Awake()
    {
        instance = this;

        equipments = new EquipmentSpace();

        allUI = new List<GameObject>();
        screenH = Screen.height;
        screenW = Screen.width;

        //인벤이미지 크기 조절인데 어떻게 맞추는지 모르겠네
        invenSize = new Vector2((screenW/16)*4.758f, (screenH/9)*7.5f);

        slotSize = Mathf.RoundToInt(screenH * 0.074f);

        uiPanel.sizeDelta = new Vector2(screenW, screenH);

        inventoryDownSpaceSize = screenH / 36;
        inventoryLeftSpaceSize = Mathf.RoundToInt(screenW / 38.4f);

        RectTransform ivTr = inventory.GetComponent<RectTransform>();
        ivTr.sizeDelta = invenSize;
        //장비칸의 크기 및 위치 함수 구현
    }

    IEnumerator Start()
    {
        EquipmentComponentInit();

        equipments.XYSize();

        EquipmentSlotSizeInit();

        equipments.FildSizeInit();

        goldSize.sizeDelta = new Vector2(slotSize * 1.375f, slotSize * 0.191f);
        goldSize.anchoredPosition = new Vector2(slotSize * 1.375f, slotSize * 0.191f);
        goldSize.GetComponent<Text>().fontSize = Mathf.RoundToInt(slotSize * 0.375f);
        allUI.Add(inventory);
        allUI.Add(shopWindow);     //추후 추가 20200824전재현
        allUI.Add(acceptedWindow);
        //allUI.Add(status);

        yield return new WaitForSeconds(0.01f);
        AllUIClose();
    }

    void Update()
    {
        KeyChecks();
    }

    void KeyChecks()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // i를 눌렀을 때 인벤창이 나옴
            UIInventory();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //UIStatus(); //추후 추가 스탯창
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            AllUIClose();
        }
    }

    void AllUIClose()
    {
        for(int i = 0; i<allUI.Count; i++)
        {
            allUI[i].SetActive(false);
        }
    }

    void EquipmentComponentInit()
    {
        Transform tr = uiPanel.Find("InvenImg").Find("EquipmentSpace");

        equipments.hat.item = tr.Find("Hat").GetComponent<RectTransform>();
        equipments.body.item = tr.Find("Body").GetComponent<RectTransform>();
        equipments.belt.item = tr.Find("Belt").GetComponent<RectTransform>();
        equipments.pants.item = tr.Find("Pants").GetComponent<RectTransform>();
        equipments.foot.item = tr.Find("Foot").GetComponent<RectTransform>();
        equipments.glove.item = tr.Find("Glove").GetComponent<RectTransform>();
        equipments.lHand.item = tr.Find("LHand").GetComponent<RectTransform>();
        equipments.rHand.item = tr.Find("RHand").GetComponent<RectTransform>();
        equipments.lRing.item = tr.Find("LRing").GetComponent<RectTransform>();
        equipments.rRing.item = tr.Find("RRing").GetComponent<RectTransform>();
    }

    public void UIInventory()
    {
        if (inventory.activeInHierarchy)
        {
            inventory.SetActive(false);
        }
        else
        {
            inventory.SetActive(true);
        }
    }

    public void UIInventoryOpen()
    {
        inventory.SetActive(true);
    }

    //스탯 나중에 구현
    //void UIStatus()
    //{
    //    if (status.activeInHierarchy)
    //    {
    //        status.SetActive(false);
    //    }
    //    else
    //    {
    //        status.SetActive(true);
    //    }
    //}

    void EquipmentSlotSizeInit()
    {
        //시작 위치 계산
        //모자
        equipments.hat.startX = Mathf.RoundToInt((screenW / 16) * 3.1f);
        equipments.hat.startY = Mathf.RoundToInt((screenH / 9) * 7.625f);
        //상의(갑옷)
        equipments.body.startX = Mathf.RoundToInt((screenW / 16) * 0.645f);
        equipments.body.startY = Mathf.RoundToInt((screenH / 9) * 0.833f);
        //벨트
        equipments.belt.startX = Mathf.RoundToInt((screenW / 16) * 3.08f);
        equipments.belt.startY = Mathf.RoundToInt((screenH / 9) * 4.17f);
        //바지
        equipments.pants.startX = Mathf.RoundToInt((screenW / 16) * 3.0f);
        equipments.pants.startY = Mathf.RoundToInt((screenH / 9) * 5.1f);
        //신발
        equipments.foot.startX = Mathf.RoundToInt((screenW / 16) * 5.75f);
        equipments.foot.startY = Mathf.RoundToInt((screenH / 9) * 3.5f);
        //장갑
        equipments.glove.startX = inventoryLeftSpaceSize;
        equipments.glove.startY = Mathf.RoundToInt((screenH / 9) * 3.5f);
        //왼쪽무기
        equipments.lHand.startX = inventoryLeftSpaceSize;
        equipments.lHand.startY = Mathf.RoundToInt((screenH / 9) * 5.25f);
        //오른쪽무기
        equipments.rHand.startX = Mathf.RoundToInt((screenW / 16) * 5.75f);
        equipments.rHand.startY = Mathf.RoundToInt((screenH / 9) * 5.25f);
        //왼쪽 반지
        equipments.lRing.startX = Mathf.RoundToInt((screenW / 16) * 2.17f);
        equipments.lRing.startY = Mathf.RoundToInt((screenH / 9) * 4.17f);
        //오른쪽 반지
        equipments.rRing.startX = Mathf.RoundToInt((screenW / 16) * 4.75f);
        equipments.rRing.startY = Mathf.RoundToInt((screenH / 9) * 4.17f);

        //////마지막위치 계산
        //모자
        equipments.hat.endX = equipments.hat.startX + (slotSize * equipments.hat.sizeX);
        equipments.hat.endY = equipments.hat.startY + (slotSize * equipments.hat.sizeY);
        //상의(갑옷)
        equipments.body.endX = equipments.body.startX + (slotSize * equipments.body.sizeX);
        equipments.body.endY = equipments.body.startY + (slotSize * equipments.body.sizeY);
        //벨트
        equipments.belt.endX = equipments.belt.startX + (slotSize * equipments.belt.sizeX);
        equipments.belt.endY = equipments.belt.startY + (slotSize * equipments.belt.sizeY);
        //바지
        equipments.pants.endX = equipments.pants.startX + (slotSize * equipments.pants.sizeX);
        equipments.pants.endY = equipments.pants.startY + (slotSize * equipments.pants.sizeY);
        //신발
        equipments.foot.endX = equipments.foot.startX + (slotSize * equipments.foot.sizeX);
        equipments.foot.endY = equipments.foot.startY + (slotSize * equipments.foot.sizeY);
        //장갑
        equipments.glove.endX = equipments.glove.startX + (slotSize * equipments.glove.sizeX);
        equipments.glove.endY = equipments.glove.startY + (slotSize * equipments.glove.sizeY);
        //왼쪽무기
        equipments.lHand.endX = equipments.lHand.startX + (slotSize * equipments.lHand.sizeX);
        equipments.lHand.endY = equipments.lHand.startY + (slotSize * equipments.lHand.sizeY);
        //오른쪽 무기
        equipments.rHand.endX = equipments.rHand.startX + (slotSize * equipments.rHand.sizeX);
        equipments.rHand.endY = equipments.rHand.startY + (slotSize * equipments.rHand.sizeY);
        //왼쪽 반지
        equipments.lRing.endX = equipments.lRing.startX + (slotSize * equipments.lRing.sizeX);
        equipments.lRing.endY = equipments.lRing.startY + (slotSize * equipments.lRing.sizeY);
        //오른쪽반지
        equipments.rRing.endX = equipments.rRing.startX + (slotSize * equipments.rRing.sizeX);
        equipments.rRing.endY = equipments.rRing.startY + (slotSize * equipments.rRing.sizeY);

        //사이즈 조절
        //모자
        equipments.hat.item.sizeDelta = new Vector2(equipments.hat.sizeX * slotSize, equipments.hat.sizeY * slotSize);
        //상의(갑옷)
        equipments.body.item.sizeDelta = new Vector2(equipments.body.sizeX * slotSize, equipments.body.sizeY * slotSize);
        //벨트
        equipments.belt.item.sizeDelta = new Vector2(equipments.belt.sizeX * slotSize, equipments.belt.sizeY * slotSize);
        //바지
        equipments.pants.item.sizeDelta = new Vector2(equipments.pants.sizeX * slotSize, equipments.pants.sizeY * slotSize);
        //신발
        equipments.foot.item.sizeDelta = new Vector2(equipments.foot.sizeX * slotSize, equipments.foot.sizeY * slotSize);
        //장갑
        equipments.glove.item.sizeDelta = new Vector2(equipments.glove.sizeX * slotSize, equipments.glove.sizeY * slotSize);
        //왼쪽 무기
        equipments.lHand.item.sizeDelta = new Vector2(equipments.lHand.sizeX * slotSize, equipments.lHand.sizeY * slotSize);
        //오른쪽무기
        equipments.rHand.item.sizeDelta = new Vector2(equipments.rHand.sizeX * slotSize, equipments.rHand.sizeY * slotSize);
        //왼쪽 반지
        equipments.lRing.item.sizeDelta = new Vector2(equipments.lRing.sizeX * slotSize, equipments.lRing.sizeY * slotSize);
        //오른쪽 반지
        equipments.rRing.item.sizeDelta = new Vector2(equipments.rRing.sizeX * slotSize, equipments.rRing.sizeY * slotSize);
        
    }
}
