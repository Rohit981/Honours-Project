using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float Health;
    private Animator anim;
    [SerializeField] private Slider HealthBar;
    private Collider2D col;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        HealthBar = GetComponentInChildren<Slider>();
        //col = GetComponent<Collider2D>();
        Health = 100;
    }

    // Update is called once per frame
    void Update()
    {
        UIUpdate();
        Dead();
    }

    void Dead()
    {
        if (Health <= 0)
        {
            HealthBar.gameObject.SetActive(false);
            anim.SetBool("IsDead", true);
           //col.enabled = false;
        }
    }

    void UIUpdate()
    {
        HealthBar.value = Health / 100;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Throwables")
        {
            Health -= 10;
        }
    }
}
