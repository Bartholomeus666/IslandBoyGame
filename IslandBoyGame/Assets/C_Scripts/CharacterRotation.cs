using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterRotation : MonoBehaviour
{
    [SerializeField] private float lookSpeedX = 2.0f; // Rotation speed on the X-axis
    [SerializeField] private float lookSpeedY = 2.0f; // Rotation speed on the Y-axis
    //[SerializeField] private Transform playerBody; // Reference to the player's body (for rotating horizontally)

    private float rotationX = 0f; // Current X rotation of the camera
    private float rotationY = 0f; // Current Y rotation of the camera

    // Update is called once per frame
    //void Update()
    //{
    //    // Get mouse movement input (delta)
    //    Vector2 mouseDelta = lookAction.ReadValue<Vector2>();

    //    // Rotate the camera based on mouse movement
    //    rotationX -= mouseDelta.y * lookSpeedY; // Invert the vertical mouse movement
    //    rotationY += mouseDelta.x * lookSpeedX;

    //    // Clamp the vertical rotation to avoid flipping
    //    rotationX = Mathf.Clamp(rotationX, -90f, 90f);

    //    // Apply the rotation to the camera
    //    transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

    //    // Rotate the player body (if provided)
    //    if (playerBody != null)
    //    {
    //        playerBody.rotation = Quaternion.Euler(0f, rotationY, 0f); // Rotate the player horizontally
    //    }
    //}

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void LookAction(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();

        rotationX -= mouseDelta.y * lookSpeedY * Time.deltaTime; 
        rotationY += mouseDelta.x * lookSpeedX * Time.deltaTime;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

        //if (playerBody != null)
        //{
        //    playerBody.rotation = Quaternion.Euler(0f, rotationY, 0f);
        //}
    }
    private void Update()
    {
        Debug.Log(rotationX);
    }
}

