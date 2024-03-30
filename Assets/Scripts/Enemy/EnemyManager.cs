using UnityEngine;

namespace Dungeoneer
{
    public class EnemyManager : MonoBehaviour
    {
        public static void KillAllEnemies()
        {
            foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
            {
                Destroy(enemy);
            }
        }
    }
}