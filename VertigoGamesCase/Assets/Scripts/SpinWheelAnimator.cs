using UnityEngine;
using System.Collections;
using System;

public class SpinWheelAnimator : MonoBehaviour
{
    [Header("Refs")]
    public Transform wheelRoot;       
    public int slotCount = 8;

    [Header("Anim")]
    public float spinDuration = 3f;
    public int extraRounds = 3;
    public AnimationCurve curve = AnimationCurve.EaseInOut(0,0,1,1);

    bool _busy;

    public bool IsBusy => _busy;

    public void PlayToIndex(int targetIndex, Action onDone)
    {
        if (_busy || !wheelRoot) return;
        StartCoroutine(Spin(targetIndex, onDone));
    }

    IEnumerator Spin(int idx, Action onDone)
    {
        _busy = true;
        float anglePer = 360f / Mathf.Max(1, slotCount);

        float start = wheelRoot.localEulerAngles.z;
        float end = -idx * anglePer - extraRounds * 360f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, spinDuration);
            float k = curve.Evaluate(t);
            float a = Mathf.LerpAngle(start, end, k);
            wheelRoot.localEulerAngles = new Vector3(0, 0, a);
            yield return null;
        }

        wheelRoot.localEulerAngles = new Vector3(0, 0, -idx * anglePer);
        _busy = false;
        onDone?.Invoke();
    }
}