using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalPlatform : MonoBehaviour
{
    private PlatformEffector2D effector;
    [SerializeField] private LayerMask ignorePlayer;
    [SerializeField] private LayerMask collideWithPlayer;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    public void RotatePlatform(){
        effector.colliderMask = ignorePlayer;
        StartCoroutine(TimeWaiter());
    }

    IEnumerator TimeWaiter(){
        yield return new WaitForSeconds(0.2f);
        effector.colliderMask = collideWithPlayer;
    }
}
