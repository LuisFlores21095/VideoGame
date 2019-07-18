using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The Green goblin moves back and forth like the blue goblin
//The only difference is that it stops and moves on a timer
//The attack cooldown is also double
//Overall the green goblin is weaker
public class gGoblinAI : MonoBehaviour
{
    public float attackCooldown = 2.0f;
    public float moveSpeed = 1.0f;
    public float moveCooldown = 5.0f;

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
    bool move = true; //whether to move or not
    bool jumping;
    bool pause = false;



    float oldMoveSpeed;
    float attackTimer;
    float moveTimer;

    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        moveTimer = moveCooldown;
        attackTriggerFront.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        isGrounded = Physics2D.Linecast(pos, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground")); //check directly infront of feet for edge
        wallAhead = Physics2D.Linecast(pos, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")); //sets true if detects wall ahead
        playerAhead = Physics2D.Linecast(pos, playerCheck.position, 1 << LayerMask.NameToLayer("Player")); //sets true if player is ahead
        animator.SetBool("Move", move);

        if (moveTimer <= 0 && !pause) //if moving and move timer ran out
        {
            pause = true; // stop moving
            jumping = true;
        }
 
        if (pause)
        {
            moveTimer += Time.deltaTime;
            

        }
        if (jumping && isGrounded)
        {
            animator.SetTrigger("startjump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5), ForceMode2D.Impulse);


        }
        if (moveTimer >= 1f && pause) //if not moving and move cooldown reset
        {
            pause = false; //start moving
            jumping = false;
            moveTimer = 5f;
        }
        if (isGrounded)
        {

            animator.SetBool("isJumping", false);
        }
        else
        {
            animator.SetBool("isJumping", true);

        }

        if (!playerAhead && !pause)
        {
            if (isGrounded && !wallAhead) //if there is ground ahead, and no wall ahead, keep moving
            {
                if (move) //if able to move, move
                {
                    pos.x += moveSpeed * Time.deltaTime;
                    moveTimer -= Time.deltaTime;
                }
                
            }
            else  //else, turn around
            {
                moveSpeed *= -1;
                facingRight = !facingRight;
                Vector2 charScale = transform.localScale;
                charScale.x *= -1;
                transform.localScale = charScale;
            }
        }
        if (!attack && playerAhead && !pause) //if player is ahead, attack
        {
            animator.SetTrigger("Attack");
            oldMoveSpeed = moveSpeed; //save current moving direction
            moveSpeed = 0; //stop moving to start attack
            attack = true;
            attackTimer = attackCooldown;
            attackTriggerFront.enabled = true;
        }

        if (attack && !pause)
        {
            if (attackTimer < (attackCooldown - 0.3))
            {
                attackTriggerFront.enabled = false;
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                moveSpeed = oldMoveSpeed; //once attack has finished, start moving again
                attack = false;
            }
        }
        

        

        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }

    public void Damage(int dmg) //detects attack from player (can add HP and stuff here later)
    {
        Destroy(gameObject);
    }
}
