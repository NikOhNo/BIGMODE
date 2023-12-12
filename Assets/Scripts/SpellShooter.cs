using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SpellShooter : MonoBehaviour
{
    private PlayerController controller;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    public void ShootSpell(int damageVal, float direction)
    {
        //play spell attack animation and sfx
        controller.Animator.SetTrigger("spellBasicAttack");
        Debug.Log("Spell basic attack");

        GameObject spell = Instantiate(controller.PlayerSettings.ProjectilePrefab, controller.AttackPoint, transform.rotation);

        spell.GetComponent<PlayerProjectile>().Init(controller, direction, damageVal, controller.Rigidbody2D.velocity.x, controller.PlayerSettings.ProjectileAcceleration);
        spell.GetComponent<SpriteRenderer>().flipX = !controller.Sprite.flipX; //sprite for projectile faces opposite direction of player by default, so we need the reverse
    }
}
