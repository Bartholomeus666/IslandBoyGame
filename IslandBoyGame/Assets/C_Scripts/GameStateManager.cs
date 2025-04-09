using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager instance {  get; private set; }
    public bool IsInventoryOpen { get; private set; }
    [SerializeField] private GameObject inventoryUI;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ToggleInventory()
    {
        IsInventoryOpen = !IsInventoryOpen;

        if (inventoryUI != null)
        {
            inventoryUI.SetActive(IsInventoryOpen);
        }

        Cursor.visible = IsInventoryOpen;

        if (IsInventoryOpen)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
