using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private float lookSpeedX = 2.0f;
    [SerializeField] private float lookSpeedY = 2.0f;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform cameraTransform;

    private float rotationX = 0f;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = transform;

        if (playerBody == null && transform.parent != null)
            playerBody = transform.parent;
    }

    private void OnEnable()
    {
        PlayerInputHandler.OnLookInput += HandleLookInput;
    }

    private void OnDisable()
    {
        PlayerInputHandler.OnLookInput -= HandleLookInput;
    }

    private void HandleLookInput(Vector2 mouseDelta)
    {
        rotationX -= mouseDelta.y * lookSpeedY * Time.deltaTime;
        rotationX = Mathf.Clamp(rotationX, -55f, 55f);

        cameraTransform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);

        if (playerBody != null)
        {
            float yRotation = playerBody.eulerAngles.y + (mouseDelta.x * lookSpeedX * Time.deltaTime);
            playerBody.rotation = Quaternion.Euler(playerBody.eulerAngles.x, yRotation, playerBody.eulerAngles.z);
        }
    }
}