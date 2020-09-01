using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectManager : MonoBehaviour
{
    //초기 선택화면
    public Canvas SelectStart;
    //선택한 닉네임과 영웅 확인화면
    //public Canvas SelectFinal;

    //4개의 영웅과 설명을 담는 배열
    [Header("Hero Select")]
    public GameObject[] Char;
    public GameObject[] CharEx;

    //생성시 오류가 났을때 빨간!뜨면서 !를 누르면 에러메시지 출력
    [Header("Create Error")]
    public Text ErrorMsg;
    public Image ErrorImg;
    //닉네임을 입력하는 인풋필드
    public InputField InputName;

    //영웅들의 애니메이션 참조
    [Header("Hero Animator")]
    public Animator DevilHunter;
    public Animator Wizard;
    public Animator Monk;
    public Animator Barbarian;

    //유저의 닉네임을 넘겨주는 텍스트
    [Header("UserSelect")]
    public Text UserId;

    //유저가 어떤 영웅을 선택했는지 변수로 저장
    public int SelChar;

    public bool Select = false;

    void Awake()
    {
        //생성 확인 캔버스를 비활성화
        //  SelectFinal.gameObject.SetActive(false);

        //에러 메시지를 출력할 텍스트의 색상과 이미지 초기에는 아무 에러가 있으면 안되므로 비활성화
        ErrorMsg.color = new Color(255, 0, 0, 200);
        ErrorMsg.gameObject.SetActive(false);
        ErrorImg.enabled = false;

        //모든 영웅들을 비활성화
        foreach (GameObject CharArr in Char)
        {
            CharArr.SetActive(false);
        }
        //모든 설명창을 비활성화
        foreach (GameObject ExArr in CharEx)
        {
            ExArr.SetActive(false);
        }
    }

    void Start()
    {
        //가장 위에 있는 영웅이 악마사냥꾼이므로 먼저 해당 오브젝트들만 활성화 시키고 애니메이션 플레이
        //악마사냥꾼은 변수 1로 지정
        Char[0].SetActive(true);
        CharEx[0].SetActive(true);
        SelChar = 1;
        DevilHunter.SetTrigger("Anim");
    }

    //악마사냥꾼을 선택했을때, 다른 오브젝트를 비활성화 및 악마사냥꾼만 활성화 시키고 애니메이션 플레이
    //악마사냥꾼은 변수 1로 지정
    public void OnClickDevilHunter()
    {
        foreach (GameObject CharArr in Char)
        {
            CharArr.SetActive(false);
        }
        foreach (GameObject ExArr in CharEx)
        {
            ExArr.SetActive(false);
        }

        Char[0].SetActive(true);
        CharEx[0].SetActive(true);
        DevilHunter.SetTrigger("Anim");
        SelChar = 1;
    }

    //마법사을 선택했을때, 다른 오브젝트를 비활성화 및 마법사만 활성화 시키고 애니메이션 플레이
    //마법사는 변수 2로 지정
    public void OnClickWizard()
    {
        foreach (GameObject CharArr in Char)
        {
            CharArr.SetActive(false);
        }
        foreach (GameObject ExArr in CharEx)
        {
            ExArr.SetActive(false);
        }

        Char[1].SetActive(true);
        CharEx[1].SetActive(true);
        SelChar = 2;
        Wizard.SetTrigger("Anim");
    }

    //수도사을 선택했을때, 다른 오브젝트를 비활성화 및 수도사만 활성화 시키고 애니메이션 플레이
    //수도사는 변수 3로 지정
    public void OnClickMonk()
    {
        foreach (GameObject CharArr in Char)
        {
            CharArr.SetActive(false);
        }
        foreach (GameObject ExArr in CharEx)
        {
            ExArr.SetActive(false);
        }

        Char[2].SetActive(true);
        CharEx[2].SetActive(true);
        SelChar = 3;
        Monk.SetTrigger("Anim");
    }

    //야만용사을 선택했을때, 다른 오브젝트를 비활성화 및 야만용사만 활성화 시키고 애니메이션 플레이
    //야만용사는 변수 4로 지정
    public void OnClickBarbarian()
    {
        foreach (GameObject CharArr in Char)
        {
            CharArr.SetActive(false);
        }
        foreach (GameObject ExArr in CharEx)
        {
            ExArr.SetActive(false);
        }

        Char[3].SetActive(true);
        CharEx[3].SetActive(true);
        SelChar = 4;
        Barbarian.SetTrigger("Anim");
    }

    //닉네임을 입력하고 생성버튼을 눌렀을때
    public void OnClickCreate()
    {
        //인풋필드의 텍스트에 입력된 값이 NULL값이 아니거나 길이가 최대길이를 넘어가지 않았을때
        //유저닉네임을 기억할 수 있는 string형 변수에 저장하고 생성확인창에서 다시한 번 확인하기 위해 참조하는 텍스트에 저장
        if (InputName.text != string.Empty && InputName.text.Length < 10)
        {
            string playerNickName = InputName.text;
            UserId.text = InputName.text;

            Select = true;
            SelectStart.gameObject.SetActive(false);

            //초기 선택화면 캔버스를 비활성화 하고 선택확인화면 캔버스를 활성화
            // SelectStart.gameObject.SetActive(false);
            // SelectFinal.gameObject.SetActive(true);
        }

        //만약 인풋필드에 아무 값도 입력되지 않았다면 에러 이미지를 띄우고 해당 에러 메시지를 저장
        else if (InputName.text == string.Empty)
        {
            ErrorImg.enabled = true;
            ErrorMsg.text = "생성할 영웅의 이름을 입력하세요.";

        }
        //만약 인풋필드의 입력된 값이 제한글자 이상으로 넘어갔을 경우 에러 이미지를 띄우고 해당 에러 메시지를 저장
        else if (InputName.text.Length > 10)
        {
            ErrorImg.enabled = true;
            ErrorMsg.text = "생성 가능한 이름의 최대 길이는 10자 입니다.";
        }
    }

    //에러 발생 시 빨간 느낌표가 나오는데 그것을 누르면 에러메시지가 출력
    public void OnClickShowError()
    {
        ErrorMsg.gameObject.SetActive(true);
    }



    ////모든 확인이 끝나고 게임시작버튼을 누르면 씬을 이동시킨다.
    //public void OnClickStart()
    //{
    //    SceneManager.LoadScene("scStage1");
    //}
}
