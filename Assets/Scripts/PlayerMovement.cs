using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public struct Inputs
    {
        public byte Jump;
        public byte Move;
        public byte MoveBackward;
        public byte Attack;
    }

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

    public Inputs inputStruct;

    public float frameCount;

    internal bool IsRewinding = false;
    internal bool IsAddElement = false;

    internal List<Inputs> RecentInput = new List<Inputs>();

    Inputs CurrentInput;

    internal bool IsPredicting = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxcollider2D = GetComponent<BoxCollider2D>();
        input = FindObjectOfType<InputManager>();

        //IsGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //frameCount = Time.frameCount;
        frameCount += Time.deltaTime  ;
        MovementInput();

        

    }

    private void Update()
    {
        if(IsPredicting == true)
        {
           PredictedInput();
           IsPredicting = false;

        }
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

        if(inputStruct.Move == 1)
        {
            float Movement = 0.05f*speed * Time.deltaTime;
            Movement++;
            transform.position = new Vector2(transform.position.x + Movement, transform.position.y);
            //sprite.flipX = false;
            characterScale.x = 1;
            anim.SetBool("IsRunning", true);
            inputStruct.Move = 0;
            IsAddElement = true;

        }

        else if (inputStruct.MoveBackward == 1)
        {
            float Movement = 0.05f*speed * Time.deltaTime;
            Movement--;
            transform.position = new Vector2(transform.position.x + Movement, transform.position.y);
            //sprite.flipX = true;
            characterScale.x = -1;
            anim.SetBool("IsRunning", true);
            inputStruct.MoveBackward = 0;
            IsAddElement = true;
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

        if (inputStruct.Jump == 1  && IsGrounded == true )
        {
            
            rb.AddForce(Vector2.up * JumpHeight);
            inputStruct.Jump = 0;
            IsAddElement = true;

        }


        Debug.DrawRay(boxcollider2D.bounds.center, Vector2.down*(boxcollider2D.bounds.extents.y + .05f), rayColor);

        //Debug.Log(hitInfo.collider);

    }

    
    public void AddJumpInputMessage(Inputs InputValue)
    {
       
        RecentInput.Add(InputValue);

        

    }

    void PredictedInput()
    {
        if (RecentInput[0].Move == 1)
        {
            inputStruct.Move = 1;
        }

        if (RecentInput[0].MoveBackward == 1)
        {
            inputStruct.MoveBackward = 1;
        }

        if (RecentInput[0].Jump == 1)
        {
            inputStruct.Jump = 1;
        }

        if (RecentInput[0].Attack == 1)
        {
            inputStruct.Attack = 1;
        }

        if (RecentInput.Count == 1)
        {
            RecentInput.RemoveAt(0);
        }
    }

   

   




}
