using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ZoneView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI superZoneText;
    [SerializeField] private TextMeshProUGUI safeZoneText;

    private void Start()
    {
        UpdateZoneTexts(0);
    }

    public void UpdateZoneTexts(int idx)
    {
        int nextSafeZone = ((idx / 5) + 1) * 5;
        int nextSuperZone = ((idx / 30) + 1) * 30;

        superZoneText.text = $"{nextSuperZone}";
        safeZoneText.text = $"{nextSafeZone}";
    }
}