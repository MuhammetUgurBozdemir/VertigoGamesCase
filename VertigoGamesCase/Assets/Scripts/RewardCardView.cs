using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCardView : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemAmountext;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = transform.localPosition;
    }

    public void Init(Sprite sprite, string name, int amount)
    {
        itemImage.sprite = sprite;
        itemNameText.text = name;
        itemAmountext.text = amount.ToString();
        CardRevealAnim();
    }

    [Button]
    public void CardRevealAnim()
    {
        transform.DOLocalMove(Vector3.zero, .4f).SetEase(Ease.InOutBack);
        transform.DOLocalMove(initialPos, .4f).SetEase(Ease.InOutBack).SetDelay(1f);
    }
}