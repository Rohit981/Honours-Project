using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastForward : MonoBehaviour
{
    private bool IsForwarding = false;

    private List<Vector2> positions;

    private Rollback rollback;
    // Start is called before the first frame update
    void Start()
    {
        positions = new List<Vector2>();

        rollback = GetComponent<Rollback>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            StartForwarding();
    }

    private void FixedUpdate()
    {
        if (IsForwarding == true)
            Forward();

       if(rollback.IsForwardRecording == true)
            Record();


    }

    void Forward()
    {
        transform.position = positions[0];
        positions.RemoveAt(0);
    }

    void Record()
    {
        positions.Insert(0, transform.position);

    }

    void StartForwarding()
    {
        IsForwarding = true;
    }
}
