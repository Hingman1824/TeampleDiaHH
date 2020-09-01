using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    //이 스크립트는 아이템 안에 캔버스에 집어넣어 버튼과 텍스트가 카메라쪽을 바라보도록 해준다.

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(-Camera.main.transform.position);
    }
}
