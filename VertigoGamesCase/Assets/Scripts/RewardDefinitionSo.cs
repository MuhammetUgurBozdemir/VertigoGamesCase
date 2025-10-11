using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Spin/Data/Reward Definition", fileName = "reward_")]
public class RewardDefinitionSo : ScriptableObject
{
    [Header("Kimlik")] public string id;
    public string displayName;

    [Header("Görsel")] public Sprite icon;
    public Color tint = Color.white;

    [Header("Tür")] public RewardType type = RewardType.Currency;
    [ShowIf("type", RewardType.Currency)] public CurrencyKind currency;

    [Header("Miktar Varsayılanı")] [Min(0)]
    public int baseAmount = 1;

    public int GetRandomAmount()
    {
        return Random.Range(1, baseAmount);
    }

    public bool IsBomb()
    {
        return type == RewardType.Bomb;
    }
}