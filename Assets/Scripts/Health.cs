using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float _maxHealth = 100f;
        [SerializeField] float _currentHealth;
        [SerializeField] float _healthRegen = 0f;
        [SerializeField] private float dropChance = 20f;
        [SerializeField] private float _armor = 100f;
        [Tooltip("Player/Boss Only")] public Image healthBar;

        public GameObject bloodFX;
        public GameObject bloodSplatter;
        public GameObject damagePopUp;
        private GameObject damagePopUpGO;
        private Camera camera;
        private DropPool dropPool;
        [SerializeField] private bool invincible = false;
        [SerializeField] private AnimationClip[] hitAnimations;
        [SerializeField] private AnimationClip deathAnimation;
        [SerializeField] private float _expReward;
        [SerializeField] private Vector2 _goldReward;
        [SerializeField] private GameObject droppedItemPrefab;
        [SerializeField] private GameObject _goldPrefab;
        [SerializeField] private int _rewardSouls;

        void Start()
        {
            if(gameObject.TryGetComponent<DropPool>(out DropPool pool))
                dropPool = pool;
            else if (GameObject.Find("DropPool") != null)
                GameObject.Find("DropPool").GetComponent<DropPool>();
            
            _currentHealth = _maxHealth;
            camera = GameObject.Find("Camera").GetComponent<Camera>();
        }

        public void SetMaxHealth(float health)
        {
            _maxHealth = health;
        }

        public void SetHealthRegen(float regen)
        {
            _healthRegen = regen;
        }

        public void SetCurrentHealth(float health)
        {
            _currentHealth = health;
        }

        void Update()
        {
            if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
                _currentHealth += _healthRegen * Time.deltaTime;
            
            _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
            if (healthBar != null)
                healthBar.fillAmount = _currentHealth / _maxHealth;
        }

        public void TakeDamage(float damage, Vector3 pos, float knockback = 0f, bool isCrit = false,
            GameObject attacker = null)
        {
            if (invincible)
                return;
            float _takenDamage = CalculateDamage(damage);
            _currentHealth -= _takenDamage;

            if (damagePopUp != null)
            {
                damagePopUpGO = Instantiate(damagePopUp, transform.position, Quaternion.identity);
                Vector3 textPos = transform.position + Vector3.up;
                if(attacker == null)
                    damagePopUpGO.GetComponent<DamagePopUp>().SetUp(textPos, transform.position, _takenDamage, isCrit);
                else
                    damagePopUpGO.GetComponent<DamagePopUp>().SetUp(textPos,attacker.transform.position, _takenDamage, isCrit);
            }
            
            if(bloodFX != null)
                Destroy(Instantiate(bloodFX, transform.position, Quaternion.identity), 1f);

            if (hitAnimations.Length > 0 && GetComponent<Animator>())
                GetComponent<Animator>().Play(hitAnimations[Random.Range(0, hitAnimations.Length)].name);
            else if (hitAnimations.Length > 0 && GetComponentInChildren<Animator>())
                GetComponentInChildren<Animator>().Play(hitAnimations[Random.Range(0, hitAnimations.Length)].name);
                
            if(gameObject.TryGetComponent<Enemy>(out Enemy enemy) && attacker!=null)
                enemy.TakeKnockback(attacker.transform.position-transform.position, 9999);
            
            if (_currentHealth <= 0)
                Die(pos, attacker);
        }

        private float CalculateDamage(float damage)
        {
            if(_armor <= 0)
                return damage;
            return damage * (1 - _armor/(100 + _armor));
        }

        private void Die(Vector3 pos, GameObject attacker = null)
        {
            float deathTime = 0f;
            if (bloodFX != null)
                Destroy(Instantiate(bloodFX, transform.position, Quaternion.identity), .45f);
            if (deathAnimation != null)
            {
                deathTime = deathAnimation.length;
                if(GetComponent<Animator>())
                    GetComponent<Animator>().Play(deathAnimation.name);
                else if(GetComponentInChildren<Animator>())
                    GetComponentInChildren<Animator>().Play(deathAnimation.name);
            }

            Vector3 direction = transform.position - pos;
            if (bloodSplatter != null)
            {
                GameObject bloodSplatterGO = Instantiate(bloodSplatter, transform.position, Quaternion.identity);
                float rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bloodSplatterGO.transform.GetComponentInChildren<ParticleSystem>().startRotation = rotation;
                Destroy(bloodSplatterGO, 4f);
            }

            //if (Random.Range(0f, 100f) <= dropChance)
            //    DropItem();

            // --- ONLY FOR BOSSES ---
            if (gameObject.GetComponentInParent<BossPhase>() != null)
            {
                gameObject.GetComponentInParent<BossPhase>().Die();
                return;
            }
            // --- ONLY FOR BOSSES ---

            Enums.SkillType skillType = Enums.SkillType.Main;
            if (gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                //Enum.SkillType.TryParse(enemy.GetFamily().ToString(),
                //    out skillType);
                GameObject.Find("Generator").GetComponent<GradingSystem>().AddScore(_expReward);
            }

            if (gameObject.transform.parent != null &&
                gameObject.transform.parent.TryGetComponent<BossPhase>(out BossPhase bossPhase))
                Enums.SkillType.TryParse(gameObject.transform.parent.GetComponent<BossPhase>().GetFamily().ToString(),
                    out skillType);
            
            if(gameObject.TryGetComponent<Player>(out Player player))
            {
                GameObject.FindWithTag("Canvas").GetComponent<UIManager>().Die();
                Destroy(gameObject);
                return;
            }
            
            if (attacker != null && enemy != null)
            {
                attacker.GetComponent<Player>().AddExpAndSouls(enemy);
            }
            if (attacker != null && attacker.GetComponent<PlayerAttack>() != null)
            {
                attacker.GetComponent<PlayerAttack>().AddCombo();
            }
            
            if(Random.Range(0,100) <= dropChance)
                DropItem();

            if (_goldPrefab != null)
            {
                GameObject goldGO = Instantiate(_goldPrefab, transform.position, Quaternion.identity);
                goldGO.GetComponent<GoldDropped>().goldAmount = Random.Range((int)_goldReward.x, (int)_goldReward.y);
            }
                 
            StartCoroutine(DieAfter(deathTime));
        }
        
        private IEnumerator DieAfter(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }

        private void DropItem()
        {
            GameObject itemPrefab = dropPool.GetRandomItem();
            if (itemPrefab == null)
                return;
            
            GameObject item = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity);
            item.GetComponent<DroppedItem>().InitItem(itemPrefab);
            //item.AddComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)) * 50);
        }

        public void SetInvincible(bool b)
        {
            invincible = b;
        }
        
        public void SetArmor(float armor)
        {
            _armor = armor;
        }
    }
}