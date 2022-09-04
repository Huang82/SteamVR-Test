using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 1000f;

    [SerializeField] private Rigidbody rd;

    void Start()
    {
        if (rd == null)
            rd = this.GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            rd.AddForce((Vector3.forward * Time.deltaTime * speed), ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            rd.AddForce((Vector3.back * Time.deltaTime * speed), ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rd.AddForce((Vector3.right * Time.deltaTime * speed), ForceMode.Force);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            rd.AddForce((Vector3.left * Time.deltaTime * speed), ForceMode.Force);
        }
    }
}
