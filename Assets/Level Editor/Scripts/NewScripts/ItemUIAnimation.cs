using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemUIAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private Animator animator;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        source.PlayOneShot(clip);
        animator.Play("EditorItemAnim");
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        animator.Play("EditorItemAnimIdle");
    }
}
