using UnityEngine;
using TMPro;
using DG.Tweening;
using System;
public class TimerUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform _timerRotatableRectTransform;
    [SerializeField] private TMP_Text _timerText;
    [Header("Settings")]
    [SerializeField] private float _rotationDuration = 0.5f;
    [SerializeField] private Ease _rotationEase = Ease.Linear;
    private float _elapsedTime;
    private bool _isTimerRunning;
    private Tween _rotationTween;
    private string _finalTime;

    private void Start()
    {
      
        GameManager.Instance.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Play:
              PlayRotateAnimation();
              StartTimer();
            break;

            case GameState.Pause:
                StopTimer();
                break;
            case GameState.Resume:
                ResumeTimer();
                break;
            case GameState.GameOver:
                FinishTimer();
                break;
        }
    }

    private void PlayRotateAnimation()
    {
        _rotationTween = _timerRotatableRectTransform.DORotate(new Vector3(0, 0, -360), _rotationDuration, RotateMode.FastBeyond360).SetEase(_rotationEase).SetLoops(-1, LoopType.Restart);
    }
    private void StartTimer()
    {
        _isTimerRunning = true;
        _elapsedTime = 0f;
        InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);

    }
    private void StopTimer()
    {
        _isTimerRunning = false;
        CancelInvoke(nameof(UpdateTimerUI));
        _rotationTween.Pause();
    }
    private void ResumeTimer()
    {

        if (!_isTimerRunning)
        {
            _isTimerRunning = true;
            InvokeRepeating(nameof(UpdateTimerUI), 0f, 1f);
            _rotationTween.Play();
        }

    }

    private void FinishTimer()
    {
        StopTimer();
        _finalTime = GetFormattedElapsedTime();
    }
    private string GetFormattedElapsedTime()
    {
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);

       return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    private void UpdateTimerUI()
    {
        if (!_isTimerRunning)
        {
            return;
        }
        _elapsedTime += 1f;
        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
        _timerText.text = $"{minutes:00}:{seconds:00}";
    }
    public string GetFinalTime()
    {
        return _finalTime;
    }
}

