using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private float _damage;
        [SerializeField] private float _heaviness;
        [SerializeField] private float _sharpess;
        [SerializeField] private GameObject _vfx;

        private string _contactTag = "Default";

        public void SetUp(float speed, Vector2 direction, string contactTag)
        {
            _rb.velocity = Vector2.zero;
            _rb.AddForce(direction * speed);
            _contactTag = contactTag;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(_contactTag))
            {
                if (_vfx != null)
                    Destroy(Instantiate(_vfx, transform.position, Quaternion.identity), 1f);

                other.gameObject.GetComponent<Health>().TakeDamage(_damage, transform.position,0f,false,gameObject);
                Destroy(gameObject);
            }

            if (other.gameObject.layer == LayerMask.NameToLayer("WallCollider"))
            {
                if (_vfx != null)
                    Destroy(Instantiate(_vfx, transform.position, Quaternion.identity), 1f);

                Destroy(gameObject);
            }
        }
    }
}