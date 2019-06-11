using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed = 4.0f;

    private bool isGrounded = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isGrounded);
        Vector2 pos = transform.position;
        Vector2 jump = new Vector2(0.0f, 1.0f);

        if(Input.GetKey(KeyCode.A))
        {
            pos.x += -moveSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.D))
        {
            pos.x += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if(isGrounded)
            {
                rb.AddForce(jump, ForceMode2D.Impulse);
            }
        }

        transform.position = pos;
    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.layer);
        if(collision.gameObject.layer == 8 && !isGrounded)
        {
            isGrounded = true;
        }
    }
    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 8 && isGrounded)
        {
            isGrounded = false;
        }
    }
}
