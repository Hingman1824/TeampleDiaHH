using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI; // 이 선언이 있어야 UI관련 컴포넌트를 연결 및 사용 가능.

[RequireComponent(typeof(AudioSource))]//현재 스크립트에서 넓게는 현재 게임오브젝트에서 반드시 필요로하는 컴포넌트를 Attribute로 명시하여 
                                       //해당 컴포넌트의 자동 생성 및 삭제되는 것을 막는다.

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundFiles;
    public Text volumeTxt;
    public float soundVolume = 1.0f;
    public bool isSoundMute = false;

    public Slider sl; // 슬라이더 컴포넌트 연결 변수
    public Toggle tg; // 토글 컴포넌트 연결 변수

    public GameObject Sound;
    public GameObject PlaySoundBtn;
    private int tempVolume;

    private SFXManager sfx;
    public static SoundManager instance;

    public AudioSource audio; //녹색줄의 의미는 앞으로 삭제될 수 있음을 경고, 그러나 그냥써도 되고, 변수 이름만 바꿔도 된다.


    void Awake() //Start()보다 먼저 초기화함수, 레퍼런스초기화 용, 컴포넌트가  비활성화 되어도 호출된다. 단, gameObject 비활성화시는 아님.
    {
        instance = this;
        sfx = transform.Find("SFXManager").GetComponent<SFXManager>();
        audio = GetComponent<AudioSource>();
        // 이 오브젝트는 씬 전환시 사라지지 않음
        DontDestroyOnLoad(this.gameObject);
        // 게임 사운드설정 로드
        LoadData();
    }

    // 초기화 문에서 각 컴포넌트의 요소로 멤버 soundVolume, isSoundMute 변수를 초기화
    // 이 함수는 각 변경된 컴포넌트 값으로 soundVolume, isSoundMute 의 값을 변경(따로 분리해서 함수 선언 정의 해도 됨)
    void Start()
    {
        soundVolume = sl.value;
        isSoundMute = tg.isOn;
        PlaySoundBtn.SetActive(true); // 비 활성화 되었던 사운드 Ui 실행 버튼이 활성화 되어져 보일것이다.
        AudioSet(); //오디오 파일과 툴바를 연결해준다.
    }
    private void FixedUpdate()
    {
        tempVolume = (int)(soundVolume * 100);
        volumeTxt.text = tempVolume.ToString();
    }
    //SFX 볼륨설정
    public void SetSfx(float vol, bool isMute)
    {
        sfx.SetSFXSound(vol, isMute);
    }

    // Slider 와 Toggle 컴포넌트에서 이벤트 발생시 호출해줄 함수를 선언 (public 키워드에 의해 외부접근 가능)
    public void SetSound() //Slider와 Toggle 컴포넌트에서 이벤트 발생 시 호출해 줄 함수를 선언
    {
        soundVolume = sl.value;
        isSoundMute = tg.isOn;
        AudioSet();
    }

    // AudioSource 셋팅 (사운드 UI에서 설정한 값의 적용)
    void AudioSet()
    {
        audio.volume = soundVolume; //AudioSource의 볼륨셋팅
        audio.mute = isSoundMute;   //AudioSource의 뮤트셋팅
    }

    // 사운드 UI 창 오픈
    public void SoundUiOpen()
    {
        // 사운드 UI 활성화
        Sound.SetActive(true);
        // 사운드 UI 오픈 버튼 비활성화
        PlaySoundBtn.SetActive(false);
    }

    // 사운드 UI 창 닫음
    public void SoundUiClose()
    {
        // 사운드 UI 비 활성화
        Sound.SetActive(false);
        // 사운드 UI 오픈 버튼 활성화
        PlaySoundBtn.SetActive(true);

        // 게임 사운드설정 세이브
        SaveData();
    }


    // 스테이지 시작시 호출되는 함수
    public void PlayBackground(int stage)
    {
        // AudioSource의 사운드 연결
        audio.clip = soundFiles[stage - 1];
        // AudioSource 셋팅
        AudioSet();
        // 사운드 플레이. Mute 설정시 사운드 안나옴
        audio.Play();
    }

    // 사운드 공용함수 정의(어디서든 동적으로 사운드 게임오브젝트 생성)
    public void PlayEffct(Vector3 pos, AudioClip sfx)
    {
        // Mute 옵션 설정시 이 함수를 바로 빠져나가자.
        if (isSoundMute)
        {
            return;
        }

        // 게임오브젝트 동적 생성하자.
        GameObject _soundObj = new GameObject("sfx");

        // 사운드 발생 위치 지정하자.
        _soundObj.transform.position = pos;

        // 생성한 게임오브젝트에 AuidoSource 컴포넌트를 추가하자.
        AudioSource _audioSource = _soundObj.AddComponent<AudioSource>();

        // AudioSource 속성을 설정
        // 사운드 파일 연결하자.
        _audioSource.clip = sfx;

        // 설정되어있는 볼륨을 적용키자. 즉 soundVolume 으로 게임전체 사운드 볼륨 조절.
        _audioSource.volume = soundVolume;

        // 사운드 3d 셋팅에 최소 범위를 설정하자.
        _audioSource.minDistance = 15.0f;

        // 사운드 3d 셋팅에 최대 범위를 설정하자.
        _audioSource.maxDistance = 30.0f;

        // 사운드를 실행시키자
        _audioSource.Play();

        // 모든 사운드가 플레이 종료되면 동적 생성된 게임오브젝트 삭제하자.
        Destroy(_soundObj, sfx.length + 0.2f); // tip) 0.2f 정도 더 해주면 음악이 짤릴일은 없다.
    }

    // 게임 사운드데이터 저장
    public void SaveData()
    {
        PlayerPrefs.SetFloat("SOUNDVOLUME", soundVolume);
        // PlayerPrefs 클래스 내부 함수에는 bool형을 저장해주는 함수가 없다.
        // bool형 데이터는 형변환을 해야 PlayerPrefs.SetInt() 함수를 사용가능
        PlayerPrefs.SetInt("ISSOUNDMUTE", System.Convert.ToInt32(isSoundMute));
    }

    // 게임 사운드데이터 불러오기
    // 바로 사운드 UI 슬라이드와 토글에 적용하자.
    public void LoadData()
    {
        sl.value = PlayerPrefs.GetFloat("SOUNDVOLUME");
        // int 형 데이타는 bool 형으로 형변환.
        tg.isOn = System.Convert.ToBoolean(PlayerPrefs.GetInt("ISSOUNDMUTE"));

        // 첫 세이브시 설정 -> 이 로직없으면 첫 시작시 사운드 볼륨 0
        int isSave = PlayerPrefs.GetInt("ISSAVE");
        if (isSave == 0)
        {
            sl.value = 1.0f;
            tg.isOn = false;

            SaveData();
            PlayerPrefs.SetInt("ISSAVE", 1);
        }
    }
    //3d sfx 재생 sfx매니저에서 코루틴으로 실행
    public void PlaySfx(AudioClip clip, Vector3 pos)
    {
        sfx.SoundSfx(clip, pos);
    }
    //2d sfx 재생
    public void PlaySfx(AudioClip clip)
    {
        sfx.SoundSfx(clip);
    }
}
