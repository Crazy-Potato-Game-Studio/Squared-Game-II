using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float shootingForce = 20f;

    void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;

        if(Input.GetMouseButtonDown(0)){
            Shoot();
        }
    }

    void Shoot(){
        GameObject newArrow = Instantiate(arrow, shootingPoint.position, shootingPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * shootingForce;
    }
}
