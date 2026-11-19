using UnityEngine;

public class CatStateController : MonoBehaviour
{
   [SerializeField] CatState _currentCatState=CatState.Walking;
    void Start()
    {
        ChangeState(CatState.Walking);
    }
    public void ChangeState(CatState newState)
    {
        if(newState==_currentCatState){return;}
        _currentCatState=newState;
    }
    public CatState GetCurrentState()
    {
        return _currentCatState;
    }
}
