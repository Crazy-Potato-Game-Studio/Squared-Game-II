using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Inventory inventory;
    public GameObject itemButton;
    public AudioClip collectPotion;

    private void Start() {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")){
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                if(inventory.isFull[i] == false){

                    inventory.isFull[i] = true;
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(gameObject);
                    break;
                }
            }
            other.GetComponent<AudioSource>().PlayOneShot(collectPotion);
        }
    }
}
