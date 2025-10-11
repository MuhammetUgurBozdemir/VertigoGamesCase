using UnityEngine;
using UnityEngine.UI;

public class SpinController : MonoBehaviour
{
    [Header("Data")]
    public WheelTypeSo config;                 

    [Header("Refs")]
    public SpinWheelAnimator animator;       
    public Button spinButton;                  
    // public StreakBarView streakBar;          
    // public RewardsPanelView rewardsPanel;      

    SpinManager manager;

    void Awake()
    {
        manager = new SpinManager(config);

        
        if (spinButton != null)
        {
            spinButton.onClick.AddListener(OnSpinPressed);
        }

        
        manager.OnSlotSelected += idx => Debug.Log("Slot: " + idx);
        manager.OnRewardResolved += slice => { /* popup/FX vs. */ };
    }

    void OnDestroy()
    {
        if (spinButton != null)
            spinButton.onClick.RemoveListener(OnSpinPressed);
    }

    void OnSpinPressed()
    {
        if (animator.IsBusy) return;

        int idx = manager.PickNextIndex();             
        animator.slotCount = 8;        

        animator.PlayToIndex(idx, () =>                // 2) animasyon
        {
            var (slice, amount) = manager.Resolve(idx);// 3) ödülü hesapla / streak güncelle
            // 4) UI güncelle
            // streakBar?.Refresh(manager.Streak);
            // rewardsPanel?.Apply(slice, amount);
            // burada return’e ihtiyacın varsa event ya da callback ile yayınla
        });
    }
}