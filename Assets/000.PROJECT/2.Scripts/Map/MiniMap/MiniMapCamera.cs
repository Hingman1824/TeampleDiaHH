using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniMapCamera : MonoBehaviour
{
    // 이 스크립트는 카메라가 마커를 따라다니며 미니맵에 비쳐줌
    
    [SerializeField]
    private Transform PlayerMarker; //마커의 위치정보를 가져오기 위해

    public GameObject MinimapS; //작은 미니맵
    public GameObject MinimapB; //큰 미니맵
    public GameObject Alpha; //마커 안에 있는 알파값

    public int mapNum =0; //현재위치를 알려주는 변수 0=마을, 1 = 1페, 2 = 2페, 3 = 3페, 4=은빛탑

    private SoundManager playBgm;
    private int bgmNum = 0; //현재 재생중인 브금정보를 알려주기위해
    // Update is called once per frame
    void Awake()
    {
        playBgm = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }
    void Update()
    {
        if (!PlayerMarker) // 마커가 없으면 리턴
            return;

        

        if (MinimapS.activeSelf) //작은 미니맵이 활성화 되어 있다면
        {
            transform.position = new Vector3(PlayerMarker.position.x, PlayerMarker.position.y + 10.0f, PlayerMarker.position.z); // 카메라의 위치를 마커의 위치로 변경
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);  //// 미니맵카메라는 x축 로테이션을 90도로 돌려서 수직으로 미니맵을 보게 하여 2D 느낌이 나도록 설정
            PlayerMarker.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); //마커의 크기를 미니맵의 크기에 따라 변경
            this.GetComponent<Camera>().fieldOfView = 90f; //카메라의 필드오브뷰를 90고정.

            //마커 크기 변경에 따른 알파값 크기 변경 (알파값크기를 조절하며 미니맵을 비춰주는 것을 조절)
            if (mapNum == 0)
            {
                Alpha.transform.localScale = new Vector3(3f, 3f, 3f);
                if (!playBgm.audio.isPlaying || bgmNum != 1) //재생이 멈췄거나 마을브금이 아니라면
                {
                    playBgm.PlayBackground(3); //마을브금을 재생
                    bgmNum = 1;
                }
            }
            else if (mapNum == 1)
            {
                Alpha.transform.localScale = new Vector3(3f, 3f, 3f);  //1페
                if (!playBgm.audio.isPlaying || bgmNum != 2)
                {
                    playBgm.PlayBackground(4);
                    bgmNum = 2;
                }
            }
            else if (mapNum == 2) Alpha.transform.localScale = new Vector3(5f, 5f, 5f);  //2패
            else if (mapNum == 3) Alpha.transform.localScale = new Vector3(5f, 5f, 5f);  //3페
            else if (mapNum == 4)
            { 
                Alpha.transform.localScale = new Vector3(5f, 5f, 5f);  //은빛탑
                if (!playBgm.audio.isPlaying || bgmNum != 1) //재생이 멈췄거나 마을브금이 아니라면
                {
                    playBgm.PlayBackground(3); //마을브금을 재생
                    bgmNum = 1;
                }
            }

        }
        else if (MinimapB.activeSelf) //커다란 미니맵이 활성화 되어 있다면
        {
            if (mapNum == 0) // 마을일 경우
            {
                transform.position = new Vector3(2f, -78f, -1f);
                PlayerMarker.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f); //마커의 크기를 미니맵의 크기에 따라 변경
            }
            if (mapNum == 4) // 은빛탑일 경우
            {
                transform.position = new Vector3(-1000f,50f,1010f); // 카메라의 위치조절 
                PlayerMarker.transform.localScale = new Vector3(1f, 1f, 1f); //마커의 크기를 미니맵의 크기에 따라 변경
                Alpha.transform.localScale = new Vector3(1f, 1f, 1f); //마커 크기 변경에 따른 알파값 크기 변경
            }
            if (mapNum == 1) // 수정회랑일 경우
            {
                transform.position = new Vector3(1f ,-10f, 1045f);  
                PlayerMarker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Alpha.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //마커 크기 변경에 따른 알파값 크기 변경
            }
            if (mapNum == 2) // 디아블로 2페이즈일 경우
            {
                transform.position = new Vector3(1000f, -58f, 1000f); 
                PlayerMarker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); //마커 크기 변경
                Alpha.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //마커 크기 변경에 따른 알파값 크기 변경
            }
            if (mapNum == 3) // 수정회랑일 경우
            {
                transform.position = new Vector3(2000f , -48f, 1000f); 
                PlayerMarker.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                Alpha.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f); //마커 크기 변경에 따른 알파값 크기 변경
            }
        }
        else return;
    }
}

/*
    미니맵을 구현하는 방법은 다양하지만 이번에 구현한 방법은 
    실제 맵과 미니맵의 크기를 동일하게 맞춘 방식을 사용했다.
    이와 같은 방식을 사용한 경우 IMarKer스크립트를 사용할 필요도 없이
    마커 오브젝트를 플레이어 오브젝트에 자식으로 상속시키기만 해도 된다.
    

    간단하게 미니맵 구현하기
    
    1. Plane을 스케일을 2,2,2로 2개 만든다.
    2. 만들어진 2개 Plane중 1개는 포지션을 0,0,0으로 한다 이것을 APlane이라 하고
    3. 나머지 1개는 0,-100,0으로 하고 레이어에 Minimap을 추가하고 Minimap으로 설정 이것을 BPlane이라 한다.
    4. 메인카메라는 APlane만 보이도록 위치를 조절하고 CullingMask에서 Minimap에 표시가되어있으면 해제한다.
    5. 카메라를 한 개 추가하고 미니맵만 볼 수 있게 CullingMask를 Minimap만 표시되도록 한다. 그리고 미니맵이 잘 보이도록 위치를 조절한다. 2d 느낌을 내기위해 로테이션x값을 90도로 설정.
    6. RenderTexture 를 새로 생성하고 미니맵 카메라에 TargetTexture에 드래그 드랍한다.
    7. Material을 새로 생성 LegacyShaders->Particles->AlphaBlended로 설정 Particle Texture에 6번에서 생성한 RenderTexture를 드래그 드랍
    8. UI에 RawImage 새로 생성하고 위치 조정을 한 뒤 7번에서 만든 Material을 드래그 드랍해준다.
    9. Capsule을 2개 만들고 1개는 포지션을 0,1,0으로 나머지 한개는 자식으로 만든뒤 0,-100,0으로 해준다. 자식인 캡슐은 레이어를 Minimap으로 변경
   10. 메인 카메라에 비춰지는 부모 캡슐을 움직이면 미니맵에서 비춰지는 자식 캡슐 또한 같이 움직여진다.
    
 */
