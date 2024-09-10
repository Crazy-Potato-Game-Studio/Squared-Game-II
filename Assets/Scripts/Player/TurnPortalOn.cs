using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnPortalOn : MonoBehaviour
{
    public bool playerPressedE;
    public bool playerHasTeleported;

    public void GetInput(InputAction.CallbackContext context){
        if(!playerHasTeleported && context.performed){
            playerPressedE = true;
        }else if(context.canceled){
            playerPressedE = false;
            playerHasTeleported = false;
        }
    }
}
