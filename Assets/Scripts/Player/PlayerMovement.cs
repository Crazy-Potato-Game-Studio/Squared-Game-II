using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    [SerializeField] private Collider2D resistanceCollider;
    public float speed;
    public float jumpForce;
    float moveInput;
    float climbSpeed = 7f;
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material frozenMaterial;
    [SerializeField] private GameObject frozenParticles;

    public bool isFrozen;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    
    public int extraJumps;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        defaultMaterial = playerSprite.material;
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");

        if(!isFrozen){
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }

        if(isFrozen){
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

    }

    void Update(){

        if(isGrounded){
            extraJumps = 1;
        }

        if(!isFrozen){
            if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0){
                rb.velocity = Vector2.up * jumpForce;
                extraJumps--;
            }

            if(col.IsTouchingLayers(LayerMask.GetMask("Climbing")) || resistanceCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))){
                ClimbLadder();
            }
        }
    }

    void ClimbLadder(){
        Vector2 climbVelocity = new Vector2 (rb.velocity.x, Input.GetAxis("Vertical") * climbSpeed);
        rb.velocity = climbVelocity;
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
