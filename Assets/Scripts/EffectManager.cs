using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dungeoneer
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] protected List<Enums.Effect> _immuneTo;
        [SerializeField] protected List<Effect> _effects;
        [SerializeField] protected List<GameObject> _effectsGO;

        private void Update()
        {
            if (_effects.Count > 0)
                EffectHandler();
        }
        
        private void EffectHandler()
        {
            if (_effects.Count == 0 && _effectsGO.Count != 0)
            {
                foreach (GameObject effect in _effectsGO)
                {
                    Destroy(effect);
                }
                _effectsGO.Clear();
                return;
            }

            for (int i = 0; i < _effects.Count; i++)
            {
                _effects[i].UpdateEffect(Time.deltaTime);
                if (_effects[i].GetTimeLeft() <= 0)
                {
                    RemoveEffect(i);
                }
            }

            foreach (Effect effect in _effects)
            {
                switch (effect.GetEffect())
                {
                    case Enums.Effect.Poison:
                        break;
                    case Enums.Effect.Bleed:
                        HurtOwner(effect.GetMagicDamage() * Time.deltaTime);
                        break;
                    case Enums.Effect.Burn:
                        HurtOwner(effect.GetMagicDamage() * Time.deltaTime);
                        break;
                    case Enums.Effect.Freeze:
                        break;
                    case Enums.Effect.Stun:
                        break;
                    case Enums.Effect.Glue:
                        break;
                    case Enums.Effect.Tar:
                        break;
                    case Enums.Effect.Absorbtion:
                        break;
                    case Enums.Effect.Healing:
                        break;
                    case Enums.Effect.MuscleBoost:
                        break;
                    case Enums.Effect.SpeedBoost:
                        break;
                    case Enums.Effect.MagicBoost:
                        break;
                    case Enums.Effect.DefenseBoost:
                        break;
                    case Enums.Effect.LuckBoost:
                        break;
                    case Enums.Effect.Invisibility:
                        break;
                    case Enums.Effect.Invulnerability:
                        break;
                    case Enums.Effect.None:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
            }
        }

        private void FreezeOwner(Effect effect)
        {
            if (gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                playerMovement.FreezeFor(effect.GetDuration());
            }
            else if (gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.FreezeFor(effect.GetDuration());
            }
        }

        private void HurtOwner(float getMagicDamage)
        {
            gameObject.GetComponent<Health>().TakeDamage(getMagicDamage, transform.position);
        }

        public void AddEffect(Effect effect)
        {
            if(_immuneTo.Contains(effect.GetEffect()))
                return;
            // if the list contains the added effect then remove it
            if (_effects.Contains(effect))
            {
                RemoveEffect(_effects.IndexOf(effect));
            }
                
            // if the list contains tar and the added effect is burn increase the burn damage and duration by 25%
            if (ContainsEffect(Enums.Effect.Tar) && effect.GetEffect() == Enums.Effect.Burn)
            {
                effect.SetDuration(effect.GetDuration() * 1.5f);
                effect.SetMagicDamage(effect.GetMagicDamage() * 1.25f);
            }
            
            // if the list contains freeze and the added effect is burn remove the freeze effect
            else if (ContainsEffect(Enums.Effect.Freeze) && effect.GetEffect() == Enums.Effect.Burn)
            {
                RemoveEffect(_effects.IndexOf(_effects.Find(e => e.GetEffect() == Enums.Effect.Freeze)));
                UnFreezeOwner(effect);
            }
            
            // if the list contains burn and the added effect is freeze remove the burn effect
            else if (ContainsEffect(Enums.Effect.Burn) && effect.GetEffect() == Enums.Effect.Freeze)
            {
                RemoveEffect(_effects.IndexOf(_effects.Find(e => e.GetEffect() == Enums.Effect.Burn)));
            }
            
            else if (effect.GetEffect() == Enums.Effect.Freeze)
                FreezeOwner(effect);
            
            _effects.Add(effect);
            GameObject effectGO = Instantiate(effect.GetEffectGameObject(), transform);
            _effectsGO.Add(effectGO);
        }

        private void UnFreezeOwner(Effect effect)
        {
            if (gameObject.TryGetComponent<PlayerMovement>(out PlayerMovement playerMovement))
            {
                playerMovement.UnFreeze();
            }
            else if (gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.UnFreeze();
            }
        }

        public void RemoveEffect(int i)
        {
            _effects.RemoveAt(i);
            Destroy(_effectsGO[i]);
            _effectsGO.RemoveAt(i);
        }
        
        public bool ContainsEffect(Enums.Effect effect)
        {
            foreach (Effect e in _effects)
            {
                if (e.GetEffect() == effect)
                    return true;
            }
            return false;
        }
    }
}