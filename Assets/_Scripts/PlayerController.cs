using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Animation anim;
    readonly string Idle = "Idle";
    readonly string RunF = "RunF";
    readonly string RunB = "RunB";
    readonly string RunR = "RunR";
    readonly string RunL = "RunL";
    [SerializeField] float moveSpeed = 10.0f;
    [SerializeField] float turnSpeed = 80.0f;
    readonly float initHp = 100.0f;
    public float currentHP;
    Transform tr;

    public delegate void PlayerDieHandler();
    public static event PlayerDieHandler OnPlayerDie;

    IEnumerator Start()
    {
        tr = GetComponent<Transform>();
        anim = GetComponent<Animation>();
        anim.Play(Idle);

        turnSpeed = 0.0f;
        currentHP = initHp;
        yield return new WaitForSeconds(0.7f);
        turnSpeed = 80.0f;
    }
    void Update()
    {
        float h = Input.GetAxisRaw(AllString.Horizontal);
        float v = Input.GetAxisRaw(AllString.Vertical);
        float r = Input.GetAxisRaw(AllString.MouseX);

        Vector3 moveDir = (Vector3.forward * v) + (Vector3.right * h);

        tr.Translate(moveSpeed * Time.deltaTime * moveDir.normalized);

        tr.Rotate(turnSpeed * Time.deltaTime * r * Vector3.up);

        PlayerAnim(h, v);
    }
    private void PlayerAnim(float h, float v)
    {
        if (v >= 0.1f)
        {
            anim.CrossFade(RunF, 0.25f);
        }
        else if (v <= -0.1f)
        {
            anim.CrossFade(RunB, 0.25f);
        }
        else if (h >= 0.1f)
        {
            anim.CrossFade(RunR, 0.25f);
        }
        else if (h <= -0.1f)
        {
            anim.CrossFade(RunL, 0.25f);
        }
        else
        {
            anim.CrossFade(Idle, 0.25f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (currentHP >= 0.0f && other.CompareTag(AllString.Punch))
        {
            currentHP -= 10.0f;
            Debug.Log($"Player Hp : ={currentHP / initHp}");

            if (currentHP <= 0.0f)
            {
                PlayerDie();
            }
        }

    }

    private void PlayerDie()
    {
        Debug.Log("Player Die !");
        //GameObject[] monsters = GameObject.FindGameObjectsWithTag("Monster");

        //foreach (GameObject monster in monsters)
        //{
        //    monster.SendMessage("OnPlayerDie", SendMessageOptions.DontRequireReceiver);
        //}

        OnPlayerDie();
    }
}

