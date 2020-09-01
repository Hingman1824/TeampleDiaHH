using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class CameraView : MonoBehaviour
{

    [DllImport("user32")]
    private static extern int SetCursorPos(int x, int y);
    [DllImport("user32")]
    private static extern int GetCursorPos(out Vector2Int pt);


    public Transform player;
    public new Transform transform;
    public Transform cam;
    public float minY;
    public float maxY;
    private bool isUiOpen;

    public float camSpeed = 150;

    private Vector2Int mousePos;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        mousePos.x = Screen.width / 2;
        mousePos.y = Screen.height / 2;
    }

    // Use this for initialization
    void Start()
    {
        Cursor.visible = true;

        CursorLock();

    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 temp = Input.mousePosition;
        //if (isUiOpen || PlayerCtrl.instance.isPlayerDIe) return; //추후 주석 해제

        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");
        CameraRotationToMouseX(mX);
        CameraRotationToMouseY(mY);



    }

    private void LateUpdate()
    {
        float mX = Input.GetAxis("Mouse X");

        //Debug.Log(mX);

        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Locked;

    }

    void CameraRotationToMouseX(float mX)
    {
        transform.RotateAround(player.position, Vector3.up, mX * Time.deltaTime * camSpeed);

    }
    void CameraRotationToMouseY(float mY)
    {

        //transform.RotateAround(player.position, player.right, mY * Time.deltaTime * camSpeed);
        //Debug.Log(transform.rotation.x);
        //transform.Rotate(Vector3.right,(-mY*camSpeed*Time.deltaTime));


        //cam.Translate(transform.up * mY * camSpeed * Time.deltaTime);

        cam.position = new Vector3(cam.position.x, cam.position.y + mY * (camSpeed / 10f) * Time.deltaTime, cam.position.z);

        if (cam.position.y >= maxY)
            cam.position = new Vector3(cam.position.x, maxY, cam.position.z);

        if (cam.position.y <= minY)
            cam.position = new Vector3(cam.position.x, minY, cam.position.z);

        cam.LookAt(transform);
    }

    public void CursorLock()
    {
        //mousePos = Input.mousePosition;

        GetCursorPos(out mousePos);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isUiOpen = false;

    }

    public void CursorUnLock()
    {
        StartCoroutine(CursorUnLock1());
    }

    private IEnumerator CursorUnLock1()
    {

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        yield return null;
        SetCursorPos(mousePos.x, mousePos.y);


        isUiOpen = true;
    }

}
