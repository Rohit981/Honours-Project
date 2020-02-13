using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float JumpHeight;
    private SpriteRenderer sprite;
    private float MoveVelocity;
    private Animator anim;
    public bool IsGrounded;
    [SerializeField] private LayerMask groundLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        //IsGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementInput();
        
    }

    void MovementInput()
    {

        float Movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.position = new Vector2(transform.position.x + Movement, transform.position.y);
             

        FlipCharacter();

        Jump();
    }

    void FlipCharacter()
    {
        Vector3 characterScale = transform.localScale;

        if(Input.GetAxis("Horizontal") > 0)
        {
            sprite.flipX = false;
            anim.SetBool("IsRunning", true);
        }       

       else if (Input.GetAxis("Horizontal") < 0)
       {
           sprite.flipX = true;
           anim.SetBool("IsRunning", true);
       }

       else
       {
          anim.SetBool("IsRunning", false);

       }

        transform.localScale = characterScale;
    }

    void Jump()
    {
        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(transform.position + new Vector3(0, sprite.bounds.extents.y - 3.3f, 0), Vector2.down, 1f, groundLayer);

        if (hitInfo)
        {
            Debug.Log("Touching the ground");
            IsGrounded = true;
         
        }
        else
        {
            Debug.Log("Not Touching the ground");
            IsGrounded = false;

        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded == true)
        {
            rb.AddForce(Vector2.up * JumpHeight);
        }

        Debug.DrawRay(transform.position + new Vector3(0,sprite.bounds.extents.y - 3.3f,0), Vector2.down * 1f, Color.red);

        if(IsGrounded == true)
        {
            anim.SetBool("IsJumping", false);

        }
        else if(IsGrounded == false)
        {
            anim.SetBool("IsJumping", true);
        }
    }
}
