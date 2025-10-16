using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class RewardsPanel : MonoBehaviour
{
    [FoldoutGroup("Refs")] [SerializeField]
    Transform content;

    [FoldoutGroup("Refs")] [SerializeField]
    RewardPanelItemView rewardPanelItemViewPrefab;

    [FoldoutGroup("Profiler")] [SerializeField]
    List<RewardPanelItemView> rewardPanelItemViews;


    public void UpdateRewardListItems(RewardDefinitionSo item, int amount)
    {
        var existing = rewardPanelItemViews.Find(x => x.GetRewardsId() == item.id);

        if (existing != null)
        {
            existing.UpdateCount(amount);

            return;
        }

        var newItem = Instantiate(rewardPanelItemViewPrefab, content);
        newItem.Init(item.icon, amount, item);
        rewardPanelItemViews.Add(newItem);
    }

    public void Reset()
    {
        for (var i = rewardPanelItemViews.Count - 1; i >= 0; i--)
        {
            Destroy(rewardPanelItemViews[i].gameObject);
        }

        rewardPanelItemViews.Clear();
    }
}