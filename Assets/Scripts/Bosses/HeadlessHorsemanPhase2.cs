using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class HeadlessHorsemanPhase2 : BossPhase
    {
        [SerializeField] private Collider2D _attackCollider;
        [SerializeField] private float _attackCooldown;
        [SerializeField] private float _attackCooldownTimer;

        private bool isClose = false;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _direction = GetPlayerDirection();
            _player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void Update()
        {
            _direction = GetPlayerDirection();

            isClose = Vector2.Distance(_player.transform.position, transform.position) < 3f;
            if (isClose)
            {
                _animator.SetBool("isAttacking", true);
            }
            else
            {
                _animator.SetBool("isAttacking", false);
            }

            if (_canMove && Vector2.Distance(GetPlayerPosition(), transform.position) > .5f)
                Move(_direction);
            else
            {
                _rb.velocity = Vector2.zero;
            }

            if (_rb.velocity.x > 0)
                _renderer.transform.localScale = new Vector3(-1, 1, 1);
            else if (_rb.velocity.x < 0)
                _renderer.transform.localScale = new Vector3(1, 1, 1);

            if (_rb.velocity.magnitude > 0)
                _animator.SetBool("isWalking", true);
            else
                _animator.SetBool("isWalking", false);
        }

        protected override void Move(Vector2 direction)
        {
            if (isClose)
                _rb.velocity = direction * _speed / 2;
            else
                _rb.velocity = direction * _speed;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            _attackWaitTimer -= Time.deltaTime;
            if (other.gameObject.CompareTag("Player") && _attackWaitTimer <= 0 && _canAttack)
            {
                _attackWaitTimer = _attackWait;
                other.GetComponent<Health>().TakeDamage(GenerateDamage(), transform.position, 0f, false, gameObject);
            }
        }
    }
}