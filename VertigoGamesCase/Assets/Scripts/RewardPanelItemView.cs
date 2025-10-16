using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardPanelItemView : MonoBehaviour
{
    [FoldoutGroup("Refs")] [SerializeField]
    private Image image;

    [FoldoutGroup("Refs")] [SerializeField]
    private TextMeshProUGUI countText;

    [FoldoutGroup("Profiler")] private RewardDefinitionSo rewardDefinitionSo;
    int amount;
    
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
    
    public string GetRewardsId()
    {
        return rewardDefinitionSo.id;
    }
    

    public void UpdateCount(int amount)
    {
        this.amount += amount;
        countText.text = this.amount.ToString();
    }
}