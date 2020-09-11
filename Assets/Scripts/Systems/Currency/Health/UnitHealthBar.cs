using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Object health;

    public void Onhit()
    {
        ICurrency Unit = (ICurrency)health;
        healthBar.value = Unit.CurrentValue / Unit.MaxValue;
    }

}
