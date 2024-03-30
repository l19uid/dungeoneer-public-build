using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class EnemyPool : MonoBehaviour
    {
        [SerializeField] private List<GameObject> enemies;
        [SerializeField] private List<GameObject> bigEnemies;
        [SerializeField] private List<GameObject> miniBosses;
        [SerializeField] private GameObject boss;

        public GameObject GetRandomEnemy()
        {
            float random = Random.Range(0, 100);

            if (random < 75)
            {
                return enemies[Random.Range(0, enemies.Count)];
            }
            else if (random < 95)
            {
                return bigEnemies[Random.Range(0, bigEnemies.Count)];
            }
            else
            {
                return miniBosses[Random.Range(0, miniBosses.Count)];
            }
        }

        public List<GameObject> GetEnemies()
        {
            return enemies;
        }

        public List<GameObject> GetBigEnemies()
        {
            return bigEnemies;
        }

        public List<GameObject> GetMiniBosses()
        {
            return miniBosses;
        }

        public GameObject GetBoss()
        {
            return boss;
        }
    }
}