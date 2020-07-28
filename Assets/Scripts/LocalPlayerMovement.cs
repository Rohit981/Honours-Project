using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalPlayerMovement : MonoBehaviour
{  // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] internal float speed;
    [SerializeField] private float JumpHeight;
    internal Animator anim;
    public bool IsGrounded;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxcollider2D;
    internal bool IsFacingRight;
    internal bool IsRefMe = false;

    internal bool IsAddElement = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider2D = GetComponent<BoxCollider2D>();
      

        //IsGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        MovementInput();

    }



    void MovementInput()
    {

        //float Movement = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        //transform.position = new Vector2(transform.position.x + Movement, transform.position.y);


        FlipCharacter();

        Jump();
    }

    void FlipCharacter()
    {
        Vector3 characterScale = transform.localScale;

        if (Input.GetAxis("Horizontal") > 0)
        {
            float Movement = 0.05f * speed * Time.deltaTime;
            Movement++;
            transform.position = new Vector2(transform.position.x + Movement, transform.position.y);
            //sprite.flipX = false;
            characterScale.x = 1;
            anim.SetBool("IsRunning", true);

            IsAddElement = true;

        }

        else if (Input.GetAxis("Horizontal") < 0)
        {
            float Movement = 0.05f * speed * Time.deltaTime;
            Movement--;
            transform.position = new Vector2(transform.position.x + Movement, transform.position.y);
            //sprite.flipX = true;
            characterScale.x = -1;
            anim.SetBool("IsRunning", true);

        }

        else
        {
            anim.SetBool("IsRunning", false);

        }

        transform.localScale = characterScale;

        if (characterScale.x == 1)
        {
            IsFacingRight = true;

        }
        else if (characterScale.x == -1)
        {
            IsFacingRight = false;
        }
    }

    void Jump()
    {
        RaycastHit2D hitInfo;
        hitInfo = Physics2D.Raycast(boxcollider2D.bounds.center, Vector2.down, boxcollider2D.bounds.extents.y + .05f, groundLayer);
        Color rayColor;

        if (hitInfo.collider != null)
        {
            // Debug.Log("Touching the ground");
            IsGrounded = true;
            rayColor = Color.red;
            anim.SetBool("IsJumping", false);

        }
        else
        {
            // Debug.Log("Not Touching the ground");
            IsGrounded = false;
            rayColor = Color.green;
            anim.SetBool("IsJumping", true);
            anim.SetBool("IsRunning", false);

        }

        if (Input.GetAxis("Jump") > 0 && IsGrounded == true)
        {

            rb.AddForce(Vector2.up * JumpHeight);

        }


        Debug.DrawRay(boxcollider2D.bounds.center, Vector2.down * (boxcollider2D.bounds.extents.y + .05f), rayColor);

        //Debug.Log(hitInfo.collider);

    }
}
