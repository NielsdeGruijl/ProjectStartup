using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private string currentAnimState;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeAnimationState(string newAnimState)
    {
        if(currentAnimState != newAnimState)
            animator.Play(newAnimState);

        currentAnimState = newAnimState;
    }
}
