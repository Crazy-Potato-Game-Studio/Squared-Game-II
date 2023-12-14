using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    public float speed;
    public float jumpForce;
    float moveInput;
    float climbSpeed = 7f;

    private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;
    
    public int extraJumps;

    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");

        canMove = true;

        if(canMove){
            rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
        }else{
            rb.AddForce(new Vector2(1000f, 0), ForceMode2D.Impulse);
            Debug.Log("Add knockback");
            canMove = true;
        }
        
    }

    void Update(){

        if(isGrounded){
            extraJumps = 1;
        }

        if(Input.GetKeyDown(KeyCode.Space) && extraJumps > 0){
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }

        ClimbLadder();

    }

    void ClimbLadder(){

        if(!col.IsTouchingLayers(LayerMask.GetMask("Climbing"))){return;}

        Vector2 climbVelocity = new Vector2 (rb.velocity.x, Input.GetAxis("Vertical") * climbSpeed);
        rb.velocity = climbVelocity;
    }
}
