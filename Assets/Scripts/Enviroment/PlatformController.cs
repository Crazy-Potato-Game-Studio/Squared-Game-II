using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RaycastController
{
    public LayerMask passengerMask;

    public Vector3[] localWaypoints;
    Vector3[] globalWaypoints;

    [SerializeField] private float speed;
    int fromWaypointIndex;
    float percentBetweenWaypoints;

    public override void Start() {
        base.Start();

        globalWaypoints = new Vector3[localWaypoints.Length];

        for(int i = 0; i < localWaypoints.Length; i++){
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Cube"){
            other.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if(other.gameObject.tag == "Cube"){
            other.transform.parent = null;
        }
    }

    void Update(){
        UpdateRaycastOrigins();

        Vector3 velocity = CalculatePlatformMovement();

        MovePassengers(velocity);
        transform.Translate(velocity);
    }

    Vector3 CalculatePlatformMovement(){
        int toWaypointIndex = fromWaypointIndex + 1;
        float distanceBetweenWaypoints= Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;

        Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWaypointIndex], percentBetweenWaypoints);

        if(percentBetweenWaypoints >= 1){
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            if(fromWaypointIndex >= globalWaypoints.Length -1){
                fromWaypointIndex = 0;
                System.Array.Reverse(globalWaypoints);
            }
        }

        return newPos - transform.position;
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

    private void OnDrawGizmos() {
        if(localWaypoints != null){
            Gizmos.color = Color.red;
            float size = 0.3f;

            for(int i = 0; i < localWaypoints.Length; i ++){
                Vector3 globalWaypointPos = (Application.isPlaying)?globalWaypoints[i]:localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up * size, globalWaypointPos + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left * size, globalWaypointPos + Vector3.left * size);
            }
        }
    }
}
