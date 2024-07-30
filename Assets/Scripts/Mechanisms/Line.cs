using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Line : MonoBehaviour
{
    [SerializeField] private GameObject pressurePlate;
    [SerializeField] private Light2D light2D;
    private LineRenderer lineRenderer;
    int elementsAmount;

    private void OnMouseEnter() {
        lineRenderer.enabled = true;
        light2D.enabled = true;
    }

    private void OnMouseExit() {
        lineRenderer.enabled = false;
        light2D.enabled = false;
    }

    private void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        ShowLine();
        lineRenderer.enabled = false;
    }

    private void ShowLine() {
        elementsAmount = pressurePlate.GetComponent<PressurePlate>().obejctsToTurnOn.Length;
        lineRenderer.positionCount = elementsAmount + 1;
        lineRenderer.SetPosition(0, pressurePlate.transform.position);
        
        for(int i = 0; i < elementsAmount; i++){
            lineRenderer.SetPosition(i + 1, pressurePlate.GetComponent<PressurePlate>().obejctsToTurnOn[i].transform.position);
        }
    }
}
