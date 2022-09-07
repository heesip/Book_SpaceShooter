using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animation anim;
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float turnSpeed = 80.0f;


    IEnumerator Start()
    {
        anim = GetComponent<Animation>();
        anim.Play("Idle");

        turnSpeed = 0.0f;
        yield return new WaitForSeconds(0.7f);
        turnSpeed = 80.0f;
    }
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        float r = Input.GetAxisRaw("Mouse X");

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        transform.Translate(moveSpeed * Time.deltaTime * moveDir.normalized);

        transform.Rotate(turnSpeed * Time.deltaTime * r * Vector3.up);

        PlayerAnim(h, v);
    }
    private void PlayerAnim(float h, float v)
    {
        if (v >= 0.1f)
        {
            anim.CrossFade("RunF", 0.25f);
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade("RunB", 0.25f);
        }
        else if(h >= 0.1f)
        {
            anim.CrossFade("RunR", 0.25f);
        }
        else if(h <= -0.1f)
        {
            anim.CrossFade("RunL", 0.25f);
        }
        else
        {
            anim.CrossFade("Idle", 0.25f);
        }
    }
}
