using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    internal bool IsJumpPressed = false;
    internal bool IsForwardPressed = false;
    internal bool IsBackPressed = false;
    internal bool IsAttackPressed = false;

  
    private NetworkManager networkManager;

    private void Start()
    {
       
        networkManager = FindObjectOfType<NetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            IsJumpPressed = true;
            networkManager.input.Jump = 1;
        }
        else
        {
            IsJumpPressed = false;
            networkManager.input.Jump = 0;

        }

        if (Input.GetAxis("Horizontal") > 0)
        {
            IsForwardPressed = true;
            networkManager.input.Move = 1;

        }

        else
        {
            networkManager.input.Move = 0;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            IsBackPressed = true;
            networkManager.input.MoveBackward = 1;

        }

        else
        {
            networkManager.input.MoveBackward = 0;
        }

        //else
        //{
        //    IsBackPressed = false;
        //    networkManager.input.Move = 0;
        //    networkManager.input.MoveBackward = 0;


        //}

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            IsAttackPressed = true;
            networkManager.input.Attack = 1;
        }

        else
        {
            IsAttackPressed = false;
            networkManager.input.Attack = 0;

        }
    }
}
