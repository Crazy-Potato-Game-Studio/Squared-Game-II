using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationName : MonoBehaviour
{
    [SerializeField] private GameObject locationManager;
    [SerializeField] private string locationName;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Player"){
            locationManager.GetComponent<LocationManager>().ShowLocationName(locationName);
        }
    }
}
