using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Line : MonoBehaviour
{
    [SerializeField] private GameObject mechanism;
    [SerializeField] private string mechanismType;
    [SerializeField] private Light2D light2D;
    private LineRenderer lineRenderer;
    int elementsAmount;

    private void OnMouseEnter() {
        if(!UsedDevice.usingGamepad){
            lineRenderer.enabled = true;
            light2D.enabled = true;
        }
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
        if(mechanismType == "PressurePlate"){
            elementsAmount = mechanism.GetComponent<PressurePlate>().obejctsToTurnOn.Count;
            lineRenderer.positionCount = elementsAmount + 1;
            lineRenderer.SetPosition(0, mechanism.transform.position);

            for(int i = 0; i < elementsAmount; i++){
                lineRenderer.SetPosition(i + 1, mechanism.GetComponent<PressurePlate>().obejctsToTurnOn[i].transform.position);
            }
        }

        if(mechanismType == "LaserDetector"){
            elementsAmount = mechanism.GetComponent<LaserDetector>().obejctsToTurnOn.Count;
            lineRenderer.positionCount = elementsAmount + 1;
            lineRenderer.SetPosition(0, mechanism.transform.position);

            for(int i = 0; i < elementsAmount; i++){
                lineRenderer.SetPosition(i + 1, mechanism.GetComponent<LaserDetector>().obejctsToTurnOn[i].transform.position);
            }
        }
        
    }
}
