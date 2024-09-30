using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UsedDevice : MonoBehaviour
{

    private void Update() {
        if(Gamepad.current != null){
           // Debug.Log("Gamepad connected");
        }
    }
}
