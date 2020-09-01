using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class LoadingScript : MonoBehaviour
{
    /*이 스크립트는 로딩화면을 보여주며 */
    public Image[] Ruby; //로딩 하면서 보석이 채워지는 효과를 주기위해
    public GameObject roadingImg; //로딩 이미지

    TextManager txtmanager;
    MapPortalMove mapText;

    public void Awake()
    {
        txtmanager = FindObjectOfType<TextManager>();
        mapText = FindObjectOfType<MapPortalMove>();
    }
    public void Loading()
    {
        roadingImg.SetActive(true); //로딩화면 띄워주고
        StartCoroutine(LodingBar()); //보석 채워주는 효과
    }

   IEnumerator LodingBar()
    {
        for (int i = -2; i <= 7; i++) //Ruby이미지를 첫번째 부터 변경시켜줌으로써 점점 채워지는효과를 줌.
        {
            yield return new WaitForSeconds(0.5f);
            Color aColor = new Color(200f / 255f, 40f / 255f, 40f / 255f); //밝은 보석
            Color bColor = new Color(100f / 255f, 30f / 255f, 30f / 255f); //조금 어두운 보석
            Color cColor = new Color(80f / 255f, 80f / 255f, 80f / 255f); // 어두운 보석

            if (i == -2) //첫번째 루비가 어두워지고
            {
                Ruby[i + 2].color = cColor;
            }
            if (i == -1) //첫번째 루비가 조금 어두워지고 2번째 루비는 어둡게
            {
                Ruby[i + 1].color = bColor;
                Ruby[i + 2].color = cColor;
            }
            if (i == 0) //첫번째 루비가 채워지고 2번째 루비는 조금 어둡게 3번째 루비는 어둡게 만듬
            {
                Ruby[i].color = aColor;
                Ruby[i + 1].color = bColor;
                Ruby[i + 2].color = cColor;
            }
            if (i == 1) //두번째 루비가 채워지고 양옆은 조금어둡게 양끝은 어둡게
            {
                Ruby[i - 1].color = bColor;
                Ruby[i].color = aColor;
                Ruby[i + 1].color = bColor;
                Ruby[i + 2].color = cColor;
            }
            if (i == 2)//세번째 루비가 채워지고 양옆은 조금어둡게 맨양쪽은 어둡게
            {
                Ruby[i - 2].color = cColor;
                Ruby[i - 1].color = bColor;
                Ruby[i].color = aColor;
                Ruby[i + 1].color = bColor;
                Ruby[i + 2].color = cColor;

            }
            if (i == 3) //네번째 루비가 채워지고 양옆은 조금 어둡게 2번째 루비는 어둡게
            {
                Ruby[i - 2].color = cColor;
                Ruby[i - 1].color = bColor;
                Ruby[i].color = aColor;
                Ruby[i + 1].color = bColor;
            }
            if (i == 4) //다섯번째 루비가 채워지고 4번째 루비는 조금 어둡게 3번째 루비는 어둡게 만듬
            {
                Ruby[i - 2].color = cColor;
                Ruby[i - 1].color = bColor;
                Ruby[i].color = aColor;
            }
            if (i == 5) //다섯번째 루비가 조금 어두워지고 4번째는 어둡게
            {
                Ruby[i - 2].color = cColor;
                Ruby[i - 1].color = bColor;
            }
            if (i == 6) //5번째 루비를 어둡게
            {
                Ruby[i - 2].color = cColor;
            }
            
        }
        roadingImg.SetActive(false); //끝나면 로딩화면 비활성화

        if (mapText.mapText.text == "신 트리스트럼")
        {
            txtmanager.mTxt.text = mapText.mapText.text;
        }
        else if (mapText.mapText.text == "은빛 탑")
        {
            txtmanager.mTxt.text = mapText.mapText.text;
        }
        else if (mapText.mapText.text == "수정회랑")
        {
            txtmanager.mTxt.text = mapText.mapText.text;
        }
        //if (mapName.mapNum == 3)
        //{
        //    txtmanager.mTxt.text = txtmanager.nTxt.text = "공포의 영역";
        //}
        //if (mapName.mapNum == 4)
        //{
        //    txtmanager.mTxt.text = txtmanager.nTxt.text = "수정 회랑";
        //}

        txtmanager.FadeOut();
    }
}
