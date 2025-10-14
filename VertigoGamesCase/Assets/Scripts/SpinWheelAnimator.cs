using UnityEngine;
using DG.Tweening;
using System;

public class SpinWheelAnimator : MonoBehaviour
{
    [Header("Refs")] public Transform wheelRoot;
    public int slotCount = 8;

    [Header("Anim")] public float spinDuration = 3f;
    public int extraRounds = 3;
    public Ease easeType = Ease.InOutCubic;

    private Tween _spinTween;
    bool _busy;
    public bool IsBusy => _busy;


    public void PlayToIndex(int targetIndex, Action onDone)
    {
        if (_busy || !wheelRoot) return;
        _busy = true;
        
        float targetAngle = 225f + targetIndex * 45f;
        
        float startZ = wheelRoot.localEulerAngles.z;

       
        float delta = Mathf.Repeat(targetAngle - startZ, 360f);
        
        float finalZ = startZ + extraRounds * 360f + delta;

        _spinTween?.Kill();
        _spinTween = wheelRoot
            .DOLocalRotate(new Vector3(0, 0, finalZ), spinDuration, RotateMode.FastBeyond360)
            .SetEase(easeType)
            .OnComplete(() =>
            {
                wheelRoot.localEulerAngles = new Vector3(0, 0, targetAngle);
                _busy = false;
                onDone?.Invoke();
            });
    }
}