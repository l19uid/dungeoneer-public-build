using System.Collections;
using UnityEngine;

namespace Dungeoneer
{
    public class ArtilleryEnemy : Enemy
    {
        public GameObject bullet;
        public GameObject focus;

        private readonly Vector2[] _directions = new Vector2[12]
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

        private float[] _weights = new float[12];
        
        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            _rb = GetComponent<Rigidbody2D>();
            _nextAttack = Time.time + _attackSpeed;
            stamina = 10f;
            if (_spawnAnimation != null)
                StartCoroutine(Spawn());
        }
        

        // Update is called once per frame
        void Update()
        {
            _animator.SetBool("isMoving", LookForPlayer(_viewRange));
            _animator.SetBool("isAttacking", _isAttacking);
            _hasVision -= Time.deltaTime;
            _nextAttack -= Time.deltaTime;

            stamina += Time.deltaTime * 10;
            stamina = Mathf.Clamp(stamina, 0, 10);

            if (Vector2.Distance(transform.position, player.transform.position) < _scareRange)
                _canMove = false;
            else
                _canMove = true;
            
            if (_canMove && !_isAttacking)
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

        private void WeightManager()
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
            if (Vector2.Distance(player.transform.position, transform.position) <= _attackRange
                && _nextAttack <= Time.time && !_isAttacking)
            {
                _isAttacking = true;
                _nextAttack = Time.time + _attackSpeed;
                StartCoroutine(RangedAttack());
            }
        }

        private IEnumerator RangedAttack()
        {
            _isAttacking = true;
            _rb.velocity = Vector2.zero;

            yield return new WaitForSeconds(_attackWait);
            GameObject bulletInstance = Instantiate(bullet, transform.position, Quaternion.identity);
            bulletInstance.transform.right = player.transform.position - transform.position;
            bulletInstance.GetComponent<Projectile>().collisionName = "Player";
            _isAttacking = false;
        }

        protected virtual void Move()
        {
            //use weights to determine the direction of the enemy.
            Vector2 direction = Vector2.zero;
            for (int i = 0; i < _directions.Length; i++)
            {
                direction += _directions[i] * _weights[i];
            }
            _rb.velocity = direction.normalized * _speed;
        }

        protected virtual bool LookForPlayer(float range)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, player.transform.position, playerMask);
            return (hit.collider.CompareTag("Player") && 
                    Vector2.Distance(transform.position, player.transform.position) <= range);
        }
        
        protected virtual bool LookAroundForWall(float range, Vector2 dir)
        {
            Vector2 end = transform.position + (Vector3)dir*range;
            return Physics2D.Linecast(transform.position, end,LayerMask.GetMask("WallCollider"));
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
                Gizmos.DrawRay(transform.position, _directions[i]*_weights[i]);
            }
        }
    }
}