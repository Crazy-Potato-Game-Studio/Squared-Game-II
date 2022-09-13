using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public GameObject pasekZycia;
    public float playerHealth;
    public float maxPlayerHealth;

    void Start() {
        pasekZycia.GetComponent<Slider>().value = maxPlayerHealth;
        playerHealth = maxPlayerHealth;
    }

    void LoseHealth(int healthPoints){
        playerHealth-= healthPoints;
        pasekZycia.GetComponent<Slider>().value = playerHealth;
    }

    void GainHealth(int healthPoints){
        playerHealth+= healthPoints;
        pasekZycia.GetComponent<Slider>().value = playerHealth;
        if(playerHealth>10) playerHealth = 10;
    }


}
