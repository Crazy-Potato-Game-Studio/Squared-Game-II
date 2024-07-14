using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeKing : MonoBehaviour
{
    private Rigidbody2D rb;
    private int direction;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject[] shootPoints;
    [SerializeField] private GameObject slimeBulletPrefab;
    [SerializeField] private AudioClip slimeShootingClip;
    private AudioSource audioSource;
    [SerializeField] private float shootTimeMin;
    [SerializeField] private float shootTimeMax;

    [SerializeField] private GameObject doors;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("ChangeDirection", 0f, 5f);
        InvokeRepeating("Shoot", 7f, Random.Range(shootTimeMin, shootTimeMax));
        audioSource = GetComponent<AudioSource>();
        direction = 1;
    }

    void Update()
    {
        Move(direction);
    }

    void Move(int direction){
        rb.velocity = new Vector2(moveSpeed * direction, rb.velocity.y);
    }

    void ChangeDirection(){
        direction = -direction;
        transform.localScale = new Vector2(direction, transform.localScale.y);
    }

    void Shoot(){
        for(int i = 0; i <= 5; i++){
            GameObject currentBullet = Instantiate(slimeBulletPrefab, shootPoints[i].transform);
            currentBullet.transform.parent = null;
            currentBullet.GetComponent<Rigidbody2D>().AddForce(shootPoints[i].transform.up * 20f, ForceMode2D.Impulse);
        }
        audioSource.PlayOneShot(slimeShootingClip);
    }

    public void OpenTheDoors(){
        doors.GetComponent<Doors>().OpenDoors();
    }

}
