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
	
		if (character.Type == Defs.CharacterType.NPC) {
			npcGenerator.OnNPCDeath();
		} else {
			Restart();
		}
	}

	void OnCharacterGenerate(Character character) {
		characters.Add(character);
		InitializeCharacter(character);
	}

	void OnLastWaveCompleted()
	{
		
	}
}
