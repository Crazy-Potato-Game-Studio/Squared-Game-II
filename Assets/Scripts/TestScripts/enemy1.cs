using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy1 : MonoBehaviour
{
    // Start is called before the first frame update
    private Transform playerTranform;
    public GameObject bulletPrefab;
    private bool inrange;
    void Start()
    {
        playerTranform = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            inrange = true;
        }
    }
    void OnTriggerExit2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            inrange = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        transform.right = playerTranform.position - transform.position;
        if(inrange){
            GameObject currentBullet = Instantiate(bulletPrefab, transform);
            currentBullet.transform.parent = null;
            currentBullet.GetComponent<Rigidbody2D>().AddForce(transform.right*100);
        }
    }
}
