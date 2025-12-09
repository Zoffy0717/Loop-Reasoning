using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");  // A/D or Left/Right

        animator.SetFloat("Speed", Mathf.Abs(movement.x));
    }

    void FixedUpdate()
    {
        if (movement.x > 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (movement.x < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
