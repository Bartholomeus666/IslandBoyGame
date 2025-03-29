using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    [SerializeField] private string itemName = "New Item";
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int width = 1;
    [SerializeField] private int height = 1;
    [SerializeField, TextArea(3, 10)] private string shape = "1"; // Default is a single cell
    // Shape pattern as a 2D array for easier access
    private bool[,] _shapePattern;
    // Properties
    public string ItemName => itemName;
    public GameObject ItemPrefab => itemPrefab;
    public int Width => width;
    public int Height => height;
    public string Shape => shape;
    // Method to get the shape pattern as a 2D array
    public bool[,] GetShapePattern()
    {
        // Only parse the shape if it hasn't been parsed yet or if the shape string has changed
        if (_shapePattern == null || _shapePattern.GetLength(0) != height || _shapePattern.GetLength(1) != width)
        {
            ParseShape();
        }
        return _shapePattern;
    }
    // Parse the shape string into a 2D array
    private void ParseShape()
    {
        // Initialize the shape pattern array
        _shapePattern = new bool[height, width];
        // Clean up the shape string (remove spaces, newlines, etc.)
        string cleanShape = shape.Replace(" ", "").Replace("\n", "").Replace("\r", "");
        // Ensure the shape string is valid
        if (string.IsNullOrEmpty(cleanShape))
        {
            // Default to a single cell if shape is invalid
            _shapePattern[0, 0] = true;
            return;
        }
        // Parse the shape string
        int index = 0;
        for (int y = 0; y < height && index < cleanShape.Length; y++)
        {
            for (int x = 0; x < width && index < cleanShape.Length; x++)
            {
                // Set the cell based on the character in the shape string
                _shapePattern[y, x] = cleanShape[index] == '1';
                index++;
            }
        }
        // Validate/adjust dimensions based on actual shape
        ValidateShapeDimensions();
    }
    // Optional: Validate and adjust dimensions based on the actual shape
    private void ValidateShapeDimensions()
    {
        // This method could be expanded to automatically adjust width/height
        // based on the provided shape, if needed
    }
    // Helper method to check if a specific cell in the shape is occupied
    public bool IsCellOccupied(int localX, int localY)
    {
        if (localX < 0 || localX >= width || localY < 0 || localY >= height)
            return false;
        return GetShapePattern()[localY, localX];
    }
    // Utility method for editor to visualize the shape
    public void OnValidate()
    {
        // Ensure width and height are at least 1
        width = Mathf.Max(1, width);
        height = Mathf.Max(1, height);
        // Optionally update shape string if width/height changed drastically
        // This is just a safety check for the editor
        string[] lines = shape.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        if (lines.Length > 0)
        {
            int maxLineLength = 0;
            foreach (string line in lines)
            {
                maxLineLength = Mathf.Max(maxLineLength, line.Replace(" ", "").Length);
            }
            // If shape is much smaller than specified dimensions, pad it
            if (lines.Length < height || maxLineLength < width)
            {
                // This is optional and only needed if you want to auto-adjust the shape text
                // when dimensions change in the editor
            }
        }
    }
}