using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ImageHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image image;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.gray;

    void Start()
    {
        image = GetComponent<Image>();
        image.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = normalColor;
    }
}