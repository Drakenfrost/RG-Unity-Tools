using UnityEngine;
using TMPro;

namespace RGUnityTools.CharacterStats
{
    public class RGCharacterStatsUI : MonoBehaviour
    {
        #region SINGLETON
        public static RGCharacterStatsUI instance;

        private void Awake()
        {
            instance = this;
        }
        #endregion

        [Header("Stats")]
        public TextMeshProUGUI levelUI;
        public RGCharacterStatBarUI xpUI;
        public RGCharacterStatBarUI healthUI;
        public RGCharacterStatBarUI armourUI;
        public RGCharacterStatBarUI nourishmentUI;
        public RGCharacterStatBarUI hydrationUI;
        public RGCharacterStatBarUI staminaUI;
        public RGCharacterStatBarUI manaUI;
        [Header("Effects")]
        public RGCharacterStatBarUI bleedUI;
        public RGCharacterStatBarUI poisonUI;
        public RGCharacterStatBarUI burnUI;
        public RGCharacterStatBarUI freezeUI;

        bool alwaysShow;

        private void OnEnable()
        {
            RGCharacterStats.levelUpdated += UpdateLevel;
            RGCharacterStats.xpUpdated += UpdateXP;
            RGCharacterStats.healthUpdated += UpdateHealth;
            RGCharacterStats.armourUpdated += UpdateArmour;
            RGCharacterStats.nourishmentUpdated += UpdateNourishment;
            RGCharacterStats.hydrationUpdated += UpdateHydration;
            RGCharacterStats.staminaUpdated += UpdateStamina;
            RGCharacterStats.manaUpdated += UpdateMana;

            RGCharacterStats.bleedUpdated += UpdateBleed;
            RGCharacterStats.poisonUpdated += UpdatePoison;
            RGCharacterStats.burnUpdated += UpdateBurn;
            RGCharacterStats.freezeUpdated += UpdateFreeze;
        }

        private void Start()
        {
            UpdateAllStats();
        }

        #region UPDATE_FUNCTIONS
        void UpdateAllStats()
        {
            UpdateLevel(RGCharacterStats.level);
            UpdateXP(RGCharacterStats.xp, RGCharacterStats.maxXp);
            UpdateHealth(RGCharacterStats.health);
            UpdateArmour(RGCharacterStats.armour);
            UpdateNourishment(RGCharacterStats.nourishment);
            UpdateHydration(RGCharacterStats.hydration);
            UpdateStamina(RGCharacterStats.stamina);
            UpdateMana(RGCharacterStats.mana);

            UpdateBleed(RGCharacterStats.bleed);
            UpdatePoison(RGCharacterStats.poison);
            UpdateBurn(RGCharacterStats.burn);
            UpdateFreeze(RGCharacterStats.freeze);
        }

        void UpdateLevel(int value)
        {
            levelUI.text = value.ToString();
        }

        void UpdateXP(float value, float maxValue)
        {
            xpUI.SetValue(value);
            xpUI.SetMaxValue(maxValue);

            DeactivateStat(xpUI);
        }

        void UpdateHealth(float value)
        {
            healthUI.SetValue(value);
            healthUI.SetMaxValue(RGCharacterStats.maxHealth);

            if (value < RGCharacterStats.maxHealth - 1)
                ActivateStat(healthUI);
            if (value >= RGCharacterStats.maxHealth)
                DeactivateStat(healthUI);
        }

        void UpdateArmour(float value)
        {
            armourUI.SetValue(value);
            armourUI.SetMaxValue(100);

            DeactivateStat(armourUI);
        }

        void UpdateNourishment(float value)
        {
            nourishmentUI.SetValue(value);
            nourishmentUI.SetMaxValue(100);

            if (value < 25f)
                ActivateStat(nourishmentUI);
            else
                DeactivateStat(nourishmentUI);
        }

        void UpdateHydration(float value)
        {
            hydrationUI.SetValue(value);
            hydrationUI.SetMaxValue(100);

            if (value < 25f)
                ActivateStat(hydrationUI);
            else
                DeactivateStat(hydrationUI);
        }

        void UpdateStamina(float value)
        {
            staminaUI.SetValue(value);
            staminaUI.SetMaxValue(RGCharacterStats.maxStamina);

            if (value < RGCharacterStats.maxStamina - 1)
                ActivateStat(staminaUI);
            if (value == RGCharacterStats.maxStamina)
                DeactivateStat(staminaUI);
        }

        void UpdateMana(float value)
        {
            manaUI.SetValue(value);
            manaUI.SetMaxValue(RGCharacterStats.maxMana);

            if (value < RGCharacterStats.maxMana - 1)
                ActivateStat(manaUI);
            if (value == RGCharacterStats.maxMana)
                DeactivateStat(manaUI);
        }

        void UpdateBleed(float value)
        {
            bleedUI.SetValue(value);
            bleedUI.SetMaxValue(100);

            if (value >= 1)
                ActivateStat(bleedUI);
            if (value == 0)
                DeactivateStat(bleedUI);
        }

        void UpdatePoison(float value)
        {
            poisonUI.SetValue(value);
            poisonUI.SetMaxValue(100);

            if (value >= 1)
                ActivateStat(poisonUI);
            if (value == 0)
                DeactivateStat(poisonUI);
        }

        void UpdateBurn(float value)
        {
            burnUI.SetValue(value);
            burnUI.SetMaxValue(100);

            if (value >= 1)
                ActivateStat(burnUI);
            if (value == 0)
                DeactivateStat(burnUI);
        }

        void UpdateFreeze(float value)
        {
            freezeUI.SetValue(value);
            freezeUI.SetMaxValue(100);

            if (value >= 1)
                ActivateStat(freezeUI);
            if (value == 0)
                DeactivateStat(freezeUI);
        }
        #endregion

        #region VISIBILITY_FUNCTIONS
        public void ShowAllStatsAndEffects()
        {
            alwaysShow = true;

            ActivateStat(xpUI);
            ActivateStat(healthUI);
            ActivateStat(armourUI);
            ActivateStat(nourishmentUI);
            ActivateStat(hydrationUI);
            ActivateStat(staminaUI);
            ActivateStat(manaUI);

            ActivateStat(bleedUI);
            ActivateStat(poisonUI);
            ActivateStat(burnUI);
            ActivateStat(freezeUI);

            UpdateAllStats();
        }

        public void OnlyShowActiveStats()
        {
            alwaysShow = false;
            UpdateAllStats();
        }

        public void ActivateStat(RGCharacterStatBarUI statUI)
        {
            statUI.gameObject.SetActive(true);
        }

        public void DeactivateStat(RGCharacterStatBarUI statUI)
        {
            if (alwaysShow) return;
            statUI.gameObject.SetActive(false);
        }
        #endregion
    }
}
