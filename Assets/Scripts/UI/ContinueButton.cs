using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject continueText;

    public static bool playerStartedGame = false;

    public void PlayerPressedStart(){
        playerStartedGame = true;
    }

    private void Awake() {
        if(playerStartedGame){
            continueText.SetActive(false);
            continueButton.SetActive(true);
        }
    }
}
