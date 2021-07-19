using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChibiAnimatorManager : MonoBehaviour
{
    public Animator animator;
    public PlayerInput playerInput;
    // Start is called before the first frame update
    void Start()
    {
        if (!animator)
            animator = this.GetComponentInChildren<Animator>();
        if (!playerInput)
            playerInput = this.GetComponent<PlayerInput>();

        if (!animator)
            Debug.LogError(this.ToString() + "Missing Animator Component!");
        if (!animator)
            Debug.LogError(this.ToString() + "Missing PlayerInput Component!");
    }

    // Update is called once per frame

    void Update()
    {
        if (animator && playerInput)
        {
            // if(playerInput.positionAxis.magnitude != 0)
            // {
            //animator.SetFloat("Forward", playerInput.adjustedVelocity.magnitude);
            animator.SetFloat("Right", playerInput.adjustedVelocity.x);
            animator.SetFloat("Forward", playerInput.adjustedVelocity.z);
            animator.SetBool("Crouch",playerInput.isCrouching);
            animator.SetBool("OnGround",!playerInput.isJumping);
            //animator.SetFloat("Turn", playerInput.adjustedVelocity.x);
            // }
            // else
            // {
            //     animator.SetFloat("Forward", 0);
            //     animator.SetFloat("Right", 0);
            // }
        }
    }

    void OnGUI()
    {
        if (!animator)
            animator = this.GetComponentInChildren<Animator>();
        if (!playerInput)
            playerInput = this.GetComponent<PlayerInput>();
    }
}
