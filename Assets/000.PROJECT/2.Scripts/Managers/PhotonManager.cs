using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class PhotonManager : MonoBehaviour
{
    public string version = "Version 1.0.0";
    public PhotonLogLevel LogLevel = PhotonLogLevel.Full;//로그 단계
    public InputField userId;
    public GameObject roomList;
    public Transform playerPos;
    public GameObject scrollRect;
    SelectManager selectManager;
    public Canvas SelectFinal;

    private void Awake()
    {
        SelectFinal.gameObject.SetActive(false);
        selectManager = GameObject.Find("SelectManager").GetComponent<SelectManager>();


        if (!PhotonNetwork.connected)
        {
            //포톤 접속시 처음 호출
            PhotonNetwork.ConnectUsingSettings(version);//버전세팅
            PhotonNetwork.logLevel = LogLevel;
            PhotonNetwork.playerName = "Guest" + Random.Range(1, 99);
        }
        scrollRect.GetComponent<RectTransform>().pivot = new Vector2(0.0f, 1.0f);//피봇 재설정
    }

    void Update()
    {
        if (selectManager.Select == true)
        {
            SelectFinal.gameObject.SetActive(true);
        }

    }

    //생성확인화면에서 만약 뒤로가기 버튼이 눌린다면 다시 초기 선택창으로 돌아오게됨
    public void OnClickBack()
    {
        SelectFinal.gameObject.SetActive(false);
        selectManager.Select = false;
        selectManager.SelectStart.gameObject.SetActive(true);
    }

    //방이 없는경우 방을 생성하여 입장
    private void OnPhotonRandomJoinFailed()
    {
        bool isSucces = PhotonNetwork.CreateRoom("DiabloRoom");
    }

    //방생성 실패시 로그
    private void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        Debug.Log(codeAndMsg[0].ToString());
        Debug.Log(codeAndMsg[1].ToString());
    }

    //방접속시
    private void OnJoinedRoom()
    {
        StartCoroutine(this.LoadStage());
    }

    private IEnumerator LoadStage()
    {
        //씬 전환 동안 포톤 클라우드 서버로부터 네트워크 메세지 수신을 중단함
        PhotonNetwork.isMessageQueueRunning = false;
        MatchJob(selectManager.SelChar);
        AsyncOperation async = SceneManager.LoadSceneAsync("NewTristrum");

        yield return async;
    }
    public void MatchJob(int selChar)
    {
        if(selChar == 1)
        {
            PhotonNetwork.player.UserId = "DevilHunter";
        }else if(selChar == 2)
        {
            PhotonNetwork.player.UserId = "Magicion";
        }else if(selChar == 3)
        {
            PhotonNetwork.player.UserId = "Monk";
        }else if(selChar == 4)
        {
            PhotonNetwork.player.UserId = "Babarian";
        }
    }
    //버튼용
    public void OnClickJoinRoom()
    {
        PhotonNetwork.player.NickName = userId.text;
        PhotonNetwork.JoinRandomRoom();
    }

    //방 목록 변경시 또는 최초 접속 시 호출
    private void OnReceivedRoomListUpdate()
    {
        //기존 방 삭제
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("RoomList"))
        {
            Destroy(obj);
        }
        int rowCount = 0;
        scrollRect.GetComponent<RectTransform>().sizeDelta = Vector2.zero;

        foreach (RoomInfo _room in PhotonNetwork.GetRoomList())
        {
            GameObject room = (GameObject)Instantiate(roomList);
            room.transform.SetParent(scrollRect.transform, false);

            //룸 데이터
            RoomData roomData = room.GetComponent<RoomData>();
            roomData.roomName = _room.Name;
            roomData.connectPlayer = _room.PlayerCount;
            roomData.maxPlayer = _room.MaxPlayers;

            roomData.DisplayRoomData();

            //동적연결
            roomData.GetComponent<Button>().onClick.AddListener(delegate { OnClickRoomList(roomData.roomName); });
            //레이아웃 카운트 증가
            scrollRect.GetComponent<GridLayoutGroup>().constraintCount = ++rowCount;
            //영역 높이 증가
            scrollRect.GetComponent<RectTransform>().sizeDelta += new Vector2(0, 40);
        }
    }

    private void OnClickRoomList(string roomName)
    {
        PhotonNetwork.player.NickName = userId.text;
        PhotonNetwork.JoinRoom(roomName);
    }
}
