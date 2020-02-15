using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNinjaStar : MonoBehaviour
{
    [SerializeField] private GameObject ninjaStar;
    private PlayerMovement player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && player.IsFacingRight == true)
        {
            player.anim.SetBool("IsShooting", true);
            Instantiate(ninjaStar, transform.position, Quaternion.Euler(0, 0, -90));


        }

        else if (Input.GetKeyDown(KeyCode.E) && player.IsFacingRight == false)
        {
            player.anim.SetBool("IsShooting", true);
            Instantiate(ninjaStar, transform.position, Quaternion.Euler(0, 0, 90));

        }

        else
        {
            player.anim.SetBool("IsShooting", false);

        }
    }
}
