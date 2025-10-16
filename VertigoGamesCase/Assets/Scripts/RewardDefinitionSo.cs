using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Spin/Data/Reward Definition", fileName = "reward_")]
public class RewardDefinitionSo : ScriptableObject
{
    public string id;
    public string displayName;

    public Sprite icon;

    public RewardType type = RewardType.Currency;

    [Min(0)] public int baseAmount = 1;

    public int GetRandomAmount()
    {
        return Random.Range(1, baseAmount);
    }

    public bool IsBomb()
    {
        return type == RewardType.Bomb;
    }
}