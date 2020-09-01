using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class open : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("scUi");    //동기방식
    }
}
