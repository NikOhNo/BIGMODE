using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlatformEffector2D))]
[RequireComponent(typeof(Collider2D))]
public class DropThruPlatform : MonoBehaviour
{
    PlatformEffector2D platformEffector;
    Collider2D collider2d;

    //-- cached reference
    private float originalArc;
    PlayerController playerReference;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        originalArc = platformEffector.surfaceArc;
        collider2d = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (playerReference?.MoveInput.y < -Mathf.Epsilon)
        {
            StartCoroutine(DropPlayer());
        }
    }

    IEnumerator DropPlayer()
    {
        playerReference = null;
        platformEffector.surfaceArc = 0;
        yield return new WaitForSeconds(0.3f);
        platformEffector.surfaceArc = originalArc;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();

        if (playerController != null)
        {
            playerReference = playerController;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerReference = null;
    }
}
