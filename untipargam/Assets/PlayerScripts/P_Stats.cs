using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class P_Stats : MonoBehaviour
{
    private float maxHP;
    private float currentHP;
    private Image[] hpBars;
    private bool healing;
    
    private enum ElementTypes
    {
        Metal, Water, Nature, Fire, Earth
    }
    
    void Start()
    {
        currentHP = maxHP;
        HPChange(0);
    }

    void Update()
    {
        if (currentHP > maxHP)
        {
            StopCoroutine(Healing());
            currentHP = maxHP;
        }
    }

    void HPChange(float damageTaken)
    {
        foreach (Image _hpBar in hpBars)
        {
            _hpBar.fillAmount = (currentHP - damageTaken)  / maxHP;
        }
    }

    private IEnumerator Healing()
    {
        while (healing && currentHP < maxHP)
        {
            HPChange(-0.1f);
            yield return new WaitForSeconds(0.1f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            StartCoroutine(Healing());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone"))
        {
            StopCoroutine(Healing());
        }
    }
}
