using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemsManager : MonoBehaviour
{
    public int arrowCount;
    public int potionsCount;
    private TextMeshProUGUI arrowsText;
    public TextMeshProUGUI potionsText;
    [SerializeField] private GameObject heartParticles;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private AudioClip powerPickupSound;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        arrowsText = GameObject.FindGameObjectWithTag("ArrowsUI").GetComponentInChildren<TextMeshProUGUI>();
        potionsText = GameObject.FindGameObjectWithTag("PotionsUI").GetComponentInChildren<TextMeshProUGUI>();
        UpdatePotionsCount();
        UpdateArrowsCount();

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Potions.performed += UsePotion;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Heart" || other.gameObject.tag == "ArrowPickup" || other.gameObject.tag == "Potion" || other.gameObject.tag == "Power" || other.gameObject.tag == "UnoReverseCard"){
            PickupObject(other.gameObject.tag, other);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.gameObject.tag == "Heart" || other.gameObject.tag == "ArrowPickup" || other.gameObject.tag == "Potion" || other.gameObject.tag == "Power" || other.gameObject.tag == "UnoReverseCard"){
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
            case "UnoReverseCard":
                source.PlayOneShot(pickupSound);
                UnoReverseCard.hasUnoReverseCard = true; 
                GetComponent<UnoReverseCard>().AddUnoCard();
            break;
            default:
            break;
        }
    }

    public void UsePotion(InputAction.CallbackContext context){
        if(context.performed){
            if(potionsCount > 0){
                potionsCount--;
                GetComponent<HealthManager>().GainHealth(20f);
                GameObject currentParticles = Instantiate(heartParticles, transform.position, transform.rotation);
                Destroy(currentParticles, 3f);
                UpdatePotionsCount();
            }
        }
    }

    public void UpdatePotionsCount(){
        potionsText.text = potionsCount.ToString();
    }

    public void UpdateArrowsCount(){
        arrowsText.text = arrowCount.ToString();
    }

    private void OnDestroy() {
        playerInputActions.Player.Disable();
        playerInputActions.Player.Potions.performed -= UsePotion;
        playerInputActions = null;
    }
}
