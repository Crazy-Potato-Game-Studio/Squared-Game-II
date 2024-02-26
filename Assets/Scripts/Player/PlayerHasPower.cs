using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHasPower : MonoBehaviour
{
    private GameObject PowerUI;
    public bool playerHasPowerPickup;
    
    private void Awake() {
        PowerUI = GameObject.FindGameObjectWithTag("PowerUI");
    }

    public void playerPickedUpPower(){
        playerHasPowerPickup = true;
        PowerUI.GetComponent<Image>().enabled = true;
    }

    public void PlayerLeftPower(){
        playerHasPowerPickup = false;
        PowerUI.GetComponent<Image>().enabled = false;
    }
}
