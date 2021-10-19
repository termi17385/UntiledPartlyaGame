using UnityEngine;

/// <summary> The type of
/// card we are using </summary>
public enum CardType
{
	MatchCard,
	PlayerCard,
	TeamCard
}
/// <summary> The effect the
/// card will have </summary>
public enum EffectType
{
	BonusPoints,
	BonusSpeed,
}

public class BaseCard : ScriptableObject
{
	[SerializeField] protected string cardName = "";
	protected CardType cardType = CardType.PlayerCard;
	
	[SerializeField] protected EffectType effectType;
	[SerializeField] protected float effectPercentage;
	[SerializeField] protected int rarityValue;

	private void OnValidate() => cardName = name;

	/// <summary> Returns all the data
	/// of the card when called </summary>
	/// <param name="_type">what type of card we are using</param>
	/// <param name="_effectType">what the effect of that card will be</param>
	/// <param name="_percentage">how much the card will effect the player</param>
	/// <param name="_rarity">the rarity of the card for being drawn</param>
	/// <param name="_cardName">the name of the card</param>
	public virtual void GetCardData(out CardType _type, out EffectType _effectType, out float _percentage, out float _rarity, out string _cardName)
	{
		_cardName = cardName;
		_type = cardType;
		_effectType = effectType;
		_percentage = effectPercentage;
		_rarity = rarityValue;
	}
}
