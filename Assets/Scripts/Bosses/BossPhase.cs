using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class BossPhase : MonoBehaviour
    {
        [SerializeField] protected string _name;
        [SerializeField] protected float _speed;
        [SerializeField] protected Vector2 _damage;
        [SerializeField] protected float _heaviness;
        [SerializeField] protected float _sharpness;
        [SerializeField] protected float _attackWait;
        protected float _attackWaitTimer;
        [SerializeField] protected float _attackSpeed;
        [SerializeField] protected float _attackRange;
        [SerializeField] protected Animator _animator;
        [SerializeField] protected Rigidbody2D _rb;
        [SerializeField] protected GameObject _renderer;
        [SerializeField] protected Enums.EnemyFamily _enemyFamily;
        [SerializeField] protected Enums.EnemyType _enemyType;
        [SerializeField] protected bool _isDead = false;
        protected bool _canMove = true;
        protected bool _canAttack = true;

        protected Vector2 _direction;
        protected BossManager _bossManager;
        protected GameObject _player;

        public float GetSpeed()
        {
            return _speed;
        }

        public Vector2 GetDamage()
        {
            return _damage;
        }

        public float GetHeaviness()
        {
            return _heaviness;
        }

        public float GetSharpness()
        {
            return _sharpness;
        }

        public float GetAttackWait()
        {
            return _attackWait;
        }

        public float GetAttackSpeed()
        {
            return _attackSpeed;
        }

        public float GetAttackRange()
        {
            return _attackRange;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        public void CreateBoss(BossManager bossManager)
        {
            string text = "A " + _name + " boss has appeared!";
            UIManager.Instance.DisplayMessage(text);
            _bossManager = bossManager;
        }

        protected virtual void Move(Vector2 direction)
        {
            _rb.velocity = direction * _speed;
        }

        protected virtual void Attack()
        {
            _animator.SetTrigger("Attack");
        }

        public void Die()
        {
            if(_isDead)
                return;
            _isDead = true;
            _canMove = false;
            _canAttack = false;
            _bossManager.EndPhase();
        }

        protected virtual void Die(GameObject attacker)
        {
            Die();
            if (attacker != null)
                attacker.GetComponent<Player>().AddExpAndSouls();
            Destroy(gameObject, 1f);
        }

        protected virtual float GenerateDamage()
        {
            float damage = Random.Range(_damage.x, _damage.y);

            damage += _heaviness * Mathf.Clamp((_heaviness / _heaviness), 0, 1);
            damage += _sharpness * Mathf.Clamp((_sharpness / _sharpness), 0, 1);

            return damage;
        }

        protected virtual Vector2 GetPlayerDirection()
        {
            Vector2 playerPosition = GetPlayerPosition();
            Vector2 direction = playerPosition - (Vector2)transform.position;
            direction.Normalize();
            _direction = direction;
            return direction;
        }

        protected virtual Vector2 GetPlayerPosition()
        {
            return GameObject.FindGameObjectWithTag("Player").transform.position;
        }

        public void SetCanMove(bool b)
        {
            _canMove = b;
        }

        public Enums.EnemyFamily GetFamily()
        {
            return _enemyFamily;
        }
    }
}