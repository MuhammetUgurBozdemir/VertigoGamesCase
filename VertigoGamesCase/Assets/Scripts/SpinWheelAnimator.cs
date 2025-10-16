using UnityEngine;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;

public class SpinWheelAnimator : MonoBehaviour
{
    [FoldoutGroup("Refs"), SerializeField] private Transform wheelRoot;

    [FoldoutGroup("AnimSettings"), SerializeField]
    private float spinDuration = 3f;

    [FoldoutGroup("AnimSettings"), SerializeField]
    private int extraRounds = 3;

    [FoldoutGroup("AnimSettings"), SerializeField]
    private Ease easeType = Ease.InOutCubic;

    private Tween _spinTween;
    private int slotCount = 8;
    public int GetSlotCount => slotCount;
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