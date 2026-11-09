using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currentPlayerState = PlayerState.Idle;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        ChangeState(PlayerState.Idle);
    }
    public void ChangeState(PlayerState newPlayerState)
    {
        if (_currentPlayerState == newPlayerState)
        {
            return;
        }
        _currentPlayerState = newPlayerState;
    }
    public PlayerState GetCurrentState()
    {
        return _currentPlayerState;
    }
}
