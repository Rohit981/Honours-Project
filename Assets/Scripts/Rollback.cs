using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rollback : MonoBehaviour
{
    private bool IsRewinding = false;

    private List<Vector2> positions;

    internal bool IsForwardRecording = false;
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Vector2>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Return))
        {
            //IsForwardRecording = true;
            StartRewinding();

        }

        if (Input.GetKeyUp(KeyCode.E))
        {

            StopRewinding();

        }

    }

    private void FixedUpdate()
    {
        if (IsRewinding == true)
            Rewind();

        else
            Record();

       
    }

    void Rewind()
    {
        //positions.Insert(1, transform.position);
        transform.position = positions[0];
        positions.Remove(transform.position);

        
        //StopRewinding();
    }

    void Record()
    {
        positions.Add( transform.position);
      
    }

    void StartRewinding()
    {
        IsRewinding = true;
    }

    void StopRewinding()
    {
        IsRewinding = false;
    }

  
}
