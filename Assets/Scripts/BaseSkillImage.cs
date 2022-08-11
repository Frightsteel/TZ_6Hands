using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseSkillImage : MonoBehaviour
{
    private Image skillImage; // if need to to swap skill
    private Image cooldownImage;


    private void Start()
    {
        cooldownImage = GetComponent<Image>();
        
    }
}
