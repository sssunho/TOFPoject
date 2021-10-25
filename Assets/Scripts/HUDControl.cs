using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDControl : MonoBehaviour
{
    #region General Variables

    #region Health/Stamina Variables
    [Header("Health/Stamina/Focus/Strength")]
    public Slider healthSlider;
    public Slider staminaSlider;
    public Slider focusSlider;
    public Slider strengthSlider;

    [Header("DamageHUD")]
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);
    [HideInInspector] public bool damaged;
    #endregion

    #endregion
}