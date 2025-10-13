using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StreakBarView : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> streakTexts;
    [SerializeField] private List<RectTransform> streakTextHolders;
    [SerializeField] private TextMeshProUGUI cursorText;
    [SerializeField] private Image cursorImage;

    [SerializeField] private RectTransform content;
    [SerializeField] private HorizontalLayoutGroup hLayout;

    private Tween _slideTween;

    [SerializeField] RectTransform mover;
    [SerializeField] ContentSizeFitter fitter;
    [SerializeField] ScrollRect scrollRect;


    private int _currentStreak;

    public void Start()
    {
        for (var i = 0; i < streakTexts.Count; i++)
        {
            streakTexts[i].text = (i).ToString();
        }

        CheckColors();
    }

    float MeasureStep()
    {
        Canvas.ForceUpdateCanvases();
        var a = streakTextHolders[0];
        var b = streakTextHolders[1];
        float ax = a.anchoredPosition.x + a.rect.width * 0.5f;
        float bx = b.anchoredPosition.x + b.rect.width * 0.5f;
        return Mathf.Abs(bx - ax);
    }

    private void CheckColors()
    {
        cursorImage.color = _currentStreak % 30 == 0 && _currentStreak != 0 ? new Color(1f, 0.84f, 0f) :
            _currentStreak % 5 == 0 && _currentStreak != 0 ? Color.green : Color.white;

        for (var i = 0; i < streakTexts.Count; i++)
        {
            TextMeshProUGUI t = streakTexts[i];
            var number = Convert.ToInt16(t.text);
            var color = Color.white;

            if (number % 30 == 0 && number != 0)
                color = new Color(1f, 0.84f, 0f);

            else if (number % 5 == 0 && number != 0)
                color = Color.green;

            color.a = number < _currentStreak ? 0.3f : 1f;

            t.color = color;
        }
    }

    public void SlideAnim(int currentStreak)
    {
        float step = MeasureStep();
        if (step <= 0f) return;

        _slideTween?.Kill(true);

        bool srEnabled = scrollRect && scrollRect.enabled;
        if (scrollRect) scrollRect.enabled = false;

        bool hlgEnabled = hLayout && hLayout.enabled;
        bool fitEnabled = fitter && fitter.enabled;

        if (hLayout) hLayout.enabled = true;
        if (fitter) fitter.enabled = false;

        var startX = mover.anchoredPosition.x;
        var targetX = startX - step;

        _slideTween = mover.DOAnchorPosX(targetX, .25f).SetOptions(snapping: true).OnComplete(() =>
        {
            mover.anchoredPosition = new Vector2(startX, mover.anchoredPosition.y);
            IncreaseStreak(currentStreak);

            CheckColors();

            if (hLayout) hLayout.enabled = hlgEnabled;
            if (fitter) fitter.enabled = fitEnabled;
            if (scrollRect) scrollRect.enabled = srEnabled;
            _slideTween = null;
        });
    }

    private void IncreaseStreak(int currentStreak)
    {
        _currentStreak = currentStreak;

        cursorText.text = _currentStreak.ToString();

        bool isAllActivated = streakTextHolders.TrueForAll(x => x.gameObject.activeSelf);

        if (!isAllActivated)
        {
            var holder = streakTextHolders.Find(x => !x.gameObject.activeSelf);
            holder.gameObject.SetActive(true);
        }
        else
        {
            var holder = streakTextHolders[0];
            var text = streakTexts[0];

            streakTextHolders.Remove(holder);
            streakTextHolders.Add(holder);

            streakTexts.Remove(text);
            streakTexts.Add(text);

            holder.SetAsLastSibling();
            text.text = (_currentStreak + 6).ToString();
        }
    }
}