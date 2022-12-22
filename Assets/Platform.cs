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
   public void rotateX(float angle)
    {
        rb.rotation = Quaternion.Euler(angle,
            rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.z);
    }
    public void rotateZ(float angle)
    {
        rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles.x,
            rb.rotation.eulerAngles.y,angle);
    }
}
