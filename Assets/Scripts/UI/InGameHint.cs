using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

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
            if(Gamepad.current is DualShockGamepad){
                playStationHint.SetActive(true);
                XboxHint.SetActive(false);
                PCHint.SetActive(false);
            }else{
                XboxHint.SetActive(true);
                playStationHint.SetActive(false);
                PCHint.SetActive(false);
            }
        }else{
            XboxHint.SetActive(false);
            PCHint.SetActive(true);
            playStationHint.SetActive(false);
        }
    }
}
