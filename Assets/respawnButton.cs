using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawnButton : MonoBehaviour
{
    PlayerManager player;
    // Start is called before the first frame update

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    public void OnClickEvent()
    {
        player.Respawn();
    }
}
