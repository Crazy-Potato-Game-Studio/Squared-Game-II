using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ContinueButton : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject continueText;

    public static bool playerStartedGame = false;

    public void PlayerPressedStart(){
        playerStartedGame = true;
    }

    private void Awake() {
        string path = Application.persistentDataPath + "/player.potato";
        if(playerStartedGame || File.Exists(path)){
            continueText.SetActive(false);
            continueButton.SetActive(true);
        }
    }
}
