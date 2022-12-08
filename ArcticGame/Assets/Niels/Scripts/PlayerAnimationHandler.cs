using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHandler : MonoBehaviour
{
    private PlayerAnimation anim;
    private PlayerInteractions playerInteractions;
    private Movement playerMovement;

    private void Start()
    {
        anim = GetComponent<PlayerAnimation>();
        playerInteractions = GetComponent<PlayerInteractions>();
        playerMovement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (playerInteractions.idle && !playerMovement.moving && !playerInteractions.fishing)
            anim.ChangeAnimationState("Idle");
        if (!playerInteractions.idle || playerMovement.moving)
            anim.ChangeAnimationState("Walking");
        if (playerInteractions.fishing)
            anim.ChangeAnimationState("FishLooking");
        if (playerInteractions.fishStabbing)
        {
            anim.ChangeAnimationState("FishStabbing");
            playerInteractions.fishStabbing = false;
        }
            
    }
}
