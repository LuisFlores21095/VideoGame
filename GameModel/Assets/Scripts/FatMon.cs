﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatMon : MonoBehaviour
{
    public float attackCooldown = 1.0f;
    public float moveSpeed = 1.0f;
    int random;
    public Transform edgeCheck;
    public Transform wallCheck;
    public Transform playerCheck;
    public Collider2D attackTriggerFront;
    public Animator animator;

    bool isGrounded = true;
    bool wallAhead = false;
    bool playerAhead = false;
    bool attack = false;
    bool facingRight = true;
    bool charging = false;
    float oldMoveSpeed;
    float attackTimer;

    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        attackTriggerFront.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        isGrounded = Physics2D.Linecast(pos, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground")); //check directly infront of feet for edge
        wallAhead = Physics2D.Linecast(pos, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")); //sets true if detects wall ahead
        playerAhead = Physics2D.Linecast(pos, playerCheck.position, 1 << LayerMask.NameToLayer("Player")); //sets true if player is ahead

        if (!playerAhead)
        {
            if (isGrounded && !wallAhead) //if there is ground ahead, and no wall ahead, keep moving
            {
                pos.x += moveSpeed * Time.deltaTime;
            }
            else //else, turn around
            {
                attackTriggerFront.enabled = false;
                charging = false;
                random = Random.Range(0, 2);

                if (random == 0)
                {
                    animator.SetTrigger("isWalking");
                    if (moveSpeed < 0)
                    {
                        moveSpeed = 1f;
                    }
                    else {
                        moveSpeed = -1f;

                    }
                }

               if (random == 1)
                {
                    animator.SetTrigger("isCharging");
                    charging = true;
                    attackTriggerFront.enabled = true;

                    if (moveSpeed < 0)
                    {
                        moveSpeed = 3f;
                    }
                    else
                    {
                        moveSpeed = -3f;

                    }
                }
                facingRight = !facingRight;
                Vector2 charScale = transform.localScale;
                charScale.x *= -1;
                transform.localScale = charScale;
            }
        }
        if (!attack && playerAhead && !charging) //if player is ahead, attack
        {
            animator.SetTrigger("isAttacking");
            attack = true;

           

            oldMoveSpeed = moveSpeed; //save current moving direction
            moveSpeed = 0; //stop moving to start attack
        }


        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }

    public void Damage(int dmg) //detects attack from player (can add HP and stuff here later)
    {
        Destroy(gameObject);
    }

    public void AlertObservers(string message)
    {
        if (message.Equals("attack"))
        {

            attackTriggerFront.enabled = true;

        }

        if (message.Equals("attackEnd"))
        {
            attackTriggerFront.enabled = false;
            attack = false;


            moveSpeed = oldMoveSpeed; //once attack has finished, start moving again

        }
    }
}
