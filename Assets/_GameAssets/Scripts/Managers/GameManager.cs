using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action<GameState>OnGameStateChanged;
    [Header("References")]
    [SerializeField] EggCounterUI _eggCounterUI;
    [SerializeField] WinLoseUI _winloseUI;
    [Header("Settings")]
    [SerializeField] private int _maxEggCount = 5;
    [SerializeField] private float _delay=0.5f;
    
    private GameState _currentGameState;
        private void Awake()
    {
     
            Instance = this;
           
        
    }
    void Start()
    {
        HealthManager.Instance.OnPlayerDeath+=HealthManager_OnPlayerDeath;
    }

    private void HealthManager_OnPlayerDeath()
    {
        StartCoroutine(OnGameOver());
    }

    void OnEnable()
    {
        ChangeGameState(GameState.Play);
    }
    public void ChangeGameState(GameState gameState)
    {
        OnGameStateChanged?.Invoke(gameState);
        _currentGameState=gameState;
        Debug.Log("GameState"+gameState);
    
    }
    private int _currentEggCount = 0;
    

    public void OnEggCollected()
    {
        _currentEggCount++;
      
        _eggCounterUI.SetEggCounterText(_currentEggCount,_maxEggCount);
        if (_currentEggCount == _maxEggCount)
        {
            //Win
            _eggCounterUI.SetEggCompleted();
            ChangeGameState(GameState.GameOver);
            _winloseUI.OnGameWin();
        }
        
    }

    private IEnumerator OnGameOver()
    {
        yield return new WaitForSeconds(_delay);
        ChangeGameState(GameState.GameOver);
        _winloseUI.OnGameLose();
    }
  
    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

   
}
