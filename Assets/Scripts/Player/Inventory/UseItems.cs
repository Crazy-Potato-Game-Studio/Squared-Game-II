using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItems : MonoBehaviour
{
    [SerializeField] private float healthPotionPoints = 5f;
    [SerializeField] private GameObject healthPotionParticles;

    public GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UseHealthPotion(){
        player.GetComponent<HealthManager>().GainHealth(healthPotionPoints);
        string parentName = transform.parent.name;
        Debug.Log("Used " + this.name + ", from slot: " + parentName);
        string slotNumber = parentName.Substring(parentName.Length - 1);
        player.GetComponent<Inventory>().isFull[Int64.Parse(slotNumber)] = false;

        GameObject particles = Instantiate(healthPotionParticles, player.transform.position, player.transform.rotation);
        Destroy(particles, 1.2f);
        Destroy(this.gameObject);
    }
}
