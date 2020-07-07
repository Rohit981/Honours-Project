using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectile : MonoBehaviour
{
    [SerializeField] private GameObject projectile;   
    private PlayerMovement player;
    private float shootRate;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
        shootRate = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        shootRate += Time.deltaTime;
        if(shootRate >= 0.8f)
        {
           
             Shooting();
           
        }
    }

    void Shooting()
    {
        if (player.inputStruct.Attack == 1 && player.IsFacingRight == true)
        {
            player.anim.SetBool("IsShooting", true);
            Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, -90));
            shootRate = 0;
            player.inputStruct.Attack = 0;

        }

        else if (player.inputStruct.Attack == 1 && player.IsFacingRight == false)
        {
            player.anim.SetBool("IsShooting", true);
            Instantiate(projectile, transform.position, Quaternion.Euler(0, 0, 90));
            shootRate = 0;
            player.inputStruct.Attack = 0;

        }

        else
        {
            player.anim.SetBool("IsShooting", false);

        }

    }
}
