using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;
    public Vector3 move;

    public override void Start() {
        base.Start();
    }

    void Update(){
        UpdateRaycastOrigins();

        Vector3 velocity = move * Time.deltaTime;

        MovePassengers(velocity);
        transform.Translate(velocity);
    }

    void MovePassengers(Vector3 velocity){

        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        if(velocity.y != 0){
            float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

            for(int i = 0; i < verticalRayCount; i++){
                Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (raySpacing  * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, passengerMask);

                if(hit){
                    if(!movedPassengers.Contains(hit.transform)){
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1)?velocity.x:0;
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;
                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }

        if(velocity.x != 0){
            float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

            for(int i = 0; i < verticalRayCount; i++){
                Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
                rayOrigin += Vector2.right * (raySpacing  * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, passengerMask);

                if(hit){
                    if(!movedPassengers.Contains(hit.transform)){
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = 0;
                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }

        if(directionY == -1 || velocity.y == 0 && velocity.x != 0){
            float rayLenght = skinWidth * 2;

            for(int i = 0; i < verticalRayCount; i++){
                Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (raySpacing  * i);

                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLenght, passengerMask);

                if(hit){
                    if(!movedPassengers.Contains(hit.transform)){
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x;
                        float pushY = velocity.y;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }
    }
}
