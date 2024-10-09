using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Bow : MonoBehaviour
{
    static public float difficultyLevel = 1; 
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
    private PlayerInputActions playerInputActions;
    private bool isPressing;
    private bool canPress;

    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.Player.Shoot.performed += SetOnPerformed;
        playerInputActions.Player.Shoot.canceled += SetOnCanceled;
    }

    private void SetOnPerformed(InputAction.CallbackContext context){
        isPressing = true;
    }

    private void SetOnCanceled(InputAction.CallbackContext context){
        isPressing = false;
    }

    private void Update() {
        if(GetComponentInParent<ItemsManager>().arrowCount > 0){
            canFire = true;
        }else{
            canFire = false;
        }

        if(isPressing && canFire && Time.timeScale == 1){
            canPress = true;
            if(!blockShooting){
                ChargeBow();
            }
        }else if(canPress && !isPressing && canFire && Time.timeScale == 1){
            if(!blockShooting){
                Shoot();
                canPress = false;
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
        newArrow.GetComponent<Arrow>().arrowDamage = shootingForce * bowCharge * difficultyLevel;
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

    private void OnDestroy() {
        playerInputActions.Player.Shoot.performed -= SetOnPerformed;
        playerInputActions.Player.Shoot.canceled -= SetOnCanceled;
        playerInputActions.Player.Disable();
        playerInputActions.Disable();
    }
}