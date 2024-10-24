using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TilesetSelectionHint : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject hintText;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        hintText.SetActive(true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        hintText.SetActive(false);
    }
}
