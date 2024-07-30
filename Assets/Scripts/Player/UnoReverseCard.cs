using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnoReverseCard : MonoBehaviour
{
    public static bool hasUnoReverseCard;

    private void Awake() {
        if(hasUnoReverseCard){
            AddUnoCard();
        }
    }

    public void AddUnoCard(){
        GameObject unoCard = GameObject.FindGameObjectWithTag("UnoReverse");
        unoCard.GetComponent<Image>().enabled = true;
        unoCard.transform.GetChild(0).GetComponent<Image>().enabled = true;
    }

    private void Update() {
        if(hasUnoReverseCard){
            if(Input.GetKeyDown(KeyCode.X)){
                Physics2D.gravity *= -1;
            }
        }
    }
}
