using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    public GameObject arrow;
    public AudioSource source;
    public AudioClip clip;
    public bool isSelected;
    private PlayerInputActions playerInputActions;
    [SerializeField] private GameObject menuController;

    private void Start() {
        arrow.SetActive(false);
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.UseSelectedButton.performed += UseButton;
    }

    public void ShowArrow(){
        if(!UsedDevice.usingGamepad){
            arrow.SetActive(true);
        }
    }

    public void PlaySoundOnMouseHover(){
        if(!UsedDevice.usingGamepad){
            source.PlayOneShot(clip);
        }
    }

    public void UseButton(InputAction.CallbackContext context){
        if(isSelected){
            if(GetComponent<Button>() && GetComponent<EventTrigger>()){
                GetComponent<Button>().Select();
            }else if(GetComponent<ContinueButton>()){
                menuController.GetComponent<MenuController>().Continue(); 
            }
        }
    }

    private void OnDestroy() {
        playerInputActions.Player.UseSelectedButton.performed -= UseButton;
        playerInputActions.Player.Disable();
        playerInputActions = null;
    }
}
