/*

test하는 방법
1. 우선 28th May UI Android 폴더에  test 폴더를 만든다
2. SaveData() 주석을 풀고, TestSave씬의 게임오브젝트에 Test Save 스크립트를 연결하고
3. Score값, Point값 적당히 입력한 후 씬을 플레이한다. 그럼 Save.dll파일이 생긴다.

4. LoadData() 주석을 풀고,
5. Score값, Point값 Delete한 후 플레이 하면 세이브 시에 입력한 숫자가 나올 것.


 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class TestSave : MonoBehaviour
{
    public int score;
    public int point;

    // 문자열 저장 변수
    string strFilePath;

    // Start is called before the first frame update
    void Start()
    {
        SaveData();
        //LoadData();
    }

    public void SaveData()
    {
        // strFilePath = Application.dataPath + "/Save.dll"; // 실행파일 루트에 저장파일 생성(절대저장)
        // strFilePath = "C:/test/Save.dll";

        // 실행파일 루트에 저장파일 생성(상대저장)
        strFilePath = "./test/Save.dll";
        // 디버깅을 위한 함수로 콜솔 뷰로 문자열 등 여러 데이터를 보낼수 있다.(함수 오버로딩)
        Debug.Log(strFilePath);
        // 파일 스트림을 쓰기 모드로 오픈한다.
        FileStream fs = new FileStream(strFilePath, FileMode.Create, FileAccess.Write);
        // 오픈 실패시 함수 종료
        if (fs == null)
        {
            return;
        }
        // 문자열로 저장한다.
        // StreamWriter sv = new StreamWriter(fs);
        // sv.WriteLine(score); // 한 라인씩 저장
        // sv.WriteLine(point);

        // 기계어로 저장한다 (보통 이걸 사용)
        BinaryWriter sw = new BinaryWriter(fs);
        sw.Write(score);
        sw.Write(point);

        sw.Close();
        fs.Close();
    }

    void LoadData()
    {
        strFilePath = "./test/Save.dll"; // test폴더 생성할 곳은  sln파일있는 곳.

        // 해당 파일이 없을 시 함수 종료
        if (File.Exists(strFilePath) == false)
        {
            return;
        }
        // 파일 스트림을 읽기 모드로 오픈한다.
        FileStream fs = new FileStream(strFilePath, FileMode.Open, FileAccess.Read);
        // 오픈 실패시 함수 종료
        if (fs == null)
        {
            return;
        }
        // 문자열을 읽기 위한 StreamReader 생성
        // StreamReader sr = new StreamReader(fs);
        // score = int.Parse(sr.ReadLine()); // 한 라인씩 읽어드리고 인트형 변환
        // point = int.Parse(sr.ReadLine());

        // 기계어를 읽기 위한 StreamReader 생성
        BinaryReader sr = new BinaryReader(fs);
        score = sr.ReadInt32();
        point = sr.ReadInt32();

        sr.Close();
        fs.Close();

        // 문자열 저장들 확인한다.
        Debug.Log("END");
    }
}


///*
//134) 게임저장

//  -  데이터를 저장 및 불러오는 방법은 총 4가지 있다.

//   1) 하드웨어 저장 방식으로 레지스트리에 저장 및 불러오기.(모바일에서 해당 게임을 삭제하면 해당 데이타도 사라짐)

//   2) 하드웨어 저장 방식으로 원하는 디렉터리에 저장 및 불러오기.(저장 파일을 다른 pc에 복사해서 게임을 연결해서 할 수 있다.)

//   3) 각종 클라우드 서비스에 저장 및 불러오기. (게임과 관련 데이타가 삭제되더라도 불러오기 가능)

//   4) Sever 와 데이타베이스 연동으로 저장 및 불러오기. (많은 데이타를 저장 및 관리 할수있다)



// * 차후 3,4 번을 다룰 것이며 이 번장에선 1,2 번을 진행할것이다.



//-  PlayerPrefs -> 이 클래스는 안드로이드에서 사용한 프리퍼렌스와 같은 의미이고 사용법도 같다.
//                     -> 게임 씬 간의 데이터를 공유할 때 사용
//                     -> 배열의 값도 집어넣을 수 있다.



//-  레지스트리에 저장 및 불러오기


//    - 저장하기-


//    -  PlayerPrefs.Set자료형(키값, 저장할값);


//    * 저장하고 싶은 데이터의 자료형에 따라서 다음과 같이 Int, Float, String 을 입력한다.


//    * PlayerPrefs.SetInt(?,?)
//    * PlayerPrefs.SetFloat(?,?)
//    * PlayerPrefs.SetString(?,?)


//    - 불러오기-


//    -  PlayerPrefs.Get자료형(저장한키값, 초기화값);


//    * 불러오고 싶은 데이터의 자료형에 따라서 다음과 같이 Int, Float, String 을 입력한다.

//    * PlayerPrefs.GetInt(?,?)
//    * PlayerPrefs.GetFloat(?,?)
//    * PlayerPrefs.GetString(?,?)


//   -> 활용

//       int score = 777;

//// 다른 씬에서 저장

//public void SaveData()

//{
//    PlayerPrefs.SetInt("SCORE", score);
//}



//여기서 SCORE 는 문자열로 할당한 키값이다.저장한 데이터를 나중에 불러오기 위해선 이 문자열로 레지스트리에 할당한 키값을 참고하게 된다.

//// 다른 씬에서 불러올수있다. 물론 저장 한 씬에서도 불러올수있다.

//public void LoadData()

//{
//    score = PlayerPrefs.GetInt("SCORE");
//}

//score에 레지스트리에 PlayerPrefs로 저장한 값들 중에서 "SCORE"로 정의한 키를 가진 값을 가져와서 score에 저장.


//(참고)  윈도우 시작버튼->regedit.exe->HKEY_CURRENT_USER->Software->DefaultCompany(유니티 프로젝트설정)
//        ->Study(내 프로젝트)에 보면 우리가 저장한 키값과 데이터를 볼 수있다.

//*/
