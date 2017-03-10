using System;

public partial class GameManager
{
	void OnCharacterAttack(Character attackingCharacter, Attack attack)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter.HDirection));
	}

	void OnCharacterSpecialAttack(Character attackingCharacter, Attack attack)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter.HDirection));
	}

	void OnCharacterDeath(Character character)
	{
		characters.Remove(character);
	}
}
