using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupItemImg : MonoBehaviour
{
    public Camera cam;
    public Image img;
    public Text text;
    Vector3 pos;

    public Transform tr;

    private GameObject target;

    private bool isShow;

    public Transform player;

    void Start()
    {
        isShow = false;
        StartCoroutine(ItemFind());
    }

    void Update()
    {
        if (target == null)
            return;

        if (isShow)
        {
            pos = cam.WorldToScreenPoint(target.transform.position);
            pos.y += 9f;
            img.rectTransform.anchoredPosition = pos;
            if (Input.GetKeyUp(KeyCode.F))
            {
                target.GetComponent<DropItem>().GetItem();
                isShow = true;
            }
        }
    }

    // 아이템과 플레이어간 거리가 일정 이하라면 획득 ui를 표시
    IEnumerator ItemFind()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("DropItem");

            GameObject tempGO = null;

            float dis = 500;

            for(int i =0; i< gameObjects.Length; i++)
            {
                float temp = Vector3.Distance(player.position, gameObjects[i].transform.position);

                if(dis > temp)
                {
                    dis = temp;
                    tempGO = gameObjects[i];
                }
            }
            if(dis <= 10)
            {
                ItemPickUpUiOpen();
            }
            else
            {
                ItemPickUpUiClose();
            }

            target = tempGO;
        }
    }

    void ItemPickUpUiOpen()
    {
        isShow = true;
        img.enabled = true;
        text.enabled = true;
    }
    
    void ItemPickUpUiClose()
    {
        isShow = false;
        img.enabled = false;
        text.enabled = false;
    }
}
