using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    [SerializeField] private float JumpHeight;
    private float MoveVelocity;
    private Animator anim;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            characterScale.x = 0.81f;
            anim.SetBool("IsRunning", true);
        }       

       else if (Input.GetAxis("Horizontal") < 0)
       {
           characterScale.x = -0.81f;
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
        float JumpSpeed = Input.GetAxis("Jump") * JumpHeight * Time.deltaTime;

        transform.position = new Vector2(transform.position.x, transform.position.y + JumpSpeed);
    }
}
