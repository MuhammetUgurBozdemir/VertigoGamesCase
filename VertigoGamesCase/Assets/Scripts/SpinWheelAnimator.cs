using UnityEngine;
using DG.Tweening;
using System;

public class SpinWheelAnimator : MonoBehaviour
{
    [Header("Refs")]
    public Transform wheelRoot;
    public int slotCount = 8;

    [Header("Anim")]
    public float spinDuration = 3f;
    public int extraRounds = 3;
    public Ease easeType = Ease.InOutCubic;

    private Tween _spinTween;
    bool _busy;
    public bool IsBusy => _busy;

    public void PlayToIndex(int targetIndex, Action onDone)
    {
        if (_busy || !wheelRoot) return;

        _busy = true;

        float anglePer = 360f / Mathf.Max(1, slotCount);

        float startZ = wheelRoot.localEulerAngles.z;
        if (startZ > 180f) startZ -= 360f;

        float endZ = startZ - (extraRounds * 360f) - (targetIndex * anglePer);

        _spinTween?.Kill();

        _spinTween = DOTween.To(() => startZ, x =>
            {
                wheelRoot.localEulerAngles = new Vector3(0, 0, x);
            }, endZ, spinDuration)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                wheelRoot.localEulerAngles = new Vector3(0, 0, -targetIndex * anglePer);
                _busy = false;
                onDone?.Invoke();
            });
    }

    public void StopImmediately()
    {
        _spinTween?.Kill();
        _busy = false;
    }
}