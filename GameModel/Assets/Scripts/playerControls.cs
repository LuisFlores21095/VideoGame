using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//once we start to finish the first room and enemies and have a working game going,
//i can clean up the code and make it easily customizable.
public class playerControls : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 jump;
    Collider2D coll;

    public float attackCooldown = 0.5f;
    public float moveSpeed = 4.0f;
    public float jumpForce = 8.0f;

    public LayerMask groundLayer;
    public Transform groundedEnd;
    public Animator animator;
    public Animator attackAnimator;
    public Collider2D attackTriggerFront;

    bool isGrounded = false;
    bool attack = false;
    bool facingRight = true;
    bool hurt = false;

    float hurtTimer;
    float attackTimer;

    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        attackTriggerFront.enabled = false; //start player not attacking

        hurtTimer = 1.0f;
        attackTimer = 0.5f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position; //position of the player
        isGrounded = Physics2D.Linecast(pos, groundedEnd.position, 1 << LayerMask.NameToLayer("Ground")); //line from middle to bottom of player
        jump = new Vector2(0.0f, jumpForce); // jumpforce applied in positive Y direction
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")*moveSpeed)); //checks speed, if speed>0, walking animation
        animator.SetBool("Ground", isGrounded); //if grounded, do ground animations

        float horizontal = Input.GetAxis("Horizontal"); //takes in horizontal movement inputs
        flipSprite(horizontal); //if changing directions horizontally, flip the sprite

        if (isGrounded) // if grounded, play grounded animations over air animations
        {
            animator.SetLayerWeight(1, 0);
        }
        else
        {
            animator.SetLayerWeight(1, 1);
        }

        if (hurt) // if hurt, play hurt animation over all grounded or air animations
        {
            animator.SetLayerWeight(2, 1);
        }
        else
        {
            animator.SetLayerWeight(2, 0);
        }

        //Player Controls
        if (!hurt) // if not hurt, accept input
        {
            if (Input.GetKey(KeyCode.A)) // move left
            {
                pos.x += -moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.D)) // move right
            {
                pos.x += moveSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.W) && isGrounded) //jump only when grounded
            {
                rb.AddForce(jump, ForceMode2D.Impulse);
                animator.SetTrigger("Jump");
                isGrounded = false;
            }
            if (Input.GetKey(KeyCode.S) && !isGrounded)
            {
                rb.AddForce(new Vector2(0.0f, -0.5f), ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.Space) && !attack)
            {
                attack = true;
                animator.SetTrigger("Attack"); //knight attack animation
                attackAnimator.SetTrigger("Attack"); //blue attack effect on collider animation
                attackTimer = attackCooldown;
                attackTriggerFront.enabled = true;
            }
        }

        if (attack) // if attack started
        {
            if(attackTimer < 0.3)
            {
                attackTriggerFront.enabled = false; //once cool down reaches 0.3, the attack hitbox disappears (but attack timer continues)
                //I did this so that the player cannot attack infinitely, there is a 0.3 ms opening for an attack right now between swings
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime; //counting down attack timer
            }
            else
            {
                attack = false; //attack ends
            }
        }

        if (hurt) // count down the hurt timer
        {
            if (hurtTimer > 0)
            {
                hurtTimer -= Time.deltaTime;
            }
            else
            {
                hurt = false;
            }
        }

        transform.position = pos; // update position
    }

    private void flipSprite(float horizontal) //basically flips the sprite if changing direction horizontally
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector2 charScale = transform.localScale;
            charScale.x *= -1;
            transform.localScale = charScale;
        }
    }

    void OnCollisionEnter2D(Collision2D col) // hurts player if it comes in contact with enemy or enemy attack
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            hurt = true;
            if (facingRight)
            {
                rb.AddForce(new Vector2(-4.0f, 4.0f), ForceMode2D.Impulse);
            }
            else
            {
                rb.AddForce(new Vector2(4.0f, 4.0f), ForceMode2D.Impulse);
            }
            hurtTimer = 0.5f;
        }
    }
}
