using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamDontClear : MonoBehaviour
{
    //이 스크립트는 전장의 안개 기능 구현을 위한 스크립트로 카메라 ClearFlags기능을 Don't Clear로 변경하여
    //까만 부분을 알파값으로 지워주고 알파값으로 지워진 부분을 유지 시키기 위한 기능이다.

    //FogOfWar안에 있는 CamAlphaMid에 사용된다.

    private Camera cam;
    // Start is called before the first frame update
    void Awake()
    {
        if(cam==null) //cam이 없다면
        {
            cam = this.GetComponent<Camera>();  //이 스크립트가 들어간 카메라와 연결
        }
        Initalize();
    }

    public void Initalize()
    {
        cam.clearFlags = CameraClearFlags.Color;
    }

    // Update is called once per frame
    void OnPostRender() //이 함수는 카메라에서 장면 렌더링이 완료된후 불러오는 함수 이다.
    {
        cam.clearFlags = CameraClearFlags.Nothing;
    }
}

/*
    전장의 안개(Fog Of War)란? 시야가 닿지 않는 곳의 정보를 알려주지 않는 시스템을 말한다
    주로 사용된 게임으로는 스타크래프트 같은 RTS장르의 게임이 있다.

    전장의 안개(Fog Of War) 구현하기 --FowShader와miniAlpha라는 알파값을 나타내는? 그림이 필요

    1. Plane을 스케일을 1,1,1로 2개 만든다.
    2. 만들어진 2개 Plane중 1개는 포지션을 0,0,0으로 한다 이것을 APlane이라 하고
    3. 나머지 1개는 0,2,0으로 하고 BPlane이라 한다.
    4. 카메라 1개 생성하고 포지션 0,10,-10, 로테이션 x값 50 설정 AlphaMid로 이름을 변경, ClearFlags를 Don'tClear로 변경
    4-1. 메인카메라는 잠시 비활성화.
    5. Material 1개 생성 LegacyShaders->Particles->AlphaBlended로 설정, miniAlpha를 드래그드랍, 컬러는 마음대로 설정
    6. Sphere 1개 생성 포지션을 0,1,0으로 변경 5번에서 생성한 Material을 드래그드랍. 레이어에 Alpha를 추가하고 Alpha로 설정.
    6-1. AlphaMid카메라에서 CullingMask를 Alpha만 표시
    6-2. 플레이 하고 Sphere를 이동시켜보면 까만 배경에 내가 선택한 색상으로 남는것을 확인할수 있다. 
    7. AlphaMid 카메라를 복사 생성하고 이름을 AlphaZero로 변경
    8. 두 카메라에 Clearflags를 Solid Color로 변경하고 백그라운드 컬러를 검은색으로 변경
    9. CamDontClear 스크립트 생성, 위에 있는 코드 짜고 AlphaMid 카메라에 드래그드랍
   10. RenderTexture 2개 생성 각각 RendMid,RendZero로 변경하고 AlphaMid, AlphaZero에 드래그드랍
   11. Material 1개 생성 Custom->FowShader설정 RendMid,RendZero를 각각 AlphaMid,AlphaZero에 드래그드랍
   12. 메인카메라 활성화하고 BPlane에 11번에서 만든 Material을 드래그드랍
   12-1. 메인 카메라의 CullingMask는 전부 표시되어 있어야 한다.
   12-2. 플레이하고 Sphere를 움직여보면 투명해지는것을 볼 수 있다.
   12-3. Material을 1개 생성 Unit->Color로 변경 색깔 마음대로 설정하고 APlane에 드래그드랍하면 좀더 편하게 알아볼수 있다.
   12-4. 투명해지는 것을 확인했지만 Sphere에 반대방향으로 투명해진다.
   13. AlphaMid카메라와 AlphaZero카메라에 포지션 Z값을 반대로, 로테이션 Y값을 180으로 설정해주면 된다.
   13-1. AlphaZero카메라는 투명해지는 부분을 보여주고 AlphaMid카메라는 투명해진 부분을 유지시켜준다.
   14. Sphere를 상하좌우 움직여보면 뭔가 이상하게 안맞는다. 이럴때 AlphaMid,AlphaZero 카메라에 포지션 y값과 FieldOfView를 통해 조절할 수 있다.
 
 */
