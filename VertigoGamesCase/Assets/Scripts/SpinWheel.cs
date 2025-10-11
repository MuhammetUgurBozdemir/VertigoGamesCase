using UnityEngine;
using System.Collections;
using System;
using Sirenix.OdinInspector;

public class SpinWheel : MonoBehaviour
{
    [Header("Wheel Settings")] public Transform wheelTransform; // ui_wheel_root
    public int slotCount = 8; // slot sayısı
    public float spinDuration = 3f; // dönüş süresi
    public int spinRounds = 3; // kaç tur atacak (ekstra tam tur)
    public AnimationCurve spinCurve; // easing (örnek: EaseOut)

    [Header("Events")] public Action<int> OnSlotSelected; // sonucu dışarı bildirir

    bool spinning = false;

    [Button]
    public void Spin()
    {
        if (spinning || wheelTransform == null) return;
        StartCoroutine(SpinRoutine());
    }

    IEnumerator SpinRoutine()
    {
        spinning = true;

        // rastgele hedef slot seç
        int selectedIndex = UnityEngine.Random.Range(0, slotCount);

        // her slotun açısı
        float anglePerSlot = 360f / slotCount;
        float targetAngle = (selectedIndex * anglePerSlot) + (spinRounds * 360f);

        float startAngle = wheelTransform.localEulerAngles.z;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = spinCurve != null ? spinCurve.Evaluate(elapsed / spinDuration) : (elapsed / spinDuration);
            float currentAngle = Mathf.Lerp(startAngle, -targetAngle, t); // - çünkü UI ters döner
            wheelTransform.localEulerAngles = new Vector3(0, 0, currentAngle);
            yield return null;
        }

        // tam hizala
        float finalAngle = -selectedIndex * anglePerSlot;
        wheelTransform.localEulerAngles = new Vector3(0, 0, finalAngle);

        spinning = false;

        // seçilen slotu bildir
        OnSlotSelected?.Invoke(selectedIndex);
        Debug.Log($"🎯 Selected Slot: {selectedIndex}");
    }
}