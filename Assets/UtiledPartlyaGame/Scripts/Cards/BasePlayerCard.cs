using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType
{
	MatchCard,
	PlayerCard,
	TeamCard
}

public enum EffectType
{
	// effects that go here
}

public class BasePlayerCard : ScriptableObject
{
	public string cardName = "";
	[SerializeField] protected CardType cardType = CardType.PlayerCard;
	[SerializeField] protected EffectType effectType;

	[SerializeField] protected float effectPercentage;
	[SerializeField] protected int value;

	public virtual void GetCardData()
	{
			
	}
}
