using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    [SerializeField] private SpriteRenderer playerSprite;
    private Color defaultColor;

    private void Awake() {
        defaultColor = playerSprite.color;
    }

    public void PlayerChangeColor(Color color){
        playerSprite.color = color;
    }

    public void SetDefaultColor(){
        playerSprite.color = defaultColor;
    }
}
