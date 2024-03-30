using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class DropPool : MonoBehaviour
    {
        [Header("Common Items")] public List<GameObject> common;
        public List<Vector2Int> commonCount;
        [Header("Uncommon Items")] public List<GameObject> uncommon;
        public List<Vector2Int> uncommonCount;
        [Header("Rare Items")] public List<GameObject> rare;
        public List<Vector2Int> rareCount;
        [Header("Epic Items")] public List<GameObject> epic;
        public List<Vector2Int> epicCount;
        [Header("Legendary Items")] public List<GameObject> legendary;
        public List<Vector2Int> legendaryCount;
        [Header("Fabled Items")] public List<GameObject> fabled;
        public List<Vector2Int> fabledCount;
        [Header("Celestial Items")] public List<GameObject> celestial;
        public List<Vector2Int> celestialCount;
        [Header("Demonic Items")] public List<GameObject> demonic;
        public List<Vector2Int> demonicCount;
        [Header("Gold")] public GameObject goldPrefab;
        [Header("Gold Drop Range")] public Vector2 goldRange = new Vector2(5, 10);

        public GameObject GetRandomItem()
        {
            GameObject item = null;
            int randomItem = 0;
            switch (GetRandomRarity())
            {
                case Enums.ItemRarity.Demonic:
                    randomItem = Random.Range(0, demonic.Count);
                    item = demonic[randomItem];
                    item.GetComponent<Item>().
                        SetAmount(Random.Range(demonicCount[randomItem].x, demonicCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Celestial:
                    randomItem = Random.Range(0, celestial.Count);
                    item = celestial[randomItem];
                    item.GetComponent<Item>()
                        .SetAmount(Random.Range(celestialCount[randomItem].x, celestialCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Fabled:
                    randomItem = Random.Range(0, fabled.Count);
                    item = fabled[randomItem];
                    item.GetComponent<Item>().
                        SetAmount(Random.Range(fabledCount[randomItem].x, fabledCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Legendary:
                    randomItem = Random.Range(0, legendary.Count);
                    item = legendary[randomItem];
                    item.GetComponent<Item>()
                        .SetAmount(Random.Range(legendaryCount[randomItem].x, legendaryCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Epic:
                    randomItem = Random.Range(0, epic.Count);
                    item = epic[randomItem];
                    item.GetComponent<Item>().
                        SetAmount(Random.Range(epicCount[randomItem].x, epicCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Rare:
                    randomItem = Random.Range(0, rare.Count);
                    item = rare[randomItem];
                    item.GetComponent<Item>().
                        SetAmount(Random.Range(rareCount[randomItem].x, rareCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Uncommon: 
                    randomItem = Random.Range(0, uncommon.Count);
                    item = uncommon[randomItem];
                    item.GetComponent<Item>()
                        .SetAmount(Random.Range(uncommonCount[randomItem].x, uncommonCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Common: 
                    randomItem = Random.Range(0, common.Count);
                    item = common[randomItem];
                    item.GetComponent<Item>().
                        SetAmount(Random.Range(commonCount[randomItem].x, commonCount[randomItem].y));
                    break;
            }
            return item;
        }

        public GameObject GetRandomItem(Enums.ItemRarity rarity)
        {
            GameObject item = null;
            int randomItem = 0;
            switch (rarity)
            {
                case Enums.ItemRarity.Demonic:
                    randomItem = Random.Range(0, demonic.Count);
                    item = demonic[randomItem];
                    item.GetComponent<Item>().SetAmount(Random.Range(demonicCount[randomItem].x, demonicCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Celestial:
                    randomItem = Random.Range(0, celestial.Count);
                    item = celestial[randomItem];
                    item.GetComponent<Item>()
                        .SetAmount(Random.Range(celestialCount[randomItem].x, celestialCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Fabled:
                    randomItem = Random.Range(0, fabled.Count);
                    item = fabled[randomItem];
                    item.GetComponent<Item>().SetAmount(Random.Range(fabledCount[randomItem].x, fabledCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Legendary:
                    randomItem = Random.Range(0, legendary.Count);
                    item = legendary[randomItem];
                    item.GetComponent<Item>()
                        .SetAmount(Random.Range(legendaryCount[randomItem].x, legendaryCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Epic:
                    randomItem = Random.Range(0, epic.Count);
                    item = epic[randomItem];
                    item.GetComponent<Item>().SetAmount(Random.Range(epicCount[randomItem].x, epicCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Rare:
                    randomItem = Random.Range(0, rare.Count);
                    item = rare[randomItem];
                    item.GetComponent<Item>().SetAmount(Random.Range(rareCount[randomItem].x, rareCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Uncommon: 
                    randomItem = Random.Range(0, uncommon.Count);
                    item = uncommon[randomItem];
                    item.GetComponent<Item>()
                        .SetAmount(Random.Range(uncommonCount[randomItem].x, uncommonCount[randomItem].y));
                    break;
                case Enums.ItemRarity.Common: 
                    randomItem = Random.Range(0, common.Count);
                    item = common[randomItem];
                    item.GetComponent<Item>().SetAmount(Random.Range(commonCount[randomItem].x, commonCount[randomItem].y));
                    break;
            }
            return item;
        }

        public GameObject GetRandomGold()
        {
            GameObject gold = goldPrefab;
            gold.GetComponent<GoldDropped>().goldAmount = GetRandomGoldAmount();
            return gold;
        }

        public int GetRandomGoldAmount()
        {
            return Random.Range((int)goldRange.x, (int)goldRange.y);
        }

        public Enums.ItemRarity GetRandomRarity()
        {
            float luck = GameObject.Find("Player").GetComponent<Player>().GetLuck();

            float random = Random.Range(0, 100000) - Mathf.Pow(Mathf.Sqrt(luck),1.4f);
            if (random < 1) // 1 IN 12,500 // .008%
            {
                return Enums.ItemRarity.Demonic;
            }

            if (random < 8) // 1 IN 12,500 // .008%
            {
                return Enums.ItemRarity.Celestial;
            }

            if (random < 40) // 1 IN 2,500 // .04%
            {
                return Enums.ItemRarity.Fabled;
            }

            if (random < 250) // 1 IN 400 // .1%
            {
                return Enums.ItemRarity.Legendary;
            }

            if (random < 1500) // 1 IN 66 // 1%
            {
                return Enums.ItemRarity.Epic;
            }

            if (random < 4000) // 1 IN 20 // 4%
            {
                return Enums.ItemRarity.Rare;
            }

            if (random < 19000) // 1 IN 6 // 20%
            {
                return Enums.ItemRarity.Uncommon;
            }
            else // ? IN ? // 74?%
            {
                return Enums.ItemRarity.Common;
            }
        }
    }
}