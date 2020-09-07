using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/* 메인카메라가 플레이어를 따라다니며 플레이어를 가리는 오브젝트를 안보이게 해주는 스크립트
 * 
 * Standard Assets에 올라와 있는 스크립트를 수정*/
public class SmoothFollow : MonoBehaviour
{

    // The target we are following
    //골드를 주웠을때 사운드 출력을 위해
    public AudioClip getGold;
    public AudioSource GoldAudio;

    public Transform target;
    // The distance in the x-z plane to the target
    [SerializeField]
    private float distance = 10.0f;
    // the height we want the camera to be above the target
    [SerializeField]
    private float height = 5.0f;

    [SerializeField]
    private float rotationDamping;
    [SerializeField]
    private float heightDamping;
    [SerializeField]
    private LayerMask layerMask; //플레이어를 가릴만한 오브젝트의 레이어를 지정

    private List<GameObject> listPrevObstacleObject = new List<GameObject>(); //플레이어를 가렸던 오브젝트

    private CapsuleCollider bounder;
    // Use this for initialization
    void Start()
    {
        bounder = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // 타겟(플레이어)정보가 없으면 함수를 타지 않음
        if (!target)
            return;

        // 
        var wantedRotationAngle = target.eulerAngles.y; //플레이어가 y축을 기준으로 회전한 값
        var wantedHeight = target.position.y + height; //플레이어의 y값 + 설정한 높이

        var currentRotationAngle = transform.eulerAngles.y; //카메라가 y축을 기준으로 회전한 값
        var currentHeight = transform.position.y; //카메라의 높이

        // 현재 카메라의 y축 회전값을 플레이어의 y축 회전값으로 설정한 시간에 걸쳐서 변경
        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

        // 현재 카메라의 높이를 플레이어의 높이+설정한 높이 만큼 설정한 시간에 걸쳐서 변경
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        // 카메라의 회전값 변경을 위해 저장..
        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        // 카메라의 위치정보 변경.
        // distance meters behind the target 카메라의 거리 조절.
        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * distance;

        // 카메라의 위치정보를 갱신
        transform.position = new Vector3(transform.position.x, wantedHeight, transform.position.z);

        // 항상 플레이어를 바라보도록
        transform.LookAt(target);

        /*   플레이어를 가리는 오브젝트 투명화   */

        //레이가 씬뷰에서 눈에보이게 발사
        Debug.DrawRay(transform.position, transform.forward * 15f, Color.red);

        Vector3 pointCenter = transform.position; //카메라의 위치정보를 저장 (카메라에서 플레이어를 향해 레이를 발사)
        Vector3 targetPosition = target.position;    // 플레이어의 위치정보

        

        
        List<RaycastHit[]> listHitInfo = new List<RaycastHit[]>(); //레이에 맞은 오브젝트의 정보를 빼오기 위해 RaycastHit을 사용
                                                                   //밑에서 RaycastAll을 사용하여 플레이어와 카메라사이의 모든 오브젝트를 투명화 시켜야해서 RaycastHit을 배열선언
                                                                   //Ray로 얻은 오브젝트정보를 저장하고 Ray에 안닿아 있을때 투명화 시킨 오브젝트를 되돌리기 위해 List로 관리

        listHitInfo.Add(Physics.RaycastAll(new Ray(pointCenter, targetPosition - pointCenter), 100.0f, layerMask));


        RaycastHit[] listHit = listHitInfo[0]; //레이캐스트힛 배열을 선언 하고 리스트에 첫번째로 저장한 값을 대입.

        List<GameObject> listNewObstacleObject = new List<GameObject>(); //현재 레이에 닿아있는 게임오브젝트를 리스트에 저장하기 위해
        
        foreach (RaycastHit hitInfo in listHit)
        {
            listNewObstacleObject.Add(hitInfo.transform.gameObject); //게임오브젝트 정보를 게임오브젝트 리스트에 추가
            
        }

        foreach (GameObject obstacleObject in listNewObstacleObject) //현재 레이에 닿아있는 오브젝트 정보를 
        {
            //현재 레이에 닿아 있는 오브젝트 정보가 플레이어를 가렸던 오브젝트 정보를 저장한 리스트에 동일한게 있는지 확인 동일한게 없다면
            if (!listPrevObstacleObject.Find(delegate (GameObject inObject) { return (inObject.name == obstacleObject.name); }))
            {
                listPrevObstacleObject.Add(obstacleObject); //오브젝트 정보를 플레이어를 가렸던 오브젝트 리스트에 저장
                obstacleObject.GetComponent<MeshRenderer>().enabled = false; ; //오브젝트 투명화(메쉬렌더러를 그려주지 않음)
            }
        }

        for (int a = 0; a < listPrevObstacleObject.Count; a++) //플레이어를 가렸던 오브젝트 리스트들 중에서
        {
            if (!CheckObjectInCamera(listPrevObstacleObject[a])) //화면에 안나온다면
            {
                listPrevObstacleObject[a].GetComponent<MeshRenderer>().enabled = true; //투명화 시켰던 오브젝트를 다시 그려줌()
                listPrevObstacleObject.Remove(listPrevObstacleObject[a]); //저장되어 있는 오브젝트 제거
            }
        }

        // foreach문으로 돌리면 에러가 발생, if문을 통해 에러를 잡았지만 처음만 오브젝트를 가리고 다시그려줌
        //foreach (GameObject obstacleObject in listPrevObstacleObject) //플레이어를 가렸던 오브젝트 리스트들 중에서
        //{
        //    if (!CheckObjectInCamera(obstacleObject)) //화면에 안나온다면
        //    {
        //        obstacleObject.GetComponent<MeshRenderer>().enabled = true; //투명화 시켰던 오브젝트를 다시 그려줌()
        //        listPrevObstacleObject.Remove(obstacleObject); //저장되어 있는 오브젝트 제거
        //    }
        //}

        listNewObstacleObject = listPrevObstacleObject; //현재 닿아있는 오브젝트 정보 리스트를 플레이어를 가렸던 오브젝트 리스트와 동일화
    }

