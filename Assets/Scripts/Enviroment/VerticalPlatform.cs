using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;


    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.S)){
            effector.rotationalOffset = 180f;
            StartCoroutine(TimeWaiter());
        }
    }

    IEnumerator TimeWaiter(){
        yield return new WaitForSeconds(0.2f);
        effector.rotationalOffset = 0f;
    }
}
