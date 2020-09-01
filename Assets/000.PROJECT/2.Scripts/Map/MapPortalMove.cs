using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPortalMove : MonoBehaviour
{
    public Transform PortalPoint;
    LoadingScript loading;
    public GameObject currentMap;
    public GameObject nextMap;
    public Text mapText;
    public GameObject midCam; //미니맵 알파캠(미드)
    public GameObject zeroCam; //미니맵 알파캠(제로)
    MiniMapCamera mapName;

    public int mapNum = 0;
    //-1000,0,1000 = 은빛탑 0,0,1000 = 보스 1000,0,1000 = 2페 2000,0,1000 = 3페
    void Awake()
    {
        mapText.text = "신트리스트럼";
        loading = FindObjectOfType<LoadingScript>();
        mapName = FindObjectOfType<MiniMapCamera>();
    }

    void OnTriggerEnter(Collider other) //문에 닿으면
    {
        if (other.tag == "Player") //플레이어가 문에 닿았을 때
        {
            currentMap = nextMap;

            if (currentMap.name == "SilverTop")
            {
                mapNum = 4;
                mapText.text = "은빛 탑";
                midCam.transform.position = new Vector3(-1001.8f, -10f, 1013f);
                zeroCam.transform.position = new Vector3(-1001.8f, -10f, 1013f);
            }
            else if (currentMap.name == "FirstCrystal")
            {
                mapNum = 1;
                mapText.text = "수정회랑";
                midCam.transform.position = new Vector3(0f, -45f, 1045f); //마커 알파값 = 3,3
                zeroCam.transform.position = new Vector3(0f, -45f, 1045f);
            }
            else if(currentMap.name == "Tristrum")
            { 
                mapText.text = "신트리스트럼";
                mapNum = 0;
            }
            else if(currentMap.name == "SecondCrystal")
            {
                mapNum = 2;
                mapText.text = "공포의 영역";
                //보스 2페이즈 미니맵 카메라 포지션
                midCam.transform.position = new Vector3(1000f, -70f, 1000f); //마커 알파값 = 5,5
                zeroCam.transform.position = new Vector3(1000f, -70f, 1000f); //미니맵 알파캠의 위치 변경
            }
            else if (currentMap.name == "LastCrystal")
            {
                mapNum = 3;
                mapText.text = "수정회랑";
                //보스 3페이즈 미니맵 카메라 포지션
                midCam.transform.position = new Vector3(2000f, -30f, 1000f);
                zeroCam.transform.position = new Vector3(2000f, -30f, 1000f); //미니맵 알파캠의 위치 변경
            }

            loading.Loading(); //로딩화면을 띄워주고

            other.gameObject.transform.position = PortalPoint.position; //플레이어의 위치를 변경 //마커는 플레이어의 위치를 따라감
        }
    }
}
