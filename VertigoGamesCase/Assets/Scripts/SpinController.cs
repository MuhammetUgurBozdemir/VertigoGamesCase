using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class SpinController : MonoBehaviour
{
    [FoldoutGroup("Data"), SerializeField] public WheelTypeSo bronzeConfig;
    [FoldoutGroup("Data"), SerializeField] WheelTypeSo silverConfig;
    [FoldoutGroup("Data"), SerializeField] WheelTypeSo goldConfig;

    [FoldoutGroup("Refs")] [SerializeField]
    SpinWheelAnimator animator;

    [FoldoutGroup("Refs")] [SerializeField]
    Button spinButton;

    [FoldoutGroup("Refs")] [SerializeField]
    Button exitButton;

    [FoldoutGroup("Refs")] [SerializeField]
    StreakBarView streakBar;

    [FoldoutGroup("Refs")] [SerializeField]
    RewardsPanel rewardsPanel;

    [FoldoutGroup("Refs")] [SerializeField]
    Image wheelImage;

    [FoldoutGroup("Refs")] [SerializeField]
    ZoneView zoneView;

    [FoldoutGroup("Refs")] [SerializeField]
    RewardCardView rewardCardView;

    [FoldoutGroup("Refs")] [SerializeField]
    List<SliceView> sliceViews;

    SpinManager manager;

    void Awake()
    {
        Application.targetFrameRate = 60;
        manager = new SpinManager(bronzeConfig);
        wheelImage.sprite = bronzeConfig.bgSprite;
        manager.OnBomb += OnBomb;

        spinButton.onClick.AddListener(OnSpinClicked);
        exitButton.onClick.AddListener(OnExitClicked);

        for (int i = 0; i < sliceViews.Count; i++)
        {
            sliceViews[i].Init(bronzeConfig.rewardDefinitions[i]);
        }
    }

    private void OnBomb()
    {
        streakBar.Reset();
        rewardsPanel.Reset();
        zoneView.UpdateZoneTexts(0);
    }

    private void OnExitClicked()
    {
        manager.EndStreak();
        streakBar.Reset(true);
        rewardsPanel.Reset();
        zoneView.UpdateZoneTexts(0);
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
            spinButton.onClick.RemoveListener(OnSpinClicked);
    }


    void OnSpinClicked()
    {
        if (animator.IsBusy) return;

        int idx = manager.PickNextIndex();
        SetWheelConfig(manager.Streak + 1);
        zoneView.UpdateZoneTexts(manager.Streak + 1);
        animator.PlayToIndex(idx, () =>
        {
            var (slice, amount) = manager.Resolve(idx);
            streakBar?.SlideAnim(manager.Streak);
            if (!slice.IsBomb()) rewardsPanel?.UpdateRewardListItems(slice, amount);
            rewardCardView.Init(slice, amount);
        });
    }
}