using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

    [SerializeField] private Camera cam;
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float shootingForce = 5f;
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip clip;
    [SerializeField] private SpriteRenderer arrowGFX;
    [SerializeField] private Sprite[] bowSprites;

    [SerializeField] float maxBowCharge = 1.5f;
    [SerializeField] SpriteRenderer bowGFX;
    public float bowCharge;
    bool canFire;

    void Update()
    {
        Vector2 bowPosition = transform.position;
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - bowPosition;
        transform.right = direction;

        if(Input.GetMouseButton(0) && canFire){
            ChargeBow();
        }else if(Input.GetMouseButtonUp(0) && canFire){
            Shoot();
        }else{
            if(bowCharge > 0f){
                bowCharge -= 1f * Time.deltaTime;
            } else{
                bowCharge = 0;
                canFire = true;
            }
        }
    }

    void Shoot(){
        GameObject newArrow = Instantiate(arrow, shootingPoint.position, shootingPoint.rotation);
        newArrow.GetComponent<Rigidbody2D>().velocity = transform.right * shootingForce * bowCharge;
        newArrow.GetComponent<Arrow>().arrowDamage = shootingForce * bowCharge;
        bowGFX.sprite = bowSprites[0];

        source.PlayOneShot(clip);

        bowCharge = 0;
        canFire = true;
        arrowGFX.enabled = false;
    }

    void ChargeBow(){
        arrowGFX.enabled = true;
        bowCharge += Time.deltaTime * 2;

        if(bowCharge > maxBowCharge){
            bowCharge = maxBowCharge;
        }

        if(bowCharge > 0 && bowCharge <= 0.5f){
            bowGFX.sprite = bowSprites[0];
        }

        if(bowCharge > 0.5f && bowCharge <= 1f){
            bowGFX.sprite = bowSprites[1];
        }

        if(bowCharge > 1f && bowCharge <= 1.5f){
            bowGFX.sprite = bowSprites[2];
        }
    }
}
