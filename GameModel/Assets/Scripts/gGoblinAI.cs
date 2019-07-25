using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The Green goblin moves back and forth like the blue goblin
//The only difference is that it stops and jumps on a timer
//Overall the green goblin is weaker
public class gGoblinAI : MonoBehaviour
{
    public float attackCooldown = 1.0f;
    public float moveSpeed = 1.0f;
    public float moveCooldown = 6.0f;

    public Transform edgeCheck;
    public Transform wallCheck;
    public Transform playerCheck;
    public Collider2D attackTriggerFront;
    public Animator animator;

    bool hurt = false;
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
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = 0;
        moveTimer = moveCooldown;
        attackTriggerFront.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        isGrounded = Physics2D.Linecast(pos, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground")); //check directly infront of feet for edge
        wallAhead = Physics2D.Linecast(pos, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")); //sets true if detects wall ahead
        playerAhead = Physics2D.Linecast(pos, playerCheck.position, 1 << LayerMask.NameToLayer("Player")); //sets true if player is ahead
        animator.SetBool("Move", move);
        if (!hurt)
        {
            if (moveTimer <= 0 && !pause && !jumping && !attack) //if moving and move timer ran out
            {
                pause = true; // stop moving
                jumping = true;
                animator.SetTrigger("startjump");
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
            }

            if (pause)
            {
                moveTimer += Time.deltaTime;


            }

            if (moveTimer >= 1f && pause) //if not moving and move cooldown reset
            {
                pause = false; //start moving
                jumping = false;
                moveTimer = 6f;
            }





            if (!playerAhead && !pause && !jumping)
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


            if (!attack && playerAhead &&!jumping) //if player is ahead, attack
            {
                attack = true;

                animator.SetTrigger("Attack");

                oldMoveSpeed = moveSpeed; //save current moving direction
                moveSpeed = 0; //stop moving to start attack
            }

            if (isGrounded && !jumping)
            {

                animator.SetBool("isJumping", false);
            }
            else
            {
                animator.SetBool("isJumping", true);

            }



        }
        transform.position = pos;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());
        }
    }

    public void Damage(int dmg) //detects attack from player (can add HP and stuff here later)
    {
        if (!hurt && !attack)
        {
            animator.SetTrigger("isDead");
            hurt = true;
            if (player.transform.position.x >= gameObject.transform.position.x)
            {

                if (!facingRight)
                {
                    moveSpeed *= -1;
                    facingRight = !facingRight;
                    Vector2 charScale = transform.localScale;
                    charScale.x *= -1;
                    transform.localScale = charScale;
                }

            }
            else
            {

                if (facingRight)
                {

                    moveSpeed *= -1;
                    facingRight = !facingRight;
                    Vector2 charScale = transform.localScale;
                    charScale.x *= -1;
                    transform.localScale = charScale;
                }


            }
        }
    }



    public void AlertObservers(string message)
    {
        if (message.Equals("hurtEnd"))
        {
            Destroy(gameObject);
        }

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
