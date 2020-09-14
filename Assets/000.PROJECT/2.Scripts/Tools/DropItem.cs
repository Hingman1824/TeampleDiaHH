using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public Transform inven;

    public int weaponNum;

    public ItemManager.ItemKind itemKind;

    public ItemManager.Rarity rarity;

    public string itemName;

    public int needLevel;

    public int damage;

    public int defense;

    public GameObject itemPF;

    public bool isHpPotion;

    private Rigidbody rigidbody;

    public GameObject[] paricles;

    private int xSize;
    private int ySize;

    private ParticleSystem[] particleSystems;

    private SoundManager soundManager;

    // 아이템이 바닥에 닿을 시 물리효과끄고 소리 재생
    private void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag == "Ground")
        {
            rigidbody.isKinematic = true;
            if (itemKind == ItemManager.ItemKind.use) return;

            ShowParticle(); // 아이템 드랍 이펙트가 나오게하고
            PlaySfx();  // 아이템 드랍 사운드 재생
            
        }

        //if (col.gameObject.CompareTag("Player"))
        //{
        //    ItemPooling.ReturnObject(this);
        //}
    }

    //움직이지 않는 벽 : Collider

    //움직이지 않는 감지 지역 : Collider(IsTrigger)

    //물리로 움직이는 벽 : Collider & Rigidbody

    //물리로 움직이는 감지 발견 : Collider(IsTrigger) & Rigidbody

    //변형으로 움직이는 벽 : Collider & Rigidbody(IsKinematic)

    //transform으로 움직이는 감지 지역 : Collider(IsTrigger) & Rigidbody(IsKinematic)

    //물리 이동 만한다면 : Rigidbody

    //이동 만한다면 : Rigidbody(IsKinematic)

    // 리소스 폴더에서 아이템에 따라 클립을 불러와서 소리를 재생

    private void PlaySfx()
    {
        string path = "ItemSounds\\";

        if (rarity == ItemManager.Rarity.legendary)
            path += "legendarydrop";
        else
        {
            switch (itemKind)
            {
                case ItemManager.ItemKind.use:

                    path += "potionDrop";
                    break;
                case ItemManager.ItemKind.LHand:
                case ItemManager.ItemKind.RHand:
                    path += "weaponDrop1";

                    break;

                case ItemManager.ItemKind.Hat:
                    path += "hatDrop";

                    break;

                case ItemManager.ItemKind.Body:
                    path += "bodyDrop";

                    break;

                case ItemManager.ItemKind.Belt:
                    path += "beltDrop";

                    break;

                case ItemManager.ItemKind.Pants:
                    path += "pantsDrop";

                    break;

                case ItemManager.ItemKind.Foot:
                    path += "footDrop";

                    break;

                case ItemManager.ItemKind.LRing:
                case ItemManager.ItemKind.RRing:
                    path += "ringDrop";

                    break;

                case ItemManager.ItemKind.Glove:
                    path += "gloveDrop";

                    break;

            }
        }

        AudioClip temp = Resources.Load<AudioClip>(path);
        soundManager.PlaySfx(temp, transform.position);
    }

    //드랍 파티클 생성
    private void ShowParticle()
    {
        GameObject temp = null;

        if(particleSystems == null)
        {
            switch (rarity)
            {
                case ItemManager.Rarity.normal:

                    return;

                case ItemManager.Rarity.magic:
                    temp = paricles[0];

                    break;
                case ItemManager.Rarity.unique:
                    temp = paricles[1];

                    break;

                case ItemManager.Rarity.legendary:
                    temp = paricles[2];

                    break;
            }

            particleSystems = Instantiate(temp, transform.position, Quaternion.identity, transform).GetComponentsInChildren<ParticleSystem>();
        }
        else
        {
            for (int i = 0; i < particleSystems.Length; i++) particleSystems[i].Play();
        }
    }

    private void Awake()
    {
        inven = GameObject.Find("UIPanel").transform.Find("InvenImg");
        rigidbody = GetComponent<Rigidbody>();
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
    }

    private void Start()
    {
        ItemDropUp();
        ItemSizeInit();
    }

    void ItemSizeInit()
    {
        switch (itemKind)
        {
            case ItemManager.ItemKind.use:
                ySize = 1;
                xSize = 1;
                
                break;

            case ItemManager.ItemKind.LHand:
            case ItemManager.ItemKind.RHand:

                ySize = 4;
                xSize = 2;
                break;

            case ItemManager.ItemKind.Hat:
                ySize = 2;
                xSize = 2;
                break;

            case ItemManager.ItemKind.Body:
                ySize = 3;
                xSize = 2;
                break;

            case ItemManager.ItemKind.Belt:
                ySize = 1;
                xSize = 2;
                break;

            case ItemManager.ItemKind.Pants:
                ySize = 1;
                xSize = 1;
                break;

            case ItemManager.ItemKind.Foot:
                ySize = 2;
                xSize = 2;
                break;

            case ItemManager.ItemKind.LRing:
            case ItemManager.ItemKind.RRing:
                ySize = 1;
                xSize = 1;

                break;

            case ItemManager.ItemKind.Glove:
                ySize = 2;
                xSize = 2;
                break;

        }

    }

    // 아이템 최초 드랍시 또는 더이상 아이템을 먹을 수 없을때 아이템을
    //캐릭터 머리위로 던짐

    public void ItemDropUp()
    {
        soundManager.PlaySfx(Resources.Load<AudioClip>("ItemSounds\\itemUp"), transform.position);
        transform.Translate(Vector3.up);
        rigidbody.AddForce(Vector3.up * 300f);
        rigidbody.AddTorque(Vector3.up * 300f);
    }

    // 아이템 습득
    public void GetItem()
    {
        StartCoroutine(ItemInvenAdd());
    }

    private IEnumerator ItemInvenAdd()
    {
        if(!InventoryManager.instance.AutoItemInput(xSize, ySize))
        {
            if (!(particleSystems == null))
                for (int i = 0; i < particleSystems.Length; i++)
                    particleSystems[i].Stop();

            rigidbody.isKinematic = false;
            ItemDropUp();

            yield break;
        }

        ItemManager temp = Instantiate(itemPF, Vector3.zero, Quaternion.identity, inven).GetComponent<ItemManager>();

        temp.isEquipment = false;
        temp.itemKind = itemKind;
        temp.rarity = rarity;
        temp.itemName = itemName;
        temp.needLevel = needLevel;
        temp.damage = damage;
        temp.defense = defense;
        temp.weaponNum = weaponNum;
        temp.isHpPotion = isHpPotion;

        Destroy(this.gameObject);

        yield return null;
    }
}
