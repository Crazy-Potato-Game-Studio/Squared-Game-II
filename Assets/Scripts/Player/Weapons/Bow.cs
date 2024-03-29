using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{

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
    bool blockShooting;

    private void Update() {
        if(GetComponentInParent<ItemsManager>().arrowCount > 0){
            canFire = true;
        }else{
            canFire = false;
        }

        if(Input.GetMouseButton(0) && canFire && Time.timeScale == 1){
            if(!blockShooting){
                ChargeBow();
            }
        }else if(Input.GetMouseButtonUp(0) && canFire && Time.timeScale == 1){
            if(!blockShooting){
                Shoot();
            }
            blockShooting = false;
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

        GetComponentInParent<ItemsManager>().arrowCount--;
        GetComponentInParent<ItemsManager>().UpdateArrowsCount();
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

        if(Input.GetMouseButtonDown(1)){
            bowCharge = 0;
            canFire = false;
            blockShooting = true;
            bowGFX.sprite = bowSprites[0];
            arrowGFX.enabled = false;
        }
    }
}
