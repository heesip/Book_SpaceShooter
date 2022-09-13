﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class MonsterController : MonoBehaviour
{
    readonly int hashTrace = Animator.StringToHash("isTrace");
    readonly int hashAttack = Animator.StringToHash("isAttack");
    readonly int hashHit = Animator.StringToHash("Hit");
    readonly int hashPlayerDie = Animator.StringToHash("PlayerDie");
    readonly int hashSpeed = Animator.StringToHash("Speed");
    readonly int hashDie = Animator.StringToHash("Die");
    public enum State { IDLE, TRACE, ATTACK, DIE }

    Transform monsterTr;
    Transform target;
    NavMeshAgent agent;
    Animator anim;

    [SerializeField] State state = State.IDLE;
    [SerializeField] float traceDist = 10.0f;
    [SerializeField] float attackDist = 2.0f;
    [SerializeField] bool isDead;
    float thinkTime = 0.3f;
    float bloodDuration = 1.0f;
    int hp = 100;

    GameObject bloodEffect;
    public SphereCollider RightHand;
    public SphereCollider LeftHand;

    private void OnEnable()
    {
        PlayerController.OnPlayerDie += this.OnPlayerDie;
    }

    private void OnDisable()
    {
        PlayerController.OnPlayerDie -= this.OnPlayerDie;
    }

    void Start()
    {
        monsterTr = GetComponent<Transform>();
        target = GameObject.FindWithTag(AllString.Player).GetComponent<Transform>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        bloodEffect = Resources.Load<GameObject>("BloodSprayEffect");

        StartCoroutine(CheckMonsterState());
        StartCoroutine(MonsterAction());

    }

    private void OnDrawGizmos()
    {
        if (state == State.TRACE)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, traceDist);
        }

        if (state == State.ATTACK)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackDist);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(AllString.Bullet))
        {
            Destroy(collision.gameObject);
            anim.SetTrigger(hashHit);

            Vector3 pos = collision.GetContact(0).point;

            Quaternion rot = Quaternion.LookRotation(-collision.GetContact(0).normal);

            ShowBloodEffect(pos, rot);

            hp -= 10;
            if (hp <= 0)
            {
                state = State.DIE;
            }
        }
    }

    IEnumerator CheckMonsterState()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(thinkTime);

            if (state == State.DIE)
            {
                yield break;
            }

            float distance = Vector3.Distance(target.position, monsterTr.position);

            if (distance <= attackDist)
            {
                state = State.ATTACK;
            }

            else if (distance <= traceDist)
            {
                state = State.TRACE;
            }
            else
            {
                state = State.IDLE;
            }
        }
    }

    IEnumerator MonsterAction()
    {
        while (!isDead)
        {
            switch (state)
            {
                case State.IDLE:
                    agent.isStopped = true;
                    anim.SetBool(hashTrace, false);
                    break;
                case State.TRACE:
                    //transform.LookAt(target);
                    agent.SetDestination(target.position);
                    agent.isStopped = false;
                    anim.SetBool(hashTrace, true);
                    anim.SetBool(hashAttack, false);
                    break;
                case State.ATTACK:
                    anim.SetBool(hashAttack, true);
                    break;
                case State.DIE:
                    isDead = true;
                    agent.isStopped = true;
                    anim.SetTrigger(hashDie);
                    GetComponent<CapsuleCollider>().enabled = false;
                    break;
            }
            yield return new WaitForSeconds(thinkTime);
        }
    }
    private void ShowBloodEffect(Vector3 pos, Quaternion rot)
    {
        GameObject blood = Instantiate<GameObject>(bloodEffect, pos, rot, monsterTr);
        Destroy(blood, bloodDuration);
    }

    void OnPlayerDie()
    {
        StopAllCoroutines();

        agent.isStopped = true;
        anim.SetFloat(hashSpeed, Random.Range(0.8f, 1.2f));
        anim.SetTrigger(hashPlayerDie);
    }

}
