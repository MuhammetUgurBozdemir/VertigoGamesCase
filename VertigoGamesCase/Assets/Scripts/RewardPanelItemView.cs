using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelItemView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private RewardDefinitionSo rewardDefinitionSo;
    int amount;

    public int GetCount()
    {
        return amount;
    }

    public void Init(Sprite sprite, int number, RewardDefinitionSo rewardDefinitionSo)
    {
        amount = number;
        image.sprite = sprite;
        countText.text = number.ToString();
        this.rewardDefinitionSo = rewardDefinitionSo;
    }

    public RewardType GetRewardsType()
    {
        return rewardDefinitionSo.type;
    }

    public CurrencyKind GetCurrencyKind()
    {
        return rewardDefinitionSo.currency;
    }

    public void UpdateCount(int amount)
    {
        this.amount += amount;
        countText.text = this.amount.ToString();
    }
}