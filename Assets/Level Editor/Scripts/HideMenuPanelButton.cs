using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HideMenuPanelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Image image;
    private Color defaultColor;
    [SerializeField] private GameObject manuPanel;
    private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private GameObject arrowImage;

    private void Awake() {
        source = GetComponent<AudioSource>();
        image = GetComponent<Image>();
        defaultColor = image.color;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        image.color = new Color(0.13f, 0.13f, 0.13f);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        image.color = defaultColor;
    }

    public void OnPointerClick(PointerEventData pointerEventData){
        manuPanel.GetComponent<HideMenuPanel>().MovePanel();
        arrowImage.transform.localScale = new Vector2(arrowImage.transform.localScale.x * -1, 1);
        source.PlayOneShot(clip);
    }
}
