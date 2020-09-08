using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class SpawnManager : MonoBehaviour
{
    private static int maxSkeleton = 100;
    private static int maxGargoyle = 40;
    private static int maxArac = 1;
    //private int maxDiablo = 1;

    /* 스폰매니저 
        함수를 스태틱 선언하여 스폰
     */

    private void Start()
    {
        //if()
    }
    public static int SpawnGargoyle(GameObject spawnPos ,int InitNum, float rand)
    {
        if (CheckObjectInCamera(spawnPos) ) //스폰장소가 카메라화면에 잡히면
        {
            if (InitNum > 0 && maxGargoyle > 0)
            {
                var Mons = EnemyPooling.GetGargoyle(); //오브젝트풀링에서 가고일몬스터를 빌려온다.
                Mons.GetComponent<NavMeshAgent>().enabled = false;
                Mons.transform.position = new Vector3(spawnPos.transform.position.x + Random.Range(-rand, rand), 1, spawnPos.transform.position.z + Random.Range(-rand, rand)); //빌려온 몬스터의 위치를 랜덤배치
                Mons.GetComponent<NavMeshAgent>().enabled = true;

                InitNum--;
                maxGargoyle--;
            }
        }
        return InitNum;
    }
    public static int SpawnSkeleton(GameObject spawnPos, int InitNum, float rand)
    {
        if (CheckObjectInCamera(spawnPos)) //스폰장소가 카메라화면에 잡히면
        {
            if (InitNum > 0 && maxSkeleton > 0) //
            {
                var Mons = EnemyPooling.GetSkeleton(); //오브젝트풀링에서 몬스터를 빌려온다.
                Mons.GetComponent<NavMeshAgent>().enabled = false; //네비메쉬에이전트 끄고
                Mons.transform.position = new Vector3(spawnPos.transform.position.x + Random.Range(-rand, rand),1, spawnPos.transform.position.z + Random.Range(-rand, rand)); //빌려온 몬스터의 위치를 랜덤배치
                Mons.GetComponent<NavMeshAgent>().enabled = true; //재활성화

                InitNum--;
                maxSkeleton--;
            }
        }
        return InitNum;
    }
    public static bool CheckObjectInCamera(GameObject _target) //카메라로 바라보는 화면에 오브젝트가 보여지는지 알수 있는 함수.
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(_target.transform.position);
        bool onScreen = screenPoint.z > 0 &&
                        screenPoint.x > 0 &&
                        screenPoint.x < 1 &&
                        screenPoint.y > 0 &&
                        screenPoint.y < 1;
        return onScreen;
    }
}
