using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chwdp : MonoBehaviour
{
    public Rigidbody2D rb;
    float force=10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.L)){
            rb.AddForce(new Vector2(0,force));
            Debug.Log(force);
        }
    }
}
