using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemPrice : MonoBehaviour
{
    [System.Serializable]
    public enum UseItems
    {
        none = 0,
        hpPotion = 1,
        //mpPotion,
        upgradeSource1,
        upgradeSource2,
    }

    public UseItems useitems;

    public int price;
}
