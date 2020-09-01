using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCManager : MonoBehaviour
{
    private Transform player; //플레이 위치를 받아올 변수

    public GameObject target; //

    private bool isShow; //플레이어가 보고 있을떄

    private Vector3 pos;

    public Camera cam; // 카메라

    public Image img;
    public Text text;

    public bool isShopOpen;

    public GameObject shop;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        StartCoroutine(NPCFind());
        NPCUiClose();
    }

    void Update()
    {
        if (target == null)
            return;

        if(isShow && !isShopOpen)
        {
            pos = cam.WorldToScreenPoint(target.transform.position);
            pos.y += 10f;
            img.rectTransform.anchoredPosition = pos;
            if (Input.GetKeyUp(KeyCode.F))
            {
                shop.SetActive(true);
                isShopOpen = true;
                isShow = false;
                UIManager.instance.UIInventoryOpen();
            }
        }
    }

    IEnumerator NPCFind()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("NPC");

            GameObject tempGO = null;

            float dis = 500;

            for(int i = 0; i< gameObjects.Length; i++)
            {
                float temp = Vector3.Distance(player.position, gameObjects[i].transform.position);

                if(dis > temp)
                {
                    dis = temp;
                    tempGO = gameObjects[i];
                }
            }

            if(dis <= 3)
            {
                NPCUiOpen();
            }
            else
            {
                NPCUiClose();
            }

            target = tempGO.transform.Find("TalkBoxPos").gameObject;
        }
    }

    private void NPCUiOpen()
    {
        isShow = true;
        img.enabled = true;
        text.enabled = true;
    }

    public void NPCUiClose()
    {
        shop.SetActive(false);
        isShopOpen = false;
        isShow = false;
        img.enabled = false;
        text.enabled = false;
    }
}
