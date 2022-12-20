using System;
using UnityEngine;

namespace RGUnityTools.CharacterStats
{
    public class RGCharacterStats : MonoBehaviour
    {
        public RGCharacterStatData savedData;
        public static RGCharacterStatData data;
        
        #region STATS_VARIABLES
        //Level and XP.
        public static int level { get; private set; } = 1;
        public static Action<int> levelUpdated;
        public static float xp { get; private set; } = 0f;
        public static float maxXp { get; private set; } = 1000f;
        public static Action<float, float> xpUpdated;
        //Health.
        public static float health { get; private set; } = 100f;
        public static float maxHealth { get; private set; } = 100f;
        public static Action<float> healthUpdated;
        public static Action died;
        //Armour.
        public static float armour { get; private set; } = 0f;
        public static Action<float> armourUpdated;
        //Nourishment.
        public static float nourishment { get; private set; } = 50f;
        public static Action<float> nourishmentUpdated;
        //Hydration.
        public static float hydration { get; private set; } = 50f;
        public static Action<float> hydrationUpdated;
        //Stamina.
        public static float stamina { get; private set; } = 100f;
        public static float maxStamina { get; private set; } = 100f;
        public static Action<float> staminaUpdated;
        //Mana
        public static float mana { get; private set; } = 100f;
        public static float maxMana { get; private set; } = 100f;
        public static Action<float> manaUpdated;
        #endregion

        #region EFFECTS_VARIABLES
        //All effects maxed out at 100.
        public static float bleed { get; private set; } = 0f;
        public static Action<float> bleedUpdated;
        public static float poison { get; private set; } = 0f;
        public static Action<float> poisonUpdated;
        public static float burn { get; private set; } = 0f;
        public static Action<float> burnUpdated;
        public static float freeze { get; private set; } = 0f;
        public static Action<float> freezeUpdated;
        float freezeSpeedMultiplier = 1f;
        #endregion

        #region OTHER_VARIABLES
        public static float temperature { get; private set; } = 1f;
        public static float insulation { get; private set; } = 0f;
        #endregion

        void Update()
        {
            if (data == null)
            {
                data = ScriptableObject.CreateInstance<RGCharacterStatData>();
                return;
            }

            #region DEBUG
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                float newTemp = temperature - 0.1f;
                SetTemperature(newTemp);
                Debug.Log(temperature);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                insulation += .3f;
                Debug.Log(insulation);
            }
            #endregion

            #region CONTINUING_STATUS_EFFECTS
            float healthChange = 0f;
            float nourishmentChange = 0f;
            float hydrationChange = 0f;
            float staminaChange = 0f;
            float manaChange = 0f;

            float bleedChange = 0f;
            float poisonChange = 0f;
            float burnChange = 0f;
            float freezeChange = 0f;

            //Regenerate health:
            if (health < maxHealth)
                healthChange += data.healthRegenRate;
            //Persisting hunger:
            if (nourishment > 0)
                nourishmentChange -= data.nourishmentLossPerSec;
            else
                healthChange -= data.nourishmentEmptyDamagPerSec;
            //Persisting thirst:
            if (hydration > 0)
                hydrationChange -= data.hydrationLossPerSec;
            else
                healthChange -= data.hydrationEmptyDamagPerSec;
            //Energy recouperation:
            if (stamina < maxStamina)
            {
                staminaChange += data.staminaRegenRate;
                nourishmentChange -= data.nourishmentToStaminaPerSec;
                hydrationChange -= data.hydrationToStaminaPerSec;
            }
            //Magic regeneration:
            if (mana < maxMana)
            {
                manaChange += data.manaRegenRate;
            }
            //Bleeding:
            if (bleed > 0)
            {
                bleedChange -= data.bleedDecayRate;
                healthChange -= data.bleedDamageMultiplier * bleed;
            }
            //Poisoned:
            if (poison > 0)
            {
                poisonChange =+ data.poisonInfectionRate;
                healthChange -= data.poisonDamageMultiplier * poison;
            }
            //Burning:
            if (burn > 0)
            {
                burnChange -= data.burnDecayRate * (1 + 0.1f * freeze);
                healthChange -= data.burnDamageMultiplier * burn;
            }
            //Freezing:
            if (freeze > 0)
            {
                healthChange -= data.freezeDamageMultiplier * freeze;
            }

            if (temperature < 0.5f)
                freezeChange += (data.freezeRate - temperature - insulation);
            else
                freezeChange -= data.freezeDecayRate * (1 + 0.1f * burn) * temperature;

            //Apply effects:
            if (healthChange > 0)
                Heal(healthChange * Time.deltaTime);
            else if (healthChange < 0)
                Damage(-healthChange * Time.deltaTime);
            if (nourishmentChange != 0)
                Nourish(nourishmentChange * Time.deltaTime);
            if (hydrationChange != 0)
                Hydrate(hydrationChange * Time.deltaTime);
            if (staminaChange > 0)
                GainStamina(staminaChange * Time.deltaTime);
            else if (staminaChange < 0)
                UseStamina(-staminaChange * Time.deltaTime);
            if (manaChange > 0)
                GainMana(manaChange * Time.deltaTime);
            else if (manaChange < 0)
                UseMana(-manaChange * Time.deltaTime);
            if (bleedChange != 0)
                Bleed(bleedChange * Time.deltaTime);
            if (poisonChange != 0)
                Poison(poisonChange * Time.deltaTime);
            if (burnChange != 0)
                Burn(burnChange * Time.deltaTime);
            if (freezeChange != 0)
                Freeze(freezeChange * Time.deltaTime);
            #endregion
        }

