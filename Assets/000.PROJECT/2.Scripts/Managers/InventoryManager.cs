using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;

    public Text goldText;
    public int myGold;


    public class ItemArr
    {
        public bool isUse; //사용중인가

        public ItemManager itemInfo; //아이템정보

        public ItemArr()// 생성자
        {
            isUse = false;
            itemInfo = null;
        }
    }

    //[y,x] 
    private int[,] invenArr;//y축부터니 실수 ㄴㄴ
    public ItemArr[,] itemArrs;//아이템 정보를 담을 다차원 배열
    public bool isItemDrag;//아이템 드래깅
    public int itemCount;


    public class ItemSize
    {
        public ItemManager itemManger;//레퍼런스
        public Vector2 startPos;//아이템 사이즈 시작 위치
        public Vector2 endPos;//아이템 사이즈 끝 위치
    }
    public List<ItemSize> items;//아이템사이즈를 담을 리스트 선언

    private void Awake()
    {   //아이템 리스트 동적할당, 레퍼런스 값 설정, 드래그 안되어있으니 false, 아이템은 무조건 하나부터 시작
        items = new List<ItemSize>();
        instance = this;
        isItemDrag = false;
        itemCount = 1;

        //각각 아이템을 담을 그릇 + 위치마다 아이템 정보 동적할당
        itemArrs = new ItemArr[6, 10];
        for(int i = 0; i<6; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                itemArrs[i,j] = new ItemArr();
            }
        }
    }

    private void Start()
    {
        invenArr = new int[6, 10];//인벤토리 크기 설정
        invenArr.Initialize();//초기값 설정
        GoldUpdate(); //보유자산 업데이트
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ShowArr();//p키 입력시 인벤토리 배열정보 보여짐
        }
    }

    void ShowArr()
    {
        for(int i = 0; i < 6; i++)
        {
            for(int j = 0; j < 10; j++)
            {
                Debug.Log(invenArr[i, j] + "y : " + i.ToString() + ", x : " + j.ToString());
            }
        }
    }

    public void GoldUpdate()
    {
        goldText.text = myGold.ToString();// 보유골드 보여줌
    }

    public int GetItemCount()
    {
        return itemCount++;
    }


    //x,y의 시작 위치와 크기정보를 받아서 bool형태로 리턴
    public bool ArrInputCheck(int startX, int startY, int xSize, int ySize)
    {
        int _X = startX;
        int _Y = startY;
        int toX = _X + xSize;
        int toY = _Y + ySize;


        if(toX > 10 || toY > 6)//시작위치+크기값 비교 (결국은 크기를 넘는지 체크하는것 [인벤 경계]10,6) 
        {
            return false;
        }

        for(_X = startX; _X < toX; _X++)
        {   
            for(_Y = startY; _Y < toY; _Y++)
            {
                if (itemArrs[_Y, _X].isUse)//위의 예외처리를 통과하였으나 그 자리가 사용중일경우 리턴false
                {
                    return false;
                }
            }

        }

        return true;//다 통과하면 트루
    }

    //아이템 놓는 로직
    public void ArrInput(int startX, int startY, int xSize, int ySize, ref ItemManager itemManager)
    {
        int _X = startX;
        int _Y = startY;

        int toX = _X + xSize;
        int toY = _Y + ySize;

        for(_X = startX; _X < toX; _X++)
        {
            for(_Y = startY; _Y < toY; _Y++)
            {
                itemArrs[_Y, _X].isUse = true;
                itemArrs[_Y, _X].itemInfo = itemManager;
            }
        }
    }

    //아이템 뺴는 로직
    public void RemoveArr(int startX, int startY, int xSize, int ySize)
    {
        int _X = startX;
        int _Y = startY;
        int toX = _X + xSize;
        int toY = _Y + ySize;

        for(_X = startX; _X < toX; _X++)
        {
            for(_Y = startY; _Y<toY; _Y++)
            {
                itemArrs[_Y, _X].isUse = false;
                itemArrs[_Y, _X].itemInfo = null;
            }
        }
    }

    //아이템 위치 서로 바꾸기
    public bool itemSwap(int startX, int startY, int xSize, int ySize)
    {
        int _X = startX;
        int _Y = startY;
        int toX = _X + xSize;
        int toY = _Y + ySize;

        int[] _items = new int[8];

        return true;
    }


    /* 아직 구현 x 습득하면 자동으로 인벤토리에 들어가는 로직 */
    public bool AutoItemInput(ref ItemManager itemManager)
    {
        int xSize = itemManager.xSize;
        int ySize = itemManager.ySize;

        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 4; i++)
            {
                if (ArrInputCheck(j, i, xSize, ySize))
                {
                    ArrInput(j, i, xSize, ySize, ref itemManager);
                    itemManager.xPosition = j;
                    itemManager.yPosition = i;
                    return true;

                }


            }

        }

        return false;

    }

    public bool AutoItemInput(int xSize, int ySize)
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (ArrInputCheck(j, i, xSize, ySize))
                {
                    return true;
                }
            }
        }
        return false;
    }


    public bool PotionBuyCheck()
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                if (!itemArrs[i, j].isUse)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
