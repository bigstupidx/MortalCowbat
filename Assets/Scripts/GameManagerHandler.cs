using System;
using Battle.Comp;

public partial class GameManager
{
	void OnCharacterAttack(Character attackingCharacter, Attack attack)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter, attackingCharacter.GetComp<Moving>().GetFlip(), 1.0f, false));
	}

	void OnCharacterJumpAttack(Character attackingCharacter, Attack attack, int attackId)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter, attackingCharacter.GetComp<Moving>().GetFlip(), 1.0f, false, attackId));
	}


	void OnCharacterHeavyAttack(Character attackingCharacter, Attack attack, float multiplier, bool maxed)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter, attackingCharacter.GetComp<Moving>().GetFlip(), multiplier, maxed));
	}

	void OnCharacterSpecialAttack(Character attackingCharacter, Attack attack, bool maxed)
	{
		var hitCharacters = GetHitCharacters(attackingCharacter, attack);
		hitCharacters.ForEach(x=>x.Hit(attack, attackingCharacter, attackingCharacter.GetComp<Moving>().GetFlip(), 1.0f, maxed));
	}

	void OnCharacterDeathStarted(Character character)
	{
		characters.Remove(character);	
	}

	void OnCharacterDeathFinished(Character character)
	{
		if (character.Type == Defs.CharacterType.NPC) {
			npcGenerator.OnNPCDeath();
		} else {
			if (events.PlayerDied != null) {
				events.PlayerDied(level, npcGenerator.WaveIndex);
			}


			Invoke("RestartAndKillNpcs", 3.0f);
		}
	}

	void RestartAndKillNpcs()
	{
		Restart(true);
	}

	void OnCharacterGenerate(Character character) {
		characters.Add(character);
		InitializeCharacter(character);
	}

	void OnAllWavesFinished()
	{
		if (events.AllWavesFinished != null) {
			events.AllWavesFinished(level);
		}

		StartCoroutine(SetNextLevel());
	}

	void OnWaveStarted(int actualWave, int waveCount)
	{
		if (events.WaveStarted != null) {
			events.WaveStarted(actualWave,waveCount);
		}
	}

	void OnWaveFinished(int actualWave, int waveCount)
	{
		if (events.WaveFinished != null) {
			events.WaveFinished(level, actualWave);
		}
	}

	void OnNPCLeftChanged(int npcLeft)
	{
		if (events.NPCLeftChanged != null) {
			events.NPCLeftChanged(npcLeft);
		}
	}

	void CallLevelStarted(int level)
	{
		if (events.LevelStarted != null) {
			events.LevelStarted(level);
		}
	}
}
