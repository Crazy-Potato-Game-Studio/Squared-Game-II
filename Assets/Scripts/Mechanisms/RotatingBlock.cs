using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlock : MonoBehaviour
{

    [SerializeField] private float rotationSpeed;

    void FixedUpdate()
    {
        transform.eulerAngles += new Vector3(0, 0, 0.1f * rotationSpeed);
    }
}
