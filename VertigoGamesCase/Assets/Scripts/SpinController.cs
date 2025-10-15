using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinController : MonoBehaviour
{
    [Header("Data")] public WheelTypeSo bronzeConfig;
    [Header("Data")] public WheelTypeSo silverConfig;
    [Header("Data")] public WheelTypeSo goldConfig;

    [Header("Refs")] [SerializeField] SpinWheelAnimator animator;
    [SerializeField] Button spinButton;
    [SerializeField] StreakBarView streakBar;
    [SerializeField] RewardsPanel rewardsPanel;
    [SerializeField] Image wheelImage;

    SpinManager manager;

    [SerializeField] List<SliceView> sliceViews;

    [SerializeField] RewardCardView rewardCardView;

    void Awake()
    {
        manager = new SpinManager(bronzeConfig);
        wheelImage.sprite = bronzeConfig.bgSprite;

        if (spinButton != null)
        {
            spinButton.onClick.AddListener(OnSpinPressed);
        }

        for (int i = 0; i < sliceViews.Count; i++)
        {
            sliceViews[i].Init(bronzeConfig.rewardDefinitions[i]);
        }
    }

    private void SetWheelConfig(int index)
    {
        if (index % 30 == 0 && index != 0)
        {
            manager.SetConfig(goldConfig);
            wheelImage.sprite = goldConfig.bgSprite;
            for (int i = 0; i < sliceViews.Count; i++)
                sliceViews[i].Init(goldConfig.rewardDefinitions[i]);
        }
        else if (index % 5 == 0 && index != 0)
        {
            manager.SetConfig(silverConfig);
            wheelImage.sprite = silverConfig.bgSprite;

            for (int i = 0; i < sliceViews.Count; i++)
                sliceViews[i].Init(silverConfig.rewardDefinitions[i]);
        }
        else
        {
            manager.SetConfig(bronzeConfig);
            wheelImage.sprite = bronzeConfig.bgSprite;
            for (int i = 0; i < sliceViews.Count; i++)
                sliceViews[i].Init(bronzeConfig.rewardDefinitions[i]);
        }
    }


    void OnDestroy()
    {
        if (spinButton != null)
            spinButton.onClick.RemoveListener(OnSpinPressed);
    }
    

    void OnSpinPressed()
    {
        if (animator.IsBusy) return;

        int idx = manager.PickNextIndex();
        animator.slotCount = 8;
        SetWheelConfig(manager.Streak + 1);

        animator.PlayToIndex(idx, () =>
        {
            var (slice, amount) = manager.Resolve(idx);
            streakBar?.SlideAnim(manager.Streak);
            rewardsPanel?.UpdateRewardListItems(slice, amount);
            rewardCardView.Init(slice, amount);
        });
    }
}