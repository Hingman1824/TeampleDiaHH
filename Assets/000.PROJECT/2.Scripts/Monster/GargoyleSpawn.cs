using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GargoyleSpawn : MonoBehaviour
{
    public int SpawnNum = 3; //스폰 장소에서 스폰될 몬스터 갯수 조절.
    public float rand = 10f; //스폰 장소를 기준으로 생성되는 랜덤거리 조절
    // Start is called before the first frame update
    void Update()
    {
        SpawnNum = SpawnManager.SpawnGargoyle(this.gameObject, SpawnNum, rand);
    }

}

/*
    스폰 매니저  싱글톤
    
    스폰 장소 = 스폰 위치
    스폰 몬스터 수
    스폰 몬스터 종류
    
    오브젝트 풀링
 */

