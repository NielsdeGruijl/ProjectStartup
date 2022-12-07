using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private string currentAnimState;

    public void ChangeAnimationState(string newAnimState)
    {
        if(currentAnimState != newAnimState)
            animator.Play(newAnimState);

        currentAnimState = newAnimState;
    }

    public void SetBoolean(string boolean, bool state) => animator.SetBool(boolean, state);
}
