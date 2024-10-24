using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TilesetImage : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image borderImage;
    [SerializeField] GameObject tilesetSelection;
    private Color greyColor = new Color(0.25f, 0.25f, 0.25f);
    public int tilesetIndex;
    public bool selected;
    private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if(!selected){
            borderImage.color = Color.white;
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if(!selected){
            borderImage.color = greyColor;
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData){
        tilesetSelection.GetComponent<TilesetSelection>().ChangeSelectedTileset(tilesetIndex);

    }
}
