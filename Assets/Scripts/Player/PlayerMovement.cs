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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * speed, rb.velocity.y);
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
        //Na razie psuje wszystko
        //Swim();

    }

    void Swim(){
        if(!col.IsTouchingLayers(LayerMask.GetMask("Woda"))){return;}

        Vector2 swimVelocity = new Vector2 (rb.velocity.x /2, rb.velocity.y /2 );
        rb.velocity = swimVelocity;
    }

    void ClimbLadder(){

        if(!col.IsTouchingLayers(LayerMask.GetMask("Climbing"))){return;}

        Vector2 climbVelocity = new Vector2 (rb.velocity.x, Input.GetAxis("Vertical") * climbSpeed);
        rb.velocity = climbVelocity;
    }

    
}
