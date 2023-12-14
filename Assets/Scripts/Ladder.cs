using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ladder : MonoBehaviour
{
    PlayerController playerControllerRef;
    float originalGravScale;
    bool playerEnteredLadder = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var playerController = collision.gameObject.GetComponent<PlayerController>();
        if (playerController != null)
        {
            originalGravScale = playerController.Rigidbody2D.gravityScale;
            this.playerControllerRef = playerController;
            playerControllerRef.OnJump.AddListener(ExitLadder);
        }
    }

    private void FixedUpdate()
    {
        if (playerControllerRef != null)
        {
            if (DetectClimbing())
            {
                EnterLadder();
                float climbVelocity = Mathf.Sign(playerControllerRef.MoveInput.y) * playerControllerRef.PlayerSettings.ClimbSpeed;
                SetClimbSpeed(climbVelocity);
            }
            else
            {
                if (playerEnteredLadder)
                {
                    SetClimbSpeed(0f);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() == playerControllerRef && playerControllerRef != null)
        {
            ExitLadder();
            playerControllerRef.OnJump.RemoveListener(ExitLadder);
            playerControllerRef = null;
        }
    }

    private bool DetectClimbing()
    {
        return Mathf.Abs(playerControllerRef.MoveInput.y) > Mathf.Epsilon;
    }

    private void SetClimbSpeed(float speed)
    {
        Vector2 oldVelocity = playerControllerRef.Rigidbody2D.velocity;
        playerControllerRef.Rigidbody2D.velocity = new Vector2(oldVelocity.x, speed);
    }

    private void EnterLadder()
    {
        playerEnteredLadder = true;

        playerControllerRef.Rigidbody2D.gravityScale = 0;
    }

    private void ExitLadder()
    {
        playerEnteredLadder = false;

        if (playerControllerRef != null)
        {
            playerControllerRef.Rigidbody2D.gravityScale = originalGravScale;
        }
    }
}
