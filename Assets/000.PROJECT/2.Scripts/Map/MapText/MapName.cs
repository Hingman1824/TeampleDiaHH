using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MapName : MonoBehaviour
{
    //이 스크립트는 디아블로 게임에서 화면중앙 텍스트에 FadeOut효과를 주는 스크립트이다.

    private Text Maptxt; //FadeOut효과를 줄 텍스트와 연결
        
    void Start()
    {
        Maptxt = this.GetComponent<Text>(); //이 스크립트를 넣은 텍스트와 연결
        InvokeRepeating("FadeOutTxt", 3f, 0.5f); //FadeOutTxt함수를 3초후 0.5초마다 실행
    }

    void FadeOutTxt()
    {
        Color Acolor = Maptxt.color; //컬러 값을 새로 생성하고 기존텍스트의 컬러 값을 대입
        Acolor.a -= 0.15f; //컬러 값의 알파 값을 감소
        Maptxt.color = Acolor; //텍스트의 컬러값을 알파값을 감소시킨 컬러값으로 변경
        if (Maptxt.color.a <= 0) { Destroy(gameObject); } //텍스트의 알파값이 0이하면 게임오브젝트 제거;
        
    }
}
