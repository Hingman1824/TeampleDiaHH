using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPooling : MonoBehaviour
{

    public static ItemPooling Instance;

    [SerializeField]
    private GameObject[] _item; //스폰할 아이템 프리펩을 연결

    Queue<Iitem> myItem = new Queue<Iitem>();

    private void Awake()
    {
        Instance = this;
        Initialize(10);
    }

    private void Initialize(int initCount) //시작과 동시에 숫자만큼 아이템을 생성
    {
        for (int i = 0; i < initCount; i++)
        {
            myItem.Enqueue(CreateNewItem()); //아이템 생성
        }
    }

    private Iitem CreateNewItem()
    {
        int a = Random.Range(0, _item.Length);
        var newObj = Instantiate(_item[a]).GetComponent<Iitem>(); //몬스터를 새로생성하고 Monster컴포넌트를 연결

        newObj.gameObject.SetActive(false); //게임오브젝트 비활성화
        newObj.transform.SetParent(transform); //부모위치로 이동
        return newObj;
    }
    public static Iitem GetItem()  //
    {
        if (Instance.myItem.Count > 0) //
        {
            var obj = Instance.myItem.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else //
        {
            var newObj = Instance.CreateNewItem(); //새로 생성 후 빌려준다.
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    public static void ReturnObject(Iitem obj) //몬스터를 요청한 자가 사용후 반환할 때
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.myItem.Enqueue(obj);
    }
}
