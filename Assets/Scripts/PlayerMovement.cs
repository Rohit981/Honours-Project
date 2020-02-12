using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    [SerializeField] private float speed;
    private float MoveVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            MoveVelocity = -speed;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            MoveVelocity = speed;
        }

        rb.velocity = new Vector2(MoveVelocity, rb.velocity.y);
    }
}
