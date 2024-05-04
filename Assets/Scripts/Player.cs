using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Player : MonoBehaviour
{

    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer sr;
    
    
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        GroundCheckRadius = GroundCheck.GetComponent<CircleCollider2D>().radius;
    }



    private void Update()
    {
        Walk();
        Reflect();
        CheckingGround();
        Jump();
        Dash();
    }




    public Vector2 moveVector;
    public float speed = 2f;

    private void Walk()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        anim.SetFloat("moveX", Mathf.Abs(moveVector.x));
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }



    public bool FaceRight = true;

    private void Reflect()
    {
        if ((moveVector.x > 0 && !FaceRight) || (moveVector.x < 0 && FaceRight))
        {
            transform.localScale *= new Vector2(-1,1);
            FaceRight = !FaceRight;
        }
       
    }



    public float JumpForce = 40;
    private bool JumpControl;
    private float JumpTime = 0;
    public float JumpControlTime = 0.3f;
    public int JumpCount = 2;
    public int MomentJumpCount = 0;
    public float DoubleJumpForce = 12;


    private void Jump()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (onGround)
            {
                JumpControl = true;
            }
        }
        else
        {
            JumpControl = false;
        }

        if (JumpControl)
        {
            if ((JumpTime += Time.fixedDeltaTime) < JumpControlTime)
            {
                rb.AddForce(Vector2.up * JumpForce / (JumpTime * 10));
            }
        }
        else
        {
            JumpTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && !onGround && (++MomentJumpCount < JumpCount))
        {
            rb.velocity = new Vector2(0, DoubleJumpForce);
        }
        if (onGround)
        {
            MomentJumpCount = 0;
        }
    }



    public bool onGround;
    public Transform GroundCheck;
    public float GroundCheckRadius;
    public LayerMask Ground;

    private void CheckingGround()
    {
        onGround = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, Ground);
        anim.SetBool("onGround", onGround);
    }



    public int DashImpulse = 5000;
     
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            rb.velocity = new Vector2(0, 0);
            if (!FaceRight)
            {
                rb.AddForce(Vector2.left * DashImpulse);
            }
            else { rb.AddForce(Vector2.right * DashImpulse); }
        }
    }


}

