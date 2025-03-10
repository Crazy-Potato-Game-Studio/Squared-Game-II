using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownElement : MonoBehaviour
{
    [SerializeField] private GameObject previousElement;
    public GameObject subElementsList;
    private RectTransform rectTransform;
    private bool selected = false;
    [SerializeField] private GameObject dropdownManager;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetPosition(){
        if(previousElement){
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, previousElement.GetComponent<RectTransform>().localPosition.y - (previousElement.GetComponent<RectTransform>().sizeDelta.y + 10));
        }else{
            rectTransform.localPosition = new Vector2(rectTransform.localPosition.x, -15f);
        }
    }

    public void ChangeHeight(){
        selected = !selected;

        if(selected){
            subElementsList.SetActive(true);
            StartCoroutine("ChangeCategorySize");
        }else{
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 45);
            subElementsList.SetActive(false);
            dropdownManager.GetComponent<DropdownManager>().SetElements();
        }

        source.PlayOneShot(clip);
    }

    private IEnumerator ChangeCategorySize(){
        yield return new WaitForSeconds(0.05f);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, subElementsList.GetComponent<RectTransform>().sizeDelta.y + rectTransform.sizeDelta.y);
        dropdownManager.GetComponent<DropdownManager>().SetElements();
    }
}
