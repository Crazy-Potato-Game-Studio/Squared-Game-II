using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItems : MonoBehaviour
{

    [SerializeField] private float manaPotionPoints = 5f;
    [SerializeField] private float healthPotionPoints = 5f;

    public GameObject player;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UseHealthPotion(){
        player.GetComponent<HealthManager>().GainHealth(healthPotionPoints);
        string parentName = transform.parent.name;
        Debug.Log(parentName);
        string slotNumber = parentName.Substring(parentName.Length - 1);
        player.GetComponent<Inventory>().isFull[Int64.Parse(slotNumber) -1 ] = false;
        Destroy(this.gameObject);
    }

    public void UseManaPotion(){
        player.GetComponent<ManaManager>().GainMana(manaPotionPoints);
        string parentName = transform.parent.name;
        Debug.Log(parentName);
        string slotNumber = parentName.Substring(parentName.Length - 1);
        player.GetComponent<Inventory>().isFull[Int64.Parse(slotNumber) -1 ] = false;
        Destroy(this.gameObject);
    }
}
