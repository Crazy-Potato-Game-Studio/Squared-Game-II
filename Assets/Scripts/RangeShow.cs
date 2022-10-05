using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeShow : MonoBehaviour
{

    void OnMouseOver() {
        Transform child = transform.Find("TurretRange");
        Debug.Log("dwdaw");
        child.transform.localScale = new Vector3(GetComponent<Turret>().turretRange * 0.11f, GetComponent<Turret>().turretRange * 0.11f, 1f);
    }

    private void OnMouseExit() {
        Transform child = transform.Find("TurretRange");
        child.transform.localScale = new Vector3(0, 0, 1f);
    }

}
