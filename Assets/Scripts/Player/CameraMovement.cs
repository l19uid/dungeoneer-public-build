using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Dungeoneer
{
    public class CameraMovement : MonoBehaviour
    {
        public Transform player;
        public Transform camera;
        public float speed = 5f;
        public float playerBoundary;
        public Rigidbody2D rb;
        public static CameraMovement Instance { get; private set; }
        private static GameObject _focus;

        private void Awake()    
        {
            Instance = this;
            player = GameObject.Find("Player").GetComponent<Transform>();
        }

        public IEnumerator Shake(float duration, float magnitude)
        {
            Vector3 originalPos = Vector3.zero;
            float elapsed = 0f;
            while (elapsed < duration)
            {
                float x = originalPos.x + Random.Range(-1f, 1f) * magnitude;
                float y = originalPos.y + Random.Range(-1f, 1f) * magnitude;

                camera.localPosition = new Vector3(x, y, -10);
                elapsed += Time.deltaTime;
                yield return null;
            }

            camera.localPosition = originalPos;
        }

        private void Update()
        {
            if (player == null)
                return;
            if(_focus != null)
                FocusCameraOn(_focus.transform.position, 10f);
            else
                FocusCameraOn(player.position + Vector3.up, playerBoundary);
        }

        private void FocusCameraOn(Vector3 playerPos, float boundary)
        {
            var position = transform.position;
            rb.velocity = (playerPos - position) * speed;
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -speed, speed), Mathf.Clamp(rb.velocity.y, -speed, speed));

            position.x = Mathf.Clamp(position.x, playerPos.x - boundary / 2, playerPos.x + boundary / 2);
            position.y = Mathf.Clamp(position.y, playerPos.y - boundary / 2, playerPos.y + boundary / 2);
            position.z = -10f;
            transform.position = position;
        }
        
        public static void SetFocus(GameObject focus)
        {
            _focus = focus;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, playerBoundary / 2);
        }
    }
}
