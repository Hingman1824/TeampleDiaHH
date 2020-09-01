using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour 
{
    //스테이지
    public int stage;

    public Transform[] playerPos; //플레이어 스폰위치

    //채팅을 위한 text
    public Text txtChat;
    public InputField inputChat;

    PhotonView pv;
    
    // Start is called before the first frame update
    void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerPos = GameObject.Find("PlayerSpawner").GetComponentsInChildren<Transform>();
        StartCoroutine(this.CreatePlayer()); //플레이어를 생성하는 함수 호출
        PhotonNetwork.isMessageQueueRunning = true;
        playerPos = this.gameObject.GetComponentsInChildren<Transform>(); //이 스크립트가 들어간 스테이지매니저 오브젝트 자식으로 생성시킨 플레이어 스폰위치를 연결
    }

 
    
    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator CreatePlayer() //플레이어 생성을 위한 코루틴
    {
        Room currRoom = PhotonNetwork.room;

        if (PhotonNetwork.player.UserId == "DevilHunter")
        {
            GameObject player = PhotonNetwork.Instantiate("DevilHunter", playerPos[currRoom.PlayerCount].position, playerPos[currRoom.PlayerCount].rotation, 0);
            player.name = "Player";
        }
        else if (PhotonNetwork.player.UserId == "Magicion")
        {
            GameObject player = PhotonNetwork.Instantiate("Wizard", playerPos[currRoom.PlayerCount].position, playerPos[currRoom.PlayerCount].rotation, 0);
            player.name = "Player";
        }
        else if (PhotonNetwork.player.UserId == "Monk")
        {
            GameObject player = PhotonNetwork.Instantiate("Monk", playerPos[currRoom.PlayerCount].position, playerPos[currRoom.PlayerCount].rotation, 0);
            player.name = "Player";
        }
        else if (PhotonNetwork.player.UserId == "Babarian")
        {
            GameObject player = PhotonNetwork.Instantiate("Babarian", playerPos[currRoom.PlayerCount].position, playerPos[currRoom.PlayerCount].rotation, 0);
            player.name = "Player";
        }
        yield return null;
    }
    void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        //여기서 직업정보 쏴줌
    }

    //채팅을 위함 함수
    public void OnChatButtonClick()
    {
        string chat = "\n\t" + inputChat.text;
        pv.RPC("LogChat", PhotonTargets.AllBuffered, chat);
        inputChat.text = "";
    }
    [PunRPC]
    void LogChat(string msg)
    {
        txtChat.text = txtChat.text + msg;
    }
}

