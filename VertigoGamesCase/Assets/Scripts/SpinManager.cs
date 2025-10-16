using System;

public class SpinManager
{
    private WheelTypeSo Config { get; set; }
    public int Streak { get; private set; }

    public event Action OnBomb;

    public SpinManager(WheelTypeSo cfg)
    {
        Config = cfg;
    }

    public int PickNextIndex()
    {
        return UnityEngine.Random.Range(0, Config.rewardDefinitions.Length);
    }

    public void SetConfig(WheelTypeSo cfg)
    {
        Config = cfg;
    }

    public void EndStreak()
    {
        Streak = 0;
    }
    public (RewardDefinitionSo slice, int amount) Resolve(int idx)
    {
        var slice = Config.rewardDefinitions[idx];
        int amount = slice.GetRandomAmount();

        if (slice.IsBomb())
        {
            Streak = 0;
            OnBomb?.Invoke();
        }
        else
        {
            Streak++;
        }

        return (slice, amount);
    }
}