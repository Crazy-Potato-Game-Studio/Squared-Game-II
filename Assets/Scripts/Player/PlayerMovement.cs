using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    private float gravityForce;
    [SerializeField] private Collider2D resistanceCollider;
    public float speed;
    public float jumpForce;
    float moveInput;
    float verticalMoveInput;
    float climbSpeed = 7f;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material frozenMaterial;
    [SerializeField] private GameObject frozenParticles;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioSource source;

    public bool isFrozen;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    
    public int extraJumps;

    public bool gravityUp;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        defaultMaterial = playerSprite.material;
        gravityForce = rb.gravityScale;

        if(Physics2D.gravity.y > 0){
            gravityUp = true;
        }else{
            gravityUp = false;
        }

    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        if(!isFrozen){
            if(col.IsTouchingLayers(LayerMask.GetMask("Climbing")) || resistanceCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
                rb.velocity = new Vector2(moveInput * speed, verticalMoveInput * climbSpeed);
                rb.gravityScale = 0;
            }else{
                rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
                rb.gravityScale = gravityForce;
            }

        }
        if(isFrozen){
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void Update(){
        if(isGrounded){
            extraJumps = 1;
        }
    }

    public void Move(InputAction.CallbackContext context){
        moveInput = context.ReadValue<Vector2>().x;
    }

    public void ClimbLadder(InputAction.CallbackContext context){
        verticalMoveInput = context.ReadValue<Vector2>().y;
    }

    public void Jump(InputAction.CallbackContext context){
        if(context.performed){
            if(!isFrozen && extraJumps > 0){
                if(gravityUp){
                    rb.velocity = Vector2.down * jumpForce;
                }else{
                    rb.velocity = Vector2.up * jumpForce;
                }
                extraJumps--;
                if(!col.IsTouchingLayers(LayerMask.GetMask("Climbing")) || resistanceCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
                    source.PlayOneShot(jumpClip);
                }
            }
        }
    }

    public void GoThroughPlatform(InputAction.CallbackContext context){
        if(context.performed){
            GameObject platforms = GameObject.FindGameObjectWithTag("Platforms");
            platforms.GetComponent<VerticalPlatform>().RotatePlatform();
        }
    }

    IEnumerator PlayerFrozen(){
        yield return new WaitForSeconds(1f);
        isFrozen = false;
        playerSprite.material = defaultMaterial;
    }

    public void FrozePlayer(){
        isFrozen = true;
        GameObject particles = Instantiate(frozenParticles, transform.position, transform.rotation);
        particles.transform.parent = gameObject.transform;
        Destroy(particles, 1f);
        playerSprite.material = frozenMaterial;
        StopCoroutine("PlayerFrozen");
        StartCoroutine("PlayerFrozen");
    }
    
}
