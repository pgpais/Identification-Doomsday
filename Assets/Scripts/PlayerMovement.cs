using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5;

    public Animator animator;

    private Rigidbody2D rb;

    private float xMov;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPosX", transform.position.x),
                                         PlayerPrefs.GetFloat("PlayerPosY", transform.position.y),
                                         PlayerPrefs.GetFloat("PlayerPosZ", transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Mathf.Abs(xMov));
        
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(xMov, 0) * speed;
        if(xMov < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        } else if (xMov > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public void GetMovementInput(float x)
    {
        xMov = x;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("AnimatedWorld"))
        {
            Animator anim = collision.GetComponent<Animator>();
            anim.SetBool("TouchingPlayer", true);
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("AnimatedWorld"))
        {
            Vector3 newScale = collision.transform.localScale;
            if (transform.position.x > collision.transform.position.x)
            {
                newScale.x = -1;
                collision.transform.localScale = newScale;
            }
            else if (transform.position.x < collision.transform.position.x)
            {
                newScale.x = 1;
                collision.transform.localScale = newScale;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("AnimatedWorld"))
        {
            Animator anim = collision.GetComponent<Animator>();
            anim.SetBool("TouchingPlayer", false);
        }
    }
}
