using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IplayerItem : MonoBehaviour
{
    // 이 스크립트는 플레이어가 아이템의 보유현황을 체크, 아이템을 플레이어 주변에 랜덤으로 떨구는 기능을 구현

    public GameObject MaleItem; //아이템 구현
    public bool GetItem; //아이템 현황

    private Vector3 DropPos; //아이템이 떨어지는 위치
    private Quaternion DropRot; //아이템의 로테이션 값
    // Update is called once per frame
    void Update()
    {
       //플레이어의 x축과 z축을 중심으로 랜덤하게 템을 떨굼
       DropPos = new Vector3(this.transform.position.x + Random.Range(-1.0f,1.0f), this.transform.position.y + 5, this.transform.position.z+ Random.Range(-1.0f, 1.0f)); 
       DropRot = Quaternion.Euler(90f, 0f, 0f); //x를 90을 줘야 세트빔이 위를 향해서 올라감.
    }

    public void OnDropItem()
    {
        //템을 가지고 있을때 템을 떨굴
        if (GetItem)
        {
            //Instantiate(MaleItem, DropPos, DropRot); //생성하고 제거하면 최적화가 안될것 같아서 변경...
            MaleItem.transform.position = DropPos; //아이템의 위치를 플레이어 주변으로 변경
            MaleItem.transform.rotation = DropRot;
            GetItem = !GetItem; //아이템을 버렸으니 가지고 있지 않게 변경
            
        }
    }
}
