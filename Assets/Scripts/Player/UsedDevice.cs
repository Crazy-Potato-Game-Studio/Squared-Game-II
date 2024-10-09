using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UsedDevice : MonoBehaviour
{
    public static bool usingGamepad;

    private void Awake() {
        CheckForGamepad();
        InvokeRepeating("CheckForGamepad",0,1f);
    }

    private void CheckForGamepad(){
        if(Gamepad.current != null){
            usingGamepad = true;
            Cursor.visible = false;
        }else{
            usingGamepad = false;
            Cursor.visible = true;
        }
    }

}
