using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Canvas _inventoryCanvas;
    private bool _isInventoryOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isInventoryOpen = !_isInventoryOpen;
            _inventoryCanvas.enabled = _isInventoryOpen;
        } 
    }
}
