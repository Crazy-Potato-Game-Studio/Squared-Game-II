using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnOnGenerator : MonoBehaviour
{
    public bool playerInRange;

    public void GetInput(InputAction.CallbackContext context){
        if(context.performed){
            if(playerInRange && GetComponent<PlayerHasPower>().playerHasPowerPickup){
                GameObject.FindGameObjectWithTag("Generator").GetComponent<PowerGenerator>().GeneratorOn();
            }
        }
    }
}
