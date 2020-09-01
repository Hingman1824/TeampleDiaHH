using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AcceptedManager : MonoBehaviour
{
    public delegate void IsSelect();

    public static event IsSelect positive;
    public static event IsSelect negative;
    public static event IsSelect endEvent;

    public GameObject message;  //메세지 띄워줄 오브젝트
    public Text txt; // 텍스트 띄워주는

    public RectTransform positiveTr;
    public RectTransform negativeTr;

    int slotSize;
    float screenH;

    void Start()
    {
        screenH = Screen.height;
        RectTransform temp = message.GetComponent<RectTransform>();

        slotSize = UIManager.instance.slotSize;

        temp.sizeDelta = new Vector2(slotSize * 4, (slotSize * 3) + (slotSize / 2));

        positiveTr.sizeDelta = new Vector2(slotSize, slotSize);
        negativeTr.sizeDelta = new Vector2(slotSize, slotSize);

        positiveTr.anchoredPosition = new Vector2(slotSize / 2, slotSize * -2);
        negativeTr.anchoredPosition = new Vector2((slotSize / 2) + (slotSize * 2), slotSize * -2);
        txt.rectTransform.sizeDelta = new Vector2(slotSize * 4, ((slotSize * 3) + (slotSize / 2)) / 2);

    }

    public void ShowSellConfirm()
    {
        OpenWindow();

        txt.text = "정말 판매하시겠습니까?";
        message.SetActive(true);
    }

    public void ShowBuyConfirm()
    {
        OpenWindow();

        txt.text = "정말 구입하시겠습니까?";
        message.SetActive(true);
    }

    public void ShowDropConfirm()
    {
        OpenWindow();

        txt.text = "정말 버리시겠습니까?";
        message.SetActive(true);
    }

    public void OnButtonPositive()
    {
        positive();
        message.SetActive(false);
    }

    public void OnButtonNegative()
    {
        negative();
        message.SetActive(false);
    }

    void OpenWindow()
    {
        RectTransform tempTr = message.GetComponent<RectTransform>();
        Vector2 tempMouse = Input.mousePosition;

        Vector2 tempPos;

        tempPos.x = tempMouse.x - slotSize;
        tempPos.y = tempMouse.y - screenH + (slotSize * 2) + (slotSize / 2);
        tempTr.anchoredPosition = tempPos;
    }
}
