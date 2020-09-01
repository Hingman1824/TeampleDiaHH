using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LobyManager : MonoBehaviour
{

    public GameObject uiInven;
    public GameObject[] lobyBtn;
    AudioSource audio;
    Canvas canvas;

    private void Awake()
    {
        audio = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<AudioSource>();
        canvas = GameObject.FindGameObjectWithTag("SoundCanvas").GetComponent<Canvas>();
    }
    public void InvenOpen()
    {
        lobyBtn[0].SetActive(false); //1
        lobyBtn[1].SetActive(false); //1
        uiInven.SetActive(true);  //2
    }

    public void InvenClose()
    {
        uiInven.SetActive(false); //1
        lobyBtn[0].SetActive(true); //2
        lobyBtn[1].SetActive(true); //2
    }

    public void PlayGame()
    {
        audio.Stop(); //배경음악 종료
        canvas.enabled = false; // 설정창 비활성화
        SceneManager.LoadScene("scPlayUi");
    }
}

/*
87) LobyManager 스크립트의 변경.

   - Generic 타입 함수호출은 별도의 Type Casting(타입캐스팅) 없이 사용할 수 있다는 장점이 있다.

   - GetComponent 함수는 해당 게임오브젝트가 가진 특정 컴포넌트를 참조 할 때 사용되는 함수이다.
   - GetComponent 함수의 사용법은 다양한 문법으로 표현가능. 

  --- 일반함수 호출 타입 ---
  -  해당 컴포넌트 변수 = (컴포넌트데이타형)GetComponent("호출할컴포넌트 이름"); -> 이 스크립트가 포함된 게임오브젝트의 해당 컴포넌트의 참조를 변수에 저장. 
  -  해당 컴포넌트 변수 = (컴포넌트데이타형)GetComponent(typeof(호출할컴포넌트 이름)); -> 이 스크립트가 포함된 게임오브젝트의 해당 컴포넌트의 참조를 변수에 저장.
     (참고) typeof  키워드는 형식에 대한 System.Type 개체를 얻는 데 사용.(클래스 자체의 타입을 가져옴)  

  -   Generic 타입 -(타입 케스팅이 필요하지 않아 성능향상)
  -  해당 컴포넌트 변수 = gameobject.GetComponent<호출할컴포넌트 이름 >(); -> 이 스크립트가 포함된 게임오브젝트의 해당 컴포넌트의 참조를 변수에 저장. 
 
  -  해당 컴포넌트 변수 = GetComponent<호출할컴포넌트 이름 >(); -> 이 스크립트가 포함된 게임오브젝트의 해당 컴포넌트의 참조를 변수에 저장. 

   - >전부다 같은 의미이다.

   (중요) GetComponent<호출할컴포넌트 이름 >().enabled = ?;  이러한 문법을 우린 자주 사용할것이다. enabled 변수는 리턴된 해당 컴포넌트의 멤버변수로서
             컴포넌트의 활성, 비활성으로 true 나 false스를 받을수 있는 bool 형이다. 따라서 true/false를 주면 유니티 엔진은 해당 변수를 참고하여 그 컴포넌트를 실행/중지
             를 결정한다.(인스펙터창에 체크표시로 해당 컴포넌트(스크립트)의 활성화/비활성화 확인가능)
             단! Awake(); 함수는 활성화든 비활성화든 무조건 한번 실행된다.


   (참고) 만약 다음 소스에서 GameObject.Find ("SoundCanvas").SetActive (false); 이런식으로 처리했다면 이는 스크립트에서 해당 게임오브젝트에 참조를
             (즉 스크립트에 컴포넌트를 참조 할 수있는 변수 선언후 링크) 가 없다면 두 번다시 이 게임오브젝트를 활성화 시킬수 없다.  
            왜냐면 객체가 비활성 중이니 유니티 시스템은 GameObject.Find()  말 그대로 게임오브젝트를 찾는 함수이니깐 비활성 객체는 객체로 생각안하고
            해당 게임오브젝트의 참조값도 리턴 안한다. 즉, 이렇게 우리가 처리하면 씬 전환시 해당 사운드 Ui 를 다신 볼 수없다.
          
 


 */
