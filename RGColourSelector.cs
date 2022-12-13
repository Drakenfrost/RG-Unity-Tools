using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RGUnityTools
{
    public class RGColourSelector : MonoBehaviour
    {
        [SerializeField] Material materialOveride;

        [SerializeField] ColourSelectorButton[] colourOptions;

        void Start()
        {
            foreach (ColourSelectorButton b in colourOptions)
            {
                if (materialOveride) b.material = materialOveride;
                b.button.onClick.AddListener(b.OnClick);
            }
        }
    }

    [System.Serializable]
    public class ColourSelectorButton
    {
        public Button button;
        public Image imageColour;
        public Material material;

        public void OnClick()
        {
            material.color = imageColour.color;
        }
    }
}
