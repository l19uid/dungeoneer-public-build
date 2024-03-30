using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class Consumable : Item
    {
        [Header("Consumable Stats")] [SerializeField]
        private Effect effect;

        [SerializeField] private float duration;

        [SerializeField]
        [Tooltip("0 = instant after duration ends, other values = linear fall off by how much each second")]
        private float fallOff = 0;

        public override string ItemToString()
        {
            string stats = GetName() + "\n";
            stats += "Rarity: " + GetRarityToString() + "\n";
            stats += "Type : " + GetItemType() + "\n";
            stats += "Effect : " + GetEffect().ToString() + "\n";
            stats += "Duration : " + GetDuration() + "\n";
            stats += "Fall Off : " + GetFallOff() + "\n";
            if (GetValue() != 0)
                stats += "<gradient=\"GoldGradient\">Cost: " + GetValue() + "</gradient>\n";
            return stats;
        }

        public Effect GetEffect()
        {
            return effect;
        }

        public float GetDuration()
        {
            return duration;
        }

        public float GetFallOff()
        {
            return fallOff;
        }
    }
}
