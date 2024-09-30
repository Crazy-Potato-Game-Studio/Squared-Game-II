using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCamera : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    private int counter = 0;

    void Start(){
        InvokeRepeating("ChangeInitPos", 0, Random.Range(7, 12));
    }

    void ChangeInitPos(){
        transform.position = new Vector3(points[counter].position.x, points[counter].position.y, -10);
        counter++;
        if(counter > points.Length-1){
            counter = 0;
        }
    }

    void Update()
    {
        transform.position += new Vector3(0.6f * Time.deltaTime, 0, 0);
    }
}
