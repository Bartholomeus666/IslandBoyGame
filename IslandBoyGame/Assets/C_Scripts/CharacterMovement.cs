using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _characterController;

    private Vector3 _moveDirection;
    private Vector3 _moveVector;
    [SerializeField] private float speed;
    [SerializeField] private float gravity;
    [SerializeField] private float jumpStrength;

    private float _yValue;

    private void Start()
    {
        _yValue = 0f;
    }

    private void FixedUpdate()
    {

        FindDirection();
        ApplyGravity();

        _characterController.Move(_moveVector * speed * Time.deltaTime);

        //Debug.Log(_moveVector);
        //Debug.Log(_characterController.isGrounded);
    }

    public void MoveFunction(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        _moveDirection.x = input.x;
        _moveDirection.z = input.y;
    }

    public void JumpFunction(InputAction.CallbackContext context)
    {
        if (_characterController.isGrounded)
        {
            _yValue = 0;
            _yValue += jumpStrength;
        }
    }

    private void ApplyGravity()
    {
        if (_characterController.isGrounded)
        {
            return;
        }
        _yValue += gravity * Time.deltaTime;
    }

    public void FindDirection()
    {
        // Move relative to the character's orientation
        _moveVector = transform.right * _moveDirection.x + transform.forward * _moveDirection.z;
        _moveVector.y = _yValue;
    }
}
