using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    public PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        animator = this.GetComponent<Animator>();
        if(!animator)
            Debug.LogError("Missing Animator Component!");
        if(!playerInput)
            playerInput = this.GetComponentInParent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(animator && playerInput)
        {
            if(playerInput.positionAxis.magnitude != 0)
            {
                animator.SetBool("Moving", true);
                animator.SetFloat("Blend", playerInput.adjustedVelocity.z);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
        }
    }
}
