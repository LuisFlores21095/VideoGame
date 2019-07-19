using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Jumping goblin follows the player across the map
//Periodically stops and screams to give the player an opening to attack
//Jumps if the player position is higher than the jumping goblin
public class jumpingGoblinAI : MonoBehaviour
{
    public float attackCooldown = 1.0f;
    public float moveSpeed = 2.0f;
    public float moveCooldown = 5.0f;

    public Transform edgeCheck;
    public Transform wallCheck;
    public Transform playerCheck;
    public Transform player;
    public Collider2D attackTriggerFront;
    public Animator animator;

    bool isGrounded = true;
    bool wallAhead = false;
    bool playerAhead = false;
    bool attack = false;
    bool playerUp = true;
    bool jump = false;
    bool move = true;
    bool next = false;
    bool playerDown = false;
    bool playerX = false;

    float oldMoveSpeed;
    float attackTimer;
    float moveTimer;

    Vector2 pos;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        attackTriggerFront.enabled = false;
        moveTimer = moveCooldown;

        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        isGrounded = Physics2D.Linecast(pos, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground")); //check directly infront of feet for edge
        wallAhead = Physics2D.Linecast(pos, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")); //sets true if detects wall ahead
        playerAhead = Physics2D.Linecast(pos, playerCheck.position, 1 << LayerMask.NameToLayer("Player")); //sets true if player is ahead
        playerUp = (player.transform.position.y >= gameObject.transform.position.y + 1.0f);
        playerX = (player.transform.position.x - .1f <= gameObject.transform.position.x  && player.transform.position.x + .1f >= gameObject.transform.position.x );
        playerDown = (player.transform.position.y <= gameObject.transform.position.y + 1.0f);

        if (playerDown && playerX) {
            print("below");
        }


        animator.SetBool("Move", move);
        
        if (playerUp && !jump)
        {
            if (move)
            {
                if (player.transform.position.x >= gameObject.transform.position.x)// if player is to the left
                {
                    animator.SetTrigger("startJump");
                    rb.AddForce(new Vector2(moveSpeed, 8.0f), ForceMode2D.Impulse);
                    jump = true;
                    isGrounded = false;

                }
                else
                {
                    animator.SetTrigger("startJump");

                    rb.AddForce(new Vector2(-moveSpeed, 8.0f), ForceMode2D.Impulse);
                    jump = true;
                    isGrounded = false;

                }
            }
            else
            {
                moveTimer += Time.deltaTime;
            }
        }

        

        if (move)
        {
            if (player.transform.position.x >= gameObject.transform.position.x)// if player is to the left
            {
                moveTimer -= Time.deltaTime;
                pos.x += moveSpeed * Time.deltaTime;
                if (!playerX)
                {
                    Vector2 charScale = transform.localScale;
                    charScale.x = 1;
                    transform.localScale = charScale;
                }
            }
            else
            {
                pos.x += -moveSpeed * Time.deltaTime;
                if (!playerX)
                {
                    Vector2 charScale = transform.localScale;
                    charScale.x = -1;
                    transform.localScale = charScale;
                }
            }
            moveTimer -= Time.deltaTime;

            if (!attack && playerAhead && !jump) //if player is ahead, attack
            {
                animator.SetTrigger("Attack");
                oldMoveSpeed = moveSpeed; //save current moving direction
                moveSpeed = 0; //stop moving to start attack
                attack = true;
                attackTimer = attackCooldown;
                attackTriggerFront.enabled = true;
            }
        }
        else
        {
            moveTimer += Time.deltaTime;
        }

        if (!playerAhead)
        {
            if(isGrounded && jump)
            {
                jump = false;
                animator.SetBool("isJumping", false);
                next = false;


            }
        }
       

        if (next)
        {
            animator.SetBool("isJumping", true);

        }
        


        if (attack)
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

        if(moveTimer <= 0 && move)
        {
            move = false;
        }
        else if (moveTimer >= moveCooldown && !move)
        {
            move = true;
        }
        transform.position = pos;
    }

    public void Damage(int dmg) //detects attack from player (can add HP and stuff here later)
    {
        Destroy(gameObject);
    }
    public void AlertObservers(string message)
    {
        if (message.Equals("AttackAnimationEnded"))
        {
            next = true;
            // Do other things based on an attack ending.
        }
    }
}
