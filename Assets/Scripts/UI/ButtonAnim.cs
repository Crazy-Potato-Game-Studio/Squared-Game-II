using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnim : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void DoAnimation(){
        SetBool(true);
    }

    private void SetBool(bool isHovered){
        animator.SetBool("Hovered", isHovered);
    }

    public void DoGoBackAnimation(){
        SetBool(false);
    }

}
