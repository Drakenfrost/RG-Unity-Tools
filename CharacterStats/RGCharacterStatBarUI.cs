using UnityEngine;
using UnityEngine.UI;

namespace RGUnityTools.CharacterStats
{
    public class RGCharacterStatBarUI : MonoBehaviour
    {
        public Stat stat;
        public Image icon;
        public Slider slider;
        public GameObject sliderFill;
        public Slider residualSlider;
        public GameObject residualSliderFill;
        [Range(0.1f, 1f)]
        public float residualInterpolation = 0.05f;
        public int order;

        private void Update()
        {
            Residual();
        }

        public void SetMaxValue(float value)
        {
            slider.minValue = 0f;
            slider.maxValue = value;
            residualSlider.minValue = 0f;
            residualSlider.maxValue = value;
        }

        public void SetValue(float value)
        {
            slider.value = value;

            if (sliderFill == null) return;
            if (value == 0 && sliderFill.activeSelf)
            {
                sliderFill.SetActive(false);
                residualSliderFill.SetActive(false);
            }
            else if (value > 0 && !sliderFill.activeSelf)
            {
                sliderFill.SetActive(true);
                residualSliderFill.SetActive(true);
            }
        }

        void Residual()
        {
            if (slider.value > residualSlider.value)
                residualSlider.value = slider.value;
            else
                residualSlider.value = Mathf.Lerp(residualSlider.value, slider.value, residualInterpolation * Time.deltaTime);
        }
    }

    public enum Stat { xp, health, armour, nourishment, hydration, stamina, mana, bleed, poison, burn, freeze }
}
