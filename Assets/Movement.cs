using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using FixedUpdate = Unity.VisualScripting.FixedUpdate;

public class Movement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    private Vector3 direction;
    [SerializeField] private float force;
    void Update()
    {
        direction = new Vector3(
            Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")
        );
    }

    private void FixedUpdate()
    {
        if (direction == Vector3.zero)
        {
            return;
        }
        
        rb.AddForce(direction * force);
    }
}
