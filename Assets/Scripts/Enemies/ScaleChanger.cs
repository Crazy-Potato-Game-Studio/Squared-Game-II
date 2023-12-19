using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleChanger : MonoBehaviour
{
    public void ChangeUIScale(float scale){
        transform.localScale = new Vector3(scale * 0.01f,0.01f,1);
    }
}
