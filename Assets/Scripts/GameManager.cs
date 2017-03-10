using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Ui;

public partial class GameManager : MonoBehaviour
{
	[SerializeField]
	EffectManager effectManager;

	[SerializeField]
	InGameUiRoot ui;

	List<Character> characters;
	Character player;

	public void Awake()
	{
		GatherCharacters();
		InitializeCharacters();
	}


	void GatherCharacters()
	{
		characters = GameObject.FindObjectsOfType<Character>().ToList();
	}

	void InitializeCharacters()
	{
		var characterContext = CreateCharacterContext();
		for (int i = 0; i < characters.Count; ++i) {
			if (i == 0) {
				player = characters[i];
				player.HealthChangedAction += ui.OnPlayerHealthChanged;
			};
			characters[i].Init(characterContext);
			characters[i].AttackAction = OnCharacterAttack;
			characters[i].SpecialAttackAction = OnCharacterSpecialAttack;
			characters[i].DeathAction = OnCharacterDeath;
		}
		player = characters.Count > 0 ? characters[0] : null;
	}


	float GetCharactersSqrDistance(Character character1, Character character2) 
	{
		return (character1.transform.position - character2.transform.position).sqrMagnitude;
	}

	List<Character> GetHitCharacters(Character attackingCharacter, Attack attack)
	{
		var charactersInRange = BattleUtils.GetCharactersInRange(characters, attack.Colliders);
		charactersInRange.Remove(attackingCharacter);
		if (attack.Ranged) {
			return charactersInRange;
		} else {
			return charactersInRange.Count == 0 ? 
				new List<Character>() : 
				new List<Character>() { BattleUtils.SortCharactersByDistanceTo(charactersInRange, attackingCharacter.Position)[0]};
		}
	}

	bool IsCharacterInFronOfCharacter(Character who, Character from)
	{
		if (from.HDirection == Defs.HDirection.Left){
			return who.transform.position.x < from.transform.position.x;
		} else {
			return who.transform.position.x > from.transform.position.x;
		}
	}

	public CharacterContext CreateCharacterContext()
	{
		return new CharacterContext(effectManager);
	}


}
