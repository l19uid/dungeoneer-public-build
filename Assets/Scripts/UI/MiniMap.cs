using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class MiniMap : MonoBehaviour
    {
        private GameObject player;
        public GameObject miniMapPlayer;
        public Canvas canvas;
        public float scalingFactor = 1.5f;

        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            miniMapPlayer.transform.position = ConvertToCanvasPosition(player.transform.position);
        }

        public Vector2 ConvertToCanvasPosition(Vector3 position)
        {
            // Get the scale factor of the canvas
            float canvasScaleFactor = canvas.scaleFactor;

            // Calculate the position relative to the canvas
            Vector2 canvasPosition = new Vector2(
                ((position.x * scalingFactor) / canvasScaleFactor) + (canvas.pixelRect.width / 2f),
                ((position.y * scalingFactor) / canvasScaleFactor) + (canvas.pixelRect.height / 2f)
            );

            // Return the converted canvas position
            return canvasPosition;
        }
    }
}
