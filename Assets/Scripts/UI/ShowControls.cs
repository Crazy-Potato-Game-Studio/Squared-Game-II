using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowControls : MonoBehaviour
{
    [SerializeField] private GameObject controlsPanel;
    [SerializeField] private GameObject menu;

    public void ShowControlsPanel(){
        controlsPanel.SetActive(true);
        menu.SetActive(false);
    }

    public void HideControlsPanel(){
        controlsPanel.SetActive(false);
        menu.SetActive(true);
    }  
}
