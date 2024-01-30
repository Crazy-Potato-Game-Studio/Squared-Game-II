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

    private void Awake() {
        arrowCount = 10;

        arrowsText = GameObject.FindGameObjectWithTag("ArrowsUI").GetComponentInChildren<TextMeshProUGUI>();
        potionsText = GameObject.FindGameObjectWithTag("PotionsUI").GetComponentInChildren<TextMeshProUGUI>();

        UpdatePotionsCount();
        UpdateArrowsCount();
    }

    private void Update() {
        if(Input.GetKey(KeyCode.H)){
            if(potionsCount > 0){
                UsePotion();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Heart" || other.gameObject.tag == "ArrowPickup" || other.gameObject.tag == "Potion"){
            PickupObject(other.gameObject.tag, other);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Heart" || other.gameObject.tag == "ArrowPickup" || other.gameObject.tag == "Potion"){
            other.transform.position = Vector3.MoveTowards(other.transform.position, transform.position, 0.1f);
        }
    }

    void PickupObject(string name, Collision2D other){

        Destroy(other.gameObject);
        source.PlayOneShot(pickupSound);

        switch (name)
        {
            case "Heart":
                GetComponent<HealthManager>().GainHealth(15f);
                GameObject currentParticles = Instantiate(heartParticles, transform.position, transform.rotation);
                Destroy(currentParticles, 3f);
                break;
            case "ArrowPickup":
                arrowCount++;
                UpdateArrowsCount();
            break;
                case "Potion":
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
