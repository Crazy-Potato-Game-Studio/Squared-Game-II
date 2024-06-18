using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    [SerializeField] private GameObject laserObject;
    [SerializeField] private GameObject LineRenderer;
    public RaycastHit2D trans;

    private void Awake() {
        SwitchLaser(false);
    }

    public void SwitchLaser(bool isOn){
        laserObject.GetComponent<Laser>().enabled = isOn;
        laserObject.GetComponent<Laser>().endParticlesInstantiated = isOn;
        laserObject.GetComponent<Laser>().shootPoint.transform.position = trans.point;
        LineRenderer.SetActive(isOn);
    }
}
