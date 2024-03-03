using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject innerMenu;
    [SerializeField] private GameObject buttons;

    public void ShowInGameMenu(){
        innerMenu.SetActive(true);
    }

    public void HideInGameMenu(){
        innerMenu.SetActive(false);
        buttons.SetActive(false);
    }

    public void ShowButtons(){
        innerMenu.SetActive(false);
        buttons.SetActive(true);
    }

    public void HideButtons(){
        innerMenu.SetActive(true);
        buttons.SetActive(false);
    }
}
