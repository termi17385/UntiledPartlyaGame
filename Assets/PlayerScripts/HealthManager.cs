using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
	public float currentHP, maxHP;
	public UnityEvent onDeathEvent;
	public Image healthBar;
	
    void Start()
    {
		currentHP = maxHP;
		UpdateDisplay();
    }

    public void ChangeHP(float changeBy)
	{
		currentHP += changeBy;
		UpdateDisplay();
		if (currentHP > maxHP)
			currentHP = maxHP;
		if (currentHP <= 0)
			onDeathEvent.Invoke();
	}

	void UpdateDisplay()
	{
		if (healthBar == null)
			return;
		healthBar.fillAmount = currentHP/maxHP;
	}
}
