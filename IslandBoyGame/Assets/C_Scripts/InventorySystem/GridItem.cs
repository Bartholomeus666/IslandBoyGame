using UnityEngine;
using UnityEngine.UI;

public class GridItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    private InventoryItem itemData;
    public int gridX; // Changed to public for direct access during dragging
    public int gridY; // Changed to public for direct access during dragging
    private int rotationState = 0; // 0, 1, 2, 3 for 0, 90, 180, 270 degrees

    // Runtime representation of the shape after rotation
    private bool[,] rotatedShape;
    private int rotatedWidth;
    private int rotatedHeight;

    public InventoryItem ItemData => itemData;
    public int GridX => gridX;
    public int GridY => gridY;
    public int RotationState => rotationState;

    public int Width => rotatedWidth;
    public int Height => rotatedHeight;

    public void Initialize(InventoryItem item, int x, int y, GridGenerator gridGenerator)
    {
        itemData = item;
        gridX = x;
        gridY = y;

        // Initialize the rotated shape if needed
        if (rotatedShape == null)
        {
            InitializeRotatedShape();
        }

        // Set up the visual
        SetupVisual(gridGenerator);
    }

    private void InitializeRotatedShape()
    {
        rotatedWidth = itemData.width;
        rotatedHeight = itemData.height;

        rotatedShape = new bool[rotatedWidth, rotatedHeight];

        // Copy the original shape
        for (int x = 0; x < rotatedWidth; x++)
        {
            for (int y = 0; y < rotatedHeight; y++)
            {
                rotatedShape[x, y] = itemData.IsCellOccupied(x, y);
            }
        }
    }

    private void SetupVisual(GridGenerator gridGenerator)
    {
        itemImage = GetComponent<Image>();
        itemImage.sprite = itemData.itemSprite;

        RectTransform rectTransform = GetComponent<RectTransform>();
        float cellSize = gridGenerator.CellSize;
        Vector2 gridOffset = gridGenerator.GridOffset;

        rectTransform.sizeDelta = new Vector2(rotatedWidth * cellSize, rotatedHeight * cellSize);
        rectTransform.anchoredPosition = new Vector2(
            gridOffset.x - cellSize / 2 + gridX * cellSize,
            gridOffset.y + cellSize / 2 - gridY * cellSize
        );
        rectTransform.pivot = new Vector2(0, 1);
    }

    public bool IsCellOccupied(int relativeX, int relativeY)
    {
        if (rotatedShape == null)
        {
            InitializeRotatedShape();
        }

        if (relativeX >= 0 && relativeX < rotatedWidth && relativeY >= 0 && relativeY < rotatedHeight)
        {
            return rotatedShape[relativeX, relativeY];
        }
        return false;
    }

    // Regular rotation with collision checks
    public void Rotate90Clockwise(InventoryManager inventoryManager)
    {
        // Remove the current occupation
        inventoryManager.ClearItemOccupation(this);

        // Apply rotation
        ApplyRotation();

        // Try to place the item back with the new shape
        if (!inventoryManager.UpdateItemPlacement(this))
        {
            // If we can't place it back, revert the rotation
            RotateBackToOriginal();
        }
    }

    // Rotation during drag - no collision checks yet
    public void Rotate90ClockwiseDuringDrag()
    {
        ApplyRotation();
    }

    private void ApplyRotation()
    {
        // Calculate new dimensions
        int newWidth = rotatedHeight;
        int newHeight = rotatedWidth;

        // Create a new rotated shape
        bool[,] newShape = new bool[newWidth, newHeight];

        // Apply rotation: (x,y) -> (height-1-y, x)
        for (int x = 0; x < rotatedWidth; x++)
        {
            for (int y = 0; y < rotatedHeight; y++)
            {
                int newX = rotatedHeight - 1 - y;
                int newY = x;
                newShape[newX, newY] = rotatedShape[x, y];
            }
        }

        // Update the rotation state
        rotationState = (rotationState + 1) % 4;

        // Update dimensions and shape
        rotatedShape = newShape;
        rotatedWidth = newWidth;
        rotatedHeight = newHeight;
    }

    private void RotateBackToOriginal()
    {
        // Rotate three times to get back to original
        for (int i = 0; i < 3; i++)
        {
            ApplyRotation();
        }
    }

    public void Rotate90CounterClockwise(InventoryManager inventoryManager)
    {
        // Rotate three times clockwise to achieve counter-clockwise
        for (int i = 0; i < 3; i++)
        {
            Rotate90Clockwise(inventoryManager);
        }
    }
}