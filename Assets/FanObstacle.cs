using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanObstacle : MonoBehaviour
{
    [SerializeField] private float speed;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime, Space.Self);
    }
}
