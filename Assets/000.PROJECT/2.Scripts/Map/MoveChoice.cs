using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveChoice : MonoBehaviour
{
    private GameObject player;
    MapPortalMove portal;       //포탈오브젝트
    LoadingScript loading;      //로딩창

    public Text mapText;
    public GameObject midCam; //미니맵 알파캠(미드)
    public GameObject zeroCam; //미니맵 알파캠(제로)

    private void Awake()
    {
        loading = FindObjectOfType<LoadingScript>();        //로딩창 연결
        portal = FindObjectOfType<MapPortalMove>();         //포탈오브젝트 연결

        player = GameObject.FindGameObjectWithTag("Player");


        mapText = GameObject.FindGameObjectWithTag("text").GetComponent<Text>();
        midCam = GameObject.Find("CamAlphaMid");
        zeroCam = GameObject.Find("CamAlphaZero");
    }


    public void OnClickAcceptBtn()
    {
        if (portal.currentMap.name == "SilverTop")
        {
            portal.mapNum = 4;
            /*portal.*/mapText.text = "은빛 탑";
            /*portal.*/midCam.transform.position = new Vector3(-1001.8f, -10f, 1013f);
            /*portal.*/zeroCam.transform.position = new Vector3(-1001.8f, -10f, 1013f);
            portal.movePopUpCanvas.SetActive(false);        //선택창 비활성
            loading.Loading(); //로딩화면을 띄워주고
        }
        else if (portal.currentMap.name == "FirstCrystal")
        {
            portal.mapNum = 1;
            /*portal.*/mapText.text = "수정회랑";
            /*portal.*/midCam.transform.position = new Vector3(0f, -45f, 1045f); //마커 알파값 = 3,3
            /*portal.*/zeroCam.transform.position = new Vector3(0f, -45f, 1045f);
            portal.movePopUpCanvas.SetActive(false);
            loading.Loading();
        }
        else if (portal.currentMap.name == "Tristrum")
        {
            /*portal.*/mapText.text = "신트리스트럼";
            portal.mapNum = 0;
            portal.movePopUpCanvas.SetActive(false);
            loading.Loading();
        }
        //else if (portal.currentMap.name == "SecondCrystal")
        //{
        //    portal.mapNum = 2;
        //    portal.mapText.text = "공포의 영역";
        //    //보스 2페이즈 미니맵 카메라 포지션
        //    portal.midCam.transform.position = new Vector3(1000f, -70f, 1000f); //마커 알파값 = 5,5
        //    portal.zeroCam.transform.position = new Vector3(1000f, -70f, 1000f); //미니맵 알파캠의 위치 변경
        //    portal.movePopUpCanvas.SetActive(false);
        //    loading.Loading();
        //}
        //else if (portal.currentMap.name == "LastCrystal")
        //{
        //    portal.mapNum = 3;
        //    portal.mapText.text = "수정회랑";
        //    //보스 3페이즈 미니맵 카메라 포지션
        //    portal.midCam.transform.position = new Vector3(2000f, -30f, 1000f);
        //    portal.zeroCam.transform.position = new Vector3(2000f, -30f, 1000f); //미니맵 알파캠의 위치 변경
        //    portal.movePopUpCanvas.SetActive(false);
        //    loading.Loading();
        //}
    }


    public void OnClickDieBtn()
    {
            /*portal.*/mapText.text = "신트리스트럼";
            portal.mapNum = 0;
            portal.movePopUpCanvas.SetActive(false);
            loading.Loading();
    }
    public void MapMove2()
    {
        portal.mapNum = 2;
        /*portal.*/mapText.text = "공포의 영역";
        //보스 2페이즈 미니맵 카메라 포지션
        /*portal.*/midCam.transform.position = new Vector3(1000f, -70f, 1000f); //마커 알파값 = 5,5
        /*portal.*/zeroCam.transform.position = new Vector3(1000f, -70f, 1000f); //미니맵 알파캠의 위치 변경
        //portal.movePopUpCanvas.SetActive(false);
        loading.Loading();
    }
    public void MapMove3()
    {
        portal.mapNum = 3;
        /*portal.*/mapText.text = "수정회랑";
        //보스 3페이즈 미니맵 카메라 포지션
        /*portal.*/midCam.transform.position = new Vector3(2000f, -30f, 1000f);
        /*portal.*/zeroCam.transform.position = new Vector3(2000f, -30f, 1000f); //미니맵 알파캠의 위치 변경
        //portal.movePopUpCanvas.SetActive(false);
        loading.Loading();
    }


    public void OnClickDenyBtn()            //거절 버튼을 누르면
    {
        StopAllCoroutines();                //코루틴 중지일텐데 안되네...;;;
    }

}
