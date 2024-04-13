using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemsManager : MonoBehaviour
{
    public float arrowCount;
    private float potionsCount;
    private TextMeshProUGUI arrowsText;
    public TextMeshProUGUI potionsText;
    [SerializeField] private GameObject heartParticles;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip powerPickupSound;

    private void Awake() {

        arrowsText = GameObject.FindGameObjectWithTag("ArrowsUI").GetComponentInChildren<TextMeshProUGUI>();
        potionsText = GameObject.FindGameObjectWithTag("PotionsUI").GetComponentInChildren<TextMeshProUGUI>();
        arrowCount = 5;
        UpdatePotionsCount();
        UpdateArrowsCount();
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.Q)){
            if(potionsCount > 0){
                UsePotion();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Heart" || other.gameObject.tag == "ArrowPickup" || other.gameObject.tag == "Potion" || other.gameObject.tag == "Power"){
            PickupObject(other.gameObject.tag, other);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Heart" || other.gameObject.tag == "ArrowPickup" || other.gameObject.tag == "Potion" || other.gameObject.tag == "Power"){
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, 0.1f);
        }
    }

    void PickupObject(string name, Collision2D other){

        Destroy(other.gameObject);
        
        switch (name)
        {
            case "Heart":
                source.PlayOneShot(pickupSound);
                GetComponent<HealthManager>().GainHealth(15f);
                GameObject currentParticles = Instantiate(heartParticles, transform.position, transform.rotation);
                Destroy(currentParticles, 3f);
                break;
            case "ArrowPickup":
                source.PlayOneShot(pickupSound);
                arrowCount++;
                UpdateArrowsCount();
            break;
            case "Power":
                source.PlayOneShot(powerPickupSound);
                GetComponent<PlayerHasPower>().playerPickedUpPower();
            break;
            case "Potion":
                source.PlayOneShot(pickupSound);
                potionsCount++;
                UpdatePotionsCount();  
            break;
            default:
            break;
        }
    }

    void UsePotion(){
        potionsCount--;
        GetComponent<HealthManager>().GainHealth(30f);
        GameObject currentParticles = Instantiate(heartParticles, transform.position, transform.rotation);
        Destroy(currentParticles, 3f);
        UpdatePotionsCount();
    }

    void UpdatePotionsCount(){
        potionsText.text = potionsCount.ToString();
    }

    public void UpdateArrowsCount(){
        arrowsText.text = arrowCount.ToString();
    }

}
