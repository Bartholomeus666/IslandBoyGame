using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventoryItem featherPen;
    [SerializeField] private InventoryItem potion;
    [SerializeField] private InventoryItem tetrisL; // L-shaped Tetris piece

    private void Start()
    {
        inventoryManager.PlaceItem(featherPen, 0, 0);
        inventoryManager.PlaceItem(potion, 3, 3);
        inventoryManager.PlaceItem(tetrisL, 0, 2);
    }
}