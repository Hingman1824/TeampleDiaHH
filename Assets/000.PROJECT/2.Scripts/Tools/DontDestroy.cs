using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
       // Application.LoadLevel("scUi"); // 과거방식이나 지금도 가능하다.
    }
}
