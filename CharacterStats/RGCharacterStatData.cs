using UnityEngine;

namespace RGUnityTools.CharacterStats
{
    [CreateAssetMenu(fileName = "Player Stats", menuName = "Data/Stat Data/Player")]
    public class RGCharacterStatData : ScriptableObject
    {
        [Header("Stats")]
        public float healthRegenRate;

        public float nourishmentLossPerSec;
        public float nourishmentToStaminaPerSec;
        public float nourishmentEmptyDamagPerSec;

        public float hydrationLossPerSec;
        public float hydrationToStaminaPerSec;
        public float hydrationEmptyDamagPerSec;

        public float staminaRegenRate;
        public float staminaRunUseRate;
        public float staminaUsePerJump;

        public float manaRegenRate;

        [Header("Effects")]
        public float bleedDamageMultiplier;
        public float bleedDecayRate;

        public float poisonDamageMultiplier;
        public float poisonInfectionRate;

        public float burnDamageMultiplier;
        public float burnDecayRate;

        public float freezeDamageMultiplier;
        public float freezeSlowMultiplier;
        public float freezeDecayRate;
        public float freezeRate;
    }
}
