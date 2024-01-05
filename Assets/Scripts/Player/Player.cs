using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float jumpHeight;
    [SerializeField] private float timeToJumpApex;
    [SerializeField] private float moveSpeed;

    [SerializeField] private float accelerationTimeAir;
    [SerializeField] private float accelerationTimeGround;

    [SerializeField] private float jumpVelocity;
    [SerializeField] private float gravity = -20;

    [SerializeField] private float wallSlideSpeedMax;
    [SerializeField] private Vector2 wallJumpClimb;
    [SerializeField] private Vector2 wallJumpOff;
    [SerializeField] private Vector2 wallLeap;
    [SerializeField] private float wallStickTime = 0.25f;
    float timeToWallUnstick;

    float smoothing;
    Controller2D controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight)/Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;

    }

    private void Update() {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        int wallDirX = (controller.collisions.left)?-1:1;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref smoothing, (controller.collisions.below)?accelerationTimeGround:accelerationTimeAir);

        bool wallSliding = false;

        if((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0){
            wallSliding = true;

            if(velocity.y < -wallSlideSpeedMax){
                velocity.y = -wallSlideSpeedMax;
            }

            if(timeToWallUnstick > 0){

                velocity.x = 0;
                smoothing = 0;

                if(input.x != wallDirX && input.x != 0){
                    timeToWallUnstick -= Time.deltaTime;
                }else{
                    timeToWallUnstick = wallStickTime;
                } 
            }else{
                timeToWallUnstick = wallStickTime;
            }
        }

        if(controller.collisions.above || controller.collisions.below){
            velocity.y = 0;
        }

        

        if(Input.GetKey(KeyCode.Space)){
            if(wallSliding){
                if(wallDirX == input.x){
                    velocity.x = -wallDirX * wallJumpClimb.x;
                    velocity.y = wallJumpClimb.y;
                }else if(input.x == 0){
                    velocity.x = -wallDirX * wallJumpOff.x;
                    velocity.y = wallJumpOff.y;
                }else{
                    velocity.x = -wallDirX * wallLeap.x;
                    velocity.y = wallLeap.y;
                }
            }
            if(controller.collisions.below){
                velocity.y = jumpVelocity;
            }
        }

    
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
