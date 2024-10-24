using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CreateButtonTransition : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI text;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.6f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0.6f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1f);
    }
}
