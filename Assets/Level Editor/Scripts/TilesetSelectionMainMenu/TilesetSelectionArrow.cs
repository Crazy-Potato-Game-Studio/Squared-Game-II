using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TilesetSelectionArrow : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tilesetSelection;
    [SerializeField] private int num;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private Image arrowImage;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerClick(PointerEventData pointerEventData){
        tilesetSelection.GetComponent<TilesetSelection>().IncrementOrDecrementSelectedTilesetIndex(num);
        audioSource.PlayOneShot(audioClip);
    }

    public void OnPointerEnter(PointerEventData pointerEventData){
        arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.r, arrowImage.color.r, 0.5f);
    }

    public void OnPointerExit(PointerEventData pointerEventData){
        arrowImage.color = new Color(arrowImage.color.r, arrowImage.color.r, arrowImage.color.r, 1);
    }
}
