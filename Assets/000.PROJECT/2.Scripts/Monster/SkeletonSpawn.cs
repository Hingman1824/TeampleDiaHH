using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonSpawn : MonoBehaviour
{
    public int SpawnNum = 1; //스폰 장소에서 스폰될 몬스터 갯수 조절.
    public float rand = 10f; //스폰 장소를 기준으로 생성되는 랜덤거리 조절
                             // Start is called before the first frame update

    void Update()
    {
        SpawnNum = SpawnManager.SpawnSkeleton(this.gameObject, SpawnNum, rand);
    }
}
