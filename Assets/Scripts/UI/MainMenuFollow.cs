using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class MainMenuFollow : MonoBehaviour
{
    private Vector2 startPoint;
    private Vector2 endPoint;
    [SerializeField] private GameObject cam;
    [SerializeField] private CinemachineVirtualCamera virtualCam;

    private void Start() {
        StartCoroutine("SetNewPoints");
    }

    private void Update() {
        transform.position = Vector2.MoveTowards(transform.position, endPoint, Vector2.Distance(startPoint, endPoint) / 30000);
    }

    IEnumerator SetNewPoints(){
        startPoint = new Vector2(Random.Range(-40,70), Random.Range(10,-15));
        endPoint = new Vector2(Random.Range(-40,70), Random.Range(10,-15));

        SetRandomLens();

        cam.transform.position = startPoint;
        transform.position = startPoint;

        yield return new WaitForSeconds(Random.Range(3f, 8f));
        StartCoroutine("SetNewPoints");
    }

    private void SetRandomLens(){
        virtualCam.m_Lens.OrthographicSize = Random.Range(7, 12);
    }
}
