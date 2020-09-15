using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPortalMove : MonoBehaviour
{
    public Transform PortalPoint;
    public GameObject currentMap;
    public GameObject nextMap;
    public Text mapText;
    public GameObject midCam; //미니맵 알파캠(미드)
    public GameObject zeroCam; //미니맵 알파캠(제로)
    MiniMapCamera minimap;
    
    //MoveChoice choice;
    

    public GameObject movePopUpCanvas;
    //public int mapNum;
    //0=마을, 1 = 1페, 2 = 2페, 3 = 3페, 4=은빛탑
    //-1000,0,1000 = 은빛탑 0,0,1000 = 보스 1000,0,1000 = 2페 2000,0,1000 = 3페
    void Awake()
    {
        mapText.text = "신트리스트럼";
        minimap = GameObject.Find("MiniMapCamera").GetComponent<MiniMapCamera>();
        // choice = FindObjectOfType<MoveChoice>();
        movePopUpCanvas = GameObject.FindWithTag("NextMap");
    }

    private void Start()
    {
        movePopUpCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other) //문에 닿으면
    {
        if (other.tag == "Player") //플레이어가 문에 닿았을 때
        {
            movePopUpCanvas.SetActive(true);    //선택창 띄우기
            
            

            StartCoroutine(TransPosition(other));   //이동
        }
    }

    IEnumerator TransPosition(Collider col)     //이동
    {
        yield return new WaitForSeconds(5.0f);

        col.gameObject.transform.position = PortalPoint.position;
        //minimap.mapNum = mapNum;
        currentMap = nextMap;       //현재 맵에 다음맵 대입
    }
}
