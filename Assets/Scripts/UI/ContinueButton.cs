using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using TMPro;

public class ContinueButton : MonoBehaviour
{
    public static bool playerStartedGame = false;
    [SerializeField] private GameObject eventTrigger;
    [SerializeField] private TMP_Text text;

    public void PlayerPressedStart(){
        playerStartedGame = true;
    }

    private void Awake() {
        string path = Application.persistentDataPath + "/player.potato";
        if(playerStartedGame || File.Exists(path)){
            GetComponent<Button>().enabled = true;
            eventTrigger.SetActive(true);
            text.color = new Color(1f,1f,1f);
        }else{
            GetComponent<Button>().enabled = false;
            eventTrigger.SetActive(false);
            text.color = new Color(0.5f,0.5f,0.5f);
        }
    }
}
