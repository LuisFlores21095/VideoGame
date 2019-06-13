using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControls : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 jump;
    Collider2D coll;

    public float attackTimer = 0;
    public float attackCooldown = 0.5f;

    public LayerMask groundLayer;
    public Transform groundedEnd;
    public Transform crouchCeilingEnd;
    public Animator animator;
    public Animator attackAnimator;

    public Collider2D attackTriggerFront;

    float distDown;

    bool isGrounded = true;
    bool attack = false;
    bool crouch = false;
    bool facingRight = true;

    float moveSpeed;
    float jumpForce;

    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        distDown = GetComponent<Collider2D>().bounds.extents.y;

        attackTriggerFront.enabled = false;

        moveSpeed = 4.0f;
        jumpForce = 8.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position; //position of the player
        isGrounded = Physics2D.Linecast(pos, groundedEnd.position, 1 << LayerMask.NameToLayer("Ground")); //line from middle to bottom of player
        jump = new Vector2(0.0f, jumpForce);
        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxisRaw("Horizontal")*moveSpeed));
        animator.SetBool("Ground", isGrounded);

        float horizontal = Input.GetAxis("Horizontal");
        flipSprite(horizontal);

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

        if(Input.GetKey(KeyCode.Space) && !attack)
        {
            attack = true;
            animator.SetTrigger("Attack");
            attackAnimator.SetTrigger("Attack");
            attackTimer = attackCooldown;
            attackTriggerFront.enabled = true;
        }

        if (attack)
        {
            if(attackTimer < 0.3)
            {
                attackTriggerFront.enabled = false;
            }
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attack = false;
            }
        }

        transform.position = pos; // update position
    }

    private void flipSprite(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            facingRight = !facingRight;
            Vector2 charScale = transform.localScale;
            charScale.x *= -1;
            transform.localScale = charScale;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {
            animator.SetTrigger("Hurt");
        }
    }
}
