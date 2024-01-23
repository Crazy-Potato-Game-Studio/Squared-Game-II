using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlatform : MonoBehaviour
{
    public bool isClosedInit;
    public bool isOn;
    [SerializeField] private Animator animator;

    private void Awake() {
        if(isClosedInit){
            animator.Play("MetalPlatformClosing");
        }else{
            animator.Play("MetalPlatformOpening");
        }
    }

    void Update()
    {
        if(isClosedInit){
            if(isOn){
                animator.Play("MetalPlatformOpening");
            }else{
                animator.Play("MetalPlatformClosing");
            }
        }else{
            if(isOn){
                animator.Play("MetalPlatformClosing");
            }else{
                animator.Play("MetalPlatformOpening");
            }
        }
    }
}
