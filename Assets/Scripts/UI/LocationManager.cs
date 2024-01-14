using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocationManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI locationNameText;

    public void ShowLocationName(string locationName){
        locationNameText.text = locationName;
        locationNameText.enabled = true;
        StartCoroutine("HideLocationName");
    }

    IEnumerator HideLocationName(){
        yield return new WaitForSeconds(4f);
        locationNameText.enabled = false;
    }
}
