using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    protected int nowHP;
    public int maxHP = 10;

    protected Animator anim;

    public float moveSpeed = 3.0f;
    public int attackDamage = 1;
    public float attackRange = 2.0f;
    public float attackCooldown = 2.0f;
    public float lastAttackTime = -Mathf.Infinity;

    protected Transform playerTransform;

    protected BoxCollider2D boxColl;
    protected CapsuleCollider2D capsuleColl;
    protected Rigidbody2D rb;

    public GameObject weapon;

    protected bool dead = false;
    protected virtual void Awake()
    {
        anim = GetComponent<Animator>();
        boxColl = GetComponent<BoxCollider2D>();
        capsuleColl = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        nowHP = maxHP;
    }
    protected virtual void Start()
    {
        playerTransform = cPlayer.Inst.transform;
    }
    protected virtual void Update()
    {
        if (dead) return;
        MoveToPlayer();
    }
    protected virtual void MoveToPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        if (transform.position.x - playerTransform.position.x > 0)
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x) * -1, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(Math.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (distanceToPlayer > attackRange)
        {
            anim.SetBool("move", true);
            Vector3 moveDirection = (playerTransform.position - transform.position).normalized;
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        }
        //if (Time.time - lastAttackTime >= attackCooldown && distanceToPlayer <= attackRange)
        //{
        //    anim.SetBool("move", false);
        //    AttackPlayer();
        //    lastAttackTime = Time.time;
        //}
    }

    public virtual void Hurt(int atk)
    {
        nowHP -= atk;
        if (nowHP < 0)
        {
            dead = true;
            Dead();
        }
    }
    protected virtual void Dead()
    {
        anim.SetBool("dead", true);
        boxColl.enabled = false;
        capsuleColl.enabled = false;
        rb.drag = 100;
    }
}
