using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UnoReverseCard : MonoBehaviour
{
    public static bool hasUnoReverseCard;
    private PlayerInputActions playerInputActions;
    private bool playerPressedButton;

    private void Awake() {
        if(hasUnoReverseCard){
            AddUnoCard();
        }

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.UnoReverseCard.performed += UseReverseCard;
        playerInputActions.Player.UnoReverseCard.started += PlayerPressedButton;
    }

    public void AddUnoCard(){
        GameObject unoCard = GameObject.FindGameObjectWithTag("UnoReverse");
        unoCard.GetComponent<Image>().enabled = true;
        unoCard.transform.GetChild(0).GetComponent<Image>().enabled = true;
        GameObject SteamAchievementsManager = GameObject.FindGameObjectWithTag("MainCamera");
        SteamAchievementsManager.GetComponent<SteamAchievementsManager>().UnlockAchievement("reverse_ach");
    }

    private void PlayerPressedButton(InputAction.CallbackContext context){
        playerPressedButton = true;
    }

    private void UseReverseCard(InputAction.CallbackContext context){
        if(hasUnoReverseCard && playerPressedButton){
            Physics2D.gravity *= -1;
            playerPressedButton = false;
        }
    }

    private void OnDestroy() {
        playerInputActions.Player.UnoReverseCard.performed -= UseReverseCard;
        playerInputActions.Player.UnoReverseCard.started -= PlayerPressedButton;
        playerInputActions.Player.Disable();
        playerInputActions.Disable();
    }
}
