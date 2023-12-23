using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private Animator animator;

    public void DoAnimation(){
        SetBool(true);
        PlaySound();
    }

    private void PlaySound(){
        source.PlayOneShot(clip);
    }

    private void SetBool(bool isHovered){
        animator.SetBool("Hovered", isHovered);
    }

    public void DoGoBackAnimation(){
        SetBool(false);
    }

}
