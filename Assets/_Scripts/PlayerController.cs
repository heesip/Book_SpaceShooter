using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10.0f;
    private void Start()
    {
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        //Debug.Log("h = " + h);
        //Debug.Log("v = " + v);
        //transform.position += new Vector3(h, 0, v).normalized;

        transform.Translate(Vector3.forward * Time.deltaTime * v * moveSpeed);
    }
}
