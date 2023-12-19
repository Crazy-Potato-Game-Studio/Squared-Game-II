using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthManager : MonoBehaviour
{
    public GameObject pasekZycia;
    private float playerHealth;
    [SerializeField] private float maxPlayerHealth;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;
    public GameObject hurtText;
    public GameObject lavaText;
    public GameObject floatingLava;
    public float sumOfLavaDamage = 0;
    [SerializeField] private TextMeshProUGUI healthText;

    void Start() {
        pasekZycia.GetComponent<Slider>().maxValue = maxPlayerHealth;
        playerHealth = maxPlayerHealth;
        UpdateSliderValue();
        UpdateHealthText();
    }

    public void LoseHealth(float healthPoints){

        playerHealth-= healthPoints;
        GetComponent<EntityChangeColor>().ChangeColor();
        UpdateSliderValue();

        if(playerHealth < 0){
            playerHealth = 0;
        }

        source.PlayOneShot(clip);
        FlyingDamage(healthPoints); 

        UpdateHealthText();
    }

    public void GainHealth(float healthPoints){
        playerHealth+= healthPoints;
        UpdateSliderValue();
        if(playerHealth > 10) playerHealth = 10;
        UpdateHealthText();
    }

     void FlyingDamage(float damage){
        GameObject floatingPoint =  Instantiate(hurtText, transform.position + new Vector3(0,2f,0), transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }

    public void StartLavaDamage(){
        InvokeRepeating("LavaDamage", 0, 0.2f);
        floatingLava =  Instantiate(lavaText, transform.position + new Vector3(0,1f,0), transform.rotation);
        floatingLava.transform.parent = this.transform;
    }

    public void LavaDamage(){
        playerHealth -= 1f;
        sumOfLavaDamage += 1f;
        floatingLava.GetComponent<TextMeshPro>().text = "- " + sumOfLavaDamage.ToString();
        GetComponent<EntityChangeColor>().ChangeColor();
        UpdateSliderValue();
        UpdateHealthText();
        if(playerHealth < 0){
            playerHealth = 0;
        }
    }

    public void StopLavaDamage(){
        sumOfLavaDamage = 0;
        CancelInvoke();
        Destroy(floatingLava);
    }

    private void UpdateHealthText(){
        healthText.text = playerHealth.ToString() + "/" + maxPlayerHealth.ToString();  
    }

    private void UpdateSliderValue(){
        pasekZycia.GetComponent<Slider>().value = playerHealth;
    }
}
