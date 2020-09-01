using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    // 이 스크립트는 미니맵에 표시되는 마커와 플레이어를 연동시켜서 플레이어가 움직이면 마커도 따라 움직인다.

    public GameObject Player;   //플레이어를 연결

    private float Ptry; //플레이어의 오일러앵글 값을 받기 위한 변수 선언

    //         주의
    //transform.rotation 값은 0~1 사이의 값으로 표현되기에 그대로 사용할수가 없다. 대신
    //transform.eulerAngles 로 Inspector에서 보이는 0~360도 값을 받아 올수 있다.

    // Start is called before the first frame update
    void Awake()
    {
        //플레이어 이름 확인 필수! 가끔씩 연결이 안되면 태그추가
        Player = GameObject.Find("Player");
        //Player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 PlayerPos = Player.GetComponent<Transform>().position; //벡터3로 플레이어 포지션을 가져옴 

        // 플레이어의 트랜스폼.오일러앵글을 통해 0~360도 사이의 값을 대입
        Ptry = Player.transform.eulerAngles.y + 90; //90도를 더한 것은 마커의 화살표 이미지가 왼 방향을 바라보고 있기때문에 정면으로 돌려주기 위해서이다.
        //쿼터니언.오일러를 통해 회전 시킬경우 0~180도 까지만 회전이 되고 180도 이후 180 -> 0도로 회전이 되기에 오른쪽으로 돌다가 다시 왼쪽으로 돈다.
        // 180도위치가 되었을때 음수로 만들어 반대로 돌게해 한방향으로 도는것 처럼 보이게 한다.
        if (Ptry > 180f) { Ptry -= 360f; }

        //플레이어가 회전함에 따라 마커도 회전
        transform.rotation = Quaternion.Euler(0, Ptry, 0);
        //플레이어가 이동하는 위치에 따라 마커의 위치도 변경
        transform.position = new Vector3(PlayerPos.x, transform.position.y, PlayerPos.z);
    }
}
