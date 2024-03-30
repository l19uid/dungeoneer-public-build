using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class HeadlessHorsemanPhase1 : BossPhase
    {
        private bool canChangeDirection = false;
        [SerializeField] private float stickToWallTime = .5f;

        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _direction = GetPlayerDirection();
            _player = GameObject.FindGameObjectWithTag("Player");
            _animator.SetBool("isWalking", true);
        }

        void Update()
        {
            _attackWaitTimer -= Time.deltaTime;
            IsNearWall();
            if (canChangeDirection)
                _direction = GetPlayerDirection();

            if (_canMove)
                Move(_direction);
            else
                _rb.velocity = Vector2.zero;

            if (Vector2.Distance(_player.transform.position, transform.position) < 5f)
                _animator.SetBool("isAttacking", true);
            else
                _animator.SetBool("isAttacking", false);

            if (_rb.velocity.x > 0)
                _renderer.transform.localScale = new Vector3(-1, 1, 1);
            else if (_rb.velocity.x < 0)
                _renderer.transform.localScale = new Vector3(1, 1, 1);
        }

        protected override void Move(Vector2 direction)
        {
            _rb.velocity = _direction * _speed;
        }

        private void IsNearWall()
        {
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, .1f, Vector2.zero, 1f,
                LayerMask.GetMask("WallCollider"));
            if (hit.collider != null && !canChangeDirection && hit.collider.CompareTag("Wall"))
            {
                StartCoroutine(TurnAround());
            }
        }

        private IEnumerator TurnAround()
        {
            _canMove = false;
            _direction = Vector2.zero;
            _rb.velocity = Vector2.zero;
            canChangeDirection = true;
            yield return new WaitForSeconds(stickToWallTime);
            _canMove = true;
            yield return new WaitForSeconds(.01f);
            canChangeDirection = false;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Player") && _attackWaitTimer <= 0f && _canAttack)
            {
                _attackWaitTimer = _attackWait;
                other.GetComponent<Health>().TakeDamage(GenerateDamage(), transform.position, 0f, false, gameObject);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, .01f);
        }
    }
}
