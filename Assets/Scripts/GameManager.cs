using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
	[SerializeField]
	EffectManager effectManager;

	List<Character> characters;

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
			characters[i].Init(characterContext);
			characters[i].AttackAction = OnCharacterAttack;
			characters[i].SpecialAttackAction = OnCharacterSpecialAttack;
		}
	}


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

//		var charactersInAttackDistance = new List<KeyValuePair<float, Character>>();
//		for (int i = 0; i < characters.Count; ++i) {
//			var otherCharacter = characters[i];
//			if (otherCharacter != attackingCharacter) {
//				float sqrDistance = GetCharactersSqrDistance(otherCharacter, attackingCharacter);
//				if (IsCharacterInFronOfCharacter(otherCharacter, attackingCharacter)) {
//					if (sqrDistance < attackingCharacter.Settings.AttackDistance * attackingCharacter.Settings.AttackDistance) {
//						charactersInAttackDistance.Add(new KeyValuePair<float, Character>(sqrDistance, otherCharacter));
//					}
//				}
//			}
//		}
//		charactersInAttackDistance.Sort((a, b) => a.Key < b.Key ? -1 : 1);
//		return charactersInAttackDistance.Count == 0 ? null : charactersInAttackDistance[0].Value;
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
