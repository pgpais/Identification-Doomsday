using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInteract))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerInput : MonoBehaviour
{
    private PlayerMovement mov;
    private PlayerInteract inter;

    private float xMov;
    private bool canEnter;
    private House collidedHouse;

    // Start is called before the first frame update
    void Start()
    {
        mov = GetComponent<PlayerMovement>();
        inter = GetComponent<PlayerInteract>();
    }

    // Update is called once per frame
    void Update()
    {
        
        mov.GetMovementInput(Input.GetAxisRaw("Horizontal"));
        if (canEnter)
        {
            if (Input.GetButtonDown("Interact"))
            {
                PlayerPrefs.SetFloat("PlayerPosX", transform.position.x);
                PlayerPrefs.SetFloat("PlayerPosY", transform.position.y);
                PlayerPrefs.SetFloat("PlayerPosZ", transform.position.z);
                StartCoroutine(inter.OpenDoor(collidedHouse));
                mov.GetMovementInput(0);
                this.enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("HouseDoor"))
        {
            collidedHouse = collision.GetComponent<House>();
            canEnter = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("HouseDoor"))
        {
            collidedHouse = null;
            canEnter = false;
        }
    }
}
