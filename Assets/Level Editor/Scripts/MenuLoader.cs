using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuLoader : MonoBehaviour
{
    private PlayerInputActions playerInputActions;
    [SerializeField] private GameObject inGameMenu;
    private bool gamePaused;

    private void Start() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.InGameMenu.performed += PauseGameEditor;
    }

    public void PauseGameEditor(InputAction.CallbackContext context){
        
        if(context.performed){
            if(SceneManager.GetActiveScene().buildIndex != 0 && SceneManager.GetActiveScene().buildIndex != 1){
                if(gamePaused){
                    inGameMenu.GetComponent<InGameMenu>().HideInGameMenu();
                    Time.timeScale = 1;
                    Debug.Log("Hide UI");
                    gamePaused = false;
                }else{         
                    Time.timeScale = 0;
                    inGameMenu.GetComponent<InGameMenu>().ShowInGameMenu();
                    Debug.Log("Show UI");
                    gamePaused = true;
                }
            }
        }

        Debug.Log(Time.timeScale);
    }

    private void OnDestroy() {
        playerInputActions.Player.InGameMenu.performed -= PauseGameEditor;
        playerInputActions.Player.Disable();
        playerInputActions.Disable();
    }
}
