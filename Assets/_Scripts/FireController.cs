using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireController : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Fire();
        }
    }

    private void Fire()
    {
        Instantiate(bullet, firePos.position, firePos.rotation);
    }
}
