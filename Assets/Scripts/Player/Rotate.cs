using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Rotate : MonoBehaviour
{
    private Camera cam;
    Vector2 lastDirection;
    private PlayerInputActions playerInputActions;

    private void Awake() {
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
    }

    private void FixedUpdate() {
        Vector2 direction = Vector2.right;
        if(UsedDevice.usingGamepad){
            direction = playerInputActions.Player.RotateArm.ReadValue<Vector2>().normalized;
        }else{
            Vector2 mousePosition = cam.ScreenToWorldPoint(playerInputActions.Player.RotateArmMouse.ReadValue<Vector2>());
            direction = mousePosition - new Vector2(transform.position.x, transform.position.y);
        }
        Debug.Log(direction);
        if(direction != Vector2.zero){
            lastDirection = direction;
            transform.right = direction;
        }else{
            transform.right = lastDirection;
        }
    }
}
