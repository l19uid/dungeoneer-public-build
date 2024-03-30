using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dungeoneer
{
    public class DamagePopUp : MonoBehaviour
    {
        public Vector3 position = Vector3.zero;
        public Vector3 attacker = Vector3.zero;
        public string damage;
        public bool isCrit;
        public TextMeshPro text;
        public Rigidbody2D rb;
        public float lifeTime = 1f;

        // Start is called before the first frame update
        public void SetUp(Vector3 from, Vector3 to, float damage, bool isCrit)
        {
            position = to;
            attacker = from;
            //convert damage to string and add a , every 3 digits
            this.damage = damage.ToString("N0");
            this.isCrit = isCrit;
            
            //add a jump to the pop up relevant to where the damage came from
            rb = GetComponent<Rigidbody2D>();
            if(position.x > attacker.x)
                rb.AddForce(new Vector2(-Random.Range(3,7)*30, Random.Range(3,6)*45));
            else
                rb.AddForce(new Vector2(Random.Range(3,7)*30, Random.Range(3,6)*45));

            text.text = Math.Round(damage).ToString();
            if (isCrit)
                text.text = "<b><gradient=\"StatCritChanceGradient\">" + Math.Round(damage).ToString() + "</gradient></b>";
            else
                text.text = "<b><gradient=\"StatDamageGradient\">" + Math.Round(damage).ToString() + "</gradient></b>";
        }

        // Update is called once per frame
        void Update()
        {
            if (lifeTime < .5f)
            {
                if (text.color.a > .1f)
                    text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime * 2);
                else
                    Destroy(gameObject);
            }
            lifeTime -= Time.deltaTime;
        }
    }
}