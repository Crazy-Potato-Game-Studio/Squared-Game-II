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
        pasekMany.GetComponent<Slider>().value = maxPlayerMana;
        playerMana = maxPlayerMana;
    }

    void LoseHealth(int healthPoints){
        playerMana-= healthPoints;
        pasekMany.GetComponent<Slider>().value = playerMana;
    }

    void GainHealth(int healthPoints){
        playerMana+= healthPoints;
        pasekMany.GetComponent<Slider>().value = playerMana;
        if(playerMana>10) playerMana = 10;
    }

    private void Update() {
      pasekMany.GetComponent<Slider>().value = playerMana;  
    }


}
