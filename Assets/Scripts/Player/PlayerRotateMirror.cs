using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotateMirror : MonoBehaviour
{
    public GameObject mirror;

    public void GetInputLeft(InputAction.CallbackContext context){
        if(context.performed && mirror){
            mirror.GetComponent<MirrorRotate>().rotateLeft = true;
        }else if(context.canceled && mirror){
            mirror.GetComponent<MirrorRotate>().rotateLeft = false;
        }
    }

    public void GetInputRight(InputAction.CallbackContext context){
        if(context.performed && mirror){
            mirror.GetComponent<MirrorRotate>().rotateRight = true;
        }else if(context.canceled && mirror){
            mirror.GetComponent<MirrorRotate>().rotateRight = false;
        }
    }
}
