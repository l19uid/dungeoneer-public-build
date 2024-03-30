using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class ArtilleryDrop : MonoBehaviour
    {
        public GameObject artilleryBullet;
        private Rigidbody2D _rb;
        private float damage;
        public float speed = 2;
        private float _distanceTraveled = 0f;
        public GameObject explosion;

        void Start()
        {
            StartCoroutine(Collide());
        }

        // Update is called once per frame
        void Update()
        {
            artilleryBullet.transform.position += Vector3.down * speed * Time.deltaTime;
        }

        private IEnumerator Collide()
        {
            yield return new WaitForSeconds(artilleryBullet.transform.position.y / speed);
            if (Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Player")))
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().TakeDamage(10, transform.position);
            }

            Debug.Log(transform.position);
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 1f);
            Destroy(gameObject);
        }
    }
}