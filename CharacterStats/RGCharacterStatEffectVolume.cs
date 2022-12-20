using UnityEngine;

namespace RGUnityTools.CharacterStats
{
    public class RGCharacterStatEffectVolume : MonoBehaviour
    {
        public bool onlyEffectOnce;
        public DamageEffect[] effects;
        
        private void Awake()
        {
            Collider c = GetComponent<Collider>();
            if (c)
                c.isTrigger = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                ApplyEffectsInstantly();
                if (onlyEffectOnce)
                    Destroy(gameObject);
            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.tag == "Player")
            {
                ApplyEffectsOverTime();
            }
        }

        //Called once.
        void ApplyEffectsInstantly()
        {
            foreach (DamageEffect e in effects)
            {
                if (e.instant <= 0) continue;

                switch (e.damageType)
                {
                    case DamageType.physical:
                        RGCharacterStats.Damage(e.instant);
                        break;
                    case DamageType.bleed:
                        RGCharacterStats.Bleed(e.instant);
                        break;
                    case DamageType.poison:
                        RGCharacterStats.Poison(e.instant);
                        break;
                    case DamageType.burn:
                        RGCharacterStats.Burn(e.instant);
                        break;
                    case DamageType.freeze:
                        RGCharacterStats.Freeze(e.instant);
                        break;
                    default:
                        break;
                }
            }
        }

        //Called every frame.
        void ApplyEffectsOverTime()
        {
            foreach (DamageEffect e in effects)
            {
                if (e.perSecond <= 0) continue;

                switch (e.damageType)
                {
                    case DamageType.physical:
                        RGCharacterStats.Damage(e.perSecond * Time.deltaTime);
                        break;
                    case DamageType.bleed:
                        RGCharacterStats.Bleed(e.perSecond * Time.deltaTime);
                        break;
                    case DamageType.poison:
                        RGCharacterStats.Poison(e.perSecond * Time.deltaTime);
                        break;
                    case DamageType.burn:
                        RGCharacterStats.Burn(e.perSecond * Time.deltaTime);
                        break;
                    case DamageType.freeze:
                        RGCharacterStats.Freeze(e.perSecond * Time.deltaTime);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    [System.Serializable]
    public class DamageEffect
    {
        public DamageType damageType;
        public float instant;
        public float perSecond;
    }
}