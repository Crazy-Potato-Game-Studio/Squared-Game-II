using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : MonoBehaviour
{
    [SerializeField] private GameObject cubeGFX;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject bow;
    [SerializeField] private GameObject hintPrefab;
    public bool canLiftCube;
    public bool canThrowCube = true;
    public GameObject groundCube;
    private AudioSource audioSource;
    [SerializeField] private AudioClip cubePickingSound;
    [SerializeField] private AudioClip cubeThrowingSound;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        if(Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.E)){
            if(bow.activeSelf == true && canLiftCube){
                LiftCube();
            }else if(bow.activeSelf == false && canThrowCube){
                ThrowCube();
            }
        }
    }

    void LiftCube(){
        bow.SetActive(false);
        Destroy(groundCube);
        cubeGFX.SetActive(true);
        audioSource.PlayOneShot(cubePickingSound);
    }

    void ThrowCube(){
        GameObject realCube = Instantiate(cubePrefab, cubeGFX.transform);
        realCube.transform.parent = null;
        cubeGFX.SetActive(false);
        bow.SetActive(true);
        audioSource.PlayOneShot(cubeThrowingSound);
    }
}
