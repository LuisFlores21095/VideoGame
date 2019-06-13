using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAI : MonoBehaviour
{
    public Transform edgeCheck;
    public Transform wallCheck;

    bool isGrounded = true;
    bool wallAhead = false;
    bool facingRight = true;

    float moveSpeed;

    Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 1.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        pos = transform.position;
        isGrounded = Physics2D.Linecast(pos, edgeCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        wallAhead = Physics2D.Linecast(pos, wallCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        if (isGrounded && !wallAhead)
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        else
        {
            moveSpeed *= -1;
            facingRight = !facingRight;
            Vector2 charScale = transform.localScale;
            charScale.x *= -1;
            transform.localScale = charScale;
        }

        transform.position = pos;
    }

    public void Damage(int dmg)
    {
        Destroy(gameObject);
    }
}
