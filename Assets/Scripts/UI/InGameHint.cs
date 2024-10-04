using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameHint : MonoBehaviour
{
    [SerializeField] private GameObject playStationHint;
    [SerializeField] private GameObject XboxHint;
    [SerializeField] private GameObject PCHint;

    void Update()
    {
        CheckInput();
    }

    public void CheckInput(){
        if(UsedDevice.usingGamepad){
            XboxHint.SetActive(false);
            PCHint.SetActive(false);
            playStationHint.SetActive(true);
        }else{
            XboxHint.SetActive(false);
            PCHint.SetActive(true);
            playStationHint.SetActive(false);
        }
    }
}
