using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    [CreateAssetMenu(fileName = "newPlayerSettings", menuName = "New Player Settings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        //-- SERIALIZED FIELDS
        //Large Structures
        [SerializeField]
        private AudioClip jumpSFX;
        [SerializeField]
        private AudioClip landSFX;
        [SerializeField]
        private AudioClip healSFX;
        [SerializeField]
        private AudioClip switchSFX;
        [SerializeField]
        private AudioClip slashSFX;
        [SerializeField]
        private AudioClip hitSFX;
        [SerializeField]
        private LayerMask enemyLayers;
        [SerializeField]
        private GameObject projectilePrefab;

        //floats
        [SerializeField]
        private float speed = 5f;
        [SerializeField]
        private float jumpForce = 7.5f;
        [SerializeField]
        private float climbSpeed = 4f;
        [SerializeField]
        private float switchWindow = 1f; //tbd may change
        [SerializeField]
        private float atkDelay = 0.5f; //tbd may change
        [SerializeField]
        private float maxJumpTime = 0.2f;
        [SerializeField]
        private float basicMeleeRange = 1f;
        [SerializeField]
        private float switchMeleeRange = 1f;
        [SerializeField]
        private float projectileAcceleration = 5f;
        [SerializeField]
        private float hurtTime = 0.7f;
        [SerializeField]
        private float invincibleTime = 1.5f;
        [SerializeField]
        private float knockbackForce = 3f;

        //integers
        [SerializeField]
        private int basicDamageValue = 1;
        [SerializeField]
        private int switchDamageValue = 2;
        [SerializeField]
        private int maxHealth = 5;
        [SerializeField]
        private int maxRecoveryCurrency = 100;
        [SerializeField]
        private int meleeRecovery = 10;
        [SerializeField]
        private int rangedRecovery = 4;

        //-- PROPERTIES
        public AudioClip JumpSFX => jumpSFX;
        public AudioClip LandSFX => landSFX;
        public AudioClip HealSFX => healSFX;
        public AudioClip SwitchSFX => switchSFX; 
        public AudioClip SlashSFX => slashSFX;
        public AudioClip HitSFX => hitSFX;
        public LayerMask EnemyLayers => enemyLayers; 
        public GameObject ProjectilePrefab => projectilePrefab;
        public float Speed => speed; 
        public float JumpForce => jumpForce;
        public float ClimbSpeed => climbSpeed;
        public float SwitchWindow => switchWindow;
        public float AtkDelay => atkDelay;
        public float MaxJumpTime => maxJumpTime; 
        public float BasicMeleeRange => basicMeleeRange;
        public float SwitchMeleeRange => switchMeleeRange; 
        public float ProjectileAcceleration => projectileAcceleration;
        public float HurtTime => hurtTime;
        public float InvincibleTime => invincibleTime;
        public float KnockbackForce => knockbackForce;
        public int BasicDamageValue => basicDamageValue;
        public int SwitchDamageValue => switchDamageValue;
        public int MaxHealth => maxHealth;
        public int MaxRecoveryCurrency => maxRecoveryCurrency;
        public int MeleeRecovery => meleeRecovery;
        public int RangedRecovery => rangedRecovery;
    }
}