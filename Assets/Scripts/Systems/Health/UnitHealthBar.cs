using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private Object health;

    public void Onhit(){
        IHealth Unit = (IHealth)health;
        healthBar.value = Unit.CurrentHealth/Unit.MaxHealth; 
    }

}
