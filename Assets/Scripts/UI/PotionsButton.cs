using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;

public class PotionsButton : MonoBehaviour
{
    [SerializeField] private GameObject PcButton;
    [SerializeField] private GameObject playstationButton;
    [SerializeField] private GameObject xboxButton;

    void Start()
    {
        CheckForGamepad();
        InvokeRepeating("CheckForGamepad",1f,2f);
    }

    public void CheckForGamepad(){
        if(UsedDevice.usingGamepad){
            if(Gamepad.current is DualShockGamepad){
                playstationButton.SetActive(true);
                xboxButton.SetActive(false);
                PcButton.SetActive(false);
            }else{
                playstationButton.SetActive(false);
                xboxButton.SetActive(true);
                PcButton.SetActive(false);
            }
        }else{
            PcButton.SetActive(true);
            playstationButton.SetActive(false);
            xboxButton.SetActive(false);
        }
    }
}
