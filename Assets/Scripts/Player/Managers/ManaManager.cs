using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ManaManager : MonoBehaviour
{
    public GameObject pasekMany;
    public float playerMana;
    public float maxPlayerMana;
    [SerializeField] private TextMeshProUGUI manaText;

    void Start() {
        pasekMany.GetComponent<Slider>().maxValue = maxPlayerMana;
        playerMana = maxPlayerMana;
        UpdateSlider();
        UpdateManaText();
    }

    public void LoseMana(float manaPoints){
        playerMana -= manaPoints;
        pasekMany.GetComponent<Slider>().value = playerMana;
        UpdateSlider();
        UpdateManaText();
    }

    public void GainMana(float manaPoints){
        playerMana += manaPoints;
        pasekMany.GetComponent<Slider>().value = playerMana;
        if(playerMana>10) playerMana = 10;
        UpdateSlider();
        UpdateManaText();
    }

    private void UpdateSlider(){
        pasekMany.GetComponent<Slider>().value = playerMana; 
    }

    private void UpdateManaText(){
        manaText.text = playerMana.ToString() + "/" + maxPlayerMana.ToString();  
    }


}
