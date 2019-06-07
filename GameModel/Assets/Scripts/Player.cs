using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    Vector2 jump;

    public float moveSpeed = 4.0f;
    public float jumpForce = 8.0f;
    public bool isGrounded = true;
    public LayerMask groundLayer;
    public Transform groundedEnd;

    float distDown;
    Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        jump = new Vector2(0.0f, jumpForce);
        distDown = GetComponent<Collider2D>().bounds.extents.y;
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

        transform.position = pos; // update position
    }
}
