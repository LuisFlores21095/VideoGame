using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 jump;

    public float moveSpeed = 4.0f;
    public float jumpForce = 8.0f;
    public float attackTimer = 0;
    public float attackCooldown = 0.3f;
    public bool isGrounded = true;

    public LayerMask groundLayer;
    public Transform groundedEnd;

    public Collider2D attackTriggerLeft;
    public Collider2D attackTriggerRight;
    public Collider2D attackTriggerUp;
    public Collider2D attackTriggerDown;

    float distDown;

    bool attack = false;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = new Vector2(0.0f, jumpForce);
        distDown = GetComponent<Collider2D>().bounds.extents.y;

        attackTriggerLeft.enabled = false;
        attackTriggerRight.enabled = false;
        attackTriggerUp.enabled = false;
        attackTriggerDown.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position; //position of the player
        isGrounded = Physics2D.Linecast(pos, groundedEnd.position, 1 << LayerMask.NameToLayer("Ground")); //line from middle to bottom of player
        //the line detects the Ground layer objects

        //Player Controls
        if (Input.GetKey(KeyCode.A)) // move left
        {
            pos.x += -moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) // move right
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded) //jump when grounded
        {
            rb.AddForce(jump, ForceMode2D.Impulse);
            isGrounded = false;
        }
        if(Input.GetKey(KeyCode.S) && !isGrounded)
        {
            rb.AddForce(new Vector2(0.0f, -0.5f), ForceMode2D.Impulse);
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

        transform.position = pos; // update position
    }
}
