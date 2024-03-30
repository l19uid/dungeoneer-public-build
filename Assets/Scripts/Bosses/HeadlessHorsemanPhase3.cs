using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class HeadlessHorsemanPhase3 : BossPhase
    {
        [SerializeField] protected float _projectileSpeed;
        [SerializeField] private Collider2D _attackCollider;
        [SerializeField] private Vector2 _attackCooldown;
        private float _attackCooldownTimer;
        [SerializeField] private Vector2 _attackCountMinMax;
        private float _attackCountMax;
        private float _attackCount;
        [SerializeField] private GameObject _bullet;
        [SerializeField] private List<AnimationClip> _animations;
        [Header("Stage2")] [SerializeField] private float _angleDiff;
        [SerializeField] private GameObject[] _directions;
        [SerializeField] private GameObject _pivot;
        [SerializeField] private float _rotationSpeed = 50;

        private bool isClose = false;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _direction = GetPlayerDirection();
            _player = GameObject.FindGameObjectWithTag("Player");
            _attackCountMax = Random.Range(_attackCountMinMax.x, _attackCountMinMax.y);
            _attackCount = 0;

            //StartCoroutine(BulletSpray(30, _directions[0], .25f));
            //StartCoroutine(BulletSpray(30, _directions[1], .25f));
            //StartCoroutine(BulletSpray(30, _directions[2], .25f));
            //StartCoroutine(BulletSpray(30, _directions[3], .25f));
            _rotationSpeed = -25;
        }

        // Update is called once per frame
        void Update()
        {
            //Rotate pivot 1 degree
            _pivot.transform.Rotate(0, 0, Time.deltaTime * _rotationSpeed);

            if (_attackCount >= _attackCountMax)
            {
                _bossManager.EndPhase();
                _canMove = false;
            }

            //if ((int)(_attackCountMax / 2)==_attackCount)
            //{
            //    _attackCount++;
            //    StartCoroutine(BulletSpray(30, _directions[0], .25f));
            //    StartCoroutine(BulletSpray(30, _directions[1], .25f));
            //    StartCoroutine(BulletSpray(30, _directions[2], .25f));
            //    StartCoroutine(BulletSpray(30, _directions[3], .25f));
            //    _rotationSpeed = 25;
            //}

            if (_attackCooldownTimer <= 0 && _attackCount <= _attackCountMax && _canAttack)
                Attack();

            _attackCooldownTimer -= Time.deltaTime;
        }

        protected override void Attack()
        {
            ChooseAnimation();
            _attackCooldownTimer = Random.Range(_attackCooldown.x, _attackCooldown.y);
            _attackCount++;

            StartCoroutine(SpawnBullet(.5f, _projectileSpeed, GetPlayerDirection()));
        }

        private IEnumerator SpawnBullet(float time, float speed, Vector2 direction)
        {
            yield return new WaitForSeconds(time);
            GameObject bulletGO = Instantiate(_bullet, transform.position, Quaternion.identity);
            bulletGO.GetComponent<Bullet>().SetUp(speed, direction, "Player");
        }

        private void ChooseAnimation()
        {
            int animNum = (int)(_attackCount / _animations.Count);

            _animator.SetInteger("Stage", animNum);
        }

        private IEnumerator BulletSpray(int amount, GameObject direction, float gap = .1f)
        {
            _canAttack = false;

            int i = 0;
            for (int j = 0; j < amount; j++)
            {
                yield return new WaitForSeconds(gap);
                StartCoroutine(SpawnBullet(0.1f, _projectileSpeed,
                    direction.transform.position.normalized - transform.position));
            }

            _canAttack = true;
        }
    }
}