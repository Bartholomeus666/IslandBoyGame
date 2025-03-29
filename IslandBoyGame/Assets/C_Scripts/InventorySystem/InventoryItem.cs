using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private InventoryItemData itemData;
    private GameObject itemVisual; // Reference to the instantiated visual GameObject
    private Color currentColor = Color.white;

    // Properties
    public InventoryItemData ItemData => itemData;
    public int Width => itemData != null ? itemData.Width : 1;
    public int Height => itemData != null ? itemData.Height : 1;

    private void Awake()
    {
        // Add required components for UI dragging if they don't exist
        if (GetComponent<RectTransform>() == null)
        {
            gameObject.AddComponent<RectTransform>();
        }
    }

    // Initialize the item with the specified data
    public void Initialize(InventoryItemData data)
    {
        itemData = data;
        UpdateVisual();

        // Add drag handler if it doesn't exist
        if (GetComponent<InventoryItemDragHandler>() == null)
        {
            gameObject.AddComponent<InventoryItemDragHandler>();
        }
    }

    // Update the visual appearance
    // Update the visual appearance
    private void UpdateVisual()
    {
        // Remove any existing visual
        if (itemVisual != null)
        {
            Destroy(itemVisual);
        }

        if (itemData != null && itemData.ItemPrefab != null)
        {
            // Instantiate the prefab as a child of this object
            itemVisual = Instantiate(itemData.ItemPrefab, transform);

            // Reset local position to zero before adjusting
            itemVisual.transform.localPosition = Vector3.zero;

            // Fixed local scale - don't change it
            itemVisual.transform.localScale = Vector3.one;

            // Apply item positioning without scaling
            AdjustVisualPosition();

            // Make sure it has no colliders that might interfere with UI/interaction
            Collider[] colliders = itemVisual.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
            {
                collider.enabled = false;
            }

            // Set the name based on item data
            gameObject.name = itemData.ItemName;

            // Apply the current color
            SetColor(currentColor);
        }
    }
    private void AdjustVisualPosition()
    {
        if (itemVisual != null && itemData != null)
        {
            // Always maintain original scale
            transform.localScale = Vector3.one;
            itemVisual.transform.localScale = Vector3.one;

            // Get the RectTransform component
            RectTransform rectTransform = itemVisual.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                float cellSize = 100f; // This should match your grid's cell size

                // Set the size of the RectTransform to match the item's dimensions
                rectTransform.sizeDelta = new Vector2(cellSize * Width, cellSize * Height);

                // Center the visual in the item's grid area
                // Set the pivot to center
                rectTransform.pivot = new Vector2(0.5f, 0.5f);

                // Set anchors to the center
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);

                // Reset the position to the center of the item's grid space
                rectTransform.anchoredPosition = Vector2.zero;

                // Set the proper size on the main item's RectTransform as well
                RectTransform itemRectTransform = GetComponent<RectTransform>();
                if (itemRectTransform != null)
                {
                    // Set the size to match the item dimensions
                    itemRectTransform.sizeDelta = new Vector2(cellSize * Width, cellSize * Height);

                    // Set the pivot to center (this helps with proper positioning)
                    itemRectTransform.pivot = new Vector2(0.5f, 0.5f);
                }
            }
            else
            {
                // For non-UI objects, make sure they're centered
                itemVisual.transform.localPosition = Vector3.zero;
            }
        }
    }

    // Set the color of the visual
    public void SetColor(Color color)
    {
        currentColor = color;

        if (itemVisual != null)
        {
            // Try to find renderers to apply color
            Renderer[] renderers = itemVisual.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = color;
            }

            // Also try UI components if it's a UI-based prefab
            Graphic[] graphics = itemVisual.GetComponentsInChildren<UnityEngine.UI.Graphic>();
            foreach (UnityEngine.UI.Graphic graphic in graphics)
            {
                graphic.color = color;
            }
        }
    }

    // Check if a specific local cell is occupied by the item's shape
    public bool IsCellOccupied(int localX, int localY)
    {
        if (itemData == null)
            return false;

        return itemData.IsCellOccupied(localX, localY);
    }

    // Optional: Draw gizmos to visualize the item's shape in the editor
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (itemData != null && UnityEditor.Selection.activeGameObject == gameObject)
        {
            bool[,] shape = itemData.GetShapePattern();
            float cellSize = 100f; // This should match your grid's cell size

            for (int y = 0; y < itemData.Height; y++)
            {
                for (int x = 0; x < itemData.Width; x++)
                {
                    if (shape[y, x])
                    {
                        Gizmos.color = new Color(0, 1, 0, 0.5f); // Green for occupied cells
                    }
                    else
                    {
                        Gizmos.color = new Color(1, 0, 0, 0.2f); // Red for empty cells
                    }

                    Vector3 cellPos = transform.position + new Vector3(x * cellSize, -y * cellSize, 0);
                    Gizmos.DrawCube(cellPos, new Vector3(cellSize * 0.9f, cellSize * 0.9f, 1f));
                }
            }
        }
    }
#endif
}