
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
     [SerializeField] private Transform _orientationTransform;
     [Header("Movement Settings")]
     [SerializeField] private float _movementSpeed = 5f;
      [Header("Jump Settings")]
     [SerializeField] private float _jumpForce = 10f;
     [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
     [SerializeField] private float _jumpCooldown = 1f;
     private bool _canJump=true; // Indicates whether the player can jump or not

     [Header("Ground Check Settings")]
     [SerializeField] private float _playerHeight = 2f;
     [SerializeField] private LayerMask _groundLayer; // Layer mask to identify ground objects
    private Rigidbody _playerRigidbody;
    private float _horizontalInput,_verticalInput;
    private Vector3 _movementDirection;
   

  
    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true; // Prevents the player from rotating due to physics interactions
    }   
    private void Update()
    {
        SetInputs();
    }
   private void FixedUpdate()
    {
        //Physics related updates can be handled here if needed
        SetPlayerMovement();
    }
    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(_jumpKey)&&_canJump&&IsGrounded())
        {
            _canJump = false; // Prevents multiple jumps until the player lands again
             SetPlayerJumping();
            
            Invoke(nameof(ResetJumping), _jumpCooldown); // Resets the jump ability after the cooldown
          
        }   

      //  Debug.Log($"Horizontal Input: {_horizontalInput}, Vertical Input: {_verticalInput}");
        
    }

    private void  SetPlayerJumping()
    {
        
            // Apply an upward force to the player for jumping
            _playerRigidbody.linearVelocity=new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z); // Reset vertical velocity before jumping
            _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
        
    }
    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
       // transform.position += _movementDirection.normalized * 5f * Time.deltaTime;  // Static movement without physics
        _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);//More realistic movement using physics
    }
    private void ResetJumping()
    {
        _canJump = true; // Allow the player to jump again
    }
    private bool IsGrounded()
    {
      
        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight*0.5f+0.2f,_groundLayer);
        
    }
    
    
}
