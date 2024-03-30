using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class Projectile : Item
    {
        public float p_damage;
        public float knockbackForce;
        public string collisionName;
        public float speed;
        public float lifeTime;
        public GameObject hitFX;

        private Rigidbody2D _rb;

        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            _rb.velocity = transform.right * speed;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            Debug.Log(col.name);
            if (col.CompareTag(collisionName))
            {
                Debug.Log(col.name + " was hit as a damage taker");
                col.GetComponent<Health>().TakeDamage(p_damage, transform.position, knockbackForce, false, gameObject);
                Destroy(gameObject);
            }
            else if (col.transform.gameObject.layer == LayerMask.NameToLayer("WallCollider"))
            {
                Debug.Log(col.name + " was hit as a wall");
                if(hitFX != null)
                    Destroy(Instantiate(hitFX, transform.position, Quaternion.identity), .5f);
                Destroy(gameObject);
            }
        }
    }
}