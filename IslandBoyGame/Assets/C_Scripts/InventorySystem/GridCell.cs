using System;
using UnityEngine;
using UnityEngine.UI;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Sprite cellSprite;
    [SerializeField] private Sprite occupiedCellSprite;
    [SerializeField] private Sprite placementPreviewSprite; // New texture for placement preview

    private bool isOccupied = false;
    private bool isPlacementPreview = false;
    private int gridX;
    private int gridY;

    public bool IsOccupied => isOccupied;
    public int GridX => gridX;
    public int GridY => gridY;

    public void Initialize(int x, int y)
    {
        gridX = x;
        gridY = y;
        backgroundImage = GetComponent<Image>();
        UpdateVisual();
    }

    public void SetOccupied(bool occupied)
    {
        isOccupied = occupied;
        UpdateVisual();
    }

    public void SetPlacementPreview(bool preview)
    {
        isPlacementPreview = preview;
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (backgroundImage != null)
        {
            if (isOccupied)
            {
                backgroundImage.sprite = occupiedCellSprite;
            }
            else if (isPlacementPreview)
            {
                backgroundImage.sprite = placementPreviewSprite;
            }
            else
            {
                backgroundImage.sprite = cellSprite;
            }
        }
    }
}