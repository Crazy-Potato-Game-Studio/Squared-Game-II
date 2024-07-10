using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetector : MonoBehaviour
{
    [SerializeField] private GameObject[] obejctsToTurnOn;
    [SerializeField] private GameObject detectorLight;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material lightMaterial;

    private bool lightIsOn;

    public void TurnObjects(){
        for(int i = 0; i < obejctsToTurnOn.Length; i++){
            if(obejctsToTurnOn[i].gameObject.GetComponent<Portal>() != null){
                if(obejctsToTurnOn[i].gameObject.GetComponent<Portal>().isOn){
                    obejctsToTurnOn[i].gameObject.GetComponent<Portal>().TurnOff();
                }else{
                    obejctsToTurnOn[i].gameObject.GetComponent<Portal>().TurnOn();
                } 
            }else if(obejctsToTurnOn[i].gameObject.GetComponent<Doors>() != null){
                if(obejctsToTurnOn[i].gameObject.GetComponent<Doors>().doorsOpen){
                    obejctsToTurnOn[i].gameObject.GetComponent<Doors>().CloseDoors();
                }else{
                    obejctsToTurnOn[i].gameObject.GetComponent<Doors>().OpenDoors();
                } 
            }
        } 

        if(lightIsOn){
            TurnLightOff();
        }else{
            TurnLightOn();
        }
        
    }

    void TurnLightOn(){
        detectorLight.GetComponent<SpriteRenderer>().material = lightMaterial;
        lightIsOn = true;
    }

    void TurnLightOff(){
        detectorLight.GetComponent<SpriteRenderer>().material = defaultMaterial;
        lightIsOn = false;
    }


}
