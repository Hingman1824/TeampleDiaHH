using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    /* 이 스크립트는 텍스트캔버스를 관리하는 스크립트 입니다.
     *  N Txt는 
    */
    public Text nTxt; //화면 우측 상단 맵 이름 표기
    public Text mTxt; //화면 중앙 맵
    public Text tTxt; //화면 우측 상단 시간 표시

    private int PlayerLevel = 70; // 플레이어의 레벨
    private String MapLevel = "보통"; //맵의 난이도
    private String Dt; // 오전과 오후를 구분하기 위해

    void Start()
    {
        FadeOut();
    }

    private void Update()
    {
        if (DateTime.Now.Hour > 12) { Dt = "오후 "; } //컴퓨터에 표시된 시간이 12시 이상이면 오후
        else if (DateTime.Now.Hour <= 12) { Dt = "오전 "; } //이하면 오전으로 표기

        //난이도 + 캐릭터레벨 + 오전or오후 + 시간
        //난이도 or 캐릭터레벨은 
        tTxt.text = MapLevel + " (" + PlayerLevel.ToString() + ") " + Dt + DateTime.Now.ToString(" h:m ");
    }
    public void FadeOut()
    {
        mTxt.color = new Color(1,1,1,1);
        mTxt.gameObject.SetActive(true); //게임오브젝트를 활성화;
        InvokeRepeating("FadeOutTxt", 3f, 0.5f); //FadeOutTxt함수를 3초후 0.5초마다 실행
    }
    void FadeOutTxt()
    {
        Color Acolor = mTxt.color; //컬러 값을 새로 생성하고 기존텍스트의 컬러 값을 대입
        Acolor.a -= 0.15f; //컬러 값의 알파 값을 감소
        mTxt.color = Acolor; //텍스트의 컬러값을 알파값을 감소시킨 컬러값으로 변경
        if (mTxt.color.a <= 0) { 
            mTxt.gameObject.SetActive(false);
            CancelInvoke("FadeOutTxt");
            
        } //텍스트의 알파값이 0이하면 게임오브젝트 비활성화
    }
}
