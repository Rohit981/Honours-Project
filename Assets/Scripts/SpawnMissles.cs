using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMissles : MonoBehaviour
{
    [SerializeField] private GameObject Missile;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(Missile, transform.position, Quaternion.Euler(0,0,-90));
        }
    }
}
