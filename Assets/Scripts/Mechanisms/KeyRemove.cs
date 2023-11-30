using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor.Tilemaps;
public class KeyRemove : MonoBehaviour
{
    GameObject player;
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
    }
    

    public void DeleteKey(){
        string parentName = transform.parent.name;
        Debug.Log("Used " + this.name + ", from slot: " + parentName);
        string slotNumber = parentName.Substring(parentName.Length - 1);
        player.GetComponent<Inventory>().isFull[Int64.Parse(slotNumber)] = false;
        Destroy(this.gameObject);
    }
}
