using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public GameObject pasekZycia;
    private float playerHealth;
    [SerializeField] private float maxPlayerHealth;

    void Start() {
        pasekZycia.GetComponent<Slider>().maxValue = maxPlayerHealth;
        pasekZycia.GetComponent<Slider>().value = maxPlayerHealth;
        playerHealth = maxPlayerHealth;
    }

    public void LoseHealth(float healthPoints){
        playerHealth-= healthPoints;
        pasekZycia.GetComponent<Slider>().value = playerHealth;
        if(playerHealth < 0){
            playerHealth = 0;
        }
    }

    public void GainHealth(float healthPoints){
        playerHealth+= healthPoints;
        pasekZycia.GetComponent<Slider>().value = playerHealth;
        if(playerHealth>10) playerHealth = 10;
    }
}
