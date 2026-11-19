
using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action  OnPlayerJumped;
    public event Action <PlayerState>OnplayerStateChanged;
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;
    [Header("Movement Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed = 5f;
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpCooldown = 1f;
    [SerializeField] private float _airMultiplier=1f;
    [SerializeField] private float _airDrag=1f;
    private bool _canJump = true; // Indicates whether the player can jump or not

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier = 1.5f;
    [SerializeField] private float _slideDrag;



    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private LayerMask _groundLayer; // Layer mask to identify ground objects
    [SerializeField] private float _groundDrag;
    private StateController _stateController;
    private Rigidbody _playerRigidbody;
    private float _startingMovementSpeed,_startingJumpForce;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private bool _isSliding = false; // Indicates whether the player is currently sliding or not


    void Awake()
    {
        _stateController = GetComponent<StateController>();
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true; // Prevents the player from rotating due to physics interactions
        _startingMovementSpeed=_movementSpeed;
        _startingJumpForce=_jumpForce;

    }
   
    private void Update()
    {
          if(GameManager.Instance.GetCurrentGameState()!=GameState.Play && GameManager.Instance.GetCurrentGameState()!=GameState.Resume)
        {
            return;
            
        }
        SetInputs();
        SetStates();
        SetPlayerDrag();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
          if(GameManager.Instance.GetCurrentGameState()!=GameState.Play && GameManager.Instance.GetCurrentGameState()!=GameState.Resume)
        {
            return;
            
        }
        //Physics related updates can be handled here if needed
        SetPlayerMovement();
    }
    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKey(_slideKey))
        {
            _isSliding = true;
        }

        else if (Input.GetKeyUp(_movementKey))
        {
            _isSliding = false;
        }

        else if (Input.GetKeyDown(_jumpKey) && _canJump && IsGrounded())
        {
            _canJump = false; // Prevents multiple jumps until the player lands again
            SetPlayerJumping();

            Invoke(nameof(ResetJumping), _jumpCooldown); // Resets the jump ability after the cooldown


        }

        //  Debug.Log($"Horizontal Input: {_horizontalInput}, Vertical Input: {_verticalInput}");

    }

    private void SetPlayerDrag()
    {
        _playerRigidbody.linearDamping=_stateController.GetCurrentState() switch
        {
            PlayerState.Move=>_groundDrag,
            PlayerState.Slide=> _slideDrag,
            PlayerState.jump=>_airDrag,
            _ => _playerRigidbody.linearDamping
        };



    }
    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _playerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _playerRigidbody.linearVelocity.y, limitedVelocity.z);
        }

    }
    private void SetPlayerJumping()
    {
        OnPlayerJumped.Invoke(); // Notify subscribers that the player has jumped

        // Apply an upward force to the player for jumping
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z); // Reset vertical velocity before jumping
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

    }
    private void SetStates()
    {
        var movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var isSliding=IsSliding();
        var currentState = _stateController.GetCurrentState();
        var newState= currentState switch
        {
          _ when movementDirection==Vector3.zero && isGrounded && !isSliding=>PlayerState.Idle,
          _ when movementDirection!= Vector3.zero && isGrounded && !isSliding=>PlayerState.Move,
          _ when movementDirection!= Vector3.zero && isGrounded && isSliding=>PlayerState.Slide,
          _ when movementDirection!= Vector3.zero && isGrounded && isSliding=>PlayerState.SlideIdle,
          _ when  !_canJump && !isGrounded=>PlayerState.jump,
          _ =>currentState

        };
        if(newState!=currentState)
        {
            _stateController.ChangeState(newState);
            OnplayerStateChanged?.Invoke(newState);

        }
        Debug.Log("Player State:"+newState);

    }
    private void SetPlayerMovement()
    {

        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        //Debug.Log("OrientationForward" + _orientationTransform.forward + "_orientationTransform.right " + _orientationTransform.right);
        // transform.position += _movementDirection.normalized * 5f * Time.deltaTime;  // Static movement without physics
        float forceMultiplayer=_stateController.GetCurrentState() switch
        {
            PlayerState.Move=>1f,
            PlayerState.Slide=>_slideMultiplier,
            PlayerState.jump=>_airMultiplier,
            _ => 1f
        };
       _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * forceMultiplayer, ForceMode.Force);//More realistic movement using physics
    }
    private void ResetJumping()
    {
        _canJump = true; // Allow the player to jump again
    }
    #region Helper Functions
    private bool IsGrounded()
    {

        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);

    }
    private Vector3 GetMovementDirection()
    {
        return _movementDirection.normalized;
    }
    private bool IsSliding()
    {
        return _isSliding;
    }

    public void SetMovementSpeed(float speed,float duration)
    {
        _movementSpeed+=speed;
        Invoke(nameof(ResetMovementSpeed),duration);
    }
    private void ResetMovementSpeed()
    {
        _movementSpeed=_startingMovementSpeed;
    }

      public void SetJumpForce(float force,float duration)
    {
        _jumpForce+=force;
        Invoke(nameof(ResetJumpForce),duration);
    }

      private void ResetJumpForce()
    {
        _jumpForce=_startingJumpForce;
    }
    public Rigidbody GetRigidbody()
    {
        return _playerRigidbody;
    }
    public bool CanCatChase()
    {
        if(Physics.Raycast(transform.position,Vector3.down, out RaycastHit hit,_playerHeight*0.5f+0.2f,_groundLayer))
        {
            if(hit.collider.gameObject.layer==LayerMask.NameToLayer(Consts.Layers.FLOOR_LAYER))
            {
                return true;
            }
            else if (hit.collider.gameObject.layer==LayerMask.NameToLayer(Consts.Layers.GROUND_LAYER))
            {
                return false;
            }
        }
        return false;
    }
    #endregion


}
