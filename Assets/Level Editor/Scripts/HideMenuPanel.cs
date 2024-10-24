using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideMenuPanel : MonoBehaviour
{
    private RectTransform rectTransform;
    private bool panelVisible;

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        panelVisible = true;
    }

    public void MovePanel(){
        if(panelVisible){
            rectTransform.position = new Vector2(rectTransform.position.x - 460, rectTransform.position.y);
        }else{
            rectTransform.position = new Vector2(rectTransform.position.x + 460, rectTransform.position.y);
        }
        
        panelVisible = !panelVisible;
    }
}
