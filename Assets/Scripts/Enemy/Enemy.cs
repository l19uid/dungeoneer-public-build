using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] protected string _name;
        [SerializeField] protected Enums.EnemyType _type;
        [SerializeField] protected Enums.EnemyFamily _family;
        [SerializeField] protected Enums.Enemy _enemy;
        [SerializeField] protected int _souls;
        [SerializeField] protected float _expReward;
        [SerializeField] protected Vector2 _weightLimit = new Vector2(-2,4);

        [SerializeField] protected float _speed = 3.5f;
        [SerializeField] protected Vector2 _damageRange;
        [Tooltip("Wait time for attack to go through. NOT PER SECOND")]
        [SerializeField] protected float _attackWait = .5f;
        [Tooltip("Wait time between attacks. NOT PER SECOND")]
        [SerializeField] protected float _attackSpeed = 2f;
        protected float _nextAttack = 0;
        [SerializeField] protected float _attackRange = 1f;
        [SerializeField] protected float _viewRange = 5f;
        [SerializeField] protected float _scareRange = .2f;

        [Tooltip("Temporary. Will be removed when animations are done.")] [SerializeField]
        private GameObject warning;

        [SerializeField] protected Animator _animator;
        [SerializeField] protected GameObject _spawnAnimation;
        [SerializeField] protected SpriteRenderer _sprite;

        [SerializeField] protected LayerMask playerMask;
        [SerializeField] protected LayerMask enemyMask;
        protected GameObject player;
        protected float _hasVision;
        protected bool _isScared;
        protected bool _canMove = true;
        protected bool _isFrozen= false;
        protected bool _isAttacking = false;
        protected Vector2 _knockback;

        protected float stamina;
        public GameObject pivot;
        public GameObject attackCollider;
        
        protected Rigidbody2D _rb;
        private bool isSpawning;

        protected readonly Vector2[] _directions = new Vector2[12]
        {
            new Vector2(-.45f, .8f),
            Vector2.up, 
            new Vector2(.45f, .8f),
            
            new Vector2(.8f, .45f),
            Vector2.right,
            new Vector2(.8f, -.45f),
            
            new Vector2(.45f, -.8f),
            Vector2.down, 
            new Vector2(-.45f, -.8f),
            
            new Vector2(-.8f, -.45f),
            Vector2.left, 
            new Vector2(-.8f, .45f),
        };

        protected float[] _weights = new float[12];
        public string idleAnim;
        public string runAnim;
        public string attackAnim;

        // Start is called before the first frame update
        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            _rb = GetComponent<Rigidbody2D>();
            _nextAttack = _attackSpeed;
            stamina = 10f;
            StartCoroutine(Spawn());
        }
        
        protected IEnumerator Spawn()
        {
            _canMove = false;
            isSpawning = true;
            _isAttacking = true;
            gameObject.GetComponent<Health>().SetInvincible(true);
            Destroy(Instantiate(_spawnAnimation,transform),1f);
            _sprite.enabled = false;
            yield return new WaitForSeconds(1f);
            _sprite.enabled = true;
            gameObject.GetComponent<Health>().SetInvincible(false);
            isSpawning = false;
            _canMove = true;
            _isAttacking = false;
        }

        // Update is called once per frame
        void Update()
        {
            if(isSpawning)
                return;
            RotatePivot();
            AnimationManager();

            _hasVision -= Time.deltaTime;
            _nextAttack -= Time.deltaTime;

            stamina += Time.deltaTime * 10;
            stamina = Mathf.Clamp(stamina, 0, 10);

            if (Vector2.Distance(transform.position, player.transform.position) < _scareRange)
                _isScared = true;
            else
                _isScared = false;

            KnockbackManager();
            
            if (_canMove && !_isAttacking && !_isScared)
            {
                WeightManager();
                Move();
            }
            else
            {
                _rb.velocity = Vector2.zero;
                for (int i = 0; i < _weights.Length; i++)
                {
                    _weights[i] = _weightLimit.x;
                }
            }
            
            AttackCheck();
        }

        private void AnimationManager()
        {
            if (!_isAttacking)
            {
                var state = _rb.velocity.magnitude < .01f  ? idleAnim : runAnim;
                _animator.CrossFade(state,0f);
            }
            else
            {
                _animator.CrossFade("Attack",0f);
            }
            
            if(_rb.velocity.x > 0)
                _sprite.flipX = false;
            else if(_rb.velocity.x < 0)
                _sprite.flipX = true;
        }

        private void RotatePivot()
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            pivot.transform.rotation = Quaternion.Euler(0, 0, angle - 45);
        }

        private void KnockbackManager()
        {
            //_knockback = Vector2.Lerp(_knockback, Vector2.zero, Time.deltaTime * 5);
            //_rb.velocity += _knockback;
        }

        protected virtual void WeightManager()
        {
            // reset weights
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] -= Time.deltaTime * 5;
            }
            // check for walls and add weight in the direction of the player
            // if there is a wall in the y direction add to the x direction depending on where the player is and vice versa.
            for (int i = 0; i < _directions.Length; i++)
            {
                if (LookAroundForWall(1f, _directions[i]))
                {
                    if (i < 3) // wall is on the top
                    { 
                        _weights[7] += Time.deltaTime * 5;
                    }
                    else if (i < 6) // wall is on the right
                    {
                        _weights[10] += Time.deltaTime * 5;
                    }
                    else if (i < 9) // wall is on the bottom
                    {
                        _weights[1] += Time.deltaTime * 5;
                    }
                    else // wall is on the left
                    {
                        _weights[4] += Time.deltaTime * 5;
                    }
                }
            }
            
            // check for enemy and add weight in the opposite direction.
            for (int i = 0; i < _directions.Length; i++)
            {
                GameObject enemy = LookAroundForEnemy(_scareRange, _directions[i]);
                if (enemy != null)
                {
                    Vector2 direction = (transform.position - enemy.transform.position).normalized;
                    // go through all directions and if it is close to the direction of the player add weight.
                    for (int j = 0; j < _directions.Length; j++)
                    {
                        if (Vector2.Angle(direction, _directions[j]) < 45)
                        {
                            _weights[j] += Time.deltaTime * 12;
                        }
                        else if (Vector2.Angle(direction, _directions[j]) < 60)
                        {
                            _weights[j] += Time.deltaTime * 6;
                        }
                    }
                }
            }
            
            // check for player and add weight to the direction of the player.
            if (LookForPlayer(_viewRange))
            {
                Vector2 targetDirection = (player.transform.position- transform.position).normalized;
                // go through all directions and if it is close to the direction of the player add weight.
                for (int i = 0; i < _directions.Length; i++)
                {
                    if (Vector2.Angle(targetDirection, _directions[i]) < 45)
                    {
                        _weights[i] += Time.deltaTime * 12;
                    }
                    else if (Vector2.Angle(targetDirection, _directions[i]) < 60)
                    {
                        _weights[i] += Time.deltaTime * 6;
                    }
                }
            }
            
            //clamp the weights.
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = Mathf.Clamp(_weights[i], _weightLimit.x, _weightLimit.y);
            }
        }

        protected virtual void AttackCheck()
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= _attackRange
                && _nextAttack < 0 && !_isAttacking)
                // If the player is within attack range we attack.
            {
                _nextAttack = _attackSpeed;
                StartCoroutine(MeleeAttack());
            }
        }

        private IEnumerator MeleeAttack()
        {
            _isAttacking = true;
            _rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(_attackWait); // Will wait for the attack to happen once the player is in range.
            attackCollider.SetActive(true);
            yield return new WaitForSeconds(.1f); // Will wait for the attack to happen once the player is in range.
            attackCollider.SetActive(false);
            _isAttacking = false;
        }

        protected virtual void Move()
        {
            Vector2 direction = Vector2.zero;
            if (!_isFrozen)
            {
                for (int i = 0; i < _directions.Length; i++)
                {
                    direction += _directions[i] * _weights[i];
                }
            }
            //use weights to determine the direction of the enemy.
            _rb.velocity = (direction.normalized * _speed);
        }

        protected bool LookForPlayer(float range)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, playerMask);
            return (hit.collider.CompareTag("Player") && 
                    Vector2.Distance(transform.position, player.transform.position) <= range);
        }
        
        protected bool LookAroundForWall(float range, Vector2 dir)
        {
            Vector2 end = transform.position + (Vector3)dir*range;
            return Physics2D.Linecast(transform.position, end,LayerMask.GetMask("WallCollider"));
        }
        
        protected GameObject LookAroundForEnemy(float range, Vector2 dir)
        {
            // since the linecast will hit the enemy itself we need to make an array of all enemies we hit
            // and then go throught the array and check if any enemy isnt this enemy.
            
            Vector2 end = transform.position + (Vector3)dir*range;
            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, end, enemyMask);
            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.gameObject != gameObject)
                        return hits[i].collider.gameObject;
                }
            }
            return null;
        }
        
        public Enums.EnemyFamily GetFamily()
        {
            return _family;
        }

        public void IsTooClose()
        {
            if (Vector2.Distance(transform.position, player.transform.position) <= 1)
            {
                _canMove = false;
                _rb.velocity = Vector2.zero;
            }
            else
                _canMove = true;
        }

        public Enums.Enemy GetEnemyType()
        {
            return _enemy;
        }

        public int GetSouls()
        {
            return _souls;
        }

        public float GetExpReward()
        {
            return _expReward;
        }

        public string GetName()
        {
            return _name;
        }
        
        public void TakeKnockback(Vector2 direction, float force)
        {
            _knockback = direction * force;
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, _viewRange);
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _attackRange);
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, _scareRange);

            Gizmos.color = Color.blue;
            for (int i = 0; i < _directions.Length; i++)
            {
                Gizmos.DrawRay(transform.position, _directions[i]*2);
            }
        }

        public void FreezeFor(float getDuration)
        {
            StartCoroutine(Freeze(getDuration));
        }
        
        private IEnumerator Freeze(float getDuration)
        {
            _isFrozen = true;
            yield return new WaitForSeconds(getDuration);
            _isFrozen = false;
        }

        public void UnFreeze()
        {
            _isFrozen = false;
        }
    }
}