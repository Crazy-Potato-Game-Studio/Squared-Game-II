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
        Vector2 direction = playerInputActions.Player.RotateArm.ReadValue<Vector2>().normalized;
        Debug.Log(playerInputActions.Player.RotateArm.ReadValue<Vector2>());
        if(direction != Vector2.zero){
            lastDirection = direction;
            transform.right = direction;
        }else{
            transform.right = lastDirection;
        }
    }

    public void RotateArm(InputAction.CallbackContext context){
        // Vector2 bowPosition = transform.position;
        //if(cam){
            // if(mouse != null){
            //     Vector2 mousePosition = cam.ScreenToWorldPoint(context.ReadValue<Vector2>());
            //     Vector2 direction = mousePosition - new Vector2(transform.position.x, transform.position.y);
            //     transform.right = direction;
            // }
        //}        
    }

    public void RotateWithMouse(){
        
    }

}
