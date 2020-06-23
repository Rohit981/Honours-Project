using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float JumpHeight;
    internal Animator anim;
    public bool IsGrounded;
    [SerializeField] private LayerMask groundLayer;
    private BoxCollider2D boxcollider2D;
    internal bool IsFacingRight;
    internal bool IsRefMe = false;
    [SerializeField] private InputManager input;
    private NetworkManager networkManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider2D = GetComponent<BoxCollider2D>();
        input = FindObjectOfType<InputManager>();
        networkManager = FindObjectOfType<NetworkManager>();
        //IsGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (IsRefMe == true)
        {
            MovementInput();

        }

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

        if(input.IsForwardPressed == true)
        {
            //sprite.flipX = false;
            characterScale.x = 1;
            anim.SetBool("IsRunning", true);

        }       

        else if (input.IsBackPressed == true)
        {
            //sprite.flipX = true;
            characterScale.x = -1;    
            anim.SetBool("IsRunning", true);

        }

        else
        {
            anim.SetBool("IsRunning", false);

        }

        transform.localScale = characterScale;

        if(characterScale.x == 1)
        {
            IsFacingRight = true;

        }
        else if(characterScale.x == -1)
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

        if (input.IsJumpPressed == true  && IsGrounded == true )
        {
            
            rb.AddForce(Vector2.up * JumpHeight);
           
        }


        Debug.DrawRay(boxcollider2D.bounds.center, Vector2.down*(boxcollider2D.bounds.extents.y + .05f), rayColor);

        //Debug.Log(hitInfo.collider);

    }

    void JumpRaycast()
    {

    }

   

   




}
