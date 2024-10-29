using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManagementDestroyer : MonoBehaviour
{
    private void Start()
    {
        if(FindObjectOfType<SceneManagement>())
        {
            Destroy(FindObjectOfType<SceneManagement>().gameObject);
        }
    }
}