    public bool CheckObjectInCamera(GameObject _target) //카메라로 바라보는 화면에 오브젝트가 보여지는지 알수 있는 함수.
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(_target.transform.position);
        bool onScreen = screenPoint.z > 0 &&
                        screenPoint.x > 0 &&
                        screenPoint.x < 1 &&
                        screenPoint.y > 0 &&
                        screenPoint.y < 1;
        return onScreen;
    }

    //골드를 주을때 효과음(아이템에서 출력하면 소리가 너무 짧고 작아서 안들림)
    public void GoldSound()
    {
        GoldAudio.PlayOneShot(getGold);
    }
}

/*
//레이를 리스트 선언하면 레이를 
List<Ray> listRay = new List<Ray>();

Vector3 targetPosition = target.position;    // 플레이어의 위치정보
listRay.Add(new Ray(pointCenter, targetPosition - pointCenter)); //리스트로 레이를 응용하면 각기 다른 장소에서 레이를 쏠 수 있다.
listRay.Add(new Ray(pointCenter -= new Vector3(0f,1f,0f), targetPosition - pointCenter)) // 이런 식으로 레이를 여러 장소에서 쏴서 한 오브젝트에 맞히고
            List<RaycastHit[]> listHitInfo = new List<RaycastHit[]>();

            foreach (Ray ray in listRay) //리스트에 넣은 레이를 반복
            {
                RaycastHit[] hitInfo = Physics.RaycastAll(ray, 100.0f, layerMask); //레이캐스트 힛 으로 레이에 맞은 모든 오브젝트 정보를 가져옴
                listHitInfo.Add(hitInfo); //가져온 오브젝트 정보를 레이캐스트힛 리스트에 저장
            }
*/
