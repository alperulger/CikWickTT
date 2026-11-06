
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _orientationTransform;
    [Header("Movement Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed = 5f;
    [Header("Jump Settings")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] private float _jumpCooldown = 1f;

    private bool _canJump = true; // Indicates whether the player can jump or not

    [Header("Sliding Settings")]
    [SerializeField] private KeyCode _slideKey;
    [SerializeField] private float _slideMultiplier = 1.5f;
    [SerializeField] private float _slideDrag;



    [Header("Ground Check Settings")]
    [SerializeField] private float _playerHeight = 2f;
    [SerializeField] private LayerMask _groundLayer; // Layer mask to identify ground objects
    [SerializeField] private float _groundDrag;
    private Rigidbody _playerRigidbody;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private bool _isSliding = false; // Indicates whether the player is currently sliding or not


    void Awake()
    {
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.freezeRotation = true; // Prevents the player from rotating due to physics interactions

    }
    private void Update()
    {
        SetInputs();
        SetPlayerDrag();
        LimitPlayerSpeed();
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

        if (_isSliding)
        {
            _playerRigidbody.linearDamping = _slideDrag;
        }
        else
        {
            _playerRigidbody.linearDamping = _groundDrag;
        }


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

        // Apply an upward force to the player for jumping
        _playerRigidbody.linearVelocity = new Vector3(_playerRigidbody.linearVelocity.x, 0f, _playerRigidbody.linearVelocity.z); // Reset vertical velocity before jumping
        _playerRigidbody.AddForce(transform.up * _jumpForce, ForceMode.Impulse);

    }
    private void SetPlayerMovement()
    {
        _movementDirection = _orientationTransform.forward * _verticalInput + _orientationTransform.right * _horizontalInput;
        // transform.position += _movementDirection.normalized * 5f * Time.deltaTime;  // Static movement without physics
        if (_isSliding)
        {
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * _slideMultiplier, ForceMode.Force);//More realistic movement using physics
        }
        else
        {
            _playerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);//More realistic movement using physics
        }
    }
    private void ResetJumping()
    {
        _canJump = true; // Allow the player to jump again
    }
    private bool IsGrounded()
    {

        // Check if the player is grounded using a raycast
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);

    }


}
