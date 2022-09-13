using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FlyGFX : MonoBehaviour
{
    public AIPath aiPath;

    void Update()
    {
        if(aiPath.desiredVelocity.x > 0.01f){
            transform.localScale = new Vector2(-1f, 1f);
        }
        if(aiPath.desiredVelocity.x < -0.01f){
            transform.localScale = new Vector2(1f, 1f);
        }
    }
}
