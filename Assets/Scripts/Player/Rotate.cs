using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    private Camera cam;

    private void Awake() {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void RotateArm(InputAction.CallbackContext context){
        Vector2 bowPosition = transform.position;
        if(cam){
            Vector2 mousePosition = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
            Vector2 direction = mousePosition - bowPosition;
            transform.right = direction;
        }        
    }

}
