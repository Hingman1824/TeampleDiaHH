using UnityEngine;
using IPlayer;
using System.Collections;
using UnityEngine.UI;
using System.Reflection;
using UnityEditor.XR;

public class PlayerManager : MonoBehaviour, IPlayerMove, IPlayerStats, IPlayerAnimation, IPlayerPhoton
{
    [Space(3)]
    [Header("플레이어 스탯")]
    [Tooltip("플레이어 체력")]
    [Range(0, 9999)] public int playerHp = 100;
    [Tooltip("플레이어 이동속도")]
    [Range(0, 10)] public float playerSpeed;
    [Tooltip("플레이어 공격속도")]
    [Range(0, 10)] public float playerAttackSpeed = 1.3f;
    [Tooltip("플레이어 대미지")]
    [Range(0, 9999)] public float playerDamage = 10f;
    [Space(3)]
    [Header("플레이어 상태")]
    
    public bool isHit = false;
    public bool isRot = false;
    public float playerRot = 10;
    public Animator anim;
    public SmoothFollow smooth;
    public LoadingScript loading;

    public PhotonView pv;
    public Rigidbody myRb;
    public Transform camPivot;
    public Vector3 currPos = Vector3.zero;
    public Quaternion currRot = Quaternion.identity;
    //public Monster monster;

    
    public Image expBar;
    public Image hpBar;
    public Text expText;
    public Text playerName;
    public Button respawnBtn;
    public Transform respawnPoint;

    public static PlayerManager instance;        //전재현 추가
    private EquipmentManager equipmentManager;   //전재현 추가
    public Transform weaponPos;                  //전재현 추가
    private Collider[] weaponColliders;          //전재현 추가
    public bool colOnce;                         //전재현 추가

    //일단 임의로 정해놓은 것
    public int Level;   //전재현 임시 변경
    float Hp = 100;
    int Defense = 33;
    float Exp = 0.0f;

    //플레이어 이동 체크
    [HideInInspector]
    public bool isPlayerMove;

    // 전재현 추가
    void Awake()
    {
        //pm = FindObjectOfType<PhotonManager>();
        instance = this;
        equipmentManager = GameObject.Find("EquipmentManager").GetComponent<EquipmentManager>();
        weaponColliders = weaponPos.GetComponentsInChildren<Collider>();
    }

    //private void Start()
    //{
    //    WeaponOff();
    //    Level = 1;
    //    respawnPoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>();       //부활장소
    //    loading = FindObjectOfType<LoadingScript>();                                                //로딩창
    //    expBar = GameObject.Find("ExpBar").GetComponent<Image>();
    //    hpBar = GameObject.Find("Hp").GetComponent<Image>();
    //    expText = GameObject.Find("ExpText").GetComponent<Text>();
    //    playerName = GameObject.Find("PlayerNickName").GetComponent<Text>();
    //    smooth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>();


    


    private void Start()
    {
        WeaponOff();  // 전재현 추가
        Level = 1;    // 전재현 추가
        respawnPoint = GameObject.FindGameObjectWithTag("Respawn").GetComponent<Transform>(); //부활장소
        loading = FindObjectOfType<LoadingScript>();                                         //로딩창
        expBar = GameObject.Find("ExpBar").GetComponent<Image>();                            //경험치 바
        hpBar = GameObject.Find("Hp").GetComponent<Image>();        //hp 바
        expText = GameObject.Find("ExpText").GetComponent<Text>();      
        playerName = GameObject.Find("PlayerNickName").GetComponent<Text>();
        smooth = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothFollow>();
        respawnBtn = GameObject.Find("RespawnBtn").GetComponent<Button>();
        // monster = GameObject.FindGameObjectWithTag("Enemy").GetComponent<Monster>();

        equipmentManager.WeaponSet(0);  // 전재현 추가
        colOnce = false;  // 전재현 추가
        playerName.text = PhotonNetwork.player.NickName;
        isPlayerMove = false;
    }

    // 범위 -1 ~ 1까지 플레이어 스피드는 이것과 관계없음
    public float PlayerH
    {
        get { return Input.GetAxis("Horizontal"); }

        set => Input.GetAxis("Horizontal");
    }

    //마찬가지
    public float PlayerV
    {
        get { return Input.GetAxis("Vertical"); }

        set => Input.GetAxis("Vertical");
    }

