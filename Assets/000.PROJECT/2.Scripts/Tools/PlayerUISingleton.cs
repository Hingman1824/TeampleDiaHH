using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerUISingleton : MonoBehaviour
{
    private static PlayerUISingleton instance = null;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(this.gameObject);
       // Application.LoadLevel("scUi"); // 과거방식이나 지금도 가능하다.
    }
}
