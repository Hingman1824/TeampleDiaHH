//using System; //Random 이 겹쳐서 주석처리

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayManager : MonoBehaviour
{
    // 유저인터페이스를 연결하기 위한 변수
    public GameObject playrUi;
    // 로딩(설명) Panel을 연결하기 위한 변수
    public GameObject[] exPnl;
    // 로딩 progress Text 컴포넌트를 연결하기 위한 변수
    public Text progressText;
    public Canvas canvas;
    public Image loadingbar;

    // Start 보다 먼저 호출 됨
    // 순서를 지켜 주자! 그래야 코딩도 편해지고 성능이 않조은 플랫폼에서 문제 발생을 방지.
    void Awake()
    {
        // 로딩(설명) Panel 랜덤으로 선택
        exPnl[Random.Range(0, 2)].SetActive(true);
        playrUi.SetActive(false);
        loadingbar.fillAmount = 0.0f;
    }

    // Use this for initialization
    // 코루틴 함수 호출
    void Start()
    {
        StartCoroutine(this.Loading());
        canvas = GameObject.Find("SoundCanvas").GetComponent<Canvas>();
    }

    // 유니티엔진에게 역할을 분담시켜 유니티엔진에 렌더링 루프와 별도로 처리되어 성능향상
    IEnumerator Loading()
    {
        // 1초 정도는 화면에 설명을 띄워주자 바로 게임으로 넘어가면 이상함.
        yield return new WaitForSeconds(1.0f);

        // 씬을 비동기방식으로 추가하자.
        //AsyncOperation async = Application.LoadLevelAdditiveAsync("scStage1");
        AsyncOperation async = SceneManager.LoadSceneAsync("scSelect", LoadSceneMode.Additive);

        // While 문으로 로딩진행사항을 표시해주자.
        // 현재 로딩중일 때
        while (!async.isDone)
        {
            yield return new WaitForSeconds(0.2f);
            loadingbar.fillAmount = async.progress;
            // async.progress 값은 0~1 값이다 따라서 100을 곱해주자 그래야 % 값 얻음.
            float progress = async.progress * 100.0f;
            // Mathf.RoundToInt는 float를 받아서 올림해서 인트형으로 반환
            int pRounded = Mathf.RoundToInt(progress);
            // Text 컴포넌트에 text 요소를 다음과 같이 셋팅                       
            progressText.text = "Loading..." + pRounded.ToString() + "%";
            // 한프레임동안 대기한 후 무한루프를 다시 수행한다.
            // IEnumerator문을 바로 탈출하려면 yield break 문을 사용하면 된다.
            yield return true;
        }

        // 100%를 보여주기 위함
        progressText.text = "Loading..." + Mathf.RoundToInt(async.progress * 100.0f).ToString() + "%";

        loadingbar.fillAmount = 1.0f;

        // 100% 뜨고 1초정도 기다림.
        yield return new WaitForSeconds(0.1f);

        // 아래 2줄은 StageManager.cs 있던걸 일로 옮김
        exPnl[0].transform.parent.gameObject.SetActive(false);
        canvas.enabled = true;
    }
}
