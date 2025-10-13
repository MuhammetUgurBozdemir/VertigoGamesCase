using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardsPanel : MonoBehaviour
{
    [SerializeField] RewardPanelItemView rewardPanelItemViewPrefab;
    [SerializeField] List<RewardPanelItemView> rewardPanelItemViews;
    [SerializeField] Transform content;

    public void UpdateRewardListItems(RewardDefinitionSo item, int amount)
    {
        var existing = rewardPanelItemViews.Find(x => x.GetRewardsType() == item.type);

        if (existing != null)
        {
            existing.UpdateCount(amount);

            return;
        }

        var newItem = Instantiate(rewardPanelItemViewPrefab, content);
        newItem.Init(item.icon, amount, item);
        rewardPanelItemViews.Add(newItem);
    }
}