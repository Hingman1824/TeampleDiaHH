using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class MapTimeText : MonoBehaviour
{
    private Text Timetxt;   //
    private int PlayerLevel = 70; // 플레이어의 레벨
    private String MapLevel = "보통"; //맵의 난이도
    private String Dt; // 오전과 오후를 구분하기 위해

    void Awake()
    {
        Timetxt = this.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (DateTime.Now.Hour > 12) { Dt = "오후 "; } //컴퓨터에 표시된 시간이 12시 이상이면 오후
        else if (DateTime.Now.Hour <= 12) { Dt = "오전 "; } //이하면 오전으로 표기

        //난이도 + 캐릭터레벨 + 오전or오후 + 시간
        //난이도 or 캐릭터레벨은 
        Timetxt.text =  MapLevel + " (" + PlayerLevel.ToString() + ") " + Dt + DateTime.Now.ToString(" h:m "); 
    }
}

/*              DateTime.ToString출력 양식 
               
    년 표시 y = 한 자리 연도이며, 2001은 "1"로 표시됩니다.
            yy = 연도의 마지막 두 자리이며, 2001은 "01"로 표시됩니다.
            yyyy = 완전한 형태의 연도이며, 2001은 "2001"로 표시됩니다.

    월 표시 M = 달을 나타내는 한 자리 또는 두 자리 숫자입니다.
            MM = 달을 나타내는 두 자리 숫자입니다. 한 자리로 된 값 앞에는 0이 옵니다.
            MMM = 세 문자로 된 달의 약어입니다.(Ex = 1월은 Jan으로 표시)
            MMMM = 달의 전체 이름입니다.

    일 표시 d = 한 자리 또는 두 자리 날짜입니다.
            dd = 두 자리 날짜입니다. 한 자리로 된 날짜 값 앞에는 0이 옵니다.
  요일 표시 ddd = 세 문자로 된 요일 약어입니다.
            dddd =  요일의 전체 이름입니다.

  시간 표시 h = 12시간 형식의 한 자리 또는 두 자리 시간입니다.
            hh = 12시간 형식의 두 자리 시간입니다.한 자리로 된 값 앞에는 0이 옵니다.
            H = 24시간 형식의 한 자리 또는 두 자리 시간입니다.
            HH = 24시간 형식의 두 자리 시간입니다.한 자리로 된 값 앞에는 0이 옵니다.

    분 표시 m = 한 자리 또는 두 자리 분입니다.
            mm = 두 자리 분입니다. 한 자리로 된 값 앞에는 0이 옵니다.

    초 표시 s = 한 자리 또는 두 자리 초입니다.
            ss = 두 자리 초입니다. 한 자리로 된 값 앞에는 0이 옵니다.

 Am/pm 표시 t = 한 문자로 된 A.M./P.M.약어이며, A.M.은 "A"로 표시됩니다.
            tt = 두 문자로 된 A.M./P.M.약어이며, A.M.은 "AM"으로 표시됩니다.

 Ex) 현재 시간이 2020년7월30일 오후 2시 1분 5초 일경우 
    DateTime.Now.ToString("yyyy-MM-dd HH:mm:s")
    2020-07-30 14:01:5 이런식으로 표기된다.*/
