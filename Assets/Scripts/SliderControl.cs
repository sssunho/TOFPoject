using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class SliderControl : MonoBehaviour
    {
        public Slider slider;
        public RectTransform rectTransform;
        public float multipScale = 0.1f;
        float oldMaxValue;
        void OnDrawGizmosSelected()
        {
            UpdateScale();
        }
        public void UpdateScale()
        {
            if (rectTransform && slider)
            {
                if (slider.maxValue != oldMaxValue)
                {
                    var sizeDelta = rectTransform.sizeDelta;
                    sizeDelta.x = slider.maxValue * multipScale;
                    rectTransform.sizeDelta = sizeDelta;
                    oldMaxValue = slider.maxValue;
                }
            }
        }

        public void setMaxValue(int maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }

        public void setCurValue(int curValue)
        {
            slider.value = curValue;
        }

    }
}


