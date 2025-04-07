using UnityEngine;
using System.Collections.Generic;

public class InventoryGrid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private int gridWidth = 5;
    [SerializeField] private int gridHeight = 5;
    [SerializeField] private float cellSize = 100f;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Vector2 startPosition;

    private Vector2 _resolution;

    private InventoryCell[,] grid;
    private Dictionary<InventoryItem, List<Vector2Int>> itemOccupiedCells = new Dictionary<InventoryItem, List<Vector2Int>>();

    public int Width => gridWidth;
    public int Height => gridHeight;
    public float CellSize => cellSize;

    void Start()
    {

        //startPosition = new Vector2(startPosition.x + cellSize / 2, startPosition.y - cellSize / 2);

        _resolution = new Vector2(Screen.width / 1920, Screen.height / 1080);

        cellSize *= (1+_resolution.x);
        //NAND
        startPosition = new Vector2(cellSize / 2, Screen.height - cellSize / 2);

        GenerateInventoryGrid();
    }

    void GenerateInventoryGrid()
    {
        grid = new InventoryCell[gridWidth, gridHeight];
        if (gridParent == null)
            gridParent = transform;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector2 position;

                position.x = startPosition.x + x * cellSize;
                position.y = startPosition.y - y * cellSize;

                GameObject cellObject = Instantiate(cellPrefab, position, Quaternion.identity, gridParent);
                cellObject.name = $"Cell_{x}_{y}";

                InventoryCell cell = cellObject.GetComponent<InventoryCell>();
                if (cell == null)
                    cell = cellObject.AddComponent<InventoryCell>();

                cell.Initialize(x, y, false, this);  // Pass grid reference
                grid[x, y] = cell;
            }
        }
    }

    // Find the best cell position for an item
    public Vector2Int GetCellPositionFromWorldPoint(Vector3 worldPosition)
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                InventoryCell cell = grid[x, y];
                if (cell != null)
                {
                    Vector3 cellPos = cell.transform.position;
                    // Check if the position is close to this cell
                    float distance = Vector2.Distance(new Vector2(worldPosition.x, worldPosition.y),
                                                       new Vector2(cellPos.x, cellPos.y));
                    if (distance < cellSize * 0.5f)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
        }

        // If we couldn't find a close cell, find the closest one
        float closestDistance = float.MaxValue;
        Vector2Int closestCell = new Vector2Int(0, 0);

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                InventoryCell cell = grid[x, y];
                if (cell != null)
                {
                    Vector3 cellPos = cell.transform.position;
                    float distance = Vector2.Distance(new Vector2(worldPosition.x, worldPosition.y),
                                                       new Vector2(cellPos.x, cellPos.y));
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestCell = new Vector2Int(x, y);
                    }
                }
            }
        }

        return closestCell;
    }

    // Check if an item can be placed at the specified position
    public bool CanPlaceItem(int x, int y, InventoryItem item)
    {
        if (item == null || item.ItemData == null)
            return false;

        InventoryItemData itemData = item.ItemData;
        bool[,] shapePattern = itemData.GetShapePattern();

        // Check each cell that the item would occupy
        for (int localY = 0; localY < itemData.Height; localY++)
        {
            for (int localX = 0; localX < itemData.Width; localX++)
            {
                // Skip cells that are not part of the item's shape
                if (!shapePattern[localY, localX])
                    continue;

                int gridX = x + localX;
                int gridY = y + localY;

                // Check if this cell is within the grid bounds
                if (!IsValidPosition(gridX, gridY))
                    return false;

                // Check if this cell is already in use
                InventoryCell cell = grid[gridX, gridY];
                if (cell.IsInUse && cell.CurrentItem != item)
                    return false;
            }
        }

        return true;
    }

    // Place an item in the grid
    public bool PlaceItem(int x, int y, InventoryItem item)
    {
        if (!CanPlaceItem(x, y, item))
            return false;

        // Ensure proper scale before placing
        item.transform.localScale = Vector3.one;

        InventoryItemData itemData = item.ItemData;
        bool[,] shapePattern = itemData.GetShapePattern();
        List<Vector2Int> occupiedCells = new List<Vector2Int>();

        // Place the item in the main cell (this will position the item correctly)
        grid[x, y].SetItem(item);
        occupiedCells.Add(new Vector2Int(x, y));

        // Mark additional cells as occupied
        for (int localY = 0; localY < itemData.Height; localY++)
        {
            for (int localX = 0; localX < itemData.Width; localX++)
            {
                // Skip the main cell (0,0) as it's already set
                if (localX == 0 && localY == 0)
                    continue;

                // Skip cells that are not part of the item's shape
                if (!shapePattern[localY, localX])
                    continue;

                int gridX = x + localX;
                int gridY = y + localY;

                // Mark this cell as in use (but without the actual item reference)
                grid[gridX, gridY].MarkAsOccupied(true);
                occupiedCells.Add(new Vector2Int(gridX, gridY));
            }
        }

        // Store the list of cells this item occupies
        itemOccupiedCells[item] = occupiedCells;

        return true;
    }

    // Remove an item from the grid
    public InventoryItem RemoveItem(int x, int y)
    {
        if (!IsValidPosition(x, y))
            return null;

        InventoryCell cell = grid[x, y];
        if (!cell.IsInUse)
            return null;

        InventoryItem item = cell.GetItem();
        if (item == null)
            return null;

        // Check if this item is in our dictionary
        if (itemOccupiedCells.TryGetValue(item, out List<Vector2Int> occupiedCells))
        {
            // Clear all occupied cells
            foreach (Vector2Int pos in occupiedCells)
            {
                if (IsValidPosition(pos.x, pos.y))
                {
                    if (pos.x == x && pos.y == y)
                        grid[pos.x, pos.y].ClearCell(); // Main cell with the item
                    else
                        grid[pos.x, pos.y].MarkAsOccupied(false); // Additional cells
                }
            }

            // Remove from dictionary
            itemOccupiedCells.Remove(item);
        }
        else
        {
            // Fallback if item is not in dictionary (should not happen)
            cell.ClearCell();
        }

        return item;
    }

    // Check if position is valid
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    // Get a cell at the specified position
    public InventoryCell GetCell(int x, int y)
    {
        if (!IsValidPosition(x, y))
            return null;
        return grid[x, y];
    }

    // Check if a cell is in use
    public bool IsCellInUse(int x, int y)
    {
        InventoryCell cell = GetCell(x, y);
        return cell != null && cell.IsInUse;
    }

    // Get all cells that an item occupies
    public List<Vector2Int> GetItemOccupiedCells(InventoryItem item)
    {
        if (item != null && itemOccupiedCells.TryGetValue(item, out List<Vector2Int> cells))
            return cells;

        return new List<Vector2Int>();
    }

    // Find an item within the grid
    public Vector2Int? FindItemPosition(InventoryItem item)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                InventoryCell cell = grid[x, y];
                if (cell != null && cell.CurrentItem == item)
                {
                    return new Vector2Int(x, y);
                }
            }
        }
        return null;
    }

    // Visualize the grid (for debugging)
    public void VisualizeGrid()
    {
        for (int y = 0; y < gridHeight; y++)
        {
            string row = "";
            for (int x = 0; x < gridWidth; x++)
            {
                row += IsCellInUse(x, y) ? "1" : "0";
            }
            Debug.Log(row);
        }
    }
}