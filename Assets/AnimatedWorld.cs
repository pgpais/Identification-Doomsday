using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedWorld : MonoBehaviour
{
    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnAnimatorMove()
    {
        Animator animator = GetComponent<Animator>();

        if (animator)
        {
            if (anim.GetBool("TouchingPlayer"))
            {
                Vector3 newPosition = transform.position;
                newPosition.x = newPosition.x + (anim.GetFloat("RunningSpeed") * Time.deltaTime * transform.localScale.x);
                transform.position = newPosition;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Vector3 newScale = transform.localScale;
        newScale.x *= -1;
        transform.localScale = newScale;
    }
}
