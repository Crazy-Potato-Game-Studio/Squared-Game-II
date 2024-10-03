using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadMenuSelection : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    private int buttonNum;
    private PlayerInputActions playerInputActions;
    private bool canSelect = true;
    private int numberOfInactiveButtons = 0;

    private void Start() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        for(int i = 0; i <= buttons.Length - 1; i++){
            if(buttons[i] == null){
                if(i != buttons.Length - 1){
                    buttons[i] = buttons[i + 1];
                }else{
                    buttons[i] = buttons[0];
                }
            }
            if(buttons[i].GetComponent<Button>().enabled == false){
                numberOfInactiveButtons++;
                buttons[i] = buttons[i + 1];
                buttons[i + 1] = null;
            }
        }
    }

    private void Update() {
        if(canSelect && playerInputActions.Player.MenuSelectionDown.ReadValue<float>() != 0){
           if(playerInputActions.Player.MenuSelectionDown.ReadValue<float>() <= 0){
                if(buttonNum == buttons.Length - 1 - numberOfInactiveButtons){
                    buttons[buttonNum].GetComponent<ButtonSelect>().arrow.SetActive(false);
                    buttonNum = 0;
                }else{
                    buttons[buttonNum].GetComponent<ButtonSelect>().arrow.SetActive(false);
                    buttonNum += 1;
                }
           }else{
                if(buttonNum == 0){
                    buttons[buttonNum].GetComponent<ButtonSelect>().arrow.SetActive(false);
                    buttonNum = buttons.Length - 1 - numberOfInactiveButtons;
                }else{
                    buttons[buttonNum].GetComponent<ButtonSelect>().arrow.SetActive(false);
                    buttonNum -= 1;
                }
            }

            buttons[buttonNum].GetComponent<ButtonSelect>().arrow.SetActive(true);
            buttons[buttonNum].GetComponent<ButtonSelect>().source.PlayOneShot(buttons[buttonNum].GetComponent<ButtonSelect>().clip);
            StartCoroutine(Timer());
        }else if(playerInputActions.Player.MenuSelectionDown.ReadValue<float>() == 0){
            StopAllCoroutines();
            canSelect = true;
        }
    }

    private IEnumerator Timer(){
        canSelect = false;
        yield return new WaitForSeconds(0.2f);
        canSelect = true;
    }
}
