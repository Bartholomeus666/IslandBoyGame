using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;

    [Header("Movement Parameters")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float jumpStrength = 5f;

    private Vector2 moveInput;
    private Vector3 moveVector;
    private float verticalVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        verticalVelocity = 0f;
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnMoveInput += HandleMoveInput;
        PlayerInputHandler.OnJumpInput += HandleJump;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnMoveInput -= HandleMoveInput;
        PlayerInputHandler.OnJumpInput -= HandleJump;
    }

    private void FixedUpdate()
    {
        CalculateMoveDirection();
        ApplyGravity();
        characterController.Move(moveVector * Time.deltaTime);
    }

    private void HandleMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void HandleJump()
    {
        if (characterController.isGrounded)
        {
            verticalVelocity = jumpStrength;
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
    }

    private void CalculateMoveDirection()
    {
        Vector3 horizontalMovement = (transform.right * moveInput.x + transform.forward * moveInput.y) * speed;
        moveVector = new Vector3(horizontalMovement.x, verticalVelocity, horizontalMovement.z);
    }
}