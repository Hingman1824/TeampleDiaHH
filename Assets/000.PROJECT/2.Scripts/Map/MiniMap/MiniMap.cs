using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MiniMap : MonoBehaviour
{

    // 이 스크립트는 미니맵 캔버스 안에 있는 미니맵 매니져에 들어가며 버튼형식을 통해 미니맵을 키고 끄는 기능이다.

    public GameObject miniMapB; //큰 미니맵
    public GameObject miniMapS; //작은 미니맵
    public Button miniMapBtn; //미니맵 활성화 버튼
    // Start is called before the first frame update
    void Awake()
    {
        //각각의 변수에 레퍼런스 연결
        //태그 확인
        miniMapS = GameObject.FindGameObjectWithTag("MiniMapManager").transform.GetChild(0).gameObject;
        miniMapB = GameObject.FindGameObjectWithTag("MiniMapManager").transform.GetChild(1).gameObject;
        miniMapBtn = GameObject.FindGameObjectWithTag("MiniMapManager").transform.GetChild(2).GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnMiniMapOpen()
    {
        miniMapS.SetActive(true); // 작은 미니맵 활성화
        miniMapBtn.gameObject.SetActive(false); //미니맵 버튼 비활성화
    }
    public void OnMiniMapClose()
    {
        miniMapS.SetActive(false); // 미니맵 비활성화
        miniMapBtn.gameObject.SetActive(true); //미니맵 버튼 활성화
    }
    public void MiniMapBClose() //커다란 미니맵을 끄면
    {
        miniMapS.SetActive(true); // 작은 미니맵 활성화
        miniMapB.SetActive(false);//큰 사이즈의 미니맵은 비활성화
    }
    public void MiniMapBOpen() //작은 미니맵을 클릭하면
    {
        miniMapB.SetActive(true); // 커다란 미니맵 활성화
        miniMapS.SetActive(false); //작은 사이즈의 미니맵은 비활성화
    }
}
