using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorOnGravityChange : MonoBehaviour
{
    void Update()
    {
        if(Physics2D.gravity.y > 0){
            GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x, -1, GetComponent<Transform>().localScale.z);
        }else{
            GetComponent<Transform>().localScale = new Vector3(GetComponent<Transform>().localScale.x, 1, GetComponent<Transform>().localScale.z);
        } 
    }

    
}
