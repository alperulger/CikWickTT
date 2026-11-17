using DG.Tweening;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
     [Header("References")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private Transform _orienationTransform;
    [SerializeField] private Transform _playerVisualTransform;
    [Header("Settings")]
    [SerializeField] private float _rotationSpeed = 5f;
    private void Update()
    {
        if(GameManager.Instance.GetCurrentGameState()!=GameState.Play && GameManager.Instance.GetCurrentGameState()!=GameState.Resume)
        {
            return;
            
        }
        Vector3 viewDir = _playerTransform.position - new Vector3(transform.position.x, _playerTransform.position.y, transform.position.z);
        _orienationTransform.forward = viewDir.normalized;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = _orienationTransform.forward * verticalInput + _orienationTransform.right * horizontalInput;
        if (inputDir != Vector3.zero)
        {
             _playerVisualTransform.forward = Vector3.Slerp(_playerVisualTransform.forward, inputDir.normalized, Time.deltaTime * _rotationSpeed); 
            
        }      
       

         
    }
 

}
