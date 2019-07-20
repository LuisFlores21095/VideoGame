using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GKAI : MonoBehaviour
{
    public float attackCooldown = 1.0f;
    public float moveSpeed = -1.0f;

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
    bool turning = false;
    bool charging = false;
    bool throwing = false;

    float oldMoveSpeed;
    float attackTimer;
    int randomNext;

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
        animator.SetBool("isAttacking", attack);

       
       

        if (!playerAhead && !turning && !throwing && !attack)
        {



            if (isGrounded && !wallAhead) //if there is ground ahead, and no wall ahead, keep moving
            {
                pos.x += moveSpeed * Time.deltaTime;
            }
            else //else, turn around
            {
                animator.SetBool("TurnAround", true);
                turning = true;
                oldMoveSpeed = moveSpeed; //save current moving direction
                moveSpeed = 0;
                // moveSpeed *= -1;
                //facingRight = !facingRight;
                // Vector2 charScale = transform.localScale;
                //charScale.x *= -1;
                //transform.localScale = charScale;
            }
        }



        if (!attack && playerAhead && !turning && !charging && !throwing) //if player is ahead, attack
        {
            oldMoveSpeed = moveSpeed; //save current moving direction
            moveSpeed = 0; //stop moving to start attack
            animator.SetInteger("attackNum", Random.Range(0, 3));

            
            attack = true;
            attackTimer = attackCooldown;
        }




        if (attack && !turning && !charging)
        {
            if (attackTimer < (attackCooldown - 0.3))
            {
                attackTriggerFront.enabled = true;
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
          
        }



        transform.position = pos;



    }




    public void Damage(int dmg) //detects attack from player (can add HP and stuff here later)
    {
        Destroy(gameObject);
    }
   
        public void AlertObservers(string message)
    {
        
            if (message.Equals("attack"))
            {
            attack = false;
            attackTriggerFront.enabled = false;
            moveSpeed = oldMoveSpeed; //once attack has finished, start moving again
            if (moveSpeed >= 0)
            {
                moveSpeed = 1f;
            }
            else
            {
                moveSpeed = -1f;
            }

        }

            

            if (message.Equals("throwing"))
        {
            throwing = false;
            animator.SetTrigger("isWalking");
            if (moveSpeed >= 0)
            {
                moveSpeed = 1f;
            }
            else
            {
                moveSpeed = -1f;
            }

        }




        if (message.Equals("turning"))
        {
            animator.SetBool("TurnAround", false);

            moveSpeed = oldMoveSpeed;
            charging = false;
            turning = false;
            // Do other things based on an attack ending.

            facingRight = !facingRight;
            Vector2 charScale = transform.localScale;
            charScale.x *= -1;
            transform.localScale = charScale;
            moveSpeed *= -1;
            randomNext = Random.Range(0, 4);

            if (randomNext == 0)
            {
                animator.SetTrigger("isCharging");
                if (moveSpeed >= 0)
                {
                    moveSpeed = 6f;
                }
                else
                {
                    moveSpeed = -6f;
                }

                charging = true;
            }
            else if (randomNext == 1)
            {
                animator.SetTrigger("isWalking");

                if (moveSpeed >= 0)
                {
                    moveSpeed = 1f;
                }
                else
                {
                    moveSpeed = -1f;
                }

            }
            else if (randomNext == 2)
            {
                animator.SetTrigger("isThrowing");
                throwing = true;

            }
            else if (randomNext == 3)
            {
                animator.SetTrigger("isRunning");

                if (moveSpeed >= 0)
                {
                    moveSpeed = 6f;
                }
                else
                {
                    moveSpeed = -6f;
                }


            }
        }
    }
}
