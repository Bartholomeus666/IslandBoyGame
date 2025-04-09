using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static event Action<Vector2> OnMoveInput;
    public static event Action<Vector2> OnLookInput;
    public static event Action OnJumpInput;

    public void MoveFunction(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance.IsInventoryOpen)
        {
            Vector2 moveInput = context.ReadValue<Vector2>();
            OnMoveInput?.Invoke(moveInput);
        }
        else
        {
            OnMoveInput?.Invoke(Vector2.zero);
        }
    }

    public void LookAction(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance.IsInventoryOpen)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            OnLookInput?.Invoke(lookInput);
        }
    }

    public void JumpFunction(InputAction.CallbackContext context)
    {
        if (!GameStateManager.instance.IsInventoryOpen && context.performed)
        {
            OnJumpInput?.Invoke();
        }
    }

    public void InventoryToggle(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameStateManager.instance.ToggleInventory();
        }
    }

}
