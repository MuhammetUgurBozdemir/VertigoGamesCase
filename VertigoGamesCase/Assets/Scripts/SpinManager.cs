using System;

public class SpinManager
{
    private WheelTypeSo Config { get; set; }

    private int Streak { get; set; }

    public event Action<int> OnSlotSelected; // index
    public event Action<RewardDefinitionSo> OnRewardResolved;

    public SpinManager(WheelTypeSo cfg)
    {
        Config = cfg;
    }

    public int PickNextIndex()
    {
        return UnityEngine.Random.Range(0, Config.rewardDefinitions.Length);
    }

    public (RewardDefinitionSo slice, int amount) Resolve(int idx)
    {
        var slice = Config.rewardDefinitions[idx];
        int amount = slice.GetRandomAmount();

        if (slice.IsBomb()) Streak = 0;
        else Streak++;

        OnSlotSelected?.Invoke(idx);
        OnRewardResolved?.Invoke(slice);

        return (slice, amount);
    }
}