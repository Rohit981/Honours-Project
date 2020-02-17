using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectForce : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float rocketSpeed;
    private float DestroyTime;
    [SerializeField] private float AliveTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DestroyTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Force();
        DestroyGameObject();
    }

    void Force()
    {
        if(transform.localRotation.z > 0)
        {
            rb.AddForce(new Vector2(-1,0) * rocketSpeed, ForceMode2D.Force);

        }
        else 
        {
            rb.AddForce(new Vector2(1,0) * rocketSpeed, ForceMode2D.Force);

        }

    }

    void DestroyGameObject()
    {
        DestroyTime += Time.deltaTime;

        if(DestroyTime >= AliveTime)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(this.gameObject);
        }
    }
}
