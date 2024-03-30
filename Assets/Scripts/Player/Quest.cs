using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class Quest : MonoBehaviour
    {
        [SerializeField] private string questName;
        [SerializeField] [TextArea] private string questInstructions;
        [SerializeField] private int requiredLevel;

        [Header("Requirements")] [SerializeField]
        private GameObject questItem;

        [SerializeField] private float requiredExp;
        [SerializeField] private float currentExp;
        [SerializeField] private Enums.EnemyFamily requiredEnemy;

        [Header("Rewards")] [SerializeField] private GameObject questReward;
        [SerializeField] private int questRewardGold;
        [SerializeField] private int questRewardXP;

        [Header("Current")] [SerializeField] private int currentKills;

        public string GetName()
        {
            return questName;
        }

        public string GetInstructions()
        {
            return questInstructions;
        }

        public int GetRequiredLevel()
        {
            return requiredLevel;
        }

        public GameObject GetQuestItem()
        {
            return questItem;
        }

        public float GetRequiredExp()
        {
            return requiredExp;
        }

        public Enums.EnemyFamily GetRequiredEnemy()
        {
            return requiredEnemy;
        }

        public GameObject GetQuestReward()
        {
            return questReward;
        }

        public int GetQuestRewardGold()
        {
            return questRewardGold;
        }

        public int GetQuestRewardXP()
        {
            return questRewardXP;
        }

        public int CurrentKills()
        {
            return currentKills;
        }

        public void AddKills(int current)
        {
            currentKills += current;
        }

        public string GetReward()
        {
            if (questReward != null)
                return "Item : " + questReward.name + "\nGold: " + questRewardGold + "\nXP: " + questRewardXP;
            else
                return "Gold: " + questRewardGold + "\nXP: " + questRewardXP;
        }

        public string GetRequirements()
        {
            string requirements = "";
            if (questItem != null)
                requirements += "Item: " + questItem.name + "\n";
            if (requiredExp > 0)
                requirements += requiredEnemy.ToString() + " Kills: " + currentKills + "/" + requiredExp + "\n";

            return requirements;
        }

        public Enums.EnemyFamily GetEnemyFamily()
        {
            return requiredEnemy;
        }

        public string ToString()
        {
            return GetName() + "\n" + GetRequirements() + "\n" + GetReward();
        }

        public void AddExp(float expReward)
        {
            currentExp += expReward;
            if (currentExp >= requiredExp)
            {
                FinishQuest();
            }
        }

        public float GetExp()
        {
            return currentExp;
        }

        public void FinishQuest()
        {
            if (questReward != null)
                Instantiate(questReward, transform.position, Quaternion.identity);
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            player.AddGold(questRewardGold);
            player.AddExpAndSouls();
            Destroy(gameObject);
        }
    }
}
