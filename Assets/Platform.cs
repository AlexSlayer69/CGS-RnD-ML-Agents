using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision) // Check the localPosition of the sphere
    {
        Debug.Log(collision.collider.transform.localPosition);

    }

   /* private void giveAngularVelocity()
    {
        
    }*/
}