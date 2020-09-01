using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvenOnOff : MonoBehaviour
{
    private CameraView camera;
    //private csNecroSkillManager csNecroSkill;

    private void Awake()
    {
        camera = GameObject.Find("Camera").GetComponent<CameraView>();
        //csNecroSkill = GameObject.Find("NecroSkillManager").GetComponent<csNecroSkillManager>();

    }

    private void OnEnable()
    {
        camera.CursorUnLock();
        //csNecroSkill.invenOn = true;
    }



    private void OnDisable()
    {
        camera.CursorLock();

        //csNecroSkill.invenOn = false;
    }
}
