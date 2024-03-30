using Unity.Mathematics;
using UnityEngine;

namespace Dungeoneer
{
    public class EnemySoul
    {
        [SerializeField] private string name;
        [SerializeField] private Enums.EnemyFamily enemy;
        [SerializeField] private string description;
        [SerializeField] private int level = 1;
        [SerializeField] private int count = 0;
        [SerializeField] private int toNextLevel = 0;
        [SerializeField] private int wholeCount = 0;
        [SerializeField] private int gap = 25;

        public EnemySoul(string name, Enums.EnemyFamily enemy, string description)
        {
            this.name = name;
            this.enemy = enemy;
            this.description = description;
        }
        
        public EnemySoul(string name, Enums.EnemyFamily enemy, string description, int level, int count, int toNextLevel, int wholeCount, int gap)
        {
            this.name = name;
            this.enemy = enemy;
            this.description = description;
            this.level = level;
            this.count = count;
            this.toNextLevel = gap;
            this.wholeCount = wholeCount;
            this.gap = gap;
        }
        
        public string GetName()
        {
            return name;
        }

        public Enums.EnemyFamily GetEnemy()
        {
            return enemy;
        }

        public string GetDescription()
        {
            return description;
        }

        public int GetLevel()
        {
            return level;
        }

        public int GetCount()
        {
            return count;
        }

        public int GetWholeCount()
        {
            return wholeCount;
        }

        public void AddToNextLevel()
        {
            toNextLevel += gap;
        }

        public int GetCountToNextLevel()
        {
            return toNextLevel;
        }

        public void AddSoul(int amount)
        {
            count += amount;
            wholeCount += amount;

            while (count >= GetCountToNextLevel())
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            level++;
            count -= GetCountToNextLevel();
            AddToNextLevel();
            UIManager.Instance.DisplayMessage("Your " + name + " souls have leveled up to level " + level + "!");
        }

        public int GetGap()
        {
            return gap;
        }

        public string ToString()
        {
            string text = name + "\n";
            text += description + "\n";
            text += "Level: " + level + "\n";
            text += "Souls: " + count + " / " + toNextLevel + "\n";
            return text;
        }
    }
}