using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerImage : MonoBehaviour
{
    public Sprite[] CharImg;
    int CharNum;
    Image playerImg;
    PhotonView pv;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        playerImg = GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.player.UserId == "DevilHunter")
            playerImg.sprite = CharImg[0];
        else if (PhotonNetwork.player.UserId == "Magicion")
            playerImg.sprite = CharImg[1];
        else if (PhotonNetwork.player.UserId == "Monk")
            playerImg.sprite = CharImg[2];
        else if (PhotonNetwork.player.UserId == "Babarian")
            playerImg.sprite = CharImg[3];
    }
}