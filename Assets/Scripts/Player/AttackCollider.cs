using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class AttackCollider : MonoBehaviour
    {
        public string targetTag = "Enemy";
        public float damage;
        public float knockback = 10f;
        public Vector3 knockbackDirection;
        public bool isCrit = false;
        public GameObject owner;
        public PlayerAttack _playerAttack;
        public Effect _effect;
        //public LayerMask _enemyMask;
        //public LayerMask _wallMask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.transform.CompareTag(targetTag)) // && OwnerSeesTarget(other.transform))
            {
                //_playerAttack.AddCombo();
                other.GetComponent<Health>().TakeDamage(damage, knockbackDirection, knockback, isCrit, owner);
                if(_effect != null && other.TryGetComponent<EffectManager>(out EffectManager effectManager))
                    effectManager.AddEffect(_effect);
            }
        }
        
        //private bool OwnerSeesTarget(Transform target)
        //{
        //    Vector2 direction = target.position - owner.transform.position;
        //    float distance = Vector2.Distance(owner.transform.position, target.position);
        //    RaycastHit2D wallHit = Physics2D.Raycast(owner.transform.position, direction, distance, _wallMask);
        //    RaycastHit2D enemyHit = Physics2D.Raycast(owner.transform.position, direction, distance, _enemyMask);
        //    if (wallHit.collider != null)
        //        Debug.Log("Wall: " + wallHit.collider.name);
        //    if (enemyHit.collider != null)
        //        Debug.Log("Hit: " + enemyHit.collider.name);
        //    Debug.Log("Wall: " + _wallMask + " Enemy: " + _enemyMask);
        //    if (wallHit.collider != null)
        //        return false;
        //    if (enemyHit.collider != null)
        //    {
        //        if (enemyHit.collider.transform == target)
        //        {
        //            return true;
        //        }
        //    }
//
        //    return false;
        //}

        public void SetUpCollider(float f, bool crit, string enemy, GameObject o, Effect effect = null)
        {
            //_enemyMask = LayerMask.GetMask(enemy);
            //_wallMask = LayerMask.GetMask("WallCollider");
            damage = f;
            isCrit = crit;
            targetTag = enemy;
            owner = o;
            _playerAttack = owner.GetComponent<PlayerAttack>();
            _effect = effect;
        }
    }
}