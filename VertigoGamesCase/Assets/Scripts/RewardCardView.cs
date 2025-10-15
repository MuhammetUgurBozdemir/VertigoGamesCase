using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardCardView : MonoBehaviour
{
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemNameText;
    [SerializeField] TextMeshProUGUI itemAmountext;

    [SerializeField] private GameObject bombCard;
    [SerializeField] private GameObject rewardCard;

    [SerializeField] private Image darkBg;

    private Vector3 initialPos;

    private void Start()
    {
        initialPos = rewardCard.transform.localPosition;
    }

    public void Init(RewardDefinitionSo rewardDefinitionSo, int amount)
    {
        itemImage.sprite = rewardDefinitionSo.icon;
        itemNameText.text = rewardDefinitionSo.displayName;
        itemAmountext.text = amount.ToString();
        darkBg.gameObject.SetActive(true);
        darkBg.DOFade(.8f, 0.3f);

        var transform = rewardDefinitionSo.type == RewardType.Bomb ? bombCard : rewardCard;
        CardRevealAnim(transform.transform);
    }

    [Button]
    public void CardRevealAnim(Transform cardTransform)
    {
        cardTransform.gameObject.SetActive(true);

        darkBg.DOFade(0, 0.4f).SetDelay(2).OnComplete(() => darkBg.gameObject.SetActive(false));

        cardTransform.DOLocalMove(Vector3.zero, .4f).SetEase(Ease.InOutBack);
        cardTransform.DOLocalMove(initialPos, .4f).SetEase(Ease.InOutBack).SetDelay(2f)
            .OnComplete(() => cardTransform.gameObject.SetActive(false));
    }
}