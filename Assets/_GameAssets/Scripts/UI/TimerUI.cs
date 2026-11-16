using UnityEngine;
using TMPro;
using DG.Tweening;
public class TimerUI : MonoBehaviour
{
    [Header("References")]
   [SerializeField] private RectTransform _timerRotatableRectTransform; 
   [SerializeField] private TMP_Text _timerText;
   [Header("Settings")]
[SerializeField] private float _rotationDuration = 0.5f;
[SerializeField] private Ease _rotationEase = Ease.Linear;
private float _elapsedTime;
    private void Start()
    {
        PlayRotateAnimation();
        StartTimer();
    }
   private void PlayRotateAnimation()
   {
        _timerRotatableRectTransform.DORotate(new Vector3(0, 0, -360), _rotationDuration, RotateMode.FastBeyond360).SetEase(_rotationEase).SetLoops(-1,LoopType.Restart);
   }
   private void StartTimer()
   {
        _elapsedTime = 0f;
        InvokeRepeating(nameof(UpdateTimerUI), 1f, 1f);
       
   }
   private void UpdateTimerUI()
    {
        _elapsedTime += 1f;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
}

