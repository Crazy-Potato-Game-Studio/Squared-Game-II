using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotateMirror : MonoBehaviour
{
    public GameObject mirror;
    private PlayerInputActions playerInputActions;

    private void Start() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void FixedUpdate() {
        if(mirror){
            if(playerInputActions.Player.MirrorLeft.ReadValue<float>() == 1){
                mirror.GetComponent<MirrorRotate>().rotateLeft = true;    
            }else{
                mirror.GetComponent<MirrorRotate>().rotateLeft = false;   
            }
            
            if(playerInputActions.Player.MirrorRight.ReadValue<float>() == 1){
                mirror.GetComponent<MirrorRotate>().rotateRight = true;
            }else{
                mirror.GetComponent<MirrorRotate>().rotateRight = false;
            }
        }
    }

    private void OnDestroy() {
        playerInputActions.Player.Disable();
        playerInputActions = null;
    }
}
