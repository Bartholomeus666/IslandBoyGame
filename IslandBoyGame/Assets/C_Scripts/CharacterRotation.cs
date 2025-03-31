using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private float lookSpeedX = 2.0f; // Rotation speed on the X-axis
    [SerializeField] private float lookSpeedY = 2.0f; // Rotation speed on the Y-axis
    [SerializeField] private Transform playerBody; // Reference to the player's body
    [SerializeField] private Transform cameraTransform; // Reference to the camera

    private float rotationX = 0f; // Current X rotation of the camera

    private void Start()
    {
        // If camera transform not set, use this transform
        if (cameraTransform == null)
            cameraTransform = transform;

        // If player body not set, use parent as default
        if (playerBody == null && transform.parent != null)
            playerBody = transform.parent;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LookAction(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        // Handle vertical rotation (looking up/down)
        rotationX -= mouseDelta.y * lookSpeedY * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -55f, 55f);

        // Apply vertical rotation to camera only
        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        // Handle horizontal rotation (looking left/right) - applied to the body
        if (playerBody != null)
        {
            // Rotate player body horizontally without affecting the existing rotation
            float yRotation = playerBody.eulerAngles.y + (mouseDelta.x * lookSpeedX * Time.deltaTime);
            playerBody.rotation = Quaternion.Euler(playerBody.eulerAngles.x, yRotation, playerBody.eulerAngles.z);
        }
    }
}