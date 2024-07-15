using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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
    private TextMeshProUGUI healthText;
    private IEnumerator coroutine;

    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private BoxCollider2D resistanceCollider;
    private bool isResistant;

    [SerializeField] private GameObject levelRestarterPrefab;

    [SerializeField] private GameObject audioListener;

    void Awake() {
        pasekZycia = GameObject.FindGameObjectWithTag("HealthUI");
        healthText = pasekZycia.GetComponentInChildren<TextMeshProUGUI>();
        pasekZycia.GetComponent<Slider>().maxValue = maxPlayerHealth;
        playerHealth = maxPlayerHealth;

        UpdateSliderValue();
        UpdateHealthText();
    }

    public void LoseHealth(float healthPoints, float resistanceTime){
        if(!isResistant){
            playerHealth-= healthPoints;
            GetComponent<EntityChangeColor>().ChangeColor(resistanceTime);
            UpdateSliderValue();

            if(playerHealth <= 0){
                PlayerDeath();
            }

            source.PlayOneShot(clip);
            FlyingDamage(healthPoints); 

            UpdateHealthText();
            coroutine = EnableResistanceCollider(resistanceTime);
            StartCoroutine(coroutine);
        }
        if(healthPoints == 100){
            playerHealth = 0;
            UpdateSliderValue();
            UpdateHealthText();
            FlyingDamage(healthPoints); 
            PlayerDeath();
            isResistant = true;
        }

    }

    public void GainHealth(float healthPoints){
        playerHealth+= healthPoints;
        UpdateSliderValue();
        if(playerHealth > maxPlayerHealth){
            playerHealth = maxPlayerHealth;
        } 
        UpdateHealthText();
    }

     void FlyingDamage(float damage){
        GameObject floatingPoint =  Instantiate(hurtText, transform.position + new Vector3(0,2f,0), transform.rotation);
        floatingPoint.GetComponent<TextMeshPro>().text = "- " + damage.ToString();

        GameObject.Destroy(floatingPoint, 0.5f);
    }

    private void UpdateHealthText(){
        healthText.text = playerHealth.ToString() + "/" + maxPlayerHealth.ToString();  
    }

    private void UpdateSliderValue(){
        pasekZycia.GetComponent<Slider>().value = playerHealth;
    }

    public IEnumerator EnableResistanceCollider(float resistanceTime){
        isResistant = true;
        resistanceCollider.enabled = true;
        playerCollider.enabled = false;
        yield return new WaitForSeconds(resistanceTime);

        EnablePlayerCollider();
    }

    private void EnablePlayerCollider(){
        isResistant = false;
        playerCollider.enabled = true;
        resistanceCollider.enabled = false;
    }

    public void PlayerDeath(){
        GameObject levelRestarter = Instantiate(levelRestarterPrefab, transform);
        levelRestarter.transform.parent = null;
        audioListener.transform.parent = null;
        Destroy(this.gameObject);
    }
}
