using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnRe : MonoBehaviour
{
    public Transform playerSpawnTransform;
    public GameObject playerPrefab;
    public GameObject playerObject;
    public AudioSource audioSource;
    public LayerMask groundLayerMask;
    Camera mainCam;
    private void Awake()
    {
        mainCam = Camera.main;
    }
    private void OnEnable()
    {
        mainCam.transform.position = transform.position;
        playerObject = Instantiate(playerPrefab, playerSpawnTransform.position, Quaternion.identity);
        playerObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }
    public void StartGame()
    {
        mainCam.GetComponent<CameraFollow>().player = playerObject.transform;
        mainCam.GetComponent<CameraFollow>().enabled = true;
        playerObject.GetComponent<BoxCollider2D>().isTrigger = true;
        Time.timeScale = 1;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerObject.GetComponent<BoxCollider2D>().isTrigger = false;
    }

}
