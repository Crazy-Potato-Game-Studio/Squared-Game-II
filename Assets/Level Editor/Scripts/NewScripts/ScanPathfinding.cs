using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ScanPathfinding : MonoBehaviour
{
    private void Awake() {
        StartCoroutine("Scan");
    }
    
    IEnumerator Scan(){
        yield return new WaitForSeconds(1);
        AstarPath.active.Scan();
    }
}
