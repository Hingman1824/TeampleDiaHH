using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPooling : MonoBehaviour //몬스터 오브젝트풀링 기법을 사용하여 관리
{
    /* 오브젝트 풀링으로 몬스터들을 관리할 빈게임오브젝트에 이 스크립트를 추가, 스폰할 몬스터의 프리펩을 연결
     * 오브젝트 풀링 기법을 사용하여 몬스터들을 관리하여 일정수 몬스터들을 미리 만들어두고 비활성화 시켜준뒤 필요할때만 꺼내서 활성화 시켜주고 사용한 후에 반환함.
     * 
     * Skeleton, Gargoyle = 스크립트
     * skeleton, gargoyle = 큐 변수명
     * _skeleton, _gargoyle = 게임오브젝트명
     */

    public static EnemyPooling Instance; //다른 스크립트에서 사용하기 위해 싱글톤패턴을 사용, 
    [SerializeField]
    private GameObject _skeleton; //스폰할 스켈레톤 몬스터의 프리펩을 연결
    [SerializeField]
    private GameObject _gargoyle; //스폰할 가고일 몬스터의 프리펩을 연결

    Queue<Monster> skeleton = new Queue<Monster>(); //큐를 이용해서 오브젝트를 관리, 순서관리가 잘못되면 하나의 오브젝트를 여러 곳에 빌려주게 된다.
    Queue<Monster> gargoyle = new Queue<Monster>();

    private void Awake()
    {
        Instance = this; //오브젝트 풀링으로 몬스터들을 관리할 빈게임오브젝트에 이 스크립트를 추가
        Initialize(10);
    }

    private void Initialize(int initCount) //시작과 동시에 숫자만큼 몬스터를 생성
    {
        for (int i = 0; i < initCount; i++)
        {
            skeleton.Enqueue(CreateNewSkeleton()); //스켈레톤 생성
            gargoyle.Enqueue(CreateNewGargoyle()); //가고일 생성
        }
    }

    // 스켈레톤 오브젝트 풀링 함수
    private Monster CreateNewSkeleton()
    {
        var newObj = Instantiate(_skeleton).GetComponent<Monster>(); //몬스터를 새로생성하고 Monster컴포넌트를 연결

        newObj.gameObject.SetActive(false); //게임오브젝트 비활성화
        newObj.transform.SetParent(transform); //부모위치로 이동
        //newObj.enemyMode
        return newObj;
    }
    public static Monster GetSkeleton()  //몬스터를 요청한 자에게 빌려줄 때 사용           다른 스크립트에서 사용하는 함수
    {
        if (Instance.skeleton.Count > 0) //미리 생성된 몬스터가 남아 있을 때
        {
            var obj = Instance.skeleton.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else //생성된 몬스터가 남아있지 않다면
        {
            var newObj = Instance.CreateNewSkeleton(); //새로 생성 후 빌려준다.
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }

    // 가고일 오브젝트 풀링 함수
    private Monster CreateNewGargoyle() //가고일을 미리 만들고
    {
        var newObj = Instantiate(_gargoyle).GetComponent<Monster>(); //가고일을 새로생성하고 Monster컴포넌트를 연결

        newObj.gameObject.SetActive(false); //게임오브젝트 비활성화
        newObj.transform.SetParent(transform); //부모위치로 이동
        return newObj;
    }
    public static Monster GetGargoyle()  //가고일을 요청한 자에게 빌려줄 때 사용           다른 스크립트에서 사용하는 함수
    {
        if (Instance.gargoyle.Count > 0) //미리 생성된 몬스터가 남아 있을 때
        {
            var obj = Instance.gargoyle.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else //생성된 몬스터가 남아있지 않다면
        {
            var newObj = Instance.CreateNewGargoyle(); //새로 생성 후 빌려준다.
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);
            return newObj;
        }
    }
    public static void ReturnObject(Monster obj) //몬스터를 요청한 자가 사용후 반환할 때
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(Instance.transform);
        Instance.skeleton.Enqueue(obj);
    }
}