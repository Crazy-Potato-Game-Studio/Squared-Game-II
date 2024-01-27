using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private GameObject cubeGFX;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject bow;
    public bool canLiftCube;
    public bool canThrowCube = true;
    public GameObject groundCube;

    private void Update() {
        if(Input.GetKeyDown(KeyCode.E)){
            if(bow.activeSelf == true && canLiftCube){
                bow.SetActive(false);
                Destroy(groundCube);
                cubeGFX.SetActive(true);
            }else if(bow.activeSelf == false && canThrowCube){
                GameObject realCube = Instantiate(cubePrefab, cubeGFX.transform);
                realCube.transform.parent = null;
                realCube.tag = "CubeNoHint";
                cubeGFX.SetActive(false);
                bow.SetActive(true);
            }
        }
    }
}
