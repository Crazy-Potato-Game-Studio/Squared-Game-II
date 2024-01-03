using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller2D : RaycastController
{
    public CollisionInfo collisions;

    public override void Start()
    {
        base.Start();
    }

    public void Move(Vector3 velocity){
        UpdateRaycastOrigins();
        collisions.Reset();

        if(velocity.x != 0){
            HorizontalCollisions(ref velocity);
        }

        if(velocity.y != 0){
            VerticalCollisions(ref velocity);
        }
        
        transform.Translate(velocity);
    }

    void HorizontalCollisions(ref Vector3 velocity){
        float directionX = Mathf.Sign(velocity.x);
        float rayLenght = Mathf.Abs(velocity.x) + skinWidth;

        for(int i = 0; i < verticalRayCount; i++){
            Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * raySpacing  * i;

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionX * rayLenght, Color.red);

            if(hit){
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLenght = hit.distance;

                collisions.left = directionX == -1;
                collisions.right = directionX == 1;
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity){
        float directionY = Mathf.Sign(velocity.y);
        float rayLenght = Mathf.Abs(velocity.y) + skinWidth;

        for(int i = 0; i < verticalRayCount; i++){
            Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (raySpacing  * i + velocity.x);

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLenght, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLenght, Color.red);

            if(hit){
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLenght = hit.distance;

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }
    }

    public struct CollisionInfo{
        public bool above, below;
        public bool left, right;

        public void Reset(){
            above = below = false;
            left = right = false;
        }
    }
}
