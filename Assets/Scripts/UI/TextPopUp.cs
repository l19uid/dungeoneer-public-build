using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Dungeoneer
{
    public class TextPopUp : MonoBehaviour
    {
        private TextMeshProUGUI text;
        public float length = 1;
        private float timer = 0;

        void Start()
        {
            text = GetComponent<TextMeshProUGUI>();
            timer = length;
        }

        // Update is called once per frame
        void Update()
        {
            timer -= Time.deltaTime;
            if (timer < length / 2)
                text.color = new Color(text.color.r, text.color.g, text.color.b, timer / (length / 2));

            if (timer < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
