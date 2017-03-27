﻿using System;

public partial class GameManager
{
	void OnCharacterAttack(Character attackingCharacter, Attack attack)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter.GetFlip(), 1.0f, false));
	}

	void OnCharacterHeavyAttack(Character attackingCharacter, Attack attack, float multiplier, bool maxed)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter.GetFlip(), multiplier, maxed));
	}

	void OnCharacterSpecialAttack(Character attackingCharacter, Attack attack, bool maxed)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter.GetFlip(), 1.0f, maxed));
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

	void OnAllWavesFinished()
	{
		StartCoroutine(SetNextLevel());
	}
}
