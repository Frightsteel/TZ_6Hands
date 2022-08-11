using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseBar : MonoBehaviour
{
    [SerializeField] protected bool autoRegeneration;
    [SerializeField] protected float regenerationValue;

    public float maxValue;

    protected Image barImage;

    private void Start()
    {
        barImage = GetComponent<Image>();
    }

    public void ChangeValue(float currentValue)
    {
        ChangeFillAmount(GetCurrentFill(currentValue));  
    }

    private float GetCurrentFill(float currentValue)
    {
        float fillAmount = currentValue / maxValue;
        return fillAmount;
    }

    private void ChangeFillAmount(float fillAmount)
    {
        barImage.fillAmount = fillAmount;
    }
}
