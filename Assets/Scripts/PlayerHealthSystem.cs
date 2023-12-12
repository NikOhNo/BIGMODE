﻿using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;

namespace Assets.Scripts
{
    public class PlayerHealthSystem
    {
        private int health;
        private int recoveryCurrency = 0; //think soul from hollow knight, we can decide on a name later

        //-- Cached References
        private readonly PlayerSettingsSO settings;
        private readonly PlayerController controller;

        public PlayerHealthSystem(PlayerController controller)
        {
            this.controller = controller;
            this.settings = controller.PlayerSettings;
        }

        public void AddRecoveryCurrency(int amount)
        {
            recoveryCurrency += amount;
            Recover();
        }

        private void Recover()
        {
            if (recoveryCurrency >= settings.MaxRecoveryCurrency && health < settings.MaxHealth)
            {
                //show recovery particle efx
                controller.Pfx.Play();
                controller.AudioSource.PlayOneShot(settings.HealSFX);
                health++;
                recoveryCurrency = 0;
                controller.UIController.ChangeHealth(health);
            }
            controller.UIController.ChangeRecovery(recoveryCurrency);
        }

        public void Hurt(int damage)
        {
            //play hurt animation/sfx
            //consider dealing knockback to player
            health -= damage;
            controller.UIController.ChangeHealth(health);
            if (health < 1)
            {
                Death();
            }
        }

        public void Death()
        {
            //play death animation/sfx
            //goto room with last checkpoint
            Debug.Log("You died!");

            GameObject sceneLoader = GameObject.Find("SceneLoader");
            if (sceneLoader != null)
            {
                sceneLoader.GetComponent<SceneLoader>().RestartLevel();
            }
            else //should be able to remove this once we are sure that every scene has SceneLoader
            {
                health = settings.MaxHealth;
                controller.UIController.ChangeHealth(health);
                recoveryCurrency = 0;
                Recover();
            }
        }
    }
}