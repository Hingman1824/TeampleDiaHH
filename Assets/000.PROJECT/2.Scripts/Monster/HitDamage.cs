using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitDamage : MonoBehaviour
{
    TextMesh damage;
    PlayerManager pm;
    Monster mon;

    // Start is called before the first frame update
    void Start()
    {
        pm = FindObjectOfType<PlayerManager>();
        mon = FindObjectOfType<Monster>();
        damage = GetComponent<TextMesh>();
    }

    // Update is called once per frame
    void Update()
    {
        damage.text = pm.playerDamage.ToString();
    }
}
