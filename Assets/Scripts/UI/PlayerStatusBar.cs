using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;

public class PlayerStatusBar : MonoBehaviour
{
    [Header("Component")]
    public Image healthFillImage;
    public Image healthDelayFillImage;
    public Image powerFillImage;
    //[Header("Settings")]
    //[Header("Debug")]

    /// <summary>
    /// Get the health percentage and update the health bar
    /// </summary>
    /// <param name="percentage">health percentage</param>
    public void OnHealthChange(float percentage)
    {
        healthFillImage.fillAmount = percentage;
        healthDelayFillImage.DOFillAmount(healthFillImage.fillAmount, 1);
    }
}
