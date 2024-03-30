using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class Enums
    {
        public enum  TeleportType
        {
             DungeonExit,
             DungeonEntrance,
             BossEntrance,
        }
        
        public enum ItemType
        {
            Consumable,
            Weapon,
            Ability,
            Armor,
            Quest,
            Key,
            Item,
            Artifact,
            EnemyDrop,
            None,
        }

        public enum ItemSlot
        {
            None,
            Store,
            Sell,

            Helmet,
            Chestplate,
            Leggings,
            Boots,

            Gloves,
            Necklace,

            Weapon,
            Ability,

            ArtifactPouch,
            Consume,
            Upgrade,
        }

        public enum ItemRarity
        {
            Common,
            Uncommon,
            Rare,
            Epic,
            Legendary,
            Fabled,
            Celestial,
            Demonic,
        }

        public enum AbilityElement
        {
            None,
            Fire,
            Water,
            Earth,
            Air,
            Light,
            Dark
        }

        public enum AbilityType
        {
            None,
            Melee,
            Ranged,
            Magic,
            Buff,
            Debuff,
            Heal,
            Summon
        }

        public enum Effect
        {
            Poison,
            Bleed,
            Burn,
            Freeze,
            Stun,
            Glue,
            Tar,
            Absorbtion,
            Healing,
            MuscleBoost,
            SpeedBoost,
            MagicBoost,
            DefenseBoost,
            LuckBoost,
            Invisibility,
            Invulnerability,
            None
        }

        public enum WeaponType
        {
            Melee,
            Ranged,
            Magic,
            Thrown,
            Shield,
            Consumable,
        }

        public enum WeaponFamily
        {
            Sword,
            Axe,
            Spear,
            Daggers,
            Javelin,
            Hammer,
            Guantlets,
            Glaive,
            Bow,
            Crossbow,
            Staff,
            Scythe,
            None,
        }

        public enum SkillType
        {
            Sword,
            Axe,
            Spear,
            Daggers,
            Javelin,
            Hammer,
            Guantlets,
            Glaive,
            Bow,
            Crossbow,
            Staff,
            Scythe,
            Main,
        }

        public enum EnemyType
        {
            Melee,
            Ranged,
            Artilery,
            Boss,
            Stationary,
        }

        public enum EnemyFamily
        {
            Zombie,
            Skeleton,
            Spider,
            Slime,
            Rat,
            TribeLizards,
            Plants,
            Goblins,
            Wraths,
        }

        public enum Enemy
        {
            Zombie,
            RangedSkeleton,
            MeleeSkeleton,
            StrongSkeleton,
            SpittingSpider,
            JumpingSpider,
            Slime,
            BrownRat,
            GreyRat,
            BlackRat,
            WhiteRat,
            TribeLizards,
            Plants,
            Goblins,
            Wraths,
        }

        public enum ItemStat
        {
            Strength,
            Dexterity,
            Intelligence,
            Vitality,
            None
        }

        public enum ItemStatType
        {
            Flat,
            Percent,
            None
        }

        public enum ItemStatModifier
        {
            Add,
            Subtract,
            None
        }

        public enum ItemStatTarget
        {
            Player,
            Enemy,
            None
        }

        public enum ItemStatDuration
        {
            Permanent,
            Temporary,
            None
        }

        public enum RunícEngravings
        {

        }

        public enum RoomType
        {
            Chest,
            Regular,
            MiniBoss,
            Boss,
            Start,
            End,
            Shop,
            Trap,
            Empty
        }
    }
}