        public void LoadNewStats()
        {
            if (savedData != null)
            {
                data = savedData;
                return;
            }

            SetLevel(1);
            SetXP(0f);
            SetHealth(100f);
            SetArmour(0f);
            SetNourishment(30f);
            SetHydration(30f);
            SetStamina(100f);
            SetMana(100f);

            SetBleed(0f);
            SetPoison(0f);
            SetBurn(0f);
            SetFreeze(0f);

            temperature = 1f;
            insulation = 0f;

            Debug.Log("Loaded new player stats.");
        }

        #region LEVEL_AND_XP
        static void SetLevel(int value)
        {
            level = value;
            levelUpdated?.Invoke(level);
        }

        static void SetXP(float value)
        {
            xp = value;
            xpUpdated?.Invoke(xp, maxXp);
        }

        public static void GainXP(float amount)
        {
            float amountOwed = amount;
            float amountToNextLevel = 1000 - xp;

            while (amountOwed >= amountToNextLevel)
            {
                xp = 0f;
                amountOwed -= amountToNextLevel;

                if (level < 100)
                {
                    SetLevel(level + 1);
                }
                else
                {
                    xp = 1000f;
                    return;
                }
            }

            xp += amountOwed;
            xpUpdated?.Invoke(xp, maxXp);
        }
        #endregion

        #region HEALTH
        static void SetHealth(float value)
        {
            health = value;
            if (health > maxHealth)
                health = maxHealth;
            if (health < 0f)
                health = 0f;

            healthUpdated?.Invoke(health);
        }

        public static void Heal(float amount)
        {
            health += amount;
            SetHealth(health);
        }

        public static void Damage(float amount)
        {
            if (health <= 0) return;

            health -= amount - (amount * 0.01f * armour);
            SetHealth(health);

            if (health <= 0)
                Kill();
        }

        static void Kill()
        {
            SetHealth(0f);
            Time.timeScale = 0f;
            died?.Invoke();
        }
        #endregion

        #region ARMOUR
        static void SetArmour(float value)
        {
            armour = value;
            if (armour > 100f)
                armour = 100f;
            if (armour < 0f)
                armour = 0f;

            armourUpdated?.Invoke(armour);
        }
        #endregion

        #region NOURISHMENT
        static void SetNourishment(float value)
        {
            nourishment = value;
            if (nourishment < 0)
                nourishment = 0;
            if (nourishment > 100f)
                nourishment = 100f;

            nourishmentUpdated?.Invoke(nourishment);
        }

        public static void Nourish(float amount)
        {
            nourishment += amount;
            SetNourishment(nourishment);
        }
        #endregion

        #region HYDRATION
        static void SetHydration(float value)
        {
            hydration = value;
            if (hydration < 0)
                hydration = 0;
            if (hydration > 100f)
                hydration = 100f;

            hydrationUpdated?.Invoke(hydration);
        }

        public static void Hydrate(float amount)
        {
            hydration += amount;
            SetHydration(hydration);
        }
        #endregion

        #region STAMINA
        static void SetStamina(float value)
        {
            stamina = value;
            if (stamina > maxStamina)
                stamina = maxStamina;
            if (stamina < 0)
                stamina = 0;

            staminaUpdated?.Invoke(stamina);
        }

        public static void GainStamina(float amount)
        {
            stamina += amount;
            SetStamina(stamina);
        }

        public static void UseStamina(float amount)
        {
            stamina -= amount;
            SetStamina(stamina);
        }
        #endregion

        #region MANA
        static void SetMana(float value)
        {
            mana = value;
            if (mana > maxMana)
                mana = maxMana;
            if (mana < 0)
                mana = 0;

            manaUpdated?.Invoke(mana);
        }

        public static void GainMana(float amount)
        {
            mana += amount;
            SetMana(mana);
        }

        public static void UseMana(float amount)
        {
            mana -= amount;

            SetMana(mana);
        }
        #endregion

        #region BLEED
        static void SetBleed(float value)
        {
            bleed = value;
            if (bleed > 100f)
                bleed = 100f;
            if (bleed < 0f)
                bleed = 0f;

            bleedUpdated?.Invoke(bleed);
        }

        public static void Bleed(float amount)
        {
            bleed += amount;
            SetBleed(bleed);
        }
        #endregion

        #region POISON
        static void SetPoison(float value)
        {
            poison = value;
            if (poison > 100f)
                poison = 100f;
            if (poison < 0f)
                poison = 0f;

            poisonUpdated?.Invoke(poison);
        }

        public static void Poison(float amount)
        {
            poison += amount;
            SetPoison(poison);
        }
        #endregion

        #region BURN
        static void SetBurn(float value)
        {
            burn = value;
            if (burn > 100f)
                burn = 100f;
            if (burn < 0f)
                burn = 0f;

            burnUpdated?.Invoke(burn);
        }

        public static void Burn(float amount)
        {
            burn += amount;
            SetBurn(burn);
        }
        #endregion

        #region FREEZE
        static void SetFreeze(float value)
        {
            freeze = value;
            if (freeze > 100f)
                freeze = 100f;
            if (freeze < 0f)
                freeze = 0f;

            freezeUpdated?.Invoke(freeze);
        }

        public static void Freeze(float amount)
        {
            freeze += amount;
            SetFreeze(freeze);
        }
        #endregion

        static void SetTemperature(float value)
        {
            temperature = value;
            if (temperature > 1f)
                temperature = 1f;
            if (temperature < 0f)
                temperature = 0f;
        }

        static void SetInsulation(float value)
        {
            insulation = value;
            if (insulation > 1f)
                insulation = 1f;
            if (insulation < 0f)
                insulation = 0f;
        }
    }

    public enum DamageType { physical, bleed, poison, burn, freeze }
}
