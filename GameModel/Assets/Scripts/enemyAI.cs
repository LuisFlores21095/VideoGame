using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public float attackTimer = 0;
    public float attackCooldown = 0.5f;

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

    float moveSpeed;
    float oldMoveSpeed;

    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1.0f;
        attackTriggerFront.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        isGrounded = Physics2D.Linecast(pos, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground")); //check directly infront of feet for edge
        wallAhead = Physics2D.Linecast(pos, wallCheck.position, 1 << LayerMask.NameToLayer("Ground")); //sets true if detects wall ahead
        playerAhead = Physics2D.Linecast(pos, playerCheck.position, 1 << LayerMask.NameToLayer("Player")); //sets true if player is ahead

        if (isGrounded && !wallAhead) //if there is ground ahead, and no wall ahead, keep moving
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        else //else, turn around
        {
            moveSpeed *= -1;
            facingRight = !facingRight;
            Vector2 charScale = transform.localScale;
            charScale.x *= -1;
            transform.localScale = charScale;
        }

        if (playerAhead) //if player is ahead, attack
        {
            oldMoveSpeed = moveSpeed; //save current moving direction
            moveSpeed = 0; //stop moving to start attack
            attack = true;
            animator.SetTrigger("Attack");
            attackTimer = attackCooldown;
            attackTriggerFront.enabled = true;
        }

        if (attack)
        {
            if (attackTimer < 0.3)
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

    public void Damage(int dmg) //detects attack from player (can add HP and stuff here later)
    {
        Destroy(gameObject);
    }
}
