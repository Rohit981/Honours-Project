using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollback : MonoBehaviour
{

    private PlayerMovement player;

    private List<Vector2> positions;

    //private LocalPlayerMovement local;

    internal bool IsForwardRecording = false;

    //bool IsRewinding = false;
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Vector2>();
        player = GetComponentInParent<PlayerMovement>();
        positions.Add(transform.position);

        //local = GetComponentInParent<LocalPlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKey(KeyCode.Return))
        //{
        //    //IsForwardRecording = true;
        //    StartRewinding();

        //}

        //if (Input.GetKeyUp(KeyCode.E))
        //{

        //    StopRewinding();

        //}

    }

    private void FixedUpdate()
    {
        if (player.IsRewinding == true)
            Rewind();

        else
            Record();

       
    }

    void Rewind()
    {
        //positions.Insert(1, transform.position);
        transform.position = positions[0];
        positions.Remove(transform.position);

        if(positions.Count == 0)
        player.IsRewinding = false;
        
        //StopRewinding();
    }

    void Record()
    {
        if(player.IsAddElement == true)
        {
          positions.Add( transform.position);

            player.IsAddElement = false;

        }
      
    }

    //void StartRewinding()
    //{
    //    IsRewinding = true;
    //}

    //void StopRewinding()
    //{
    //    IsRewinding = false;
    //}


}
