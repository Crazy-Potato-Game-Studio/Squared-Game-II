using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowNoGamepadInfo : MonoBehaviour
{
    [SerializeField] private GameObject gamepadInfo;

    void Update()
    {
        if(UsedDevice.usingGamepad){
            if(GetComponent<ButtonSelect>().isSelected){
                gamepadInfo.SetActive(true);
            }else{
                gamepadInfo.SetActive(false);
            }
        }else{
            gamepadInfo.SetActive(false);
        }
    }
}
