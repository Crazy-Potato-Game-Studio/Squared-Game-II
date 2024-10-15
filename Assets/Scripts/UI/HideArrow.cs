using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideArrow : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    [SerializeField] private GameObject arrow;

    private void Start() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.InGameMenu.performed += HideArrows;
    }

    private void HideArrows(InputAction.CallbackContext context){
        arrow.SetActive(false);
    }

    private void OnDestroy() {
        playerInputActions.Player.InGameMenu.performed -= HideArrows;
        playerInputActions.Player.Disable();
        playerInputActions.Disable();
    }
    
}
