﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNinjaStar : MonoBehaviour
{
    [SerializeField] private GameObject ninjaStar;
    [SerializeField] private InputManager input;
    private PlayerMovement player;
    private float shootRate;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        shootRate = 1f;
        input = FindObjectOfType<InputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        shootRate += Time.deltaTime;
        if(shootRate >= 0.8f)
        {
            //if (player.IsRefMe == true)
            //{
                Shooting();
            //}
        }
    }

    void Shooting()
    {
        if (input.IsAttackPressed == true && player.IsFacingRight == true)
        {
            player.anim.SetBool("IsShooting", true);
            Instantiate(ninjaStar, transform.position, Quaternion.Euler(0, 0, -90));
            shootRate = 0;

        }

        else if (input.IsAttackPressed == true && player.IsFacingRight == false)
        {
            player.anim.SetBool("IsShooting", true);
            Instantiate(ninjaStar, transform.position, Quaternion.Euler(0, 0, 90));
            shootRate = 0;

        }

        else
        {
            player.anim.SetBool("IsShooting", false);

        }

    }
}
