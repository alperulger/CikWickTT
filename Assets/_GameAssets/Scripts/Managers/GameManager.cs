using System;
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
    
    private GameState _currentGameState;
        private void Awake()
    {
     
            Instance = this;
           
        
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
    public GameState GetCurrentGameState()
    {
        return _currentGameState;
    }

   
}
