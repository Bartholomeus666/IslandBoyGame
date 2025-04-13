using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private InventoryItem featherPen;
    [SerializeField] private InventoryItem potion;

    private void Start()
    {
        inventoryManager.PlaceItem(featherPen, 0, 0);
        inventoryManager.PlaceItem(potion, 2, 2);
    }
}
