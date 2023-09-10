using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    public GameObject pasekMany;
    public float playerMana;
    public float maxPlayerMana;

    void Start() {
        pasekMany.GetComponent<Slider>().maxValue = maxPlayerMana;
        pasekMany.GetComponent<Slider>().value = maxPlayerMana;
        playerMana = maxPlayerMana;
    }

    public void LoseMana(float manaPoints){
        playerMana -= manaPoints;
        pasekMany.GetComponent<Slider>().value = playerMana;
    }

    public void GainMana(float manaPoints){
        playerMana += manaPoints;
        pasekMany.GetComponent<Slider>().value = playerMana;
        if(playerMana>10) playerMana = 10;
    }

    private void Update() {
      pasekMany.GetComponent<Slider>().value = playerMana;  
    }


}
