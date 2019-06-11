using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 jump;

    public float attackTimer = 0;
    public float attackCooldown = 0.3f;

    public LayerMask groundLayer;
    public Transform groundedEnd;
    public Transform crouchCeilingEnd;
    public Animator animator;

    public Collider2D attackTriggerLeft;
    public Collider2D attackTriggerRight;
    public Collider2D attackTriggerUp;
    public Collider2D attackTriggerDown;

    public Collider2D standCollider;

    float distDown;

    bool isGrounded = true;
    bool attack = false;
    bool crouch = false;

    float moveSpeed;
    float jumpForce;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        distDown = GetComponent<Collider2D>().bounds.extents.y;

        attackTriggerLeft.enabled = false;
        attackTriggerRight.enabled = false;
        attackTriggerUp.enabled = false;
        attackTriggerDown.enabled = false;

        standCollider.enabled = true;

        moveSpeed = 8.0f;
        jumpForce = 4.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position; //position of the player
        isGrounded = Physics2D.Linecast(pos, groundedEnd.position, 1 << LayerMask.NameToLayer("Ground")); //line from middle to bottom of player
        jump = new Vector2(0.0f, jumpForce);
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")*moveSpeed));
        animator.SetBool("Ground", isGrounded);

        //Player Controls
        if (Input.GetKey(KeyCode.A)) // move left
        {
            pos.x += -moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) // move right
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W) && isGrounded) //jump when grounded
        {
            rb.AddForce(jump, ForceMode2D.Impulse);
            animator.SetTrigger("Jump");
            isGrounded = false;
        }
        if(Input.GetKey(KeyCode.S) && !isGrounded)
        {
            rb.AddForce(new Vector2(0.0f, -0.5f), ForceMode2D.Impulse);
        }
        if (Input.GetKey(KeyCode.LeftControl))
        {
            crouch = true;
        }

        if(Input.GetKey(KeyCode.RightArrow) && !attack)
        {
            attack = true;
            attackTimer = attackCooldown;
            attackTriggerRight.enabled = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && !attack)
        {
            attack = true;
            attackTimer = attackCooldown;
            attackTriggerLeft.enabled = true;
        }
        if (Input.GetKey(KeyCode.UpArrow) && !attack)
        {
            attack = true;
            attackTimer = attackCooldown;
            attackTriggerUp.enabled = true;
        }
        if (Input.GetKey(KeyCode.DownArrow) && !attack)
        {
            attack = true;
            attackTimer = attackCooldown;
            attackTriggerDown.enabled = true;
        }

        if (attack)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attack = false;
                attackTriggerLeft.enabled = false;
                attackTriggerRight.enabled = false;
                attackTriggerUp.enabled = false;
                attackTriggerDown.enabled = false;
            }
        }

        if (crouch)
        {
            if (!Input.GetKey(KeyCode.LeftControl))
            {
                if (Physics2D.Linecast(pos, crouchCeilingEnd.position, 1 << LayerMask.NameToLayer("Ground")))
                {
                    //can't stand up here
                }
                else
                {
                    crouch = false;
                }
            }
            standCollider.enabled = false;
            moveSpeed = 2.0f;
            jumpForce = 4.0f;
        }
        else
        {
            standCollider.enabled = true;
            moveSpeed = 4.0f;
            jumpForce = 8.0f;
        }

        transform.position = pos; // update position
    }
}
