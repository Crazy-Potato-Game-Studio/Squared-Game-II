using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StartButton : MonoBehaviour
{
    [SerializeField] private GameObject sceneManager;
    public bool isSelected;
    [SerializeField] private PlayerInputActions playerInputActions;

    private void Start() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.UseSelectedButton.performed += CallStartGame;
    }
    
    public void CallStartGame(InputAction.CallbackContext context){
        if(isSelected){
            sceneManager.GetComponent<MenuController>().StartGame();
        }
    }
}
