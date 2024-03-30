using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace Dungeoneer
{
    public class PlayerAttack : MonoBehaviour
    {
        float nextAttackTime = 0f;
        public GameObject pivot;
        public Inventory inventory;
        public PlayerMovement playerMovement;
        public Player player;
        public GameObject weapon;
        public Weapon weaponScript;
        public LayerMask enemyLayer;

        private float chargeTime = 0f;
        private int animationCombo = 0;

        private bool canAttack = true;
        private float baseRotation;

        public Camera _camera;

        private Vector2 direction;

        private float random;
        private bool _openOverlay = false;

        [SerializeField] private GameObject defaultWeapon;

        private UIManager _uiManager;

        [Header("Combos")] private int combo = 0;
        [SerializeField] private int maxCombo = 0;
        private float comboTimer = 0;
        [SerializeField] private float comboTimeLimit = 10f;
        [SerializeField] private float comboTimeBonus = .5f;

        private void Start()
        {
            // assigning variables
            weaponScript = weapon.GetComponent<Weapon>();
            player = GetComponent<Player>();
            inventory = GetComponent<Inventory>();
            playerMovement = GetComponent<PlayerMovement>();
            _camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _uiManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<UIManager>();
        }

        public void UpdateWeapon(GameObject newWeapon)
        {
            if (newWeapon == null)
                newWeapon = defaultWeapon;

            weapon = newWeapon;
            weaponScript = weapon.GetComponent<Weapon>();
            animationCombo = 0;
        }

        void Update()
        {
            ComboHandler();

            if (_openOverlay)
                return;
            // if the player is not attacking, rotate the pivot to face the mouse
            if (Time.time >= nextAttackTime)
                RotatePivot();
            
            if(weaponScript.isChargeable)
                playerMovement.isCharging = Input.GetMouseButton(0);
            
            AttackHandler();
        }

        private void AttackHandler()
        {
            //chargable attack
            if (chargeTime > weaponScript.GetMaxChargeTime() || weaponScript.GetIsChargeable() && Input.GetMouseButtonUp(0) &&
                Time.time >= nextAttackTime && canAttack)
            {
                StartCoroutine(AttackCooldown());
                nextAttackTime = Time.time + 1f / weaponScript.GetAttackDuration();
                
                chargeTime = 0;
            }
            else if (weaponScript.GetIsChargeable() && Input.GetMouseButton(0) &&
                     Time.time >= nextAttackTime && canAttack)
            {
                chargeTime += Time.deltaTime;
            }
            //non-chargable attack
            else if (!weaponScript.GetIsChargeable() && Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime && canAttack)
            {
                StartCoroutine(AttackCooldown());
                nextAttackTime = Time.time + 1f / weaponScript.GetAttackDuration();
            }
        }

        private void ComboHandler()
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer > comboTimeLimit && combo < maxCombo)
            {
                combo++;
                comboTimer -= comboTimeLimit;
            }

            if (comboTimer < 0)
            {
                if (combo != 1)
                {
                    combo--;
                    comboTimer = comboTimeLimit;
                }
            }

            comboTimer = Mathf.Clamp(comboTimer, 0, comboTimeLimit + .1f);
            combo = Math.Clamp(combo, 1, maxCombo);
        }

        private void MeleeAttack()
        {
            if (animationCombo >= weaponScript.GetAnimations().Length)
                animationCombo = 0;
            // instantiate the weapon
            GameObject weaponGO = Instantiate(weapon, pivot.transform.position, pivot.transform.rotation);
            weaponGO.transform.SetParent(transform);
            // set the damage and other attributes of the weapon
            AttackCollider[] attackCollider = weaponGO.transform.GetComponentsInChildren<AttackCollider>();
            
            float damage = GenerateDamage();
            bool crit = isCrit(random);
            
            if (weaponScript.GetIsChargeable())
            {
                float ct = Mathf.Clamp(chargeTime / weaponScript.GetMaxChargeTime(), 0.5f, 1.25f);
                damage *= ct;
            }
            
            foreach (AttackCollider attack in attackCollider)
            {
                attack.SetUpCollider(damage, crit, "Enemy", gameObject, weaponScript.GetWeaponEffect());
            }

            // play the animation according to current combo
            float attackSpeed =  (100 + weaponScript.GetAttackSpeed() + player.GetBonusAttackSpeed()) / 100;
            if (weaponScript.GetIsChargeable())
            {
                float ct = Mathf.Clamp(chargeTime / weaponScript.GetMaxChargeTime(), 0.5f, 1.25f);
                attackSpeed *= ct;
            }
            weaponGO.GetComponent<Animator>().speed = attackSpeed;
            weaponGO.GetComponent<Animator>().Play(weaponScript.GetAnimations()[animationCombo].name);
            float animationTime = weaponScript.GetAnimations()[animationCombo].length / attackSpeed;
            // destroy the weapon after the animation is done
            Destroy(weaponGO, animationTime);

            // reset the combo if the player is not attacking
            animationCombo++;
        }

        private void RangedAttack()
        {
            GameObject projectileGO =
                Instantiate(weaponScript.projectile, pivot.transform.position, Quaternion.identity);
            projectileGO.transform.right = direction;
            projectileGO.transform.GetComponent<Projectile>().p_damage = (GenerateDamage());
            projectileGO.transform.GetComponent<Projectile>().collisionName = "Enemy";
        }

        private void Attack()
        {
            // different systems for different weapon types
            switch (weaponScript.weaponType)
            {
                case Enums.WeaponType.Melee:
                    MeleeAttack();
                    break;
                case Enums.WeaponType.Ranged:
                    RangedAttack();
                    break;
            }
        }

        private float GenerateDamage()
        {
            float damage = weaponScript.GetDamage();
            Debug.Log("Weapon damage: " + damage);

            player.UpdateStats();
            float strength = player.GetStrength();
            float heaviness = weaponScript.GetHeaviness();
            float dexterity = player.GetDexterity();
            float sharpness = weaponScript.GetSharpness();

            damage += heaviness * Mathf.Clamp((strength / heaviness), 0, 1);
            damage += sharpness * Mathf.Clamp((dexterity / sharpness), 0, 1);

            float critMultiplier = player.GetBonusCritMultiplier();
            random = UnityEngine.Random.Range(0f, 100f);

            if (isCrit(random))
            {
                damage += damage * (critMultiplier / 100);
            }

            if (weaponScript.isChargeable)
                damage *= (chargeTime+1) / weaponScript.maxChargeTime;

            Debug.Log("Weapon damage after: " + damage);
            return damage;
        }

        private bool isCrit(float luck)
        {
            return (luck <= player.GetBonusCritChance());
        }

        private IEnumerator AttackCooldown()
        {
            if (weapon == null)
            {
                Debug.Log("weapon is null");
                UpdateWeapon(defaultWeapon);
            }
            
            playerMovement.canMove = false;
            canAttack = false;
            // attack as soon as player inputs
            if(weaponScript.GetIsChargeable())
                Attack();
            else
                Attack();
            _uiManager.SetWeaponCooldown(weaponScript.GetAttackDuration());
            // wait for the attack speed of the weapon
            yield return new WaitForSeconds((1f / weaponScript.GetAttackDuration()));
            playerMovement.canMove = true;
            canAttack = true;
        }

        private void RotatePivot()
        {
            Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            direction = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);

            pivot.transform.rotation =
                Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90);
        }

        public void SetOpenOverlay(bool openOverlay)
        {
            _openOverlay = openOverlay;
        }

        public void AddCombo()
        {
            comboTimer += comboTimeBonus;
        }

        public int GetCombo()
        {
            return combo;
        }

        public float GetComboTimer()
        {
            return comboTimer;
        }

        public float GetComboTimerMax()
        {
            return comboTimeLimit;
        }

        public Weapon GetDefaultWeapon()
        {
            return defaultWeapon.GetComponent<Weapon>();
        }
    }
}