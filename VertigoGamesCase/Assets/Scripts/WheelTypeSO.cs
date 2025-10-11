using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spin/Data/WheelTypeSO", fileName = "wheelTypeSO")]
public class WheelTypeSo : ScriptableObject
{
    public WheelType wheelType;
    public Sprite bgSprite;
    public Sprite pointerSprite;
    public RewardDefinitionSo[] rewardDefinitions = new RewardDefinitionSo[8];
}