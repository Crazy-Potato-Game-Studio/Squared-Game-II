using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowsManager : MonoBehaviour
{
    public int arrowCount;
    [SerializeField] private int startArrowsAmount;
    [SerializeField] private TextMeshProUGUI arrowCountText;

    private void Awake() {
        arrowCount = startArrowsAmount;
        UpdateArrowText();
    }

    public void UpdateArrowText(){
        arrowCountText.text = arrowCount.ToString();
    }
}