    public float PlayerLife
    {
        get
        {
            return Hp;
            //throw new NotImplementedException();
        }

        set
        {
            Hp = value;
        }
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            smooth.playerHit();
            Hp -= 10;
            hpBar.fillAmount -= 10 * 0.01f;

            if (Hp <= 0)
            {
                anim.SetBool("isDie", true);
                smooth.PlayerDie.SetActive(true);
            }
        }
    }

    public void Respawn()
    {
        loading.Loading();
        transform.position = respawnPoint.position;
        anim.SetBool("isDie", false);       
        Hp = 100;
        hpBar.fillAmount = 1.0f;
        smooth.PlayerDie.SetActive(false);
    }

    public int PlayerLevel
    {
        get
        {
            return Level;
            //throw new NotImplementedException();
        }
    }

    public float PlayerExp
    {
        get
        {
            return Exp;
        }

        set
        {
            Exp = value;
        }
    }

    public int PlayerDefense
    {
        get
        {
            return Defense;
        }
    }

    public void ShowNickName()
    {
        throw new System.NotImplementedException();
    }

    public void ShowOtherNickName()
    {
        throw new System.NotImplementedException();
    }

    public float PlayerSpeed { get; }

    public float PlayerAttackSpeed { get; }

    public float PlayerDamage { get; }

    public Animator Net_Anim { get; }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = pv.instantiationData;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(this.transform.position);
            stream.SendNext(this.transform.rotation);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
        }
    }

    //private void OnEnable()
    //{
    //    playerHp = PlayerLife;
    //    playerSpeed = PlayerSpeed;
    //    playerAttackSpeed = PlayerAttackSpeed;
    //    anim = Net_Anim;
    //}

    //public void PlayerAttack()
    //{
    //    if (Input.GetKey(KeyCode.X) && isAttack == false)
    //    {
    //        isAttack = true;
    //        StartCoroutine(PlayerAttackAnimation());
    //        isAttack = false;
    //    }
    //}

    //public IEnumerator PlayerAttackAnimation()
    //{
    //    anim.SetBool("isAttack", true);
    //    yield return new WaitForSeconds(0.5f);
    //    //Invoke("PlayerAttackAnimation", 0.5f);
    //    anim.SetBool("isAttack", false);
    //}
    [PunRPC]
    public void PlayerMoveAnimation()
    {
        if (PlayerH != 0 || PlayerV != 0)
        {
            anim.SetBool("isAttack", false);
            if (PlayerH != 0 && PlayerV == 0)
            {
                anim.SetFloat("Speed", 1f);

            }
            else if (PlayerH == 0 && PlayerV != 0)
            {
                anim.SetFloat("Speed", 1f);

            }
            else if (PlayerH != 0 && PlayerV != 0)
            {
                anim.SetFloat("Speed", 1f);
            }
        }
        else if (PlayerH == 0 && PlayerV == 0)
        {
            anim.SetFloat("Speed", 0);
        }
    }

    [PunRPC]
    public void PlayerMovement()
    {
        // 전진, 왼쪽
        if (PlayerV > 0 && PlayerH < 0 && isRot == false)
        {
            transform.position += (Vector3.forward * Time.deltaTime * playerSpeed);
            transform.position += (Vector3.left * Time.deltaTime * playerSpeed);
            Quaternion target = Quaternion.Euler(0, 135, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
        }
        //전진, 우
        else if (PlayerV > 0 && PlayerH > 0 && isRot == false)
        {
            transform.position += (Vector3.forward * Time.deltaTime * playerSpeed);
            transform.position += (Vector3.right * Time.deltaTime * playerSpeed);
            Quaternion target = Quaternion.Euler(0, 225, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
        }

        //후진, 좌의 경우
        else if (PlayerV < 0 && PlayerH < 0 && isRot == false)
        {

            transform.position += (Vector3.back * Time.deltaTime * playerSpeed);
            transform.position += (Vector3.left * Time.deltaTime * playerSpeed);
            Quaternion target = Quaternion.Euler(0, 45, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
        }

        //후진, 우의 경우
        else if (PlayerV < 0 && PlayerH > 0 && isRot == false)
        {
            transform.position += (Vector3.back * Time.deltaTime * playerSpeed);
            transform.position += (Vector3.right * Time.deltaTime * playerSpeed);
            Quaternion target = Quaternion.Euler(0, 315, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
        }

        // 한 키만 누를 시
        else
        {
            if (PlayerV > 0 && isRot == false)//위
            {
                transform.position += (Vector3.forward * Time.deltaTime * playerSpeed);
                Quaternion target = Quaternion.Euler(0, 180, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
            }

            if (PlayerV < 0 && isRot == false)//아래
            {
                transform.position += (Vector3.back * Time.deltaTime * playerSpeed);
                Quaternion target = Quaternion.Euler(0, 0, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
            }

            if (PlayerH < 0 && isRot == false)//좌
            {
                transform.position += (Vector3.left * Time.deltaTime * playerSpeed);
                Quaternion target = Quaternion.Euler(0, 90, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
            }

            if (PlayerH > 0 && isRot == false)//우
            {
                transform.position += (Vector3.right * Time.deltaTime * playerSpeed);
                Quaternion target = Quaternion.Euler(0, 270, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * playerRot);
            }
        }
    }

    public bool PlayerMoveCheck()       // 컷씬 용 플레이어 이동확인
    {
        if (PlayerH != 0 || PlayerV != 0)   //플레이어가 움직이고 있으면
        {
            isPlayerMove = true;        
        }
        else if (PlayerH == 0 && PlayerV == 0)  //플레이어가 움직이지 않으면
        {
            isPlayerMove = false;
        }
        return isPlayerMove;
    }

    // 인벤 캐릭터 무기 끼고 빼는 로직임 on off 전재현 추가
    private void WeaponOn()
    {
        foreach (var it in weaponColliders)
        {
            it.enabled = true;
        }
    }

    private void WeaponOff()
    {
        //foreach (var it in weaponColliders)
        //{
        //    it.enabled = false;
        //}
    }

}