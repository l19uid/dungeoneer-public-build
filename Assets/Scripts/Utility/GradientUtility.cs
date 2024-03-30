using UnityEngine;

namespace Dungeoneer
{
    public class GradientUtility
    {
        public GradientUtility()
        {
        }

        public static Color GetColorFromGradient(Gradient gradient)
        {
            return gradient.Evaluate(Random.Range(0f, 1f));
        }

        public static Gradient GetGradient(Enums.ItemRarity rarity)
        {
            switch (rarity)
            {
                case Enums.ItemRarity.Common:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(0.7f, 0.7f, 0.7f), 0.0f),
                            new GradientColorKey(new Color(0.9f, 0.9f, 0.9f), 0.9f),
                            new GradientColorKey(new Color(0.9f, 0.9f, 0.9f, 0f), 1.0f)
                        }
                    };
                case Enums.ItemRarity.Uncommon:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(0.2f, 0.6f, 0.2f), 0.0f),
                            new GradientColorKey(new Color(0.2f, 0.9f, 0.2f), 0.9f),
                            new GradientColorKey(new Color(0.2f, 0.9f, 0.2f, 0f), 1.0f)
                        }
                    };
                case Enums.ItemRarity.Rare:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(0.1f, .2f, 0.8f), 0.0f),
                            new GradientColorKey(new Color(0.1f, .4f, 1f), 0.9f),
                            new GradientColorKey(new Color(0.2f, 0.9f, 0.2f, 0f), 1.0f)
                        }
                    };
                case Enums.ItemRarity.Epic:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(0.7f, 0.3f, 0.8f), 0.0f),
                            new GradientColorKey(new Color(1f, 0.3f, 1f), 0.9f),
                            new GradientColorKey(new Color(0.7f, 0.3f, 0.8f, 0f), 1.0f)
                        }
                    };
                case Enums.ItemRarity.Legendary:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(1f, 0.6f, 0.2f), 0.0f),
                            new GradientColorKey(new Color(1f, 1f, 0.2f), 0.9f),
                            new GradientColorKey(new Color(1f, 1f, 0.2f, 0f), 1.0f)
                        }
                    };
                case Enums.ItemRarity.Fabled:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(0.6f, 0.2f, 0.2f), 0.0f),
                            new GradientColorKey(new Color(0.7f, 0.2f, 0.2f), 0.9f),
                            new GradientColorKey(new Color(0.7f, 0.2f, 0.2f, 0f), 1.0f)
                        }
                    };
                case Enums.ItemRarity.Celestial:
                    return new Gradient()
                    {
                        colorKeys = new GradientColorKey[]
                        {
                            new GradientColorKey(new Color(0.4f, 0.7f, 0.9f), 0.0f),
                            new GradientColorKey(new Color(0.7f, 0.9f, 0.9f), 0.9f),
                            new GradientColorKey(new Color(0.7f, 0.9f, 0.9f, 0f), 1.0f)
                        }
                    };
            }
            
            return new Gradient()
            {
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey(new Color(0.8f, 0.8f, 0.8f), 0.0f),
                    new GradientColorKey(new Color(0.8f, 0.8f, 0.8f), 1.0f)
                }
            };
        }

        public static void GetColorOverLifeTimeGradient(Enums.ItemRarity rarity, ParticleSystem particleSystem)
        {
            var colorOverLifetimeModule = particleSystem.colorOverLifetime;
            colorOverLifetimeModule.color = GetGradient(rarity);
        }

        public static Color GetRarityColor(Enums.ItemRarity getItemRarity, bool primary = true)
        {
            if (primary)
            {
                switch (getItemRarity)
                {
                    case Enums.ItemRarity.Common:
                        return new Color(0.7f, 0.7f, 0.7f);
                    case Enums.ItemRarity.Uncommon:
                        return new Color(0.2f, 0.6f, 0.2f);
                    case Enums.ItemRarity.Rare:
                        return new Color(0.1f, .2f, 0.8f);
                    case Enums.ItemRarity.Epic:
                        return new Color(0.7f, 0.3f, 0.8f);
                    case Enums.ItemRarity.Legendary:
                        return new Color(1f, 0.6f, 0.2f);
                    case Enums.ItemRarity.Fabled:
                        return new Color(0.6f, 0.2f, 0.2f);
                    case Enums.ItemRarity.Celestial:
                        return new Color(0.4f, 0.7f, 0.9f);
                }
            }
            else
            {
                switch (getItemRarity)
                {
                    case Enums.ItemRarity.Common:
                        return new Color(0.9f, 0.9f, 0.9f);
                    case Enums.ItemRarity.Uncommon:
                        return new Color(0.2f, 0.9f, 0.2f);
                    case Enums.ItemRarity.Rare:
                        return new Color(0.1f, .4f, 1f);
                    case Enums.ItemRarity.Epic:
                        return new Color(1f, 0.3f, 1f);
                    case Enums.ItemRarity.Legendary:
                        return new Color(1f, 1f, 0.2f);
                    case Enums.ItemRarity.Fabled:
                        return new Color(0.7f, 0.2f, 0.2f);
                    case Enums.ItemRarity.Celestial:
                        return new Color(0.7f, 0.9f, 0.9f);
                }
            }

            return Color.white;
        }
    }
}