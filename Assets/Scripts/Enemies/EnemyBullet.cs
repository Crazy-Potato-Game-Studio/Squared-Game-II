using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Ground" || other.gameObject.tag == "Cube"){
            rb.velocity = UnityEngine.Vector2.zero;
            rb.isKinematic = true;
            Destroy(GetComponent<PolygonCollider2D>());
            Destroy(GetComponent<Rigidbody2D>());
            transform.parent = other.gameObject.transform;
        }
    }
}
