using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cam;

    public void ShowCamera(Transform trans){
        GameObject currentCamera = Instantiate(cam, new Vector3(trans.position.x, trans.position.y, -10f), trans.rotation);
        Destroy(currentCamera, 3f);
    }
}
