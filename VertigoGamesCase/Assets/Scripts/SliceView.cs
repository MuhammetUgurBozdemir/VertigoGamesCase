using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliceView : MonoBehaviour
{
   [SerializeField] private Image sliceImage;
   [SerializeField] private RewardDefinitionSo rewardDefinitionSo;

   public void Init(RewardDefinitionSo _rewardDefinitionSo)
   {
      rewardDefinitionSo = _rewardDefinitionSo;
      sliceImage.sprite = rewardDefinitionSo.icon;
   }
}
