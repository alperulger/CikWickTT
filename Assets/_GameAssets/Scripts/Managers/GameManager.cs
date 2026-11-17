using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private void Awake()
    {
     
            Instance = this;
           
        
    }
    [Header("References")]
    [SerializeField] EggCounterUI _eggCounterUI;
    [Header("Settings")]
    [SerializeField] private int _maxEggCount = 5;
    private int _currentEggCount = 0;
    

    public void OnEggCollected()
    {
        _currentEggCount++;
      
        _eggCounterUI.SetEggCounterText(_currentEggCount,_maxEggCount);
        if (_currentEggCount == _maxEggCount)
        {
            _eggCounterUI.SetEggCompleted();
        }
        Debug.Log("Egg Count: " + _currentEggCount);
    }
   
}
