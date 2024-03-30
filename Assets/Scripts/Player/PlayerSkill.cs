using Unity.Mathematics;
using UnityEngine;

namespace Dungeoneer
{
    public class PlayerSkill 
    {
        [SerializeField] private string name;
        [SerializeField] private Enums.SkillType family;
        [SerializeField] private string description;
        [SerializeField] private float level = 1;
        [SerializeField] private float experience = 0;
        [SerializeField] private float wholeExperience = 0;
        [SerializeField] private float gap = 2500;
        [SerializeField] private float toNextLevel = 0;
        
        public PlayerSkill(string name, Enums.SkillType family, string description)
        {
            this.name = name;
            this.family = family;
            this.description = description;
        }
        
        public PlayerSkill(string name, Enums.SkillType family, string description, float level, float experience, float wholeExperience, float gap, float toNextLevel)
        {
            this.name = name;
            this.family = family;
            this.description = description;
            this.level = level;
            this.experience = experience;
            this.wholeExperience = wholeExperience;
            this.gap = gap;
            this.toNextLevel = toNextLevel;
        }

        public string GetName()
        {
            return name;
        }

        public Enums.SkillType GetFamily()
        {
            return family;
        }

        public string GetDescription()
        {
            return description;
        }

        public float GetLevel()
        {
            return level;
        }

        public float GetExperience()
        {
            return experience;
        }

        public float GetWholeExperience()
        {
            return wholeExperience;
        }

        public float GetGap()
        {
            return gap;
        }

        public void AddToNextLevel()
        {
            toNextLevel += gap * level;
        }

        public float GetExperienceToNextLevel()
        {
            return toNextLevel;
        }

        public void AddExperience(float amount)
        {
            experience += amount;
            wholeExperience += amount;
            while (experience >= toNextLevel)
            {
                LevelUp();
            }
        }

        public void LevelUp()
        {
            experience -= GetExperienceToNextLevel();
            level++;
            UIManager.Instance.DisplayMessage("You leveled up " + name + " to level " + level + "!");
            AddToNextLevel();
        }

        public override string ToString()
        {
            string stats = name;
            stats += "\nLevel : " + level;
            stats += "\nExperience : " + experience + "/" + toNextLevel;
            return stats;
        }
    }
}