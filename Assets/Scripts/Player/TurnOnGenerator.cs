using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TurnOnGenerator : MonoBehaviour
{
    public bool playerInRange;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Interactions.performed += TurnGenerator;
    }

    public void TurnGenerator(InputAction.CallbackContext context){
        if(context.performed){
            if(playerInRange && GetComponent<PlayerHasPower>().playerHasPowerPickup){
                GameObject.FindGameObjectWithTag("Generator").GetComponent<PowerGenerator>().GeneratorOn();
            }
        }
    }

    private void OnDestroy() {
        playerInputActions.Player.Interactions.performed -= TurnGenerator;
        playerInputActions.Player.Disable();
        playerInputActions.Disable();
    }

}
