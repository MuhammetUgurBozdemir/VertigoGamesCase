using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;

public class SpinWheel : MonoBehaviour
{
    [Header("Wheel Settings")] public Transform wheelTransform;
    public int slotCount = 8;
    public float spinDuration = 3f; 
    public int spinRounds = 3; 
    public AnimationCurve spinCurve; 

    [Header("Events")] private readonly Action<int> _onSlotSelected;

    private bool _spinning;

    public SpinWheel(Action<int> onSlotSelected)
    {
        _onSlotSelected = onSlotSelected;
    }

    [Button]
    public void Spin()
    {
        if (_spinning || wheelTransform == null) return;
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        _spinning = true;

        int selectedIndex = UnityEngine.Random.Range(0, slotCount);

        float anglePerSlot = 360f / slotCount;
        float targetAngle = (selectedIndex * anglePerSlot) + (spinRounds * 360f);

        float startAngle = wheelTransform.localEulerAngles.z;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = spinCurve?.Evaluate(elapsed / spinDuration) ?? (elapsed / spinDuration);
            float currentAngle = Mathf.Lerp(startAngle, -targetAngle, t); 
            wheelTransform.localEulerAngles = new Vector3(0, 0, currentAngle);
            yield return null;
        }

      
        float finalAngle = -selectedIndex * anglePerSlot;
        wheelTransform.localEulerAngles = new Vector3(0, 0, finalAngle);

        _spinning = false;
        
        _onSlotSelected?.Invoke(selectedIndex);
        Debug.Log($"🎯 Selected Slot: {selectedIndex}");
    }
